using CommunityToolkit.Maui;
#if IOS
using fitness_app.Handlers;
#endif
using fitness_app.ViewModels;
using fitness_app.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Handlers.Items;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Utilities;

namespace fitness_app;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMPowerKitNavigation(mpowerBuilder =>
            {
                mpowerBuilder.ConfigureServices(s =>
                    {
                        s.RegisterForNavigation<MainPage, MainViewModel>();
                        s.RegisterForNavigation<OnboardingPage, OnboardingViewModel>();
                    })
                    .OnAppStart("NavigationPage/MainPage");
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Bebas-Regular.ttf", "BebasRegular");
                fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
                fonts.AddFont("DMSans_18pt-Regular.ttf", "DMSans_18ptRegular");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if IOS
                handlers.AddHandler(typeof(CarouselView), typeof(CarouselHandler));
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}