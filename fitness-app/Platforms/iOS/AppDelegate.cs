using fitness_app.Constants;
using Foundation;
using Google.SignIn;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;

namespace fitness_app;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        UINavigationBar.Appearance.ShadowImage = new UIImage();
        return  MauiProgram.CreateMauiApp();
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        SignIn.SharedInstance.ClientId = EnvConstants.ClientId;
        
        return base.FinishedLaunching(application, launchOptions);
    }

    public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    {
        if (SignIn.SharedInstance.HandleUrl(url))
            return true;
    
        return base.OpenUrl(application, url, options);
    }
}