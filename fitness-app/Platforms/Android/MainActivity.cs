using Android.App;
using Android.Content.PM;
using Android.OS;
using Color = Android.Graphics.Color;

namespace fitness_app;

[Activity(Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    LaunchMode = LaunchMode.SingleTop, 
    ScreenOrientation = ScreenOrientation.Portrait,  
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | 
                           ConfigChanges.UiMode | ConfigChanges.ScreenLayout | 
                           ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (SupportActionBar != null)
        {
            SupportActionBar.Elevation = 0;
            SupportActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Color.White)); 
        }
    }
}