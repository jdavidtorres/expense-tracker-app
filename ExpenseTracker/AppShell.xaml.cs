<<<<<<< HEAD
﻿using ExpenseTracker.Views;

namespace ExpenseTracker
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			// Register routes for navigation
			Routing.RegisterRoute("add-subscription", typeof(SubscriptionFormPage));
			Routing.RegisterRoute("edit-subscription", typeof(SubscriptionFormPage));
			Routing.RegisterRoute("add-invoice", typeof(InvoiceFormPage));
			Routing.RegisterRoute("edit-invoice", typeof(InvoiceFormPage));
		}
	}
}
=======
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
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
