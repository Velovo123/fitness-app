using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace fitness_app.Models.Supabase;

[Table("workouts")]
public class Workout : BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }

    [Column("workout_name")]
    public string? WorkoutName { get; set; }

    [Column("duration")]
    public int Duration { get; set; }  

    [Column("workout_video")]
    public string? WorkoutVideo { get; set; }  

    [Column("workout_preview")]
    public string? WorkoutPreview { get; set; } 

    [Column("level")]
    public string? Level { get; set; } 

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}