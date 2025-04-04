using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fitness_app.Constants;
using fitness_app.Models;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using fitness_app.Views.Welcome;
using Microsoft.Maui.Controls;
using MPowerKit.Navigation.Interfaces;
using PropertyChanged;

namespace fitness_app.ViewModels;


[AddINotifyPropertyChangedInterface]
public class OnboardingViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ObservableCollection<OnboardingSlide> Slides { get; set; }
    public ICommand NextCommand { get; }
    public ICommand SkipCommand { get; }
    public ICommand SwipeLeftCommand { get; }
    public int CurrentIndex { get; set; }
    
    [DependsOn(nameof(CurrentIndex))]
    
    public Brush CurrentBackground => Slides[CurrentIndex].Background;

    public OnboardingViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        Slides = CreateSlides();
        
        NextCommand = CreateAsyncCommand(NextCommandHandler);
        SkipCommand = CreateAsyncCommand(SkipCommandHandlerAsync);
        SwipeLeftCommand = CreateAsyncCommand(SwipeLeftCommandHandlerAsync);
    }

    private async Task NextCommandHandler()
    {
        if (CurrentIndex < Slides.Count - 1)
        {
            CurrentIndex++;
        }
        else
        {
            await NavigateToLoginPageAsync();
        }
    }
    
    private async Task SwipeLeftCommandHandlerAsync()
    {
        if (CurrentIndex == Slides.Count - 1)
        {
            await NavigateToLoginPageAsync();
        }
    }

    private async Task SkipCommandHandlerAsync()
    {
        await NavigateToLoginPageAsync();
    }
    
    private async Task NavigateToLoginPageAsync()
    {
        var result = await  _navigationService.NavigateAsync($"../{nameof(LoginPage)}", animated: false);

        if (!result.Success)
        {
            Console.WriteLine(result);
        }
    }
    
    private static ObservableCollection<OnboardingSlide> CreateSlides()
    {
        return new ObservableCollection<OnboardingSlide>
        {
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome02,
                Background = (Brush)Application.Current!.Resources[BrushResourcesConstants.Page02BackgroundGradient],
            },
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome03,
                Background = (Brush)Application.Current.Resources[BrushResourcesConstants.Page03BackgroundGradient],
            },
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome04,
                Background = (Brush)Application.Current.Resources[BrushResourcesConstants.Page04BackgroundGradient],
            }
        };
    }
}