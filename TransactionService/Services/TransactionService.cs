using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.IServices;
using TransactionService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransactionService.Services;

public class TransactionService : ITransactionService
{
    private readonly TransactionDbContext _context;

    public TransactionService(TransactionDbContext context)
    {
        _context = context;
    }
    public async Task<Transaction> GetIncomeAsync(DateTime date)
    {
        var mounth = date.Month;
        var year = date.Year;
        var transaction = await _context.Transactions.OrderByDescending(d => d.Date)
                             .Where(t => t.Type == "Income" && 
                             t.Date.Month == mounth && t.Date.Year == year)
                             .FirstOrDefaultAsync();
        return transaction;
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

    public Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime date)
    {
        var mounth = date.Month;
        var year = date.Year;
        var amount = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Month == date.Month && t.Date.Year == date.Year)
            .SumAsync(t=> t.Amount);

        return amount;
    }

    public Task<decimal> GetTotalIncomeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> GetTransactionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTransactionAsync(int id, Transaction updatedTransaction)
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> GetRamainingBudgetAsync(DateTime date)
    {
        var mounth = date.Month;
        var year = date.Year;
        decimal amount = 0;

        var income = await _context.Transactions.OrderByDescending(d => d.Date)
                             .Where(t => t.Type == "Income" &&
                             t.Date.Month == mounth && t.Date.Year == year)
                             .FirstOrDefaultAsync();

        var expenses = await _context.Transactions
            .Where(t => t.Type == "Expense" && t.Date.Month == mounth && t.Date.Year == year)
            .SumAsync(t => t.Amount);

        if (income != null)
        {
            amount = income.Amount - expenses;
        }       

        return amount;
    }

    public async Task<IEnumerable<Transaction>> GetRotatingBudgetAsync(DateTime date)
    {
        var mounth = date.Month;
        var year = date.Year;

        var transactions = await _context.Transactions
            .Where(t=> t.Type == "Expense" 
            && t.Date.Month == date.Month && t.Date.Year == date.Year)
            .ToListAsync();

        return transactions;
    }
}
