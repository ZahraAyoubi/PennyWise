using TransactionService.Models;

namespace TransactionService.IRepositories;

public interface ITransactionRepository
{
    Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task<decimal> GetIncomeAsync(DateTime date, string userId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalExpensesAsync(DateTime date, string userId, CancellationToken cancellationToken = default);
    Task<decimal> GetRamainingBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default);
    Task<List<Transaction>> GetRotatingBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default);
    Task<List<Transaction>> GetFixedBudgetAsync(DateTime date, string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteTransactionAsync(string id, CancellationToken cancellationToken = default);
}
