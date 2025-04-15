using fitness_app.Constants;
using Microsoft.Maui.Handlers;
#if IOS
using UIKit;
#elif ANDROID
using Android.Views;
#endif

namespace fitness_app.FitnessHandlers.HandlerExtensions;

public class MauiHandlerExtensions
{
    public static void ApplyButtonTextAlignmentHandler()
    {
        ButtonHandler.Mapper.AppendToMapping("TextAlignment", (handler, view) =>
        {
            if (view is Button b && b.ClassId == ClassIdConstants.TextAlignLeft)
            {
#if IOS
                if (handler.PlatformView is UIButton iOSButton)
                {
                    iOSButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                }
#elif ANDROID
                handler.PlatformView.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
#endif
            }
        });
    }
}