namespace fitness_app.Services;

public interface IFlyoutService
{
    void SetFlyout(FlyoutPage flyoutPage);
    void OpenFlyout();
    void CloseFlyout();
}