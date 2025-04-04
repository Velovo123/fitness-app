using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.Services;
using fitness_app.ViewModels.Base;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;

[AddINotifyPropertyChangedInterface]

public class MainViewModel : BaseViewModel, IInitializeAware
{
    private readonly IFlyoutService _flyoutService;
    public ObservableCollection<ToggleButtonItem> Items { get; }
    public ObservableCollection<CategoryItem> CategoryItems { get; }
    public User? User { get; set; }
    public ICommand OpenFlyoutCommand { get; set; }
    public ICommand CloseFlyoutCommand { get; set; }
    public ICommand ItemTappedCommand { get; }
    public ICommand CategoryCommand { get; set; }

    public MainViewModel(IFlyoutService flyoutService)
    {
        _flyoutService = flyoutService;
        OpenFlyoutCommand = CreateCommand(OpenFlyout);
        CloseFlyoutCommand = CreateCommand(CloseFlyout);
        CategoryCommand = CreateCommand<CategoryItem>(ChangeCategoryAsync);

        Items = InitializeToggleButtonItems();
        CategoryItems = InitializeCategoryItems();

        ItemTappedCommand = new Command<ToggleButtonItem>(OnItemTapped);
    }

    private ObservableCollection<ToggleButtonItem> InitializeToggleButtonItems()
    {
        return new ObservableCollection<ToggleButtonItem>
        {
            new ToggleButtonItem { Text = "Lose Weight" },
            new ToggleButtonItem { Text = "Gain Weight" },
            new ToggleButtonItem { Text = "Body Building" },
            new ToggleButtonItem { Text = "Healthy Lifestyle" }
        };
    }

    private ObservableCollection<CategoryItem> InitializeCategoryItems()
    {
        return new ObservableCollection<CategoryItem>
        {
            new CategoryItem { Title="Yoga", ImageSource=ImageNameConstants.YogaCategory, IsSelected=false },
            new CategoryItem { Title="Gym", ImageSource=ImageNameConstants.GymCategory, IsSelected=false },
            new CategoryItem { Title="Cardio", ImageSource=ImageNameConstants.CardioCategory, IsSelected=false },
            new CategoryItem { Title="Stretch", ImageSource=ImageNameConstants.StretchCategory, IsSelected=false },
            new CategoryItem { Title="Full Body", ImageSource=ImageNameConstants.FullbodyCategory, IsSelected=false },
        };
    }

    
    public void Initialize(INavigationParameters parameters)
    {
        if (parameters.ContainsKey("user"))
        {
            User = parameters.GetValue<User>("user");
        }
    }

    private void OpenFlyout()
    {
        _flyoutService.OpenFlyout();
    }
    
    private void CloseFlyout()
    {
        _flyoutService.CloseFlyout();
    }
    
    private void OnItemTapped(ToggleButtonItem tappedItem)
    {
        if (tappedItem.IsSelected)
        {
            tappedItem.IsSelected = false;
        }
        else
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
            tappedItem.IsSelected = true;
        }
    }
    
    private void ChangeCategoryAsync(CategoryItem category)
    {
        foreach (var item in CategoryItems)
            item.IsSelected = false;
        if(category != null)
            category.IsSelected = true;
    }
}