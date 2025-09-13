using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class InvoicesPage : ContentPage
{
    public InvoicesPage(InvoicesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InvoicesViewModel viewModel)
        {
            await viewModel.LoadInvoicesCommand.ExecuteAsync(null);
        }
    }
}
