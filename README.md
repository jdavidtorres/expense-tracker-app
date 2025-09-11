# Expense Tracker - .NET MAUI Native Edition

[![CI/CD Pipeline](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/ci.yml/badge.svg)](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/ci.yml)
[![Security Scan](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/security.yml/badge.svg)](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/security.yml)
[![Latest Release](https://img.shields.io/github/v/release/jdavidtorres/expense-tracker-app?include_prereleases)](https://github.com/jdavidtorres/expense-tracker-app/releases)
[![License](https://img.shields.io/github/license/jdavidtorres/expense-tracker-app)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/download/dotnet/9.0)

A comprehensive cross-platform expense tracking application built with pure .NET MAUI and native XAML, providing native mobile and desktop experiences with modern MVVM architecture.

## 🚀 Features

- **Cross-Platform Native Apps**: Native apps for Android, iOS, Windows, and macOS with XAML UI
- **Modern MVVM Architecture**: Built with CommunityToolkit.Mvvm and data binding
- **Flyout Navigation**: Professional hamburger menu navigation following Microsoft design guidelines
- **Subscription Management**: Track recurring expenses with automated billing cycle calculations
- **Invoice Processing**: Upload and categorize one-time expenses and documents
- **Financial Dashboard**: Interactive summaries and analytics for expense insights
- **Real-time API Integration**: Cloud-based REST API communication for data synchronization
- **Native Performance**: Platform-optimized performance with native MAUI controls

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
3. Run `ExpenseTracker.Maui.exe` to start the application

#### iOS/macOS
iOS and macOS apps require building from source due to Apple's code signing requirements:
```bash
# Build for iOS
dotnet build ExpenseTracker.Maui/ExpenseTracker.Maui.csproj -f net9.0-ios --configuration Release

# Build for macCatalyst
dotnet build ExpenseTracker.Maui/ExpenseTracker.Maui.csproj -f net9.0-maccatalyst --configuration Release
```

## 🏗️ Architecture

### Technology Stack
- **.NET 9.0**: Latest framework with enhanced performance and features
- **.NET MAUI**: Cross-platform native application framework with XAML UI
- **CommunityToolkit.Mvvm**: Modern MVVM implementation with source generators
- **Native XAML Controls**: Platform-optimized UI components (Grid, StackLayout, CollectionView, etc.)
- **Shell Navigation**: Modern navigation patterns with flyout menu
- **HttpClient**: REST API communication with dependency injection

### Project Structure
```
ExpenseTracker/
├── ExpenseTracker.Shared/           # Shared business logic
│   ├── Models/                      # Data models and entities
│   │   ├── Expense.cs              # Base expense model
│   │   ├── Subscription.cs         # Recurring subscription model
│   │   ├── Invoice.cs              # One-time invoice model
│   │   └── Summary.cs              # Financial summary model
│   └── Services/                    # API communication services
│       └── ExpenseService.cs       # HTTP client service
├── ExpenseTracker.Maui/            # Native cross-platform app
│   ├── Views/                       # XAML pages
│   │   ├── DashboardPage.xaml      # Main dashboard view
│   │   ├── SubscriptionsPage.xaml  # Subscription management
│   │   └── InvoicesPage.xaml       # Invoice management
│   ├── ViewModels/                  # MVVM ViewModels
│   │   ├── BaseViewModel.cs        # Shared ViewModel base
│   │   ├── DashboardViewModel.cs   # Dashboard logic
│   │   ├── SubscriptionsViewModel.cs # Subscription logic
│   │   └── InvoicesViewModel.cs    # Invoice logic
│   ├── Platforms/                   # Platform-specific code
│   ├── Resources/                   # App icons, images, fonts
│   ├── AppShell.xaml               # Navigation structure
│   └── MauiProgram.cs              # App configuration and DI
└── ExpenseTracker.sln              # Visual Studio solution
```

## 🛠️ Development Setup

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

#### MAUI Native Applications
```bash
# Windows (WinUI)
dotnet run --project ExpenseTracker.Maui --framework net9.0-windows10.0.19041.0

# Android (requires Android SDK)
dotnet run --project ExpenseTracker.Maui --framework net9.0-android

# iOS (requires Xcode on macOS)
dotnet run --project ExpenseTracker.Maui --framework net9.0-ios

# macOS (Mac Catalyst)
dotnet run --project ExpenseTracker.Maui --framework net9.0-maccatalyst
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

### Native Navigation
- **Flyout Menu**: Modern hamburger navigation with Microsoft design patterns
- **Shell Navigation**: Built-in MAUI Shell for seamless page transitions
- **Tab Navigation**: Alternative bottom tab navigation available
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

// Register ViewModels
builder.Services.AddTransient<ViewModels.DashboardViewModel>();
builder.Services.AddTransient<ViewModels.SubscriptionsViewModel>();
builder.Services.AddTransient<ViewModels.InvoicesViewModel>();
```

## 🚀 Deployment

### Automated Releases (CD Pipeline)
The project includes automated Continuous Deployment that creates releases with pre-built applications:

- **🤖 Automatic Builds**: Every push to main triggers multi-platform builds
- **📦 Artifact Publishing**: APK and Windows apps are automatically packaged
- **🏷️ Version Tagging**: Automatic semantic versioning with build numbers
- **📋 Release Notes**: Auto-generated release documentation
- **⬇️ Easy Downloads**: Direct download links for all supported platforms

### Manual Build Commands
```bash
# Android APK
dotnet publish ExpenseTracker.Maui -f net9.0-android -c Release

# iOS IPA (macOS required)
dotnet publish ExpenseTracker.Maui -f net9.0-ios -c Release

# Windows App
dotnet publish ExpenseTracker.Maui -f net9.0-windows10.0.19041.0 -c Release

# macOS App Bundle
dotnet publish ExpenseTracker.Maui -f net9.0-maccatalyst -c Release
```

## 🔄 CI/CD Pipeline

The project includes a comprehensive GitHub Actions CI/CD pipeline with automated releases:

### Continuous Integration (CI)
- **Build Matrix**: Multi-platform builds (Linux, Windows, macOS)
- **Quality Gates**: Code formatting, static analysis, and security scanning
- **Test Execution**: Automated test suite with coverage reporting
- **Artifact Validation**: Ensures all platforms build successfully

### Continuous Deployment (CD)
- **Automated Releases**: Creates GitHub releases on main branch pushes
- **Version Management**: Semantic versioning with automatic tagging (v1.0.x)
- **Artifact Publishing**: 
  - Android APK files for direct installation
  - Windows application packages (ZIP)
  - Release assets with installation instructions
- **Release Documentation**: Auto-generated release notes with build information

### Security & Quality
- **CodeQL Analysis**: Advanced security vulnerability detection
- **Dependency Scanning**: Automated security advisory monitoring
- **Code Formatting**: Enforced code style consistency
- **Build Caching**: Optimized build performance across platforms

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

## 🙏 Acknowledgments

- [.NET MAUI Team](https://github.com/dotnet/maui) for the excellent cross-platform framework
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) for modern MVVM implementation
- [Microsoft Design](https://docs.microsoft.com/windows/apps/design/) for navigation and UI guidelines
- [.NET Foundation](https://dotnetfoundation.org/) for the open-source ecosystem

## 📞 Support

For support and questions:
- 🐛 Issues: [GitHub Issues](https://github.com/jdavidtorres/expense-tracker-app/issues)
- 📖 Documentation: [Wiki](https://github.com/jdavidtorres/expense-tracker-app/wiki)
- 💬 Discussions: [GitHub Discussions](https://github.com/jdavidtorres/expense-tracker-app/discussions)

## 💻 Technologies Used

- **.NET 9.0**: Base framework with latest C# features
- **.NET MAUI**: Cross-platform native app development
- **CommunityToolkit.Mvvm**: Modern MVVM with source generators
- **XAML**: Native UI markup for cross-platform layouts
- **Shell Navigation**: Modern navigation patterns with flyout menus
- **HttpClient**: REST API communication with dependency injection
- **System.Text.Json**: High-performance JSON serialization
- **Microsoft.Extensions.DependencyInjection**: Built-in IoC container

## 🏛️ Architecture Patterns

### MVVM Implementation
```csharp
// BaseViewModel with CommunityToolkit.Mvvm
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty] 
    private string? title;
}

// Page ViewModels with Commands
public partial class SubscriptionsViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        // Implementation with proper async patterns
    }
}
```

### XAML Data Binding
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="ExpenseTracker.Maui.Views.DashboardPage">
    
    <Grid>
        <ActivityIndicator IsVisible="{Binding IsLoading}" />
        
        <CollectionView ItemsSource="{Binding Subscriptions}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Grid>
                        <Label Text="{Binding Name}" />
                        <Label Text="{Binding Amount, StringFormat='{0:C}'}" />
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
</ContentPage>
```

### Navigation Structure
```xml
<!-- AppShell.xaml - Modern Flyout Navigation -->
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    
    <FlyoutHeader>
        <Grid BackgroundColor="#2E86AB">
            <Label Text="Expense Tracker" />
        </Grid>
    </FlyoutHeader>

    <FlyoutItem Title="Dashboard" Route="dashboard">
        <ShellContent ContentTemplate="{DataTemplate local:DashboardPage}" />
    </FlyoutItem>
    
    <FlyoutItem Title="Subscriptions" Route="subscriptions">
        <ShellContent ContentTemplate="{DataTemplate local:SubscriptionsPage}" />
    </FlyoutItem>
    
</Shell>
```
