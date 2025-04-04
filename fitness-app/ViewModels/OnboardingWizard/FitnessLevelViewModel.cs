using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Models;
using fitness_app.Models.Supabase;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels.OnboardingWizard;

[AddINotifyPropertyChangedInterface]
public class FitnessLevelViewModel : OnboardingWizardBaseViewModel
{
    public string TitleText { get; set; } = AppResources.FitnessLevelTitle;
    
    public ObservableCollection<ToggleButtonItem> Items { get; } =
        new ObservableCollection<ToggleButtonItem>
        {
            new ToggleButtonItem { Text = AppResources.BeginnerText },
            new ToggleButtonItem { Text = AppResources.IntermediateText },
            new ToggleButtonItem { Text = AppResources.AdvancedText }
        };
    
    public string? SelectedText { get; set; }
    
    public ICommand ItemTappedCommand { get; }
    public ICommand MoveNextCommand { get; set; }
    
    public FitnessLevelViewModel(INavigationService navigationService, IDialogService dialogService, IUserFitnessDataService userFitnessDataService) : base(navigationService, dialogService, userFitnessDataService)
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
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.LevelRequired);
            return;
        }

        await _userFitnessDataService.UpdateUserFitnessDataAsync(Session, new UserFitnessData 
        { 
            FitnessLevel = SelectedText
        });
    
        await NavigateToNextPageAsync(animated: true);
    }
}