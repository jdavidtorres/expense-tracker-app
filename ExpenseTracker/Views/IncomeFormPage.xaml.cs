using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class IncomeFormPage : ContentPage
{
	public IncomeFormPage(IncomeFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
