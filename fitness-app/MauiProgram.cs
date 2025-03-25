using CommunityToolkit.Maui;
using fitness_app.Constants;
#if IOS
using fitness_app.Handlers;
#endif
using fitness_app.Services;
#if IOS
#endif
using fitness_app.ViewModels;
using fitness_app.Views;
using fitness_app.Views.Welcome;
using MauiContentButton;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Utilities;
using Supabase.Gotrue;

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
            .UseMPowerKitNavigation(mpowerBuilder =>
            {
                mpowerBuilder.ConfigureServices(s =>
                    {
                        s.RegisterForNavigation<WelcomeMainPage, WelcomeMainViewModel>();
                        s.RegisterForNavigation<OnboardingPage, OnboardingViewModel>();
                        s.RegisterForNavigation<LoginPage, LoginViewModel>();
                        s.RegisterForNavigation<SelectFavoritePage, SelectFavoriteViewModel>();
                        s.RegisterForNavigation<SignupPage, SignupViewModel>();
                        s.RegisterForNavigation<ForgotPage, ForgotViewModel>();
                        s.RegisterForNavigation<VerifyAccountPage, VerifyAccountViewModel>();
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
                fonts.AddFont("DMSans-Medium.ttf", "DMSansMedium");
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
        builder.Services.AddSingleton(supabaseAuthClient);
        builder.Services.AddSingleton(supabaseDbClient);

        builder.Services.AddTransient<IUserFitnessDataService, UserFitnessDataService>();
        #if IOS
        builder.Services.AddTransient<IAuthService,AuthService_iOS>();
        #elif ANDROID
        builder.Services.AddTransient<IAuthService,AuthService_Android>();
#endif
        builder.Services.AddTransient<IDialogService, DialogService>();
        
        return builder.Build();
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