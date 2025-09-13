# Expense Tracker App - Main Prompt

I want to plan my expenses, so I need a cross-platform app to record subscriptions and invoices, only those types of expenses. I need to calculate monthly and annual balances. Use native .NET MAUI with XAML interface and MVVM architecture for mobile and desktop applications. Apply consistent design using native MAUI controls and global styles. Create a service class to connect with localhost:8083 (backend REST API) using HttpClient and service dependency injection. Use separate folders for XAML pages, ViewModels, Models, and Services. Apply CommunityToolkit.Mvvm for MVVM pattern and Shell navigation.

## Project Structure

ExpenseTracker/ (Unified .NET MAUI Project)
├── Models/                          # Data models and entities
│   ├── Expense.cs                  # Base expense model with enums
│   ├── Subscription.cs             # Recurring subscription model
│   ├── Invoice.cs                  # One-time invoice model
│   └── Summary.cs                  # Financial summary models
├── Services/                        # API communication services
│   └── ExpenseService.cs           # HTTP client service for API calls
├── ViewModels/                      # MVVM ViewModels
│   ├── BaseViewModel.cs            # Shared ViewModel base class
│   ├── DashboardViewModel.cs       # Dashboard logic
│   ├── SubscriptionsViewModel.cs   # Subscription management logic
│   └── InvoicesViewModel.cs        # Invoice management logic
├── Views/                           # XAML pages
│   ├── DashboardPage.xaml          # Main dashboard view
│   ├── SubscriptionsPage.xaml      # Subscription management page
│   └── InvoicesPage.xaml           # Invoice management page
├── Platforms/                       # Platform-specific code
├── Resources/                       # App icons, images, fonts, styles
├── AppShell.xaml                   # Navigation structure
├── App.xaml                        # Application configuration
├── MainPage.xaml                   # Welcome/landing page
└── MauiProgram.cs                  # App configuration and DI setup
