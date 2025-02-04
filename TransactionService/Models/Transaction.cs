namespace TransactionService.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // "Income" or "Expense"
    public DateTime Date { get; set; }
}
