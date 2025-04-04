using fitness_app.Constants;
using fitness_app.Models.Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace fitness_app.Services;

public class UserFitnessDataService : IUserFitnessDataService
{
    private readonly Client _supabaseClient;

    public UserFitnessDataService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
    
    public async Task<UserFitnessData?> UpdateUserFitnessDataAsync(Session session, UserFitnessData data)
    {
        if (string.IsNullOrEmpty(session.User?.Id))
            throw new InvalidOperationException("No valid user in session.");
    
        data.Id = session.User.Id;
        var currentRecordResponse = await _supabaseClient
            .From<UserFitnessData>()
            .Where(u => u.Id == data.Id)
            .Single();

        if (currentRecordResponse == null)
        {
            return null;
        }

        MergeUserFitnessData(data, currentRecordResponse);

        var updateResponse = await _supabaseClient
            .From<UserFitnessData>()
            .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, session.User.Id)
            .Update(currentRecordResponse);

        return updateResponse.Models.FirstOrDefault();
    }

    public async Task<List<string>> RetrieveMissingPropertyNamesAsync(Session session)
    {
        var result = await _supabaseClient
            .From<UserFitnessData>()
            .Where(u => u.Id == session.User!.Id)
            .Single();

        if(result == null)
            return new List<string>();
        
        var excludedProperties = new HashSet<string> { nameof(UserFitnessData.HeightUnit), nameof(UserFitnessData.WeightUnit), nameof(UserFitnessData.DesiredWeightUnit) };
        
        var nullProperties = result.GetType()
            .GetProperties()
            .Where(prop => !excludedProperties.Contains(prop.Name) && prop.GetValue(result) == null)
            .Select(prop => prop.Name)
            .ToList();

        return nullProperties;
    }

    public async Task<Models.User> GetUserFromSessionAsync(Session session)
    {
        if (session == null || session.User == null)
            throw new InvalidOperationException("No valid session or user found.");
    
        var fitnessData = await GetUserFitnessDataAsync(session);
        return MapSessionToUser(session, fitnessData!);
    }

    private Models.User MapSessionToUser(Session session, UserFitnessData fitnessData)
    {
        var userMeta = session.User!.UserMetadata;
    
        string? GetValue(string key)
            => userMeta.TryGetValue(key, out var value) ? value?.ToString() : null;
    
        var fullName = GetValue(UserMetadataConstants.FullName)
                       ?? GetValue(UserMetadataConstants.Name)
                       ?? session.User.Email;

        var photoUrl = GetValue(UserMetadataConstants.AvatarUrl)
                       ?? GetValue(UserMetadataConstants.Picture)
                       ?? EnvConstants.DefaultImageUrl;
    
        return new Models.User
        {
            FullName = fullName,
            Email = session.User.Email,
            Weight = fitnessData.CurrentWeight ?? 0,   
            WeightUnit = fitnessData.WeightUnit,
            Height = fitnessData.Height ?? 0,   
            HeightUnit = fitnessData.HeightUnit,
            Age = fitnessData.Age ?? 0,      
            Photo = photoUrl
        };
    }


    public async Task<UserFitnessData?> GetUserFitnessDataAsync(Session session)
    {
        var result = await _supabaseClient
            .From<UserFitnessData>()
            .Where(u => u.Id == session.User!.Id)
            .Single();
        
        return result;
    }

    private void MergeUserFitnessData(UserFitnessData source, UserFitnessData target)
    {
        foreach (var property in typeof(UserFitnessData).GetProperties())
        {
            if (property.Name == nameof(UserFitnessData.Id) ||
                property.Name == nameof(UserFitnessData.CreatedAt) ||
                !property.CanWrite)
            {
                continue;
            }

            var newValue = property.GetValue(source);
            if (newValue != null)
            {
                property.SetValue(target, newValue);
            }
        }
    }
   
}