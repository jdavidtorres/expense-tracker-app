using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class InvoicesPage : ContentPage
{
<<<<<<< HEAD
    public InvoicesPage(InvoicesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
=======
    private readonly InvoicesViewModel _viewModel;

    public InvoicesPage(InvoicesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
<<<<<<< HEAD

        if (BindingContext is InvoicesViewModel viewModel)
        {
            await viewModel.LoadInvoicesCommand.ExecuteAsync(null);
        }
    }
}
=======
        await _viewModel.LoadInvoicesCommand.ExecuteAsync(null);
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
