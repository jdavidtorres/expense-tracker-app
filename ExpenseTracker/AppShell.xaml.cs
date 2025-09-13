using ExpenseTracker.Views;

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
