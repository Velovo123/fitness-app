using fitness_app.Services;

public class FlyoutService : IFlyoutService
{
    private FlyoutPage? _flyoutPage;

    public void SetFlyout(FlyoutPage flyoutPage)
    {
        _flyoutPage = flyoutPage;
    }

    public void OpenFlyout()
    {
        if (_flyoutPage != null)
        {
            _flyoutPage.IsPresented = true;
        }
    }

    public void CloseFlyout()
    {
        if (_flyoutPage != null)
        {
            _flyoutPage.IsPresented = false;
        }
    }
}