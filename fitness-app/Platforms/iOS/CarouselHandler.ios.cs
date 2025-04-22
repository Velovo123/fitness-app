using Microsoft.Maui.Controls;

namespace fitness_app.Handlers;

public partial class CarouselHandler
{
    protected override void ScrollToRequested(object sender, ScrollToRequestEventArgs args)
    {
        Controller.CollectionView.ShowsHorizontalScrollIndicator = false;
        Controller.CollectionView.ShowsVerticalScrollIndicator = false;
        base.ScrollToRequested(sender, args);
    }
}