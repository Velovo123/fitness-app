using fitness_app.Models.Supabase;
using fitness_app.Views.OnboardingWizard;

namespace fitness_app.Helpers;

public static class FitnessDataPageMap
{
    public static readonly Dictionary<string, string> PropertyToPage = new()
    {
        { nameof(UserFitnessData.FavoriteActivity), nameof(SelectFavoritesPage) },
        { nameof(UserFitnessData.Age),  nameof(AgePage) },
        { nameof(UserFitnessData.CurrentWeight), nameof(WeightPage) },
        { nameof(UserFitnessData.DesiredWeight), nameof(DesiredWeightPage) },
        { nameof(UserFitnessData.Height), nameof(HeightPage) },
        { nameof(UserFitnessData.FitnessLevel), nameof(FitnessLevelPage) },
        { nameof(UserFitnessData.Goal), nameof(GoalPage) },
    };
    
    public static readonly List<string> OrderedProperties = new()
    {
        nameof(UserFitnessData.FavoriteActivity),
        nameof(UserFitnessData.Age),
        nameof(UserFitnessData.CurrentWeight),
        nameof(UserFitnessData.DesiredWeight),
        nameof(UserFitnessData.Height),
        nameof(UserFitnessData.FitnessLevel),
        nameof(UserFitnessData.Goal),
    };
}