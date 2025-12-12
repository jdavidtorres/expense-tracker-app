using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SavingsGoalFormPage : ContentPage
{
	public SavingsGoalFormPage(SavingsGoalFormViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
