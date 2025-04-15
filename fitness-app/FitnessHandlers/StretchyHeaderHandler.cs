#if IOS
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using fitness_app.Controls;
using UIKit;

namespace fitness_app.FitnessHandlers;

public class StretchyHeaderHandler : ViewHandler<StretchyHeader, StretchyHeaderView>
{
    public static IPropertyMapper<StretchyHeader, StretchyHeaderView> Mapper =
        new PropertyMapper<StretchyHeader, StretchyHeaderView>(ViewHandler.ViewMapper);

    public StretchyHeaderHandler() : base(Mapper)
    {
    }

    protected override StretchyHeaderView CreatePlatformView()
    {
        return new StretchyHeaderView();
    }
}
#endif