using SQLite;
using ExpenseTracker.Data.Entities;

namespace ExpenseTracker.Data;

/// <summary>
/// SQLite database service for managing local data storage
/// </summary>
public class DatabaseService
{
	private readonly SQLiteAsyncConnection _database;

	public DatabaseService()
	{
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "expensetracker.db3");
		_database = new SQLiteAsyncConnection(dbPath);

		// Create tables
		InitializeDatabaseAsync().Wait();
	}

	private async Task InitializeDatabaseAsync()
	{
		await _database.CreateTableAsync<SubscriptionEntity>();
		await _database.CreateTableAsync<InvoiceEntity>();
		await _database.CreateTableAsync<DebtEntity>();
		await _database.CreateTableAsync<IncomeEntity>();
		await _database.CreateTableAsync<SavingsGoalEntity>();
	}

	#region Subscription Methods

	/// <summary>
	/// Gets all subscriptions from database
	/// </summary>
	public Task<List<SubscriptionEntity>> GetSubscriptionsAsync()
	{
		return _database.Table<SubscriptionEntity>().ToListAsync();
	}

	/// <summary>
	/// Gets a subscription by ID
	/// </summary>
	public Task<SubscriptionEntity?> GetSubscriptionAsync(int id)
	{
		return _database.Table<SubscriptionEntity>()
			.Where(s => s.Id == id)
			.FirstOrDefaultAsync();
	}

	/// <summary>
	/// Saves a subscription (insert or update)
	/// </summary>
	public async Task<int> SaveSubscriptionAsync(SubscriptionEntity subscription)
	{
		if (subscription.Id != 0)
		{
			subscription.UpdatedAt = DateTime.Now;
			await _database.UpdateAsync(subscription);
			return subscription.Id;
		}
		else
		{
			subscription.CreatedAt = DateTime.Now;
			subscription.UpdatedAt = DateTime.Now;
			return await _database.InsertAsync(subscription);
		}
	}

	/// <summary>
	/// Deletes a subscription
	/// </summary>
	public Task<int> DeleteSubscriptionAsync(SubscriptionEntity subscription)
	{
		return _database.DeleteAsync(subscription);
	}

	#endregion

	#region Invoice Methods

	/// <summary>
	/// Gets all invoices from database
	/// </summary>
	public Task<List<InvoiceEntity>> GetInvoicesAsync()
	{
		return _database.Table<InvoiceEntity>().ToListAsync();
	}

	/// <summary>
	/// Gets an invoice by ID
	/// </summary>
	public Task<InvoiceEntity?> GetInvoiceAsync(int id)
	{
		return _database.Table<InvoiceEntity>()
			.Where(i => i.Id == id)
			.FirstOrDefaultAsync();
	}

	/// <summary>
	/// Saves an invoice (insert or update)
	/// </summary>
	public async Task<int> SaveInvoiceAsync(InvoiceEntity invoice)
	{
		if (invoice.Id != 0)
		{
			invoice.UpdatedAt = DateTime.Now;
			await _database.UpdateAsync(invoice);
			return invoice.Id;
		}
		else
		{
			invoice.CreatedAt = DateTime.Now;
			invoice.UpdatedAt = DateTime.Now;
			return await _database.InsertAsync(invoice);
		}
	}

	/// <summary>
	/// Deletes an invoice
	/// </summary>
	public Task<int> DeleteInvoiceAsync(InvoiceEntity invoice)
	{
		return _database.DeleteAsync(invoice);
	}

	#endregion

	#region Summary Methods

	/// <summary>
	/// Gets total expenses for a specific month
	/// </summary>
	public async Task<decimal> GetMonthlyTotalAsync(int year, int month)
	{
		var startDate = new DateTime(year, month, 1);
		var endDate = startDate.AddMonths(1).AddDays(-1);

		var subscriptions = await _database.Table<SubscriptionEntity>()
			.Where(s => s.NextBillingDate >= startDate && s.NextBillingDate <= endDate)
			.ToListAsync();

		var invoices = await _database.Table<InvoiceEntity>()
			.Where(i => i.DueDate >= startDate && i.DueDate <= endDate)
			.ToListAsync();

		return subscriptions.Sum(s => s.Amount) + invoices.Sum(i => i.Amount);
	}

	/// <summary>
	/// Gets total expenses for a specific year
	/// </summary>
	public async Task<decimal> GetYearlyTotalAsync(int year)
	{
		var startDate = new DateTime(year, 1, 1);
		var endDate = new DateTime(year, 12, 31);

		var subscriptions = await _database.Table<SubscriptionEntity>()
			.Where(s => s.NextBillingDate >= startDate && s.NextBillingDate <= endDate)
			.ToListAsync();

		var invoices = await _database.Table<InvoiceEntity>()
			.Where(i => i.DueDate >= startDate && i.DueDate <= endDate)
			.ToListAsync();

		return subscriptions.Sum(s => s.Amount) + invoices.Sum(i => i.Amount);
	}

	/// <summary>
	/// Gets category summary grouped by category
	/// </summary>
	public async Task<Dictionary<string, decimal>> GetCategorySummaryAsync()
	{
		var subscriptions = await _database.Table<SubscriptionEntity>().ToListAsync();
		var invoices = await _database.Table<InvoiceEntity>().ToListAsync();

		var categoryTotals = new Dictionary<string, decimal>();

		foreach (var subscription in subscriptions)
		{
			var category = string.IsNullOrEmpty(subscription.Category) ? "Uncategorized" : subscription.Category;
			if (categoryTotals.ContainsKey(category))
				categoryTotals[category] += subscription.Amount;
			else
				categoryTotals[category] = subscription.Amount;
		}

		foreach (var invoice in invoices)
		{
			var category = string.IsNullOrEmpty(invoice.Category) ? "Uncategorized" : invoice.Category;
			if (categoryTotals.ContainsKey(category))
				categoryTotals[category] += invoice.Amount;
			else
				categoryTotals[category] = invoice.Amount;
		}

		return categoryTotals;
	}

	#endregion

	#region Debt Methods

	public Task<List<DebtEntity>> GetDebtsAsync() => _database.Table<DebtEntity>().ToListAsync();

	public Task<DebtEntity?> GetDebtAsync(int id) => _database.Table<DebtEntity>().Where(d => d.Id == id).FirstOrDefaultAsync();

	public async Task<int> SaveDebtAsync(DebtEntity debt)
	{
		if (debt.Id != 0)
			return await _database.UpdateAsync(debt);
		else
		{
			debt.CreatedAt = DateTime.Now;
			return await _database.InsertAsync(debt);
		}
	}

	public Task<int> DeleteDebtAsync(DebtEntity debt) => _database.DeleteAsync(debt);

	#endregion

	#region Income Methods

	public Task<List<IncomeEntity>> GetIncomesAsync() => _database.Table<IncomeEntity>().ToListAsync();

	public Task<IncomeEntity?> GetIncomeAsync(int id) => _database.Table<IncomeEntity>().Where(i => i.Id == id).FirstOrDefaultAsync();

	public Task<int> SaveIncomeAsync(IncomeEntity income)
	{
		return income.Id != 0 ? _database.UpdateAsync(income) : _database.InsertAsync(income);
	}

	public Task<int> DeleteIncomeAsync(IncomeEntity income) => _database.DeleteAsync(income);

	#endregion

	#region Savings Goal Methods

	public Task<List<SavingsGoalEntity>> GetSavingsGoalsAsync() => _database.Table<SavingsGoalEntity>().ToListAsync();

	public Task<SavingsGoalEntity?> GetSavingsGoalAsync(int id) => _database.Table<SavingsGoalEntity>().Where(s => s.Id == id).FirstOrDefaultAsync();

	public async Task<int> SaveSavingsGoalAsync(SavingsGoalEntity goal)
	{
		if (goal.Id != 0)
			return await _database.UpdateAsync(goal);
		else
		{
			goal.CreatedAt = DateTime.Now;
			return await _database.InsertAsync(goal);
		}
	}

	public Task<int> DeleteSavingsGoalAsync(SavingsGoalEntity goal) => _database.DeleteAsync(goal);

	#endregion
}
