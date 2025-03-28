using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Helpers;
using fitness_app.Models;
using fitness_app.Models.Supabase;
using fitness_app.Resources.Localization;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels.OnboardingWizard;

[AddINotifyPropertyChangedInterface]
public class SelectFavoriteViewModel : OnboardingWizardBaseViewModel
{
    public string TitleText { get; set; } = AppResources.FavoriteTitle;
    
    public ObservableCollection<CategoryItem> Categories { get; } = new ObservableCollection<CategoryItem>();
    
    public ICommand CategoryCommand { get; set; }
    public ICommand MoveNextCommand { get; set; }
    
    public SelectFavoriteViewModel(IDialogService dialogService,IUserFitnessDataService userFitnessDataService,INavigationService navigationService) 
        : base(navigationService, dialogService, userFitnessDataService)
    {
        FillCategories();
        CategoryCommand = CreateCommand<CategoryItem>(ChangeCategoryAsync);
        MoveNextCommand = CreateAsyncCommand(OnMoveNextAsync);
    }


    private void ChangeCategoryAsync(CategoryItem category)
    {
        foreach (var item in Categories)
            item.IsSelected = false;
        if(category != null)
            category.IsSelected = true;
    }
    
    private async Task OnMoveNextAsync()
    {
        var selectedCategory = Categories.FirstOrDefault(c => c.IsSelected);
        
        if (selectedCategory != null)
        {
            await _userFitnessDataService.UpdateUserFitnessDataAsync(Session, new UserFitnessData {FavoriteActivity = selectedCategory.Title});
        }
        else
        {
            await _dialogService.ShowMessageAsync(AppResources.ErrorTitle, AppResources.SelectionRequired);
            return;
        }

        await NavigateToNextPageAsync(animated: true);
    }
    
    private void FillCategories()
    {
        Categories.Add(new CategoryItem { Title = "Running", ImageSource = ImageNameConstants.Running });
        Categories.Add(new CategoryItem { Title = "Walking", ImageSource = ImageNameConstants.Walking });
        Categories.Add(new CategoryItem { Title = "Meal Plan", ImageSource = ImageNameConstants.MealPlan });
        Categories.Add(new CategoryItem { Title = "Cycling", ImageSource = ImageNameConstants.Cycling });
        Categories.Add(new CategoryItem { Title = "Yoga", ImageSource = ImageNameConstants.Yoga });
        Categories.Add(new CategoryItem { Title = "Health", ImageSource = ImageNameConstants.Health });
    }
}