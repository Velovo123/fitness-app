using System.Windows.Input;
using fitness_app.Controls;
using fitness_app.Models.Supabase;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels.OnboardingWizard;

[AddINotifyPropertyChangedInterface]
public class AgeViewModel: OnboardingWizardBaseViewModel
{
    public string TitleText { get; set; } = AppResources.AgeTitle;
    public int? SelectedAge { get; set; }
    public List<object> Days { get; } = Enumerable.Range(1, 100).Cast<object>().ToList();
    public Command ItemSelectedCommand { get; }
    public ICommand MoveNextCommand { get; set; }
    public AgeViewModel(
        IUserFitnessDataService userFitnessDataService,
        IDialogService dialogService,
        INavigationService navigationService)  
        : base(navigationService, dialogService, userFitnessDataService)
    {
        ItemSelectedCommand = new Command<int>(OnItemSelected);
        MoveNextCommand = CreateAsyncCommand(OnMoveNextAsync);
    }
    
    private void OnItemSelected(int selectedindex)
    {
        var value = Days[selectedindex]?.ToString();
        
        if (int.TryParse(value, out var age))
        {
            SelectedAge = age;
        }
        
    }
    
    private async Task OnMoveNextAsync()
    {
        if (SelectedAge == null)
        {
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.AgeRequired);
            return;
        }

        await _userFitnessDataService.UpdateUserFitnessDataAsync(Session, new UserFitnessData
        {
            Age = SelectedAge.Value
        });

        await NavigateToNextPageAsync(animated: true);
    }
}