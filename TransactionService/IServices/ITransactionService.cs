using TransactionService.Models;

namespace TransactionService.IServices;

public interface ITransactionService
{
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<Transaction> AddIncomeAsync(Transaction transaction);
    Task<decimal> GetIncomeAsync(DateTime date, string userId);
    Task<decimal> GetTotalExpensesAsync(DateTime date, string userId);
    Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId);
    Task<IEnumerable<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId);
    Task<IEnumerable<Transaction>> GetFixedBudgetAsync(DateTime date, string userId);
    Task<bool> DeleteTransactionAsync(string id);
}
