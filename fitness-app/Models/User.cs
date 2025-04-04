namespace fitness_app.Models;

public class User
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public decimal? Weight { get; set; }
    
    public string? WeightUnit { get; set; }
    public decimal? Height { get; set; }
    
    public string? HeightUnit { get; set; }
    public int? Age { get; set; }
    public string? Photo { get; set; }
}