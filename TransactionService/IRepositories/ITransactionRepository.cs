using TransactionService.Models;

namespace TransactionService.IRepositories;

public interface ITransactionRepository
{
    Task<Transaction> AddIncomeAsync(Transaction transaction);
    Task<decimal> GetIncomeAsync(DateTime date, string userId);
    Task<decimal> GetTotalExpensesAsync(DateTime date, string userId);
    Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId);
    Task<List<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId);
    Task<List<Transaction>> GetFixedBudgetAsync(DateTime date, string userId);
    Task<bool> DeleteTransactionAsync(string id);
}
