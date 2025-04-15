using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class ProfileViewModel : BaseViewModel, IInitializeAsyncAware
{
    private readonly INavigationService _navigationService;
    
    public User? User { get; set; }
    public ICommand BackCommand { get; set; }
    
    public ICommand EditCommand { get; set; }

    public ProfileViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        BackCommand = CreateAsyncCommand(NavigateBackCommand);
        EditCommand = CreateAsyncCommand(NavigateEditPage);
    }
    
    private async Task NavigateBackCommand()
    {
        var result = await _navigationService.NavigateAsync(
            $"/{nameof(MainFlyoutPage)}/{nameof(MainPage)}");
        if (!result.Success)
        {
            //log
        }
    }

    private async Task NavigateEditPage()
    {
        var navParams = new NavigationParameters
        {
            { NavigationParametersConstants.User, User! }
        };
        
        var result = await _navigationService.NavigateAsync(nameof(EditPage), navParams);

        if (!result.Success)
        {
            //log
        }
    }

    public Task InitializeAsync(INavigationParameters parameters)
    {
        if (parameters.ContainsKey(NavigationParametersConstants.User))
        {
            User = parameters.GetValue<User>(NavigationParametersConstants.User);
        }
        
        return Task.CompletedTask;
    }
}