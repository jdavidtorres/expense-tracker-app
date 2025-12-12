using ExpenseTracker.Constants;
using ExpenseTracker.Views;

namespace ExpenseTracker
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			// Register routes for navigation using constants
			Routing.RegisterRoute(NavigationRoutes.AddSubscription, typeof(SubscriptionFormPage));
			Routing.RegisterRoute(NavigationRoutes.EditSubscription, typeof(SubscriptionFormPage));
			Routing.RegisterRoute(NavigationRoutes.AddInvoice, typeof(InvoiceFormPage));
			Routing.RegisterRoute(NavigationRoutes.EditInvoice, typeof(InvoiceFormPage));
		}
	}
}