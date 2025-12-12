using System.Text;
using System.Text.Json;
using ExpenseTracker.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Services;

public class ExpenseService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;
	private readonly ILogger<ExpenseService> _logger;

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

	// Subscription methods
	public async Task<List<Subscription>> GetSubscriptionsAsync()
	{
		try
		{
			var response = await _httpClient.GetAsync("subscriptions");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<Subscription>>(json, _jsonOptions) ?? new List<Subscription>();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching subscriptions");
			throw;
		}
	}

	public async Task<Subscription> GetSubscriptionAsync(string id)
	{
		try
		{
			var response = await _httpClient.GetAsync($"subscriptions/{id}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Subscription>(json, _jsonOptions)
			       ?? throw new InvalidOperationException("Subscription not found");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching subscription {Id}", id);
			throw;
		}
	}

	public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
	{
		try
		{
			var json = JsonSerializer.Serialize(subscription, _jsonOptions);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("subscriptions", content);
			response.EnsureSuccessStatusCode();

			var responseJson = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Subscription>(responseJson, _jsonOptions)
			       ?? throw new InvalidOperationException("Failed to create subscription");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error creating subscription");
			throw;
		}
	}

	public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
	{
		try
		{
			var json = JsonSerializer.Serialize(subscription, _jsonOptions);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync($"subscriptions/{subscription.Id}", content);
			response.EnsureSuccessStatusCode();

			var responseJson = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Subscription>(responseJson, _jsonOptions)
			       ?? throw new InvalidOperationException("Failed to update subscription");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating subscription {Id}", subscription.Id);
			throw;
		}
	}

	public async Task DeleteSubscriptionAsync(string id)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"subscriptions/{id}");
			response.EnsureSuccessStatusCode();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting subscription {Id}", id);
			throw;
		}
	}

	// Invoice methods
	public async Task<List<Invoice>> GetInvoicesAsync()
	{
		try
		{
			var response = await _httpClient.GetAsync("invoices");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<Invoice>>(json, _jsonOptions) ?? new List<Invoice>();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching invoices");
			throw;
		}
	}

	public async Task<Invoice> GetInvoiceAsync(string id)
	{
		try
		{
			var response = await _httpClient.GetAsync($"invoices/{id}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Invoice>(json, _jsonOptions)
			       ?? throw new InvalidOperationException("Invoice not found");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching invoice {Id}", id);
			throw;
		}
	}

	public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
	{
		try
		{
			var json = JsonSerializer.Serialize(invoice, _jsonOptions);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("invoices", content);
			response.EnsureSuccessStatusCode();

			var responseJson = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Invoice>(responseJson, _jsonOptions)
			       ?? throw new InvalidOperationException("Failed to create invoice");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error creating invoice");
			throw;
		}
	}

	public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice)
	{
		try
		{
			var json = JsonSerializer.Serialize(invoice, _jsonOptions);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync($"invoices/{invoice.Id}", content);
			response.EnsureSuccessStatusCode();

			var responseJson = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Invoice>(responseJson, _jsonOptions)
			       ?? throw new InvalidOperationException("Failed to update invoice");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating invoice {Id}", invoice.Id);
			throw;
		}
	}

	public async Task DeleteInvoiceAsync(string id)
	{
		try
		{
			var response = await _httpClient.DeleteAsync($"invoices/{id}");
			response.EnsureSuccessStatusCode();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting invoice {Id}", id);
			throw;
		}
	}

	// Summary methods
	public async Task<MonthlySummary> GetMonthlySummaryAsync(int year, int month)
	{
		try
		{
			var response = await _httpClient.GetAsync($"summary/monthly?year={year}&month={month}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<MonthlySummary>(json, _jsonOptions)
			       ?? throw new InvalidOperationException("Monthly summary not found");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching monthly summary for {Year}-{Month}", year, month);
			throw;
		}
	}

	public async Task<YearlySummary> GetYearlySummaryAsync(int year)
	{
		try
		{
			var response = await _httpClient.GetAsync($"summary/yearly?year={year}");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<YearlySummary>(json, _jsonOptions)
			       ?? throw new InvalidOperationException("Yearly summary not found");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching yearly summary for {Year}", year);
			throw;
		}
	}

	public async Task<List<CategorySummary>> GetCategorySummaryAsync()
	{
		try
		{
			var response = await _httpClient.GetAsync("summary/categories");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<CategorySummary>>(json, _jsonOptions) ?? new List<CategorySummary>();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching category summary");
			throw;
		}
	}
}
