using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class InvoiceFormPage : ContentPage
{
    public InvoiceFormPage(InvoiceFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
