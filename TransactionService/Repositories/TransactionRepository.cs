using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.IRepositories;
using TransactionService.Models;

namespace TransactionService.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(TransactionDbContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Transaction> AddIncomeAsync(Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        try
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding income transaction for UserId: {UserId}, Amount: {Amount}", transaction.UserId, transaction.Amount);
            throw;
        }
    }

    public async Task<List<Transaction>> GetFixedBudgetAsync(DateTime date, string userId)
    {
        var userGuid = ParseUserId(userId);
        var (startDate, endDate) = GetMonthRange(date);

        try
        {
            return await _context.Transactions
                             .Where(t => t.Type == "Expense"
                                 && t.Date >= startDate
                                 && t.Date < endDate
                                 && t.UserId == userGuid
                                 && t.IsFixed)
                            .ToListAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fixed budget for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<decimal> GetIncomeAsync(DateTime date, string userId)
    {
        var userGuid = ParseUserId(userId);
        var (startDate, endDate) = GetMonthRange(date);

        try
        {
            var transaction = await _context.Transactions.OrderByDescending(d => d.Date)
                                 .Where(t => t.Type == "Income"
                                  && t.Date >= startDate
                                  && t.Date < endDate
                                  && t.UserId == userGuid)
                                 .FirstOrDefaultAsync();

            if (transaction != null)
            {
                return transaction.Amount;
            }

            return 0;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting income for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId)
    {
        var userGuid = ParseUserId(userId);
        var (startDate, endDate) = GetMonthRange(date);

        try
        {
            var result = await _context.Transactions
                            .Where(t => t.Date >= startDate
                            && t.Date < endDate
                            && t.UserId == userGuid)
                            .GroupBy(t => 1)
                            .Select(g => new
                            {
                                Income = g.OrderByDescending(d => d.Date).Where(t => t.Type == "Income").FirstOrDefault().Amount,
                                TotalExpenses = g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                            })
                            .FirstOrDefaultAsync();
            return result?.Income - result?.TotalExpenses ?? 0;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting remaining budget for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<List<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId)
    {
        var userGuid = ParseUserId(userId);
        var (startDate, endDate) = GetMonthRange(date);

        try
        {
            return await _context.Transactions
                    .Where(t => t.Type == "Expense"
                     && t.Date >= startDate
                     && t.Date < endDate
                     && t.UserId == userGuid
                     && t.IsFixed == false)
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rotating budget for UserId: {UserId}", userId);
            throw;
        }

    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime date, string userId)
    {
        var userGuid = ParseUserId(userId);
        var (startDate, endDate) = GetMonthRange(date);

        try
        {
            return await _context.Transactions
                    .Where(t => t.Type == "Expense"
                    && t.Date >= startDate
                    && t.Date < endDate
                    && t.UserId == userGuid)
                    .SumAsync(t => t.Amount);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total expense for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> DeleteTransactionAsync(string id)
    {
        var userGuid = ParseUserId(id);

        var item = await _context.Transactions.FindAsync(userGuid);
        if (item == null)
        {
            return false;
        }
        try
        {
            _context.Transactions.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction with Id: {Id}", id);
            throw;
        }
    }

    private Guid ParseUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var guid))
            throw new ArgumentException("Invalid userId", nameof(userId));
        return guid;
    }

    private (DateTime Start, DateTime End) GetMonthRange(DateTime date)
    {
        var start = new DateTime(date.Year, date.Month, 1);
        return (start, start.AddMonths(1));
    }
}
