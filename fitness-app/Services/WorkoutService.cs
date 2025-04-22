using fitness_app.Models.Supabase;
using Client = Supabase.Client;

namespace fitness_app.Services;

public class WorkoutService : IWorkoutService
{
    private readonly Client _supabaseClient;

    public WorkoutService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
    
    public async Task<List<Workout>> GetAllWorkoutsAsync()
    {
        var response = await _supabaseClient
            .From<Workout>()
            .Get();

        return response.Models;
    }

    public async Task<Workout?> GetWorkoutByIdAsync(string workoutId)
    {
        var response = await _supabaseClient
            .From<Workout>()
            .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, workoutId)
            .Single();

        return response;
    }
}