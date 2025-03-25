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
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int id);
    Task<bool> UpdateTransactionAsync(int id, Transaction updatedTransaction);
    Task<bool> DeleteTransactionAsync(int id);
    Task<decimal> GetTotalIncomeAsync();
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
