#if IOS
using fitness_app.Controls;
using Microsoft.Maui.Handlers;
using UIKit;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace fitness_app.FitnessHandlers;

public class StretchyHeaderHandler : ViewHandler<StretchyHeader, StretchyHeaderView>
{
    public StretchyHeaderHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override StretchyHeaderView CreatePlatformView()
    {
        return new StretchyHeaderView();
    }
}
#endif