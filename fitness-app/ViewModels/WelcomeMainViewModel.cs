using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using fitness_app.Views.Welcome;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class WelcomeMainViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ICommand NavigateCommand { get; }

    public WelcomeMainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        NavigateCommand = CreateAsyncCommand(NavigateAsync);
    }

    private async Task NavigateAsync()
    {
        await _navigationService.NavigateAsync(nameof(OnboardingPage));
    }
}