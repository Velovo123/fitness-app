using fitness_app.Models.Supabase;

namespace fitness_app.Services;

public interface IWorkoutService
{
    Task<List<Workout>> GetAllWorkoutsAsync();
    Task<Workout?> GetWorkoutByIdAsync(string workoutId);
}