using System.Text.RegularExpressions;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models.Supabase.Responses;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views.Welcome;
using MPowerKit;
using MPowerKit.Navigation.Interfaces;
using Newtonsoft.Json;
using PropertyChanged;
using Supabase.Gotrue.Exceptions;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class LoginViewModel : OnboardingBaseViewModel
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
    
    public LoginViewModel(INavigationService navigationService,IDialogService dialogService, IAuthService authService, IUserFitnessDataService userFitnessDataService)
        : base(userFitnessDataService, navigationService)
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
                await HandleUserOnboardingAsync(session);
            }
            else
            {
                await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.InvalidEmailOrPasswordMessage);
            }
        }
        catch (GotrueException ex)
        {
            await HandleGotrueExceptionAsync(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            // Log 
        }
    }

    private async Task HandleGotrueExceptionAsync(GotrueException ex)
    {
        var errorResponse = JsonConvert.DeserializeObject<SupabaseError>(ex.Message);
        if (errorResponse != null && errorResponse.ErrorCode.Equals(AuthErrorConstants.EmailNotConfirmed, StringComparison.OrdinalIgnoreCase))
        {
            await HandleEmailNotConfirmedAsync();
        }
        else
        {
            // log
        }
    }

    private async Task HandleEmailNotConfirmedAsync()
    {
        await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.EmailNotConfirmedMessage);
        await _authService.SendMagicLink(Email);
        await _navigationService.NavigateAsync(nameof(VerifyAccountPage), 
            new NavigationParameters { { NavigationParametersConstants.Email, Email } });
    }


    private async Task SignInWithGoogleAsync()
    {
        try
        {
            var session = await _authService.SignInWithGoogleAsync();

            if (session != null)
            {
                await HandleUserOnboardingAsync(session);
            }
        }
        catch (Exception)
        {
            // log
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