using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace fitness_app.Models.Supabase;

[Table("exercises")]
public class Exercise : BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }

    [Column("exercise_name")]
    public string? ExerciseName { get; set; }

    [Column("level")]
    public string? Level { get; set; }

    [Column("thumbnail")]
    public string? Thumbnail { get; set; }

    [Column("video")]
    public string? Video { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("category")]
    public string? Category { get; set; }
    
    [Column("duration")]
    public double? Duration { get; set; }
}