#if IOS
using CoreGraphics;
using fitness_app.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace fitness_app.FitnessHandlers;

public class CustomEntryHandler : EntryHandler
{
    public static IPropertyMapper<Entry, IEntryHandler> CustomMapper = new PropertyMapper<Entry, IEntryHandler>(EntryHandler.Mapper)
    {
        [nameof(BorderlessEntry)] = MapCustomProperties
    };
    
    public CustomEntryHandler() : base(CustomMapper)
    {
    }
    

    public static void MapCustomProperties(IEntryHandler handler, Entry entry)
    {
        if (handler.PlatformView is UITextField nativeTextField)
        {
            nativeTextField.LeftView = new UIView(new CGRect(0, 0, 0, nativeTextField.Frame.Height));
            nativeTextField.RightView = new UIView(new CGRect(0, 0, 0, nativeTextField.Frame.Height));
            nativeTextField.LeftViewMode = UITextFieldViewMode.Always;
            nativeTextField.RightViewMode = UITextFieldViewMode.Always;

            nativeTextField.BorderStyle = UITextBorderStyle.None;
            
            nativeTextField.TintColor = UIColor.Clear;

            entry.HeightRequest = 30;
            entry.MinimumWidthRequest = 40;
        }
    }
}

#endif
