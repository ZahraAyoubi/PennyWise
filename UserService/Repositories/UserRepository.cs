using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.Models;
using Microsoft.AspNetCore.Identity;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserRepository(UserDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<User>> GetAsync(CancellationToken cancellationToken = default) =>
        await _context.Users.ToListAsync(cancellationToken);
   

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email must be provided.", nameof(email));

        return await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required", nameof(password));

        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Success)
            return user;

        return user;
    }

    public async Task<User> RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required.", nameof(user.Email));
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ArgumentException("Password is required.", nameof(user.Password));

        var exists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == user.Email, cancellationToken);
        if (exists)
            throw new InvalidOperationException("Email is already registered.");

        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);       
        user.CreatedAt = DateTime.UtcNow;

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        user.Password = null;

        return user;
    }
}
