using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]

public class ForgotViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;
    [OnChangedMethod(nameof(ValidateEmail))]
    public string Email { get; set; } = string.Empty;
    
    public ICommand ResetPasswordCommand { get; }
    public bool IsEmailValid { get; set; }

    public ForgotViewModel(IAuthService authService, IDialogService dialogService)
    {
        _authService = authService;
        _dialogService = dialogService;
        
        ResetPasswordCommand = CreateAsyncCommand(ResetPasswordAsync);
    }
    private void ValidateEmail()
    {
        IsEmailValid = !string.IsNullOrWhiteSpace(Email) && 
                       Regex.IsMatch(Email, RegexConstants.EmailPattern);
    }

    private async Task ResetPasswordAsync()
    {
        var result = await _authService.ResetPasswordForEmailAsync(Email);
        if (result)
        {
            await _dialogService.ShowMessageAsync(AppResources.ResetEmailSentTitle, AppResources.ResetEmailSentMessage);
        }
        else
        {
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.ResetEmailErrorMessage);
        }
    }
}