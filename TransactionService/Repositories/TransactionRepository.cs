using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.IRepositories;
using TransactionService.Models;

namespace TransactionService.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction is null)
            throw new ArgumentNullException(nameof(transaction));

            await _context.Transactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction;
    }

    public async Task<List<Transaction>> GetFixedBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(userId);
        var (startDate, endDate) = GetMonthRange(date);

            return await _context.Transactions
                             .Where(t => t.Type == "Expense"
                                 && t.Date >= startDate
                                 && t.Date < endDate
                                 && t.UserId == userGuid
                                 && t.IsFixed)
                            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetIncomeAsync(DateTime date, string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(userId);
        var (startDate, endDate) = GetMonthRange(date);


            var transaction = await _context.Transactions.OrderByDescending(d => d.Date)
                                 .Where(t => t.Type == "Income"
                                  && t.Date >= startDate
                                  && t.Date < endDate
                                  && t.UserId == userGuid)
                                 .FirstOrDefaultAsync(cancellationToken);

            if (transaction != null)
            {
                return transaction.Amount;
            }

            return 0;

    }

    public async Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(userId);
        var (startDate, endDate) = GetMonthRange(date);

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
                            .FirstOrDefaultAsync(cancellationToken);
            return result?.Income - result?.TotalExpenses ?? 0;
    }

    public async Task<List<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(userId);
        var (startDate, endDate) = GetMonthRange(date);

            return await _context.Transactions
                    .Where(t => t.Type == "Expense"
                     && t.Date >= startDate
                     && t.Date < endDate
                     && t.UserId == userGuid
                     && t.IsFixed == false)
                    .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime date, string userId, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(userId);
        var (startDate, endDate) = GetMonthRange(date);

            return await _context.Transactions
                    .Where(t => t.Type == "Expense"
                    && t.Date >= startDate
                    && t.Date < endDate
                    && t.UserId == userGuid)
                    .SumAsync(t => t.Amount, cancellationToken);

    }

    public async Task<bool> DeleteTransactionAsync(string id, CancellationToken cancellationToken = default)
    {
        var userGuid = ParseId(id);

        var item = await _context.Transactions.FindAsync(userGuid);
        if (item is null)
            return false;

            _context.Transactions.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
    }

    private Guid ParseId(string id)
    {
        if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var guid))
            throw new ArgumentException("Invalid userId", nameof(id));
        return guid;
    }

    private (DateTime Start, DateTime End) GetMonthRange(DateTime date)
    {
        var start = new DateTime(date.Year, date.Month, 1);
        return (start, start.AddMonths(1));
    }
}
