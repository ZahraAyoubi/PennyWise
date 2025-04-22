using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.IRepositories;
using TransactionService.Models;

namespace TransactionService.Repositories;

public class TransactionRepository:ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddIncomeAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        try
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        try
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var item = await _context.Transactions.FindAsync(id);
        if (item == null)
        {
            return false;
        }
        var result = _context.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Transaction>> GetFixedBudgetAsync(DateTime date, string userId)
    {
        var mounth = date.Month;
        var year = date.Year;

        var transactions = await _context.Transactions
            .Where(t => t.Type == "Expense"
            && t.Date.Month == date.Month && t.Date.Year == date.Year
            && t.UserId.ToString().Equals(userId)
            && t.IsFixed)
            .ToListAsync();

        return transactions;
    }

    public async Task<decimal> GetIncomeAsync(DateTime date, string userId)
    {
        var month = date.Month;
        var year = date.Year;
        decimal amount = 0;
        var transaction = await _context.Transactions.OrderByDescending(d => d.Date)
                             .Where(t => t.Type == "Income" &&
                             t.Date.Month == month && t.Date.Year == year
                             && t.UserId.ToString().Equals(userId))
                             .FirstOrDefaultAsync();

        if (transaction != null)
        {
            amount = transaction.Amount;
        }

        return amount;
    }

    public async Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId)
    {
        var mounth = date.Month;
        var year = date.Year;
        decimal amount = 0;

        var income = await _context.Transactions.OrderByDescending(d => d.Date)
                             .Where(t => t.Type == "Income" &&
                             t.Date.Month == mounth && t.Date.Year == year
                             && t.UserId.ToString().Equals(userId))
                             .FirstOrDefaultAsync();

        var expenses = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Month == mounth
            && t.Date.Year == year
            && t.UserId.ToString().Equals(userId))
            .SumAsync(t => t.Amount);

        if (income != null)
        {
            amount = income.Amount - expenses;
        }

        return amount;
    }

    public async Task<IEnumerable<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId)
    {
        var mounth = date.Month;
        var year = date.Year;

        var transactions = await _context.Transactions
            .Where(t => t.Type == "Expense"
            && t.Date.Month == date.Month && t.Date.Year == date.Year
            && t.UserId.ToString().Equals(userId)
            && t.IsFixed == false)
            .ToListAsync();

        return transactions;
    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime date, string userId)
    {
        var mounth = date.Month;
        var year = date.Year;
        var amount = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Month == date.Month && t.Date.Year == date.Year
            && t.UserId.ToString().Equals(userId))
            .SumAsync(t => t.Amount);

        return amount;
    }
}
