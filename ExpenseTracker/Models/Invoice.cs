using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class Invoice : Expense
{
    [Required]
    [JsonPropertyName("invoiceNumber")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [JsonPropertyName("issueDate")]
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("vendor")]
    public string? Vendor { get; set; }

    [JsonPropertyName("status")]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

    [JsonPropertyName("attachmentPath")]
    public string? AttachmentPath { get; set; }
}

public enum InvoiceStatus
{
    Pending = 0,
    Paid = 1,
    Overdue = 2,
    Cancelled = 3
}