using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SubscriptionFormPage : ContentPage
{
    public SubscriptionFormPage(SubscriptionFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
