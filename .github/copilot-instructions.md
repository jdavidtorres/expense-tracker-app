# Expense Tracker - AI Coding Guidelines

## Architecture Overview
Cross-platform .NET MAUI application for tracking subscriptions and invoices with native XAML UI and MVVM architecture.

**Key Components:**
- `ExpenseService` - Central service for API communication to `localhost:8083` using HttpClient
- XAML pages with MVVM pattern using CommunityToolkit.Mvvm
- Data models: `Expense`, `Subscription`, `Invoice` with inheritance
- Dashboard with expense summaries and category breakdowns  
- Native platform integration through .NET MAUI Shell navigation

## Core Patterns

### XAML Page Structure with MVVM
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:ExpenseTracker.Maui.ViewModels"
             x:Class="ExpenseTracker.Maui.Views.SubscriptionsPage"
             Title="Subscriptions">
    
    <ContentPage.BindingContext>
        <vm:SubscriptionsViewModel />
    </ContentPage.BindingContext>

    <Grid>
        <ActivityIndicator IsVisible="{Binding IsLoading}" IsRunning="{Binding IsLoading}" />
        
        <CollectionView ItemsSource="{Binding Subscriptions}" IsVisible="{Binding IsNotLoading}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Grid Padding="16">
                        <Label Text="{Binding Name}" FontSize="16" FontAttributes="Bold" />
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
</ContentPage>
```

### ViewModel with CommunityToolkit.Mvvm
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Shared.Models;
using ExpenseTracker.Shared.Services;
using System.Collections.ObjectModel;

namespace ExpenseTracker.Maui.ViewModels;

public partial class SubscriptionsViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string? errorMessage;

    public SubscriptionsViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            var data = await _expenseService.GetSubscriptionsAsync();
            Subscriptions = new ObservableCollection<Subscription>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load subscriptions: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSubscriptionAsync(string id)
    {
        // Implementation
    }
}
```

### Service Registration (MauiProgram.cs)
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Configure HttpClient for API calls
        builder.Services.AddHttpClient<ExpenseService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8083/api/");
        });

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Views.DashboardPage>();
        builder.Services.AddTransient<Views.SubscriptionsPage>();
        builder.Services.AddTransient<Views.InvoicesPage>();

        // Register ViewModels
        builder.Services.AddTransient<ViewModels.DashboardViewModel>();
        builder.Services.AddTransient<ViewModels.SubscriptionsViewModel>();
        builder.Services.AddTransient<ViewModels.InvoicesViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

### BaseViewModel Pattern
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.Maui.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? title;

    public bool IsNotLoading => !IsLoading;
}
```

### Shell Navigation Configuration
```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:ExpenseTracker.Maui.Views"
       x:Class="ExpenseTracker.Maui.AppShell"
       Title="Expense Tracker">

    <TabBar>
        <ShellContent Title="Dashboard" 
                      ContentTemplate="{DataTemplate local:DashboardPage}"
                      Route="dashboard"
                      Icon="home.png" />

        <ShellContent Title="Subscriptions"
                      ContentTemplate="{DataTemplate local:SubscriptionsPage}"
                      Route="subscriptions" 
                      Icon="credit_card.png" />

        <ShellContent Title="Invoices"
                      ContentTemplate="{DataTemplate local:InvoicesPage}"
                      Route="invoices"
                      Icon="file_text.png" />
    </TabBar>
</Shell>
```

### Platform-Specific Code
```csharp
#if ANDROID
    // Android-specific implementation
#elif IOS
    // iOS-specific implementation
#elif WINDOWS
    // Windows-specific implementation
#elif MACCATALYST
    // macOS-specific implementation
#endif
```

## Best Practices & Standards

### C# and .NET Standards
- Use nullable reference types and null-conditional operators
- Implement proper async/await patterns
- Use record types for immutable data models
- Follow SOLID principles and dependency injection

### MVVM Best Practices
- Use CommunityToolkit.Mvvm source generators for ObservableProperty and RelayCommand
- Implement BaseViewModel for common properties like IsLoading
- Use ObservableCollection for collections that update the UI
- Prefer dependency injection for ViewModels

```csharp
// Proper ViewModel lifecycle with commands
public partial class SubscriptionsViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSubscriptionsAsync();
    }

    [RelayCommand]
    private async Task AddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }
}
```

### MAUI Best Practices
- Use platform-specific folder structure for custom implementations
- Implement proper navigation using Shell or traditional navigation
- Handle app lifecycle events appropriately
- Use secure storage for sensitive data

```csharp
// Secure storage example
await SecureStorage.SetAsync("api_token", token);
var token = await SecureStorage.GetAsync("api_token");
```

### Code Quality
- Use dependency injection for services
- Implement proper logging with ILogger
- Handle exceptions gracefully with user-friendly messages
- Use CancellationToken for long-running operations

### Security & Performance
- Validate inputs on both client and server
- Use HTTPS for all API communications
- Implement proper error boundaries
- Cache data appropriately to reduce API calls

## Development Workflow

### Building the Apps
```bash
# Build MAUI app for all platforms
dotnet build -f net8.0-android
dotnet build -f net8.0-ios
dotnet build -f net8.0-windows
dotnet build -f net8.0-maccatalyst

# Run in development
dotnet run --project ExpenseTracker.Maui
```

### Key Files to Reference
- `Shared/Models/Expense.cs` - Data models and interfaces
- `Shared/Services/ExpenseService.cs` - API service implementation
- `Views/SubscriptionsPage.xaml` - XAML page example
- `ViewModels/SubscriptionsViewModel.cs` - MVVM ViewModel example
- `ViewModels/BaseViewModel.cs` - Base ViewModel pattern

## Specific Conventions

### XAML UI Components
- Native MAUI controls (Button, Label, Entry, CollectionView, etc.)
- Grid and StackLayout for responsive layouts
- ActivityIndicator for loading states
- Frame and Border for visual grouping
- Shell navigation for page transitions

### Data Management
- All API calls through ExpenseService with HttpClient
- Use async/await for all data operations
- ObservableCollection for bindable collections
- Proper error handling with try-catch patterns
- Update ViewModel properties to trigger UI updates

### Forms and Validation
- Use Entry controls with data binding
- Implement validation in ViewModels or through DataAnnotations
- Use RelayCommand for form submission
- Reset form state after successful operations
- Support both create and edit scenarios

```xml
<StackLayout Padding="16">
    <Entry Text="{Binding SubscriptionName}" 
           Placeholder="Subscription Name" />
    
    <Entry Text="{Binding Amount, StringFormat='{0:C}'}" 
           Placeholder="Amount"
           Keyboard="Numeric" />
    
    <Button Text="Save" 
            Command="{Binding SaveCommand}" 
            IsEnabled="{Binding IsNotLoading}" />
</StackLayout>
```

### Navigation
- Use Shell.Current.GoToAsync() for programmatic navigation
- Define routes in AppShell.xaml
- Pass parameters using query strings or navigation parameters
- Handle back button behavior through Shell navigation

```csharp
// Navigate with parameters
await Shell.Current.GoToAsync($"edit-subscription?id={subscription.Id}");

// Navigate and pass complex objects
var parameters = new Dictionary<string, object>
{
    ["subscription"] = subscription
};
await Shell.Current.GoToAsync("edit-subscription", parameters);
```

## Common Patterns

### Loading and Error States
```xml
<Grid>
    <!-- Loading indicator -->
    <ActivityIndicator IsVisible="{Binding IsLoading}" 
                       IsRunning="{Binding IsLoading}" 
                       VerticalOptions="Center" />
    
    <!-- Error message -->
    <Frame IsVisible="{Binding HasError}" 
           BackgroundColor="Red" 
           Padding="16" 
           Margin="16">
        <Label Text="{Binding ErrorMessage}" 
               TextColor="White" />
    </Frame>
    
    <!-- Content -->
    <ScrollView IsVisible="{Binding IsNotLoading}">
        <!-- Your content here -->
    </ScrollView>
</Grid>
```

### CRUD Operations in ViewModel
```csharp
[RelayCommand]
private async Task SaveSubscriptionAsync()
{
    try
    {
        IsLoading = true;
        ErrorMessage = null;

        if (IsNewSubscription)
        {
            await _expenseService.CreateSubscriptionAsync(CurrentSubscription);
        }
        else
        {
            await _expenseService.UpdateSubscriptionAsync(CurrentSubscription);
        }

        await LoadSubscriptionsAsync();
        await Shell.Current.GoToAsync("..");
    }
    catch (Exception ex)
    {
        ErrorMessage = $"Failed to save subscription: {ex.Message}";
    }
    finally
    {
        IsLoading = false;
    }
}

[RelayCommand]
private async Task DeleteSubscriptionAsync(Subscription subscription)
{
    var result = await Application.Current.MainPage.DisplayAlert(
        "Confirm Delete", 
        $"Are you sure you want to delete {subscription.Name}?", 
        "Yes", "No");
        
    if (result)
    {
        try
        {
            await _expenseService.DeleteSubscriptionAsync(subscription.Id);
            Subscriptions.Remove(subscription);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete subscription: {ex.Message}";
        }
    }
}
```

## Dependencies
- **.NET 8.0** - Base framework
- **.NET MAUI** - Cross-platform native app development
- **CommunityToolkit.Mvvm** - MVVM source generators and base classes
- **System.Text.Json** - JSON serialization
- **Microsoft.Extensions.Http** - HttpClient dependency injection
- **Microsoft.Extensions.Logging** - Logging infrastructure

## Backend Integration
- REST API at `http://localhost:8083/api`
- Endpoints: `/subscriptions`, `/invoices`, `/summary/monthly`, `/summary/yearly`
- JSON serialization with camelCase naming policy
- Proper HTTP status code handling
- File upload support for invoice attachments through multipart/form-data

## Platform-Specific Considerations

### Android
- Network security configuration for localhost development
- File provider for sharing attachments
- Permission handling for storage access

### iOS
- Info.plist configuration for HTTP connections
- File sharing capabilities
- Keychain integration for secure storage

### Windows
- Package.appxmanifest configuration
- File associations for invoice files
- Windows Hello integration possibilities

### Web
- No longer applicable - pure MAUI application with native UI
