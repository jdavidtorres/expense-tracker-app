using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class MonthlySummary
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("totalSubscriptions")]
    public decimal TotalSubscriptions { get; set; }

    [JsonPropertyName("totalInvoices")]
    public decimal TotalInvoices { get; set; }

    [JsonPropertyName("totalExpenses")]
    public decimal TotalExpenses => TotalSubscriptions + TotalInvoices;

    [JsonPropertyName("subscriptionCount")]
    public int SubscriptionCount { get; set; }

    [JsonPropertyName("invoiceCount")]
    public int InvoiceCount { get; set; }
}

public class YearlySummary
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("totalSubscriptions")]
    public decimal TotalSubscriptions { get; set; }

    [JsonPropertyName("totalInvoices")]
    public decimal TotalInvoices { get; set; }

    [JsonPropertyName("totalExpenses")]
    public decimal TotalExpenses => TotalSubscriptions + TotalInvoices;

    [JsonPropertyName("monthlyBreakdown")]
    public List<MonthlySummary> MonthlyBreakdown { get; set; } = new();
}

public class CategorySummary
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("percentage")]
    public decimal Percentage { get; set; }
}