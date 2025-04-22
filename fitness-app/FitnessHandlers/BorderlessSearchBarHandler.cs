#if IOS
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace fitness_app.FitnessHandlers;

public class BorderlessSearchBarHandler : SearchBarHandler
{
    public new static PropertyMapper<ISearchBar, BorderlessSearchBarHandler> Mapper =
                new PropertyMapper<ISearchBar, BorderlessSearchBarHandler>(SearchBarHandler.Mapper)
                {
                    [nameof(ISearchBar.TextColor)] = MapIconColor
                };
    
    public BorderlessSearchBarHandler() : base(Mapper)
    {
    }

    protected override MauiSearchBar CreatePlatformView()
    {
        var nativeSearchBar = base.CreatePlatformView();

        nativeSearchBar.BackgroundImage = new UIImage();
        nativeSearchBar.BarTintColor = UIColor.Clear;
        nativeSearchBar.BackgroundColor = UIColor.Clear;

        var textField = nativeSearchBar.ValueForKey(new Foundation.NSString("searchField")) as UITextField;
        if (textField != null)
        {
            //textField.BackgroundColor = UIColor.Clear;
            //textField.BorderStyle = UITextBorderStyle.None;
        }

        return nativeSearchBar;
    }
    
    public void SetIconColor(UIColor value)
    {
#if IOS13_0_OR_GREATER
#pragma warning disable CA1416
        var textField = PlatformView.SearchTextField;
#pragma warning restore CA1416
        var leftView = textField.LeftView ?? throw new Exception();
        leftView.TintColor = value;
#endif
    }
    private UIColor GetTextColor() => VirtualView.TextColor.ToPlatform();
    
    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is BorderlessSearchBarHandler customHandler)
            customHandler.SetIconColor(customHandler.GetTextColor());
    }
}

#endif