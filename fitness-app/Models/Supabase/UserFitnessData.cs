using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace fitness_app.Models.Supabase;

[Table("user_fitness_data")]

public class UserFitnessData : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("current_weight")]
    public decimal? CurrentWeight { get; set; }

    [Column("desired_weight")]
    public decimal? DesiredWeight { get; set; }

    [Column("weight_unit")] 
    public string? WeightUnit { get; set; } 
    
    [Column("desired_weight_unit")]
    
    public string? DesiredWeightUnit { get; set; }

    [Column("height")]
    public decimal? Height { get; set; }

    [Column("height_unit")]
    public string? HeightUnit { get; set; }

    [Column("fitness_level")]
    public string? FitnessLevel { get; set; }
    
    [Column("favorite_activity")]
    public string? FavoriteActivity { get; set; }

    [Column("goal")]
    public string? Goal { get; set; }
    
    [Column("age")]
    public int? Age { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}