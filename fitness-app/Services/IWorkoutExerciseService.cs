using System.Collections.Generic;
using System.Threading.Tasks;
using fitness_app.Models.Supabase;

namespace fitness_app.Services;

public interface IWorkoutExerciseService
{
    Task<List<Exercise>> GetExercisesForWorkoutAsync(string workoutId);
    Task<double> GetWorkoutDurationAsync(string workoutId);
}