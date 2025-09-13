<<<<<<< HEAD
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
=======
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class Invoice : Expense
{
	[JsonPropertyName("type")]
	public string Type { get; set; } = "invoice";

	[Required(ErrorMessage = "Invoice number is required")]
	[StringLength(50, ErrorMessage = "Invoice number cannot be longer than 50 characters")]
	public string InvoiceNumber { get; set; } = string.Empty;

	[Required(ErrorMessage = "Payment status is required")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

	public string? AttachmentUrl { get; set; }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
