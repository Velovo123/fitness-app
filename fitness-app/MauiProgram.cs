using CommunityToolkit.Maui;
using fitness_app.Constants;
#if IOS
using fitness_app.Controls;
using fitness_app.FitnessHandlers;
using fitness_app.Handlers;
#endif
using fitness_app.Services;
#if IOS
#endif
using fitness_app.ViewModels;
using fitness_app.ViewModels.OnboardingWizard;
using fitness_app.Views;
using fitness_app.Views.OnboardingWizard;
using fitness_app.Views.Welcome;
using MauiContentButton;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Utilities;
using Plugin.SegmentedControl.Maui;
using Supabase.Gotrue;
using Vapolia.WheelPickers;

namespace fitness_app;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        var supabaseAuthClient = CreateAuthClient();
        var supabaseDbClient = CreateDatabaseClient();
        
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .AddMauiContentButtonHandler()
            .UseSegmentedControl()
            .UseMPowerKitNavigation(mpowerBuilder =>
            {
                mpowerBuilder.ConfigureServices(s =>
                    {
                        s.RegisterCorePages();
                        s.RegisterOnboardingWizardPages();
                    })
                    .OnAppStart($"NavigationPage/{nameof(WelcomeMainPage)}");
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Bebas-Regular.ttf", "BebasRegular");
                fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
                fonts.AddFont("Montserrat-SemiBold.ttf", "MontserratSemiBold");
                fonts.AddFont("Montserrat-Bold.ttf", "MontserratBold");
                fonts.AddFont("DMSans_18pt-Regular.ttf", "DMSans_18ptRegular");
                fonts.AddFont("DMSans_18pt-Bold.ttf", "DMSans_18ptBold");
                fonts.AddFont("DMSans-Medium.ttf", "DMSansMedium");
                fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if IOS
                handlers.AddHandler(typeof(CarouselView), typeof(CarouselHandler));
                handlers.AddHandler(typeof(BorderlessEntry), typeof(CustomEntryHandler));
                handlers.AddHandler(typeof(PickerView), typeof(PickerViewHandler));
                handlers.AddHandler(typeof(SearchBar), typeof(BorderlessSearchBarHandler));
                
                //handlers.AddHandler(typeof(FlyoutPage), typeof(FlyoutHandler));
#endif
               
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton(supabaseAuthClient);
        builder.Services.AddSingleton(supabaseDbClient);
        builder.Services.AddTransient<IUserFitnessDataService, UserFitnessDataService>();
        #if IOS
        builder.Services.AddTransient<IAuthService,AuthService_iOS>();
        #elif ANDROID
        builder.Services.AddTransient<IAuthService,AuthService_Android>();
#endif
        builder.Services.AddTransient<IDialogService, DialogService>();

        builder.Services.AddSingleton<IFlyoutService, FlyoutService>();
        
        return builder.Build();
    }
    
    public static void RegisterCorePages(this IServiceCollection services)
    {
        services.RegisterForNavigation<WelcomeMainPage, WelcomeMainViewModel>();
        services.RegisterForNavigation<OnboardingPage, OnboardingViewModel>();
        services.RegisterForNavigation<LoginPage, LoginViewModel>();
        services.RegisterForNavigation<SignupPage, SignupViewModel>();
        services.RegisterForNavigation<ForgotPage, ForgotViewModel>();
        services.RegisterForNavigation<VerifyAccountPage, VerifyAccountViewModel>();
    }
    public static void RegisterOnboardingWizardPages(this IServiceCollection services)
    {
        services.RegisterForNavigation<WeightPage, WeightViewModel>();
        services.RegisterForNavigation<AgePage, AgeViewModel>();
        services.RegisterForNavigation<DesiredWeightPage, DesiredWeightViewModel>();
        services.RegisterForNavigation<FitnessLevelPage, FitnessLevelViewModel>();
        services.RegisterForNavigation<GoalPage, GoalViewModel>();
        services.RegisterForNavigation<SelectFavoritesPage, SelectFavoriteViewModel>();
        services.RegisterForNavigation<HeightPage, HeightViewModel>();
        services.RegisterForNavigation<MainFlyoutPage, MainViewModel>();
        services.RegisterForNavigation<MainPage,MainViewModel>();
    }
    
    
    private static Client CreateAuthClient()
    {
        var supabaseUrl = EnvConstants.SupabaseUrl;
        var supabaseKey = EnvConstants.SupabaseKey;

        var options = new ClientOptions
        {
            Url = $"{supabaseUrl}/auth/v1",
            Headers = new Dictionary<string, string>
            {
                { "apikey", supabaseKey }
            },
            AutoRefreshToken = true,
        };

        return new Client(options);
    }
    
    private static Supabase.Client CreateDatabaseClient()
    {
        var supabaseUrl = EnvConstants.SupabaseUrl;
        var supabaseKey = EnvConstants.SupabaseKey;

        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
            AutoRefreshToken = true,
            Schema = "public"
        };

        return new Supabase.Client(supabaseUrl, supabaseKey, options);
    }
}