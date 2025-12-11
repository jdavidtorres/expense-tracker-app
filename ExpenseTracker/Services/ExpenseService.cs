using System.Text;
using System.Text.Json;
using ExpenseTracker.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Services;

public class ExpenseService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExpenseService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExpenseService(HttpClient httpClient, ILogger<ExpenseService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    /// <summary>
    /// Helper method to perform GET requests and deserialize response
    /// </summary>
    private async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize response from {endpoint}");
    }

    /// <summary>
    /// Helper method to perform POST requests with JSON content
    /// </summary>
    private async Task<T> PostAsync<T>(string endpoint, object content, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(content, _jsonOptions);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, httpContent, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize response from {endpoint}");
    }

    /// <summary>
    /// Helper method to perform PUT requests with JSON content
    /// </summary>
    private async Task<T> PutAsync<T>(string endpoint, object content, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(content, _jsonOptions);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(endpoint, httpContent, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize response from {endpoint}");
    }

    /// <summary>
    /// Helper method to perform DELETE requests
    /// </summary>
    private async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    // Subscription methods
    /// <summary>
    /// Gets all subscriptions
    /// </summary>
    public async Task<List<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync<List<Subscription>>("subscriptions", cancellationToken).ConfigureAwait(false) 
                ?? new List<Subscription>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscriptions");
            throw;
        }
    }

    /// <summary>
    /// Gets a specific subscription by ID
    /// </summary>
    public async Task<Subscription> GetSubscriptionAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        try
        {
            return await GetAsync<Subscription>($"subscriptions/{id}", cancellationToken).ConfigureAwait(false);
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
    public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(subscription);

        try
        {
            return await PostAsync<Subscription>("subscriptions", subscription, cancellationToken).ConfigureAwait(false);
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
    public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        ArgumentException.ThrowIfNullOrWhiteSpace(subscription.Id);

        try
        {
            return await PutAsync<Subscription>($"subscriptions/{subscription.Id}", subscription, cancellationToken).ConfigureAwait(false);
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
    public async Task DeleteSubscriptionAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        try
        {
            await DeleteAsync($"subscriptions/{id}", cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription {Id}", id);
            throw;
        }
    }

    // Invoice methods
    /// <summary>
    /// Gets all invoices
    /// </summary>
    public async Task<List<Invoice>> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync<List<Invoice>>("invoices", cancellationToken).ConfigureAwait(false)
                ?? new List<Invoice>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching invoices");
            throw;
        }
    }

    /// <summary>
    /// Gets a specific invoice by ID
    /// </summary>
    public async Task<Invoice> GetInvoiceAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        try
        {
            return await GetAsync<Invoice>($"invoices/{id}", cancellationToken).ConfigureAwait(false);
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
    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        try
        {
            return await PostAsync<Invoice>("invoices", invoice, cancellationToken).ConfigureAwait(false);
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
    public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentException.ThrowIfNullOrWhiteSpace(invoice.Id);

        try
        {
            return await PutAsync<Invoice>($"invoices/{invoice.Id}", invoice, cancellationToken).ConfigureAwait(false);
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
    public async Task DeleteInvoiceAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        try
        {
            await DeleteAsync($"invoices/{id}", cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice {Id}", id);
            throw;
        }
    }

    // Summary methods
    /// <summary>
    /// Gets monthly summary for a specific year and month
    /// </summary>
    public async Task<MonthlySummary> GetMonthlySummaryAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync<MonthlySummary>($"summary/monthly?year={year}&month={month}", cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching monthly summary for {Year}-{Month}", year, month);
            throw;
        }
    }

    /// <summary>
    /// Gets yearly summary for a specific year
    /// </summary>
    public async Task<YearlySummary> GetYearlySummaryAsync(int year, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync<YearlySummary>($"summary/yearly?year={year}", cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching yearly summary for {Year}", year);
            throw;
        }
    }

    /// <summary>
    /// Gets category summary
    /// </summary>
    public async Task<List<CategorySummary>> GetCategorySummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync<List<CategorySummary>>("summary/categories", cancellationToken).ConfigureAwait(false)
                ?? new List<CategorySummary>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category summary");
            throw;
        }
    }
}