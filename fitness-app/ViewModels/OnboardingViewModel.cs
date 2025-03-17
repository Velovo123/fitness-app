using System.Collections.ObjectModel;
using System.Windows.Input;
using fitness_app.Models;
using fitness_app.ViewModels.Base;
using fitness_app.Views;
using MPowerKit.Navigation.Interfaces;
using MPowerKit.Navigation.Utilities;
using PropertyChanged;

namespace fitness_app.ViewModels;


[AddINotifyPropertyChangedInterface]
public class OnboardingViewModel : BaseViewModel
{
    public ObservableCollection<OnboardingSlide> Slides { get; set; }
    public ICommand NextCommand { get; }
    public ICommand SkipCommand { get; }
    public ICommand SwipeLeftCommand { get; }
    public int CurrentIndex { get; set; }
    
    [DependsOn(nameof(CurrentIndex))]
    
    public Brush CurrentBackground => Slides[CurrentIndex].Background;

    public OnboardingViewModel()
    {
        Slides = CreateSlides();
        
        NextCommand = CreateCommand(NextCommandHandler);
        SkipCommand = CreateCommand(SkipCommandHandler);
        SwipeLeftCommand = CreateCommand(SwipeLeftCommandHandler);
    }

    private void NextCommandHandler()
    {
        if (CurrentIndex < Slides.Count - 1)
        {
            CurrentIndex++;
        }
        else
        {
            NavigateToLoginPage();
        }
    }
    
    private void SwipeLeftCommandHandler()
    {
        if (CurrentIndex == Slides.Count - 1)
        {
            NavigateToLoginPage();
        }
    }

    private static void SkipCommandHandler()
    {
        NavigateToLoginPage();
    }
    
    private static void NavigateToLoginPage()
    {
        Application.Current!.MainPage = new NavigationPage(new LoginPage());
    }
    
    private static ObservableCollection<OnboardingSlide> CreateSlides()
    {
        return new ObservableCollection<OnboardingSlide>
        {
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome02,
                Background = (Brush)Application.Current!.Resources["Page02BackgroundGradient"]
            },
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome03,
                Background = (Brush)Application.Current!.Resources["Page03BackgroundGradient"]
            },
            new OnboardingSlide
            {
                SlideType = OnboardingSlideType.Welcome04,
                Background = (Brush)Application.Current!.Resources["Page04BackgroundGradient"]
            }
        };
    }
}