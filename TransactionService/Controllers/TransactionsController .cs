using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionService.IServices;
using TransactionService.Models;

namespace TransactionService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    [Route("CreateTransaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
    {
        transaction.Type = "Expense";
        var createdTransaction = await _transactionService.AddTransactionAsync(transaction);
        return CreatedAtAction(nameof(CreateTransaction), new { id = createdTransaction.Id }, createdTransaction);
    }

    [HttpPost]
    [Route("AddIncome")]
    public async Task<IActionResult> AddIncome([FromBody] Transaction transaction)
    {
        transaction.Type = "Income";
        var createdTransaction = await _transactionService.AddIncomeAsync(transaction);
        return CreatedAtAction(nameof(AddIncome), new { id = createdTransaction.Id }, createdTransaction);
    }

    [HttpGet]
    [Route("Income/{date}")]
    public async Task<IActionResult> GetIncome(DateTime date)
    {
        var transaction = await _transactionService.GetIncomeAsync(date);
        return CreatedAtAction(nameof(GetIncome), transaction);
    }

    [HttpGet]
    [Route("TotalExpenses/{date}")]
    public async Task<IActionResult> GetTotalExpenses(DateTime date)
    {
        var amount = await _transactionService.GetTotalExpensesAsync(date);
        return CreatedAtAction(nameof(GetTotalExpenses), amount);
    }

    [HttpGet]
    [Route("RamainingBudget/{date}")]
    public async Task<IActionResult> GetRamainingBudget(DateTime date)
    {
        var amount = await _transactionService.GetRamainingBudgetAsync(date);
        return CreatedAtAction(nameof(GetRamainingBudget), amount);
    }

    [HttpGet]
    [Route("RotatingBudget/{date}")]
    public async Task<IActionResult> GetRotatingBudget(DateTime date)
    {
        var amount = await _transactionService.GetRotatingBudgetAsync(date);
        return CreatedAtAction(nameof(GetRotatingBudget), amount);
    }
    [HttpDelete]
    [Route("DeleteRotatingBudget/{id}")]
    public async Task<IActionResult> DeleteRotatingBudget( int id)
    {
        var deleteTransaction = await _transactionService.DeleteTransactionAsync(id);
        return CreatedAtAction(nameof(DeleteRotatingBudget), id);
    }
}
