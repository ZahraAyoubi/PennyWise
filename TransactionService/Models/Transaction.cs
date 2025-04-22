//using UserService.Models;

namespace TransactionService.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // "Income" or "Expense"
    public DateTime Date { get; set; }
    //public Profile Profile_Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsFixed { get; set; }
}
