using TransactionService.Models;

namespace TransactionService.IServices;

public interface ITransactionService
{
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<Transaction> AddIncomeAsync(Transaction transaction);
    Task<Transaction> GetIncomeAsync(DateTime date);
    Task<decimal> GetTotalExpensesAsync(DateTime date);
    Task<decimal> GetRamainingBudgetAsync(DateTime date);
    Task<IEnumerable<Transaction>> GetRotatingBudgetAsync(DateTime date);
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int id);
    Task<bool> UpdateTransactionAsync(int id, Transaction updatedTransaction);
    Task<bool> DeleteTransactionAsync(int id);
    Task<decimal> GetTotalIncomeAsync();
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
