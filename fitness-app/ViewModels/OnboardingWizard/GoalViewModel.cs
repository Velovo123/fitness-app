using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.Models.Supabase;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels.OnboardingWizard;

[AddINotifyPropertyChangedInterface]
public class GoalViewModel : OnboardingWizardBaseViewModel
{
    public string TitleText { get; set; } = AppResources.GoalTitle;
    
    public string? SelectedText { get; set; }
    
    
    public ObservableCollection<ToggleButtonItem> Items { get; } =
        new ObservableCollection<ToggleButtonItem>
        {
            new ToggleButtonItem { Text = AppResources.WeightLossText, ImageSource = ImageNameConstants.WeightLoss},
            new ToggleButtonItem { Text = AppResources.GainMuscleText, ImageSource = ImageNameConstants.Muscle},
            new ToggleButtonItem { Text = AppResources.ImproveFitnessText, ImageSource = ImageNameConstants.Gantel}
        };
    
    public ICommand ItemTappedCommand { get; }
    
    public ICommand MoveNextCommand { get; set; }

    public GoalViewModel(INavigationService navigationService, IUserFitnessDataService userFitnessDataService, IDialogService dialogService) : base(navigationService, dialogService, userFitnessDataService)
    {
        ItemTappedCommand = CreateCommand<ToggleButtonItem>(ItemTapped);
        MoveNextCommand = CreateAsyncCommand(OnMoveNextAsync);
    }
    
    private void ItemTapped(ToggleButtonItem tappedItem)
    {
        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
            SelectedText = string.Empty;
        }
        else
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
            tappedItem.IsSelected = true;
            SelectedText = tappedItem.Text;
        }
    }
    
    private async Task OnMoveNextAsync()
    {
        if (string.IsNullOrEmpty(SelectedText))
        {
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.GoalRequired);
            return;
        }

        await _userFitnessDataService.UpdateUserFitnessDataAsync(Session, new UserFitnessData
        {
            Goal = SelectedText
        });

        await NavigateToNextPageAsync(animated: true);
    }
}