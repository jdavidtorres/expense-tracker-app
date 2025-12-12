using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Services;

/// <summary>
/// Local SQLite-based expense service that replaces HTTP API calls
/// </summary>
public class LocalExpenseService
{
	private readonly DatabaseService _databaseService;
	private readonly ILogger<LocalExpenseService> _logger;

	public LocalExpenseService(DatabaseService databaseService, ILogger<LocalExpenseService> logger)
	{
		_databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	#region Subscription Methods

	/// <summary>
	/// Retrieves all subscriptions from the database
	/// </summary>
	public async Task<List<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var entities = await _databaseService.GetSubscriptionsAsync();
			return entities.Select(MapToSubscription).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching subscriptions from database");
			throw;
		}
	}

	/// <summary>
	/// Retrieves a specific subscription by ID
	/// </summary>
	public async Task<Subscription> GetSubscriptionAsync(string id, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Subscription ID cannot be null or empty", nameof(id));

		try
		{
			if (!int.TryParse(id, out var numericId))
				throw new ArgumentException("Invalid subscription ID format", nameof(id));

			var entity = await _databaseService.GetSubscriptionAsync(numericId);
			if (entity == null)
				throw new InvalidOperationException($"Subscription with ID '{id}' not found");

			return MapToSubscription(entity);
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
		if (subscription == null)
			throw new ArgumentNullException(nameof(subscription));

		try
		{
			var entity = MapToSubscriptionEntity(subscription);
			var id = await _databaseService.SaveSubscriptionAsync(entity);
			entity.Id = id;
			return MapToSubscription(entity);
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
		if (subscription == null)
			throw new ArgumentNullException(nameof(subscription));
		if (string.IsNullOrWhiteSpace(subscription.Id))
			throw new ArgumentException("Subscription ID cannot be null or empty", nameof(subscription));

		try
		{
			var entity = MapToSubscriptionEntity(subscription);
			await _databaseService.SaveSubscriptionAsync(entity);
			return MapToSubscription(entity);
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
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Subscription ID cannot be null or empty", nameof(id));

		try
		{
			if (!int.TryParse(id, out var numericId))
				throw new ArgumentException("Invalid subscription ID format", nameof(id));

			var entity = await _databaseService.GetSubscriptionAsync(numericId);
			if (entity != null)
			{
				await _databaseService.DeleteSubscriptionAsync(entity);
			}
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
	/// Retrieves all invoices from the database
	/// </summary>
	public async Task<List<Invoice>> GetInvoicesAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var entities = await _databaseService.GetInvoicesAsync();
			return entities.Select(MapToInvoice).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching invoices from database");
			throw;
		}
	}

	/// <summary>
	/// Retrieves a specific invoice by ID
	/// </summary>
	public async Task<Invoice> GetInvoiceAsync(string id, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Invoice ID cannot be null or empty", nameof(id));

		try
		{
			if (!int.TryParse(id, out var numericId))
				throw new ArgumentException("Invalid invoice ID format", nameof(id));

			var entity = await _databaseService.GetInvoiceAsync(numericId);
			if (entity == null)
				throw new InvalidOperationException($"Invoice with ID '{id}' not found");

			return MapToInvoice(entity);
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
		if (invoice == null)
			throw new ArgumentNullException(nameof(invoice));

		try
		{
			var entity = MapToInvoiceEntity(invoice);
			var id = await _databaseService.SaveInvoiceAsync(entity);
			entity.Id = id;
			return MapToInvoice(entity);
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
		if (invoice == null)
			throw new ArgumentNullException(nameof(invoice));
		if (string.IsNullOrWhiteSpace(invoice.Id))
			throw new ArgumentException("Invoice ID cannot be null or empty", nameof(invoice));

		try
		{
			var entity = MapToInvoiceEntity(invoice);
			await _databaseService.SaveInvoiceAsync(entity);
			return MapToInvoice(entity);
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
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Invoice ID cannot be null or empty", nameof(id));

		try
		{
			if (!int.TryParse(id, out var numericId))
				throw new ArgumentException("Invalid invoice ID format", nameof(id));

			var entity = await _databaseService.GetInvoiceAsync(numericId);
			if (entity != null)
			{
				await _databaseService.DeleteInvoiceAsync(entity);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting invoice {Id}", id);
			throw;
		}
	}

	#endregion

	#region Debt Methods

	public async Task<List<Debt>> GetDebtsAsync(CancellationToken cancellationToken = default)
	{
		var entities = await _databaseService.GetDebtsAsync();
		return entities.Select(MapToDebt).ToList();
	}

	public async Task<Debt> GetDebtAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetDebtAsync(numericId);
			return entity != null ? MapToDebt(entity) : throw new KeyNotFoundException($"Debt {id} not found");
		}
		throw new ArgumentException("Invalid ID");
	}

	public async Task<Debt> SaveDebtAsync(Debt debt, CancellationToken cancellationToken = default)
	{
		var entity = MapToDebtEntity(debt);
		var id = await _databaseService.SaveDebtAsync(entity);
		debt.Id = id.ToString();
		return debt;
	}

	public async Task DeleteDebtAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetDebtAsync(numericId);
			if (entity != null) await _databaseService.DeleteDebtAsync(entity);
		}
	}

	#endregion

	#region Income Methods

	public async Task<List<Income>> GetIncomesAsync(CancellationToken cancellationToken = default)
	{
		var entities = await _databaseService.GetIncomesAsync();
		return entities.Select(MapToIncome).ToList();
	}

	public async Task<Income> GetIncomeAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetIncomeAsync(numericId);
			return entity != null ? MapToIncome(entity) : throw new KeyNotFoundException($"Income {id} not found");
		}
		throw new ArgumentException("Invalid ID");
	}

	public async Task<Income> SaveIncomeAsync(Income income, CancellationToken cancellationToken = default)
	{
		var entity = MapToIncomeEntity(income);
		var id = await _databaseService.SaveIncomeAsync(entity);
		income.Id = id.ToString();
		return income;
	}

	public async Task DeleteIncomeAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetIncomeAsync(numericId);
			if (entity != null) await _databaseService.DeleteIncomeAsync(entity);
		}
	}

	#endregion

	#region Savings Goal Methods

	public async Task<List<SavingsGoal>> GetSavingsGoalsAsync(CancellationToken cancellationToken = default)
	{
		var entities = await _databaseService.GetSavingsGoalsAsync();
		return entities.Select(MapToSavingsGoal).ToList();
	}

	public async Task<SavingsGoal> GetSavingsGoalAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetSavingsGoalAsync(numericId);
			return entity != null ? MapToSavingsGoal(entity) : throw new KeyNotFoundException($"SavingsGoal {id} not found");
		}
		throw new ArgumentException("Invalid ID");
	}

	public async Task<SavingsGoal> SaveSavingsGoalAsync(SavingsGoal goal, CancellationToken cancellationToken = default)
	{
		var entity = MapToSavingsGoalEntity(goal);
		var id = await _databaseService.SaveSavingsGoalAsync(entity);
		goal.Id = id.ToString();
		return goal;
	}

	public async Task DeleteSavingsGoalAsync(string id, CancellationToken cancellationToken = default)
	{
		if (int.TryParse(id, out var numericId))
		{
			var entity = await _databaseService.GetSavingsGoalAsync(numericId);
			if (entity != null) await _databaseService.DeleteSavingsGoalAsync(entity);
		}
	}

	#endregion

	#region Summary Methods

	/// <summary>
	/// Retrieves monthly summary for a specific year and month
	/// </summary>
	public async Task<MonthlySummary> GetMonthlySummaryAsync(int year, int month, CancellationToken cancellationToken = default)
	{
		if (month < 1 || month > 12)
			throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12");

		try
		{
			var total = await _databaseService.GetMonthlyTotalAsync(year, month);
			var subscriptions = await GetSubscriptionsAsync(cancellationToken);
			var invoices = await GetInvoicesAsync(cancellationToken);
			
			return new MonthlySummary
			{
				Year = year,
				Month = month,
				TotalSubscriptions = total,
				TotalInvoices = 0,
				SubscriptionCount = subscriptions.Count,
				InvoiceCount = invoices.Count
			};
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
	public async Task<YearlySummary> GetYearlySummaryAsync(int year, CancellationToken cancellationToken = default)
	{
		try
		{
			var total = await _databaseService.GetYearlyTotalAsync(year);
			
			return new YearlySummary
			{
				Year = year,
				TotalSubscriptions = total,
				TotalInvoices = 0
			};
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
	public async Task<List<CategorySummary>> GetCategorySummaryAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var categoryTotals = await _databaseService.GetCategorySummaryAsync();
			
			return categoryTotals.Select(kvp => new CategorySummary
			{
				Category = kvp.Key,
				TotalAmount = kvp.Value
			}).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching category summary");
			throw;
		}
	}

	#endregion

	#region Mapping Methods

	private static Subscription MapToSubscription(SubscriptionEntity entity)
	{
		// Parse billing cycle from string
		if (!Enum.TryParse<BillingCycle>(entity.BillingCycle, true, out var billingCycle))
		{
			billingCycle = BillingCycle.Monthly;
		}

		return new Subscription
		{
			Id = entity.Id.ToString(),
			Name = entity.Name,
			Amount = entity.Amount,
			BillingCycle = billingCycle,
			NextBillingDate = entity.NextBillingDate,
			Category = entity.Category
		};
	}

	private static SubscriptionEntity MapToSubscriptionEntity(Subscription model)
	{
		var entity = new SubscriptionEntity
		{
			Name = model.Name,
			Amount = model.Amount,
			BillingCycle = model.BillingCycle.ToString(),
			NextBillingDate = model.NextBillingDate,
			Category = model.Category,
			Notes = model.Description
		};

		if (!string.IsNullOrEmpty(model.Id) && int.TryParse(model.Id, out var id))
		{
			entity.Id = id;
		}

		return entity;
	}

	private static Invoice MapToInvoice(InvoiceEntity entity)
	{
		// Parse invoice status from string
		if (!Enum.TryParse<InvoiceStatus>(entity.Status, true, out var status))
		{
			status = InvoiceStatus.Pending;
		}

		return new Invoice
		{
			Id = entity.Id.ToString(),
			Name = entity.Name,
			Amount = entity.Amount,
			DueDate = entity.DueDate,
			Status = status,
			Category = entity.Category,
			InvoiceNumber = entity.Id.ToString()
		};
	}

	private static InvoiceEntity MapToInvoiceEntity(Invoice model)
	{
		var entity = new InvoiceEntity
		{
			Name = model.Name,
			Amount = model.Amount,
			DueDate = model.DueDate ?? DateTime.Now,
			Status = model.Status.ToString(),
			Category = model.Category,
			Notes = model.Description
		};

		if (!string.IsNullOrEmpty(model.Id) && int.TryParse(model.Id, out var id))
		{
			entity.Id = id;
		}

		return entity;
	}

	#endregion

	private static Debt MapToDebt(DebtEntity entity)
	{
		return new Debt
		{
			Id = entity.Id.ToString(),
			Source = entity.Source,
			Amount = entity.Amount,
			DueDate = entity.DueDate,
			Status = entity.Status,
			Notes = entity.Notes,
			IsOwedToMe = entity.IsOwedToMe
		};
	}

	private static DebtEntity MapToDebtEntity(Debt model)
	{
		var entity = new DebtEntity
		{
			Source = model.Source,
			Amount = model.Amount,
			DueDate = model.DueDate,
			Status = model.Status,
			Notes = model.Notes,
			IsOwedToMe = model.IsOwedToMe
		};
		if (int.TryParse(model.Id, out var id)) entity.Id = id;
		return entity;
	}

	private static Income MapToIncome(IncomeEntity entity)
	{
		return new Income
		{
			Id = entity.Id.ToString(),
			Source = entity.Source,
			Amount = entity.Amount,
			Date = entity.Date,
			Category = entity.Category,
			IsRecurring = entity.IsRecurring,
			Notes = entity.Notes
		};
	}

	private static IncomeEntity MapToIncomeEntity(Income model)
	{
		var entity = new IncomeEntity
		{
			Source = model.Source,
			Amount = model.Amount,
			Date = model.Date,
			Category = model.Category,
			IsRecurring = model.IsRecurring,
			Notes = model.Notes
		};
		if (int.TryParse(model.Id, out var id)) entity.Id = id;
		return entity;
	}

	private static SavingsGoal MapToSavingsGoal(SavingsGoalEntity entity)
	{
		return new SavingsGoal
		{
			Id = entity.Id.ToString(),
			Name = entity.Name,
			TargetAmount = entity.TargetAmount,
			CurrentAmount = entity.CurrentAmount,
			Deadline = entity.Deadline,
			Icon = entity.Icon,
			Notes = entity.Notes
		};
	}

	private static SavingsGoalEntity MapToSavingsGoalEntity(SavingsGoal model)
	{
		var entity = new SavingsGoalEntity
		{
			Name = model.Name,
			TargetAmount = model.TargetAmount,
			CurrentAmount = model.CurrentAmount,
			Deadline = model.Deadline,
			Icon = model.Icon,
			Notes = model.Notes
		};
		if (int.TryParse(model.Id, out var id)) entity.Id = id;
		return entity;
	}
}
