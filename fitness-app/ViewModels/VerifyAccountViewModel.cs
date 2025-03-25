using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]
public class VerifyAccountViewModel : BaseViewModel, IInitializeAware
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    public string Email { get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
    
    public ICommand PINEntryCompletedCommand { get; }
    public ICommand FocusPINViewCommand { get; }
    
    public void Initialize(INavigationParameters parameters)
    {
        Email = parameters.GetValue<string>(NavigationParametersConstants.Email);
    }

    public VerifyAccountViewModel(IAuthService authService, INavigationService navigationService,IDialogService dialogService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        PINEntryCompletedCommand = CreateAsyncCommand(PINEntryCompletedAsync);
        FocusPINViewCommand = CreateCommand<PINView.Maui.PINView>(FocusPINView);
    }

    private void FocusPINView(PINView.Maui.PINView pinView)
    {
        pinView.FocusBox();
    }
    private async Task PINEntryCompletedAsync()
    {
        bool verified = await _authService.VerifyUserOtpAsync(Email, OTP);
        if (verified)
        {
            await _navigationService.NavigateAsync(nameof(SelectFavoritePage));
        }
        else
        {
            await _dialogService.ShowMessageAsync(AppResources.VerificationFailedTitle, AppResources.VerificationFailedMessage);
            OTP = string.Empty;
        }
    }
}