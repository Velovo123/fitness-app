using fitness_app.Models.Supabase;
using Supabase.Gotrue;

namespace fitness_app.Services;

public interface IUserFitnessDataService
{
    Task<UserFitnessData?> UpdateUserFitnessDataAsync(Session session, UserFitnessData data);
    Task<List<string>> RetrieveMissingPropertyNamesAsync(Session session);
    Task<Models.User> GetUserFromSessionAsync(Session session);
    Task<UserFitnessData?> GetUserFitnessDataAsync(Session session);
}