using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Configure default constraints for the Transaction entity
    //    modelBuilder.Entity<User>().Property(t => t.Name).IsRequired();
    //    modelBuilder.Entity<User>().Property(t => t.Email).IsRequired();
    //    modelBuilder.Entity<User>().Property(t => t.Password).IsRequired();
    //    modelBuilder.Entity<Profile>().Property(t => t.User).IsRequired();
    //}
}
