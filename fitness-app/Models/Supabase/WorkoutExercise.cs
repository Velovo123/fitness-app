using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace fitness_app.Models.Supabase;

[Table("workout_exercises")]
public class WorkoutExercise : BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }

    [Column("workout_id")]
    public string? WorkoutId { get; set; }

    [Column("exercise_id")]
    public string? ExerciseId { get; set; }
}