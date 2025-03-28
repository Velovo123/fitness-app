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