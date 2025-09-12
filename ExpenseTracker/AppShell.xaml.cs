namespace ExpenseTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Set initial navigation style based on device
        ConfigureNavigationForDevice();
    }

    private void ConfigureNavigationForDevice()
    {
        // Use flyout navigation for tablets and desktop, bottom tabs for phones
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            // For phones, flyout might be preferred for more screen space
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
        else
        {
            // For tablets and desktop, flyout navigation provides better experience
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
        
        // Automatically close flyout when navigating
        if (FlyoutBehavior == FlyoutBehavior.Flyout && FlyoutIsPresented)
        {
            FlyoutIsPresented = false;
        }
    }
}