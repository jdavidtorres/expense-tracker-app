using ExpenseTracker.Maui.ViewModels;

namespace ExpenseTracker.Maui.Views;

public partial class InvoicesPage : ContentPage
{
    private readonly InvoicesViewModel _viewModel;

    public InvoicesPage(InvoicesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadInvoicesCommand.ExecuteAsync(null);
    }
}