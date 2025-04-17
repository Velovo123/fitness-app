using fitness_app.Views;
using fitness_app.Views.Welcome;

namespace fitness_app.Constants;

public static class NavigationUriConstants
{
    public const string MainPageRoute = $"/{nameof(MainFlyoutPage)}/{nameof(MainPage)}";
    public const string WelcomeMainPageRoute = $"/{nameof(NavigationPage)}/{nameof(WelcomeMainPage)}";
    public const string ProfilePageRoute = $"{nameof(NavigationPage)}/{nameof(ProfilePage)}";
    public const string WorkoutPageRoute = $"/{nameof(NavigationPage)}/{nameof(WorkoutPage)}";
    public const string LoginPageRoute = $"../{nameof(LoginPage)}";
}