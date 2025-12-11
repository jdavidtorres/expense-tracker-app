using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

/// <summary>
/// Represents a one-time invoice expense
/// </summary>
public class Invoice : Expense
{
    /// <summary>
    /// Gets or sets the invoice number
    /// </summary>
    [Required]
    [JsonPropertyName("invoiceNumber")]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the invoice was issued
    /// </summary>
    [JsonPropertyName("issueDate")]
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date the invoice is due for payment
    /// </summary>
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the vendor or service provider name
    /// </summary>
    [JsonPropertyName("vendor")]
    public string? Vendor { get; set; }

    /// <summary>
    /// Gets or sets the current status of the invoice
    /// </summary>
    [JsonPropertyName("status")]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

    /// <summary>
    /// Gets or sets the file path to any attached documents
    /// </summary>
    [JsonPropertyName("attachmentPath")]
    public string? AttachmentPath { get; set; }
}

/// <summary>
/// Represents the payment status of an invoice
/// </summary>
public enum InvoiceStatus
{
    /// <summary>
    /// Invoice is pending payment
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Invoice has been paid
    /// </summary>
    Paid = 1,
    
    /// <summary>
    /// Invoice payment is overdue
    /// </summary>
    Overdue = 2,
    
    /// <summary>
    /// Invoice has been cancelled
    /// </summary>
    Cancelled = 3
}