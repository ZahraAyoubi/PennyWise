using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.IRepositories;
using TransactionService.IServices;
using TransactionService.Models;

namespace TransactionService.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }
    public async Task<decimal> GetIncomeAsync(DateTime date, string userId)
    {
        return await _repository.GetIncomeAsync(date, userId);
    }

    public async Task<Transaction> AddIncomeAsync(Transaction transaction)
    {
        return await _repository.AddIncomeAsync(transaction);
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        return await _repository.AddIncomeAsync(transaction);
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        return await _repository.DeleteTransactionAsync(id);
    }

    public async Task<decimal> GetTotalExpensesAsync(DateTime date, string userId)
    {
        return await _repository.GetTotalExpensesAsync(date, userId);
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

    public async Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId)
    {
        return await _repository.GetRamainingBudgetAsync(date, userId);
    }

    public async Task<IEnumerable<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId)
    {
        return await _repository.GetRotatingBudgetAsync(date, userId);
    }

    public async Task<IEnumerable<Transaction>> GetFixedBudgetAsync(DateTime date, string userId)
    {
        return await _repository.GetFixedBudgetAsync(date, userId);
    }
}
