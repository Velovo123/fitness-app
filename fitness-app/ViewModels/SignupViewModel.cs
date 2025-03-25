using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views.Welcome;
using MPowerKit;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]

public class SignupViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    [OnChangedMethod(nameof(ValidateEmail))]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsEmailValid { get; set; }
    public bool IsPasswordHidden { get; set; } = true;
    
    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand NavigateBackCommand { get; } 
    public ICommand SignUpCommand { get; }

    public SignupViewModel(INavigationService navigationService,IDialogService dialogService, IAuthService authService)
    {
        _navigationService = navigationService;
        _authService = authService;
        _dialogService = dialogService;
        
        TogglePasswordVisibilityCommand = CreateCommand(() => IsPasswordHidden = !IsPasswordHidden);
        NavigateBackCommand = CreateAsyncCommand(NavigateBackAsync);
        SignUpCommand = CreateAsyncCommand(SignUpAsync);
    }
    
    private async Task NavigateBackAsync()
    {
        await _navigationService.GoBackAsync();
    }
    private void ValidateEmail()
    {
        IsEmailValid = !string.IsNullOrWhiteSpace(Email) && 
                       Regex.IsMatch(Email, RegexConstants.EmailPattern);
    }
    
    private async Task SignUpAsync()
    {
        try
        {
            var session = await _authService.SignUpAsync(Email, Password, FullName, Phone);
            if (session == null)
            {
                await _dialogService.ShowMessageAsync(AppResources.AccountExistsTitle, AppResources.AccountExistsMessage);
            }
            else
            {
               await _navigationService.NavigateAsync(nameof(VerifyAccountPage), new NavigationParameters{ { NavigationParametersConstants.Email, Email} });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
    
