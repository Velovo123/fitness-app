using System.Windows.Input;
using fitness_app.Controls;
using fitness_app.Enums;
using fitness_app.Models.Supabase;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels.OnboardingWizard;

[AddINotifyPropertyChangedInterface]
public class WeightViewModel : OnboardingWizardBaseViewModel
{
    public string TitleText { get; set; } = AppResources.CurrentWeightTitle;
    public bool IsButtonInMiddle { get; set; } = true;
    public ICommand SegmentedControlChangedCommand { get; }
    public ICommand MoveNextCommand { get; set; }
    

    public WeightViewModel(IDialogService dialogService,IUserFitnessDataService userFitnessDataService, INavigationService navigationService) 
        : base(navigationService, dialogService, userFitnessDataService)
    {
        SegmentedControlChangedCommand = CreateCommand<int>(OnSegmentedControlChanged);
        MoveNextCommand = CreateAsyncCommand(OnMoveNextAsync);
    }
    
    
    private void OnSegmentedControlChanged(int index)
    {
        var unit = (WeightUnitEnum)index;
        SelectedUnit = unit.ToString();
       
    }
    
    private async Task OnMoveNextAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentStat) || !decimal.TryParse(CurrentStat, out decimal weight))
        {
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.WeightRequired);
            return;
        }

        await _userFitnessDataService.UpdateUserFitnessDataAsync(Session, new UserFitnessData 
        { 
            CurrentWeight = weight, 
            WeightUnit = SelectedUnit 
        });
    
        await NavigateToNextPageAsync(animated: true);
    }
}