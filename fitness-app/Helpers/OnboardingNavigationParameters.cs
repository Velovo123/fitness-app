using MPowerKit;
using MPowerKit.Navigation.Interfaces;
using Supabase.Gotrue;

namespace fitness_app.Helpers;

public class OnboardingNavigationParameters
{
    public static int InitalPageCount;
    public Session Session { get; init; } = null!;
    public List<string> RemainingPages { get; init; } = new();
    
    public const string SessionKey = "Session";
    public const string RemainingPagesKey = "RemainingPages";
    

    public NavigationParameters ToNavigationParameters()
    {
        return new NavigationParameters
        {
            { SessionKey, Session },
            { RemainingPagesKey, RemainingPages }
        };
    }

    public static OnboardingNavigationParameters From(INavigationParameters parameters)
    {
        return new OnboardingNavigationParameters
        {
            Session = parameters.GetValue<Session>(SessionKey),
            RemainingPages = parameters.ContainsKey(RemainingPagesKey)
                ? parameters.GetValue<List<string>>(RemainingPagesKey)
                : new List<string>(),
        };
    } 
        
}