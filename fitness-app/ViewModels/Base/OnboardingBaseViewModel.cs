using fitness_app.Helpers;
using fitness_app.Services;
using fitness_app.Views;
using MPowerKit.Navigation.Interfaces;
using Supabase.Gotrue;

namespace fitness_app.ViewModels.Base;

public class OnboardingBaseViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IUserFitnessDataService _userFitnessDataService;

    public OnboardingBaseViewModel(IUserFitnessDataService userFitnessDataService, INavigationService navigationService)
    {
        _userFitnessDataService = userFitnessDataService;
        _navigationService = navigationService;
    }
    protected async Task HandleUserOnboardingAsync(Session session)
    {
        var missingProperties = await _userFitnessDataService.RetrieveMissingPropertyNamesAsync(session);

        var pagesToNavigate = FitnessDataPageMap.OrderedProperties
            .Where(missingProperties.Contains) 
            .Where(FitnessDataPageMap.PropertyToPage.ContainsKey)
            .Select(p => FitnessDataPageMap.PropertyToPage[p])
            .ToList();

        if (pagesToNavigate.Any())
        {
            await NavigateOnboardingPagesAsync(session, pagesToNavigate);
        }
        else
        {
            await _navigationService.NavigateAsync(nameof(MainPage));
        }
    }
    
    private async Task NavigateOnboardingPagesAsync(Session session, List<string> pagesToNavigate)
    {
        var onboardingParams = new OnboardingNavigationParameters
        {
            Session = session,
            RemainingPages = pagesToNavigate.Skip(1).ToList(),
        };

        var firstPage = pagesToNavigate.First();
        var result = await _navigationService.NavigateAsync(firstPage, onboardingParams.ToNavigationParameters());

        if (!result.Success)
        {
            //log
        }
    }
}