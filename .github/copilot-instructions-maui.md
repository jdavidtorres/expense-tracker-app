# .NET MAUI Development Guidelines for Expense Tracker

## Framework & Architecture

### Technology Stack
- **.NET 9.0**: Latest framework with enhanced performance
- **.NET MAUI**: Cross-platform native application framework
- **CommunityToolkit.Mvvm**: Modern MVVM implementation with source generators
- **Native XAML Controls**: Platform-optimized UI components
- **Shell Navigation**: Modern navigation patterns with tab navigation
- **HttpClient**: REST API communication with dependency injection

### Target Platforms
- Android (API 21+)
- iOS (11.0+)
- macOS (Mac Catalyst 13.1+)
- Windows (10 Build 19041+)

## MVVM Architecture Best Practices

### ViewModels
- Use `[ObservableProperty]` from CommunityToolkit.Mvvm for bindable properties
- Use `[RelayCommand]` for commands
- Inherit from `BaseViewModel` for common properties (IsLoading, ErrorMessage, Title)
- Implement async patterns with proper error handling
- Use dependency injection for all services

**Example:**
```csharp
public partial class ExampleViewModel : BaseViewModel
{
    private readonly ExpenseService _service;

    [ObservableProperty]
    private ObservableCollection<Item> items = new();

    public ExampleViewModel(ExpenseService service)
    {
        _service = service;
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            var data = await _service.GetItemsAsync();
            Items = new ObservableCollection<Item>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

### XAML Views
- Use native MAUI controls (avoid platform-specific code in shared UI)
- Implement proper data binding with `{Binding}` syntax
- Use `x:DataType` for compiled bindings when possible
- Follow Material Design principles for Android, Human Interface Guidelines for iOS
- Use `Grid` and `StackLayout` for responsive layouts
- Implement `ActivityIndicator` for loading states
- Show error messages with `Frame` or `Label` components

**Example:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:ExpenseTracker.ViewModels"
             x:Class="ExpenseTracker.Views.ExamplePage"
             Title="{Binding Title}">

    <Grid>
        <!-- Loading indicator -->
        <ActivityIndicator IsVisible="{Binding IsLoading}"
                           IsRunning="{Binding IsLoading}"
                           VerticalOptions="Center"/>

        <!-- Error message -->
        <Frame IsVisible="{Binding HasError}"
               BackgroundColor="Red"
               Padding="16"
               Margin="16">
            <Label Text="{Binding ErrorMessage}"
                   TextColor="White"/>
        </Frame>

        <!-- Content -->
        <ScrollView IsVisible="{Binding IsNotLoading}">
            <!-- Your content here -->
        </ScrollView>
    </Grid>
</ContentPage>
```

## Service Registration

### MauiProgram.cs Pattern
Register all services, pages, and ViewModels in `MauiProgram.cs`:

```csharp
// Register services (use Singleton for app-wide state)
builder.Services.AddSingleton<GamificationService>();
builder.Services.AddTransient<ExpenseService>();

// Register pages
builder.Services.AddTransient<DashboardPage>();
builder.Services.AddTransient<SubscriptionsPage>();

// Register ViewModels
builder.Services.AddTransient<DashboardViewModel>();
builder.Services.AddTransient<SubscriptionsViewModel>();

// Configure HttpClient
builder.Services.AddHttpClient<ExpenseService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8083/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

## Navigation

### Shell Navigation
- Define routes in `AppShell.xaml` using `ShellContent` elements
- Use `Shell.Current.GoToAsync()` for programmatic navigation
- Pass parameters using query strings or navigation parameters
- Use tab bar for main navigation sections

**Example:**
```csharp
// Navigate to page
await Shell.Current.GoToAsync("page-route");

// Navigate with parameters
await Shell.Current.GoToAsync($"edit?id={itemId}");

// Navigate back
await Shell.Current.GoToAsync("..");
```

## Data Management

### API Communication
- Use `HttpClient` with dependency injection
- Implement async/await patterns for all API calls
- Use proper exception handling with user-friendly error messages
- Serialize/deserialize with `System.Text.Json`

**Example:**
```csharp
public async Task<List<Item>> GetItemsAsync()
{
    try
    {
        var response = await _httpClient.GetAsync("items");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Item>>(json, _jsonOptions) ?? new();
    }
    catch (HttpRequestException ex)
    {
        throw new Exception($"Failed to fetch items: {ex.Message}", ex);
    }
}
```

### Local Storage
- Use `SecureStorage` for sensitive data (tokens, credentials)
- Use `Preferences` for simple key-value settings
- Use JSON serialization for complex objects

**Example:**
```csharp
// Save to SecureStorage
await SecureStorage.SetAsync("profile", JsonSerializer.Serialize(profile));

// Load from SecureStorage
var json = await SecureStorage.GetAsync("profile");
if (!string.IsNullOrEmpty(json))
{
    profile = JsonSerializer.Deserialize<Profile>(json);
}
```

## UI/UX Best Practices

### Colors and Theming
- Define colors in `Resources/Styles/Colors.xaml`
- Use semantic color names (Primary, Secondary, Success, Warning, Error)
- Support both light and dark themes when possible

### Responsive Design
- Use `Grid` with star-sizing for flexible layouts
- Implement `CollectionView` for lists (not `ListView`)
- Use `RefreshView` for pull-to-refresh functionality
- Handle different screen sizes with adaptive layouts

### Performance
- Use compiled bindings with `x:DataType` when possible
- Avoid heavy operations on UI thread
- Implement virtualization with `CollectionView`
- Use `Task.Run` for CPU-intensive operations

## Code Style

### Naming Conventions
- **Classes/Methods**: PascalCase (e.g., `LoadDataAsync`)
- **Private Fields**: _camelCase with underscore (e.g., `_expenseService`)
- **Properties**: PascalCase (e.g., `CurrentStreak`)
- **Parameters/Local Variables**: camelCase (e.g., `itemId`)
- **XAML Elements**: PascalCase with descriptive names

### Async/Await
- Always use `async`/`await` for I/O operations
- Suffix async methods with `Async`
- Use `ConfigureAwait(false)` in library code (not needed in MAUI apps)
- Handle exceptions in async methods

### Error Handling
- Use try-catch blocks for all API calls
- Display user-friendly error messages
- Log errors for debugging (use `ILogger` in debug mode)
- Gracefully handle network failures

## Testing & Quality

### Build Verification
- Build for all target platforms before committing
- Fix all compilation errors (warnings are acceptable if justified)
- Test on at least one physical device or emulator

### Code Review
- Use automated code review tools
- Fix critical issues (security, crashes, data loss)
- Address performance concerns
- Verify accessibility support

## Security Best Practices

### Data Protection
- Never hardcode secrets or API keys in code
- Use SecureStorage for sensitive information
- Validate all user inputs
- Sanitize data before displaying in UI

### API Communication
- Always use HTTPS in production
- Implement proper authentication/authorization
- Handle token expiration gracefully
- Protect against injection attacks

## Common Patterns

### Loading States
```csharp
[ObservableProperty]
private bool isLoading;

public bool IsNotLoading => !IsLoading;
```

### Error Handling
```csharp
[ObservableProperty]
private string? errorMessage;

public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

protected void ClearError() => ErrorMessage = null;
```

### Page Lifecycle
```csharp
protected override async void OnAppearing()
{
    base.OnAppearing();
    if (BindingContext is MyViewModel viewModel)
    {
        await viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
```

## Project Structure

```
ExpenseTracker/
├── Models/              # Data models and DTOs
├── Services/            # Business logic and API clients
├── ViewModels/          # MVVM ViewModels
├── Views/               # XAML pages and UI
├── Converters/          # Value converters for data binding
├── Resources/           # Images, fonts, styles
│   ├── Fonts/
│   ├── Images/
│   ├── Styles/
│   └── AppIcon/
├── Platforms/           # Platform-specific code
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   └── Windows/
└── MauiProgram.cs       # App configuration and DI setup
```

## Gamification-Specific Guidelines

### Purpose
This is a **gamified user interface** for productivity and habit-building, NOT a game. All features should:
- Encourage consistent expense tracking behavior
- Provide meaningful progress feedback
- Support financial goal achievement
- Maintain professional and productivity-focused messaging

### Terminology
- Use "progress" instead of "score"
- Use "milestones" instead of "achievements" when context is professional
- Emphasize tracking habits and financial awareness
- Avoid game-centric language like "playing" or "winning"

### Messaging Guidelines
- Focus on financial responsibility and awareness
- Highlight tracking consistency and goal progress
- Use encouraging but professional tone
- Connect gamification elements to real financial benefits

**Good Examples:**
- "✅ 7 days of consistent tracking!"
- "📊 You're making great progress toward your goals"
- "💰 On track with your budget this month"

**Avoid:**
- "You're winning the expense game!"
- "High score achieved!"
- "Beat your friends!"

## Resources

- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/)
- [XAML Controls](https://docs.microsoft.com/dotnet/maui/user-interface/controls/)
- [Shell Navigation](https://docs.microsoft.com/dotnet/maui/fundamentals/shell/)
