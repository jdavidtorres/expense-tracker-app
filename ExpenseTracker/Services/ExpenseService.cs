using System.Text;
using System.Text.Json;
using ExpenseTracker.Constants;
using ExpenseTracker.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Services;

/// <summary>
/// Service for managing expense-related API operations including subscriptions, invoices, and summaries
/// </summary>
public class ExpenseService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExpenseService> _logger;
    
    /// <summary>
    /// Shared JSON serializer options for consistent serialization
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public ExpenseService(HttpClient httpClient, ILogger<ExpenseService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Subscription Methods

    /// <summary>
    /// Retrieves all subscriptions from the API
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>List of subscriptions</returns>
    public async Task<List<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiEndpoints.Subscriptions, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<Subscription>>(json, JsonOptions) ?? new List<Subscription>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscriptions");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a specific subscription by ID
    /// </summary>
    /// <param name="id">Subscription ID</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The subscription if found</returns>
    /// <exception cref="InvalidOperationException">Thrown when subscription is not found</exception>
    public async Task<Subscription> GetSubscriptionAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Subscription ID cannot be null or empty", nameof(id));

        try
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.Subscriptions}/{id}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Subscription>(json, JsonOptions)
                ?? throw new InvalidOperationException($"Subscription with ID '{id}' not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscription {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Creates a new subscription
    /// </summary>
    /// <param name="subscription">The subscription to create</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The created subscription with server-assigned values</returns>
    public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));

        try
        {
            var json = JsonSerializer.Serialize(subscription, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Subscriptions, content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Subscription>(responseJson, JsonOptions)
                ?? throw new InvalidOperationException("Failed to create subscription");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing subscription
    /// </summary>
    /// <param name="subscription">The subscription to update</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The updated subscription</returns>
    public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        if (string.IsNullOrWhiteSpace(subscription.Id))
            throw new ArgumentException("Subscription ID cannot be null or empty", nameof(subscription));

        try
        {
            var json = JsonSerializer.Serialize(subscription, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{ApiEndpoints.Subscriptions}/{subscription.Id}", content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Subscription>(responseJson, JsonOptions)
                ?? throw new InvalidOperationException($"Failed to update subscription with ID '{subscription.Id}'");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", subscription.Id);
            throw;
        }
    }

    /// <summary>
    /// Deletes a subscription by ID
    /// </summary>
    /// <param name="id">Subscription ID</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    public async Task DeleteSubscriptionAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Subscription ID cannot be null or empty", nameof(id));

        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiEndpoints.Subscriptions}/{id}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription {Id}", id);
            throw;
        }
    }

    #endregion

    #region Invoice Methods

    /// <summary>
    /// Retrieves all invoices from the API
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>List of invoices</returns>
    public async Task<List<Invoice>> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiEndpoints.Invoices, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<Invoice>>(json, JsonOptions) ?? new List<Invoice>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching invoices");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a specific invoice by ID
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The invoice if found</returns>
    public async Task<Invoice> GetInvoiceAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Invoice ID cannot be null or empty", nameof(id));

        try
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.Invoices}/{id}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Invoice>(json, JsonOptions)
                ?? throw new InvalidOperationException($"Invoice with ID '{id}' not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching invoice {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Creates a new invoice
    /// </summary>
    /// <param name="invoice">The invoice to create</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The created invoice</returns>
    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice == null)
            throw new ArgumentNullException(nameof(invoice));

        try
        {
            var json = JsonSerializer.Serialize(invoice, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.Invoices, content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Invoice>(responseJson, JsonOptions)
                ?? throw new InvalidOperationException("Failed to create invoice");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing invoice
    /// </summary>
    /// <param name="invoice">The invoice to update</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The updated invoice</returns>
    public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice == null)
            throw new ArgumentNullException(nameof(invoice));
        if (string.IsNullOrWhiteSpace(invoice.Id))
            throw new ArgumentException("Invoice ID cannot be null or empty", nameof(invoice));

        try
        {
            var json = JsonSerializer.Serialize(invoice, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{ApiEndpoints.Invoices}/{invoice.Id}", content, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Invoice>(responseJson, JsonOptions)
                ?? throw new InvalidOperationException($"Failed to update invoice with ID '{invoice.Id}'");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice {Id}", invoice.Id);
            throw;
        }
    }

    /// <summary>
    /// Deletes an invoice by ID
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    public async Task DeleteInvoiceAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Invoice ID cannot be null or empty", nameof(id));

        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiEndpoints.Invoices}/{id}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice {Id}", id);
            throw;
        }
    }

    #endregion

    #region Summary Methods

    /// <summary>
    /// Retrieves monthly summary for a specific year and month
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month (1-12)</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Monthly summary</returns>
    public async Task<MonthlySummary> GetMonthlySummaryAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12");

        try
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.SummaryMonthly}?year={year}&month={month}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<MonthlySummary>(json, JsonOptions)
                ?? throw new InvalidOperationException($"Monthly summary not found for {year}-{month:D2}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching monthly summary for {Year}-{Month}", year, month);
            throw;
        }
    }

    /// <summary>
    /// Retrieves yearly summary for a specific year
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Yearly summary</returns>
    public async Task<YearlySummary> GetYearlySummaryAsync(int year, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoints.SummaryYearly}?year={year}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<YearlySummary>(json, JsonOptions)
                ?? throw new InvalidOperationException($"Yearly summary not found for {year}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching yearly summary for {Year}", year);
            throw;
        }
    }

    /// <summary>
    /// Retrieves category summary showing spending by category
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>List of category summaries</returns>
    public async Task<List<CategorySummary>> GetCategorySummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiEndpoints.SummaryCategories, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<CategorySummary>>(json, JsonOptions) ?? new List<CategorySummary>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category summary");
            throw;
        }
    }

    #endregion
}