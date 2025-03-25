using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models.Supabase.Responses;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using fitness_app.Views.Welcome;
using MPowerKit;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using Newtonsoft.Json;
using PropertyChanged;
using Supabase.Gotrue.Exceptions;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class LoginViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;
    
    [OnChangedMethod(nameof(ValidateEmail))]
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    public bool IsEmailValid { get; set; }
    public bool IsPasswordHidden { get; set; } = true;
    
    public ICommand NavigateToRegisterCommand { get; } 
    public ICommand NavigateToForgotCommand { get; } 
    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand SignInWithGoogleCommand { get; }
    
    public ICommand SignInCommand { get; }
    
    public LoginViewModel(INavigationService navigationService,IDialogService dialogService, IAuthService authService)
    {
        _navigationService = navigationService;
        _authService = authService;
        _dialogService = dialogService;
        NavigateToRegisterCommand = CreateAsyncCommand(NavigateToRegisterAsync);
        NavigateToForgotCommand = CreateAsyncCommand(NavigateToForgotAsync);
        TogglePasswordVisibilityCommand = CreateCommand(() => IsPasswordHidden = !IsPasswordHidden);
        SignInWithGoogleCommand = CreateAsyncCommand(SignInWithGoogleAsync);
        SignInCommand = CreateAsyncCommand(SignInAsync);
    }
    
    private async Task SignInAsync()
    {
        try
        {
            var session = await _authService.SignInAsync(Email, Password);
            if (session != null)
            {
                await _navigationService.NavigateAsync(nameof(SelectFavoritePage), new NavigationParameters { { NavigationParametersConstants.Session, session } });
            }
            else
            {
                await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.InvalidEmailOrPasswordMessage);
            }
        }
        catch (GotrueException ex)
        {
            var errorResponse = JsonConvert.DeserializeObject<SupabaseError>(ex.Message);
            if (errorResponse != null && errorResponse.ErrorCode.Equals(AuthErrorConstants.EmailNotConfirmed, StringComparison.OrdinalIgnoreCase))
            {
                await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.EmailNotConfirmedMessage);
                await _authService.SendMagicLink(Email);
                await _navigationService.NavigateAsync(nameof(VerifyAccountPage), new NavigationParameters { { NavigationParametersConstants.Email, Email } });
            }
        }
        catch (Exception ex)
        {
            // log
        }
    }

    private async Task SignInWithGoogleAsync()
    {
        try
        {
            var session = await _authService.SignInWithGoogleAsync();

            if (session != null)
            {
                await _navigationService.NavigateAsync(nameof(SelectFavoritePage), new NavigationParameters { { "session", session } });
            }
            else
            {
            }
        }
        catch (Exception)
        {
            // Handle exceptions (e.g., log error, display an alert).
        }
    }
    private async Task NavigateToRegisterAsync()
    {
        await _navigationService.NavigateAsync(nameof(SignupPage));
    }
    
    private async Task NavigateToForgotAsync()
    {
        await _navigationService.NavigateAsync(nameof(ForgotPage));
    }
    
    private void ValidateEmail()
    {
        IsEmailValid = !string.IsNullOrWhiteSpace(Email) && 
                       Regex.IsMatch(Email, RegexConstants.EmailPattern);
    }
    
}