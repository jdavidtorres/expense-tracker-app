using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class DebtFormPage : ContentPage
{
	public DebtFormPage(DebtFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
