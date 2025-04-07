#if IOS
using UIKit;

namespace fitness_app.Controls;

public class StretchyHeaderView : UIView
{
    public nfloat InitialHeight { get; set; } = 450;

    // You can override LayoutSubviews to do custom sizing
    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        // If you want to manually adjust the frame or do custom drawing, do it here
    }
}

#endif