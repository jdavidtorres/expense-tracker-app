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
			
			// New routes
			Routing.RegisterRoute(NavigationRoutes.AddDebt, typeof(DebtFormPage));
			Routing.RegisterRoute(NavigationRoutes.EditDebt, typeof(DebtFormPage));
			Routing.RegisterRoute(NavigationRoutes.AddIncome, typeof(IncomeFormPage));
			Routing.RegisterRoute(NavigationRoutes.EditIncome, typeof(IncomeFormPage));
			Routing.RegisterRoute(NavigationRoutes.AddSavingsGoal, typeof(SavingsGoalFormPage));
			Routing.RegisterRoute(NavigationRoutes.EditSavingsGoal, typeof(SavingsGoalFormPage));
		}
	}
}