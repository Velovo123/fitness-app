using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fitness_app.Models.Supabase;
using Client = Supabase.Client;

namespace fitness_app.Services;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly Client _supabaseClient;

    public WorkoutExerciseService(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
    
    public async Task<List<Exercise>> GetExercisesForWorkoutAsync(string workoutId)
    {
        var joinResponse = await _supabaseClient
            .From<WorkoutExercise>()
            .Filter("workout_id", Supabase.Postgrest.Constants.Operator.Equals, workoutId)
            .Get();

        var joinEntries = joinResponse.Models;

        var exerciseIds = joinEntries
            .Where(j => !string.IsNullOrEmpty(j.ExerciseId))
            .Select(j => j.ExerciseId)
            .Distinct()
            .ToList();

        if (!exerciseIds.Any())
        {
            return new List<Exercise>();
        }

        var exercisesResponse = await _supabaseClient
            .From<Exercise>()
            .Filter("id", Supabase.Postgrest.Constants.Operator.In, exerciseIds)
            .Get();

        return exercisesResponse.Models;
    }
    
    public async Task<double> GetWorkoutDurationAsync(string workoutId)
    {
        var exercises = await GetExercisesForWorkoutAsync(workoutId);
            
        double? totalDuration = exercises.Sum(e => e.Duration);
        
        return totalDuration ?? 0f;
    }
}