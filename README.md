# Expense Tracker App

A cross-platform .NET MAUI application for tracking subscriptions and invoices with native XAML interface and MVVM architecture.

## Features

- **Dashboard**: View monthly and yearly expense summaries with category breakdowns
- **Subscriptions Management**: Add, edit, delete, and manage recurring subscriptions
- **Invoices Management**: Track one-time invoices with status management
- **Cross-Platform**: Runs on Android, iOS, macOS, and Windows
- **MVVM Architecture**: Clean separation using CommunityToolkit.Mvvm
- **REST API Integration**: Connects to backend API at localhost:8083

## Project Structure

```
ExpenseTracker/
├── Models/              # Data models (Expense, Subscription, Invoice, Summary)
├── Services/            # API communication (ExpenseService)
├── ViewModels/          # MVVM ViewModels with CommunityToolkit.Mvvm
├── Views/               # XAML pages and UI
├── Converters/          # Value converters for data binding
├── Resources/           # Images, fonts, styles
└── Platforms/           # Platform-specific code
```

## Key Components

### Models
- **Expense**: Base class for all expense types
- **Subscription**: Recurring expenses with billing cycles
- **Invoice**: One-time expenses with status tracking
- **Summary**: Monthly/yearly aggregated data

### Services
- **ExpenseService**: HttpClient-based REST API communication
- Dependency injection configured in MauiProgram.cs

### ViewModels
- **DashboardViewModel**: Summary data and navigation
- **SubscriptionsViewModel**: Subscription CRUD operations
- **InvoicesViewModel**: Invoice CRUD operations
- **SubscriptionFormViewModel**: Add/edit subscription forms
- **InvoiceFormViewModel**: Add/edit invoice forms
- **BaseViewModel**: Common properties (loading, errors)

### Views
- **DashboardPage**: Main dashboard with summaries
- **SubscriptionsPage**: List and manage subscriptions
- **InvoicesPage**: List and manage invoices
- **SubscriptionFormPage**: Add/edit subscription form
- **InvoiceFormPage**: Add/edit invoice form

## Backend API

The app expects a REST API running at `http://localhost:8083/api/` with the following endpoints:

- `GET/POST/PUT/DELETE /subscriptions`
- `GET/POST/PUT/DELETE /invoices`
- `GET /summary/monthly?year={year}&month={month}`
- `GET /summary/yearly?year={year}`
- `GET /summary/categories`

## Dependencies

- **.NET 9.0** - Base framework
- **.NET MAUI** - Cross-platform app framework
- **CommunityToolkit.Mvvm** - MVVM source generators
- **Microsoft.Extensions.Http** - HttpClient dependency injection
- **System.Text.Json** - JSON serialization

## Getting Started

1. Ensure you have .NET 9.0 and MAUI workload installed
2. Start your backend API server on localhost:8083
3. Build and run the application:

```bash
dotnet build
dotnet run --framework net9.0-windows10.0.19041.0  # For Windows
# or
dotnet run --framework net9.0-android              # For Android
```

## Development Notes

- Uses Shell navigation with dependency injection
- Form validation implemented in ViewModels
- Responsive design with native MAUI controls
- Error handling with user-friendly messages
- Loading states and activity indicators
- Clean architecture following MVVM patterns

## Migration History

This project was migrated from separate ExpenseTracker.Maui and ExpenseTracker.Shared projects into a single consolidated ExpenseTracker project with all functionality integrated.
