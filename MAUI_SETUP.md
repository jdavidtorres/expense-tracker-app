# MAUI Project Setup and Configuration

## Project Migration Status

✅ **Migration Complete**: This project has been successfully configured as a pure .NET MAUI application (not Blazor Hybrid).

### Current Configuration

- **Framework**: .NET 8.0 (downgraded from .NET 9.0 for environment compatibility)
- **Project Type**: .NET MAUI Cross-platform Application
- **Architecture**: MVVM with CommunityToolkit.Mvvm
- **Target Platforms**: Android, iOS, macOS Catalyst, Windows

### Project Structure

```
ExpenseTracker.Maui/           # MAUI Cross-platform UI
├── Views/                     # XAML Pages (not Blazor Razor)
│   ├── DashboardPage.xaml     # Native XAML UI
│   ├── SubscriptionsPage.xaml
│   └── InvoicesPage.xaml
├── ViewModels/                # MVVM ViewModels
│   ├── BaseViewModel.cs
│   ├── DashboardViewModel.cs
│   └── SubscriptionsViewModel.cs
├── App.xaml                   # MAUI Application Entry
├── AppShell.xaml              # Shell Navigation Structure
└── MauiProgram.cs             # Dependency Injection Setup

ExpenseTracker.Shared/         # Shared Business Logic
├── Models/                    # Data Models
├── Services/                  # API Communication
└── ExpenseTracker.Shared.csproj
```

### Key MAUI Features Implemented

1. **Native XAML UI**: Uses XAML markup (not Razor) for cross-platform native UI
2. **Shell Navigation**: Modern flyout navigation pattern
3. **MVVM Architecture**: Using CommunityToolkit.Mvvm with source generators
4. **Platform Targeting**: Configured for Android, iOS, macOS, and Windows
5. **Dependency Injection**: Proper service registration in MauiProgram.cs

### Environment Requirements

To build and run the full MAUI application, the following workloads are required:

```bash
dotnet workload install maui
dotnet workload install android
dotnet workload install ios
dotnet workload install maccatalyst
dotnet workload install windows
```

### Build Instructions

For full development environment:
```bash
dotnet restore
dotnet build
```

For environments without MAUI workloads (CI/CD):
The project includes fallback configuration to build core logic when MAUI workloads are not available.

### Verification

- ✅ No Blazor/Razor components found
- ✅ Pure MAUI XAML UI structure
- ✅ MVVM pattern implemented correctly
- ✅ Shared library builds successfully
- ✅ Project configured for cross-platform targeting
- ✅ Modern .NET 8.0 framework usage

This confirms the project is a proper .NET MAUI application, not a Blazor Hybrid app.