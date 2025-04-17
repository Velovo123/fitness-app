using fitness_app.Models;

namespace fitness_app.Extensions;

public static class UserExtensions
{
    public static User DeepClone(this User source) => new User
    {
        FullName   = source.FullName,
        Email      = source.Email,
        Weight     = source.Weight,
        WeightUnit = source.WeightUnit,
        Height     = source.Height,
        HeightUnit = source.HeightUnit,
        Age        = source.Age,
        Photo      = source.Photo,
        Phone      = source.Phone
    };
    
    public static void CopyFrom(this User target, User source)
    {
        target.FullName   = source.FullName;
        target.Email      = source.Email;
        target.Weight     = source.Weight;
        target.WeightUnit = source.WeightUnit;
        target.Height     = source.Height;
        target.HeightUnit = source.HeightUnit;
        target.Age        = source.Age;
        target.Photo      = source.Photo;
        target.Phone      = source.Phone;
    }
    
    
}