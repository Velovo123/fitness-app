using System.Windows.Input;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit.Navigation.Interfaces;

namespace fitness_app.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ICommand NavigateCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        NavigateCommand = CreateAsyncCommand(NavigateAsync);
    }

    private async Task NavigateAsync()
    {
        await _navigationService.NavigateAsync(nameof(OnboardingPage));
    }
}