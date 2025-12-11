using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

/// <summary>
/// Represents a summary of expenses for a specific month
/// </summary>
public class MonthlySummary
{
    /// <summary>
    /// Gets or sets the month (1-12)
    /// </summary>
    [JsonPropertyName("month")]
    public int Month { get; set; }

    /// <summary>
    /// Gets or sets the year
    /// </summary>
    [JsonPropertyName("year")]
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the total amount spent on subscriptions
    /// </summary>
    [JsonPropertyName("totalSubscriptions")]
    public decimal TotalSubscriptions { get; set; }

    /// <summary>
    /// Gets or sets the total amount spent on invoices
    /// </summary>
    [JsonPropertyName("totalInvoices")]
    public decimal TotalInvoices { get; set; }

    /// <summary>
    /// Gets the combined total of all expenses (subscriptions + invoices)
    /// </summary>
    [JsonPropertyName("totalExpenses")]
    public decimal TotalExpenses => TotalSubscriptions + TotalInvoices;

    /// <summary>
    /// Gets or sets the number of active subscriptions
    /// </summary>
    [JsonPropertyName("subscriptionCount")]
    public int SubscriptionCount { get; set; }

    /// <summary>
    /// Gets or sets the number of invoices
    /// </summary>
    [JsonPropertyName("invoiceCount")]
    public int InvoiceCount { get; set; }
}

/// <summary>
/// Represents a summary of expenses for a specific year
/// </summary>
public class YearlySummary
{
    /// <summary>
    /// Gets or sets the year
    /// </summary>
    [JsonPropertyName("year")]
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the total amount spent on subscriptions for the year
    /// </summary>
    [JsonPropertyName("totalSubscriptions")]
    public decimal TotalSubscriptions { get; set; }

    /// <summary>
    /// Gets or sets the total amount spent on invoices for the year
    /// </summary>
    [JsonPropertyName("totalInvoices")]
    public decimal TotalInvoices { get; set; }

    /// <summary>
    /// Gets the combined total of all expenses for the year (subscriptions + invoices)
    /// </summary>
    [JsonPropertyName("totalExpenses")]
    public decimal TotalExpenses => TotalSubscriptions + TotalInvoices;

    /// <summary>
    /// Gets or sets the monthly breakdown of expenses
    /// </summary>
    [JsonPropertyName("monthlyBreakdown")]
    public List<MonthlySummary> MonthlyBreakdown { get; set; } = new();
}

/// <summary>
/// Represents a summary of expenses grouped by category
/// </summary>
public class CategorySummary
{
    /// <summary>
    /// Gets or sets the category name
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount spent in this category
    /// </summary>
    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the number of expenses in this category
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the percentage of total expenses this category represents
    /// </summary>
    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }
}
