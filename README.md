# Expense Tracker App

A cross-platform .NET MAUI application for tracking subscriptions and invoices with native XAML interface and MVVM architecture.

## Features

- **Dashboard**: View monthly and yearly expense summaries with category breakdowns
- **🎮 Gamification System**: Level up, earn achievements, and build streaks while tracking expenses
- **Subscriptions Management**: Add, edit, delete, and manage recurring subscriptions
- **Invoices Management**: Track one-time invoices with status management
- **Cross-Platform**: Runs on Android, iOS, macOS, and Windows
- **MVVM Architecture**: Clean separation using CommunityToolkit.Mvvm
- **REST API Integration**: Connects to backend API at localhost:8083

### 🎯 Gamification Features
- **Level & XP System**: Progress through levels by earning experience points
- **12 Achievements**: Unlock badges for tracking milestones and streaks
- **Daily Streaks**: Build momentum with consecutive daily tracking
- **Point Rewards**: Earn points for every action (10 pts per expense tracked)
- **Visual Progress**: See your stats with colorful cards and progress bars
- **Motivational Messages**: Get encouraging feedback based on your progress

See [GAMIFICATION.md](GAMIFICATION.md) for complete details about the gamification system.

## 📱 Download & Installation

### Pre-built Releases
Download the latest pre-built applications from our [Releases](https://github.com/jdavidtorres/expense-tracker-app/releases) page:

- **🤖 Android**: Download the APK file for Android devices (API 24+)
- **🪟 Windows**: Download the ZIP package for Windows 10/11 (Build 19041+)
- **🍎 iOS/macOS**: Build from source (requires Xcode and Apple Developer account)

### Installation Instructions

#### Android
1. Download the `ExpenseTracker-Android-*.apk` file from the latest release
2. Enable "Install from unknown sources" in your Android settings
3. Install the APK file

#### Windows
1. Download the `ExpenseTracker-Windows-*.zip` file from the latest release
2. Extract the ZIP file to your desired location
3. Run `ExpenseTracker.exe` to start the application

#### iOS/macOS
iOS and macOS apps require building from source due to Apple's code signing requirements:
```bash
# Build for iOS
dotnet build ExpenseTracker/ExpenseTracker.csproj -f net9.0-ios --configuration Release

# Build for macCatalyst
dotnet build ExpenseTracker/ExpenseTracker.csproj -f net9.0-maccatalyst --configuration Release
```

## 🏗️ Architecture

### Technology Stack
- **.NET 9.0**: Latest framework with enhanced performance and features
- **.NET MAUI**: Cross-platform native application framework with XAML UI
- **CommunityToolkit.Mvvm**: Modern MVVM implementation with source generators
- **Native XAML Controls**: Platform-optimized UI components (Grid, StackLayout, CollectionView, etc.)
- **Shell Navigation**: Modern navigation patterns with tab navigation
- **HttpClient**: REST API communication with dependency injection

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

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with MAUI workload
- [.NET MAUI workload](https://docs.microsoft.com/dotnet/maui/)

### Installation
```bash
# Clone the repository
git clone https://github.com/jdavidtorres/expense-tracker-app.git
cd expense-tracker-app

# Install MAUI workloads
dotnet workload install maui

# Restore dependencies
dotnet restore

# Build the solution
dotnet build
```

### Running the Application

```bash
# Windows (WinUI)
dotnet run --project ExpenseTracker --framework net9.0-windows10.0.19041.0

# Android (requires Android SDK)
dotnet run --project ExpenseTracker --framework net9.0-android

# iOS (requires Xcode on macOS)
dotnet run --project ExpenseTracker --framework net9.0-ios

# macOS (Mac Catalyst)
dotnet run --project ExpenseTracker --framework net9.0-maccatalyst
```

## 📱 Platform Support

| Platform | Framework | Minimum Version | Status |
|----------|-----------|----------------|---------|
| Windows | WinUI 3 | Windows 10 1809+ | ✅ Supported |
| Android | Android API | API Level 24+ | ✅ Supported |
| iOS | UIKit | iOS 11.0+ | ✅ Supported |
| macOS | Mac Catalyst | macOS 13.1+ | ✅ Supported |

## 🎯 Core Features

### Modern MVVM Architecture
- **ViewModels**: Built with CommunityToolkit.Mvvm source generators
- **Data Binding**: Two-way binding between XAML views and ViewModels
- **Command Pattern**: RelayCommand for user interactions
- **Observable Properties**: Automatic UI updates with ObservableProperty
- **Dependency Injection**: Service registration in MauiProgram.cs

### Subscription Management
- **Billing Cycles**: Monthly, Quarterly, and Yearly options
- **Payment Tracking**: Automatic next billing date calculations
- **Category Organization**: Entertainment, Productivity, Health, Education
- **Status Monitoring**: Visual indicators for upcoming payments

### Dashboard Analytics
- **Expense Overview**: Total subscriptions and monthly spending summaries
- **Payment Alerts**: Upcoming payment notifications
- **Financial Insights**: Monthly and yearly spending analysis
- **Category Breakdown**: Spending distribution visualization

### Navigation
- **Shell Navigation**: Built-in MAUI Shell for seamless page transitions
- **Tab Navigation**: Bottom tab navigation with routes
- **Deep Linking**: Support for URI-based navigation

### Data Management
- **REST API Integration**: HTTP client service for backend communication at localhost:8083
- **Async Operations**: All data operations use async/await patterns
- **Error Handling**: Comprehensive exception handling with user-friendly messages
- **Loading States**: Visual indicators during data operations
- **Offline Resilience**: Graceful handling of network connectivity issues

## 🔧 Configuration

### API Configuration
Update the API base URL in MauiProgram.cs:

```csharp
// In MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Configure HttpClient for API calls
        builder.Services.AddHttpClient<ExpenseService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8083/api/");
        });

        return builder.Build();
    }
}
```

### MVVM Service Registration
Register ViewModels and services for dependency injection:

```csharp
// Register pages
builder.Services.AddTransient<Views.DashboardPage>();
builder.Services.AddTransient<Views.SubscriptionsPage>();
builder.Services.AddTransient<Views.InvoicesPage>();
builder.Services.AddTransient<Views.SubscriptionFormPage>();
builder.Services.AddTransient<Views.InvoiceFormPage>();

// Register ViewModels
builder.Services.AddTransient<ViewModels.DashboardViewModel>();
builder.Services.AddTransient<ViewModels.SubscriptionsViewModel>();
builder.Services.AddTransient<ViewModels.InvoicesViewModel>();
builder.Services.AddTransient<ViewModels.SubscriptionFormViewModel>();
builder.Services.AddTransient<ViewModels.InvoiceFormViewModel>();
```

## 🚀 Deployment

### Build Commands
```bash
# Android APK
dotnet publish ExpenseTracker -f net9.0-android -c Release

# iOS IPA (macOS required)
dotnet publish ExpenseTracker -f net9.0-ios -c Release

# Windows App
dotnet publish ExpenseTracker -f net9.0-windows10.0.19041.0 -c Release

# macOS App Bundle
dotnet publish ExpenseTracker -f net9.0-maccatalyst -c Release
```

## Development Notes

- Uses Shell navigation with dependency injection
- Form validation implemented in ViewModels
- Responsive design with native MAUI controls
- Error handling with user-friendly messages
- Loading states and activity indicators
- Clean architecture following MVVM patterns

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow [Microsoft's .NET coding conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use MVVM patterns with CommunityToolkit.Mvvm
- Implement native XAML controls and avoid platform-specific code
- Write ViewModels with proper async/await patterns
- Use dependency injection for services and ViewModels
- Follow Shell navigation patterns for page transitions
- Ensure cross-platform compatibility testing

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Migration History

This project was migrated from separate ExpenseTracker.Maui and ExpenseTracker.Shared projects into a single consolidated ExpenseTracker project with all functionality integrated.
