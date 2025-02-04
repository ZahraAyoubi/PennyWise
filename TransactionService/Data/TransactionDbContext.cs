using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
    : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure default constraints for the Transaction entity
        modelBuilder.Entity<Transaction>().Property(t => t.Description).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Transaction>().Property(t => t.Type).IsRequired();
        modelBuilder.Entity<Transaction>().Property(t => t.Date).IsRequired();
    }
}
