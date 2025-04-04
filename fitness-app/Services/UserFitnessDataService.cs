using fitness_app.Constants;
using fitness_app.Models.Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace fitness_app.Services;

public class UserFitnessDataService : IUserFitnessDataService
{
    private readonly Client _supabaseClient;

    private static readonly HashSet<string> ExcludedProperties = new HashSet<string>
    {
        nameof(UserFitnessData.HeightUnit),
        nameof(UserFitnessData.WeightUnit),
        nameof(UserFitnessData.DesiredWeightUnit)
    };

    public UserFitnessDataService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
    
    public async Task<UserFitnessData?> UpdateUserFitnessDataAsync(Session session, UserFitnessData data)
    {
        var userId = GetUserId(session);
        data.Id = userId;
        
        var currentRecord = await GetUserFitnessDataAsync(session);
        if (currentRecord == null)
        {
            return null;
        }

        MergeUserFitnessData(data, currentRecord);

        var updateResponse = await _supabaseClient
            .From<UserFitnessData>()
            .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, userId)
            .Update(currentRecord);

        return updateResponse.Models.FirstOrDefault();
    }

    public async Task<List<string>> RetrieveMissingPropertyNamesAsync(Session session)
    {
        var result = await GetUserFitnessDataAsync(session);
        if (result == null)
            return new List<string>();

        return GetNullProperties(result);
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
        var userId = GetUserId(session);
        return await _supabaseClient
            .From<UserFitnessData>()
            .Where(u => u.Id == userId)
            .Single();
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
    
    private string GetUserId(Session session)
    {
        if (session.User == null || string.IsNullOrEmpty(session.User.Id))
            throw new InvalidOperationException("No valid user in session.");
        return session.User.Id;
    }
    
    private List<string> GetNullProperties(UserFitnessData data)
    {
        return data.GetType()
                   .GetProperties()
                   .Where(prop => !ExcludedProperties.Contains(prop.Name) && prop.GetValue(data) == null)
                   .Select(prop => prop.Name)
                   .ToList();
    }
}
