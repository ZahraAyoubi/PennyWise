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
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserDbContext context,
                          IPasswordHasher<User> passwordHasher,
                          UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _userManager = userManager;
    }

    public async Task<List<ApplicationUser>> GetAsync()
    {
        var query = _userManager.Users;

        if (query is IAsyncEnumerable<ApplicationUser>)
        {
            return await query.AsNoTracking().ToListAsync();
        }

        return query.ToList();
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email must be provided.", nameof(email));

        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required", nameof(password));

        //var user = await _userManager.Users
        //    .AsNoTracking()
        //    .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return null;

        //var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        //if (result == PasswordVerificationResult.Success)
        //    return user;

        return user;
    }

    //public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    //{
    //    if (user == null)
    //        throw new ArgumentNullException(nameof(user));
    //    if (string.IsNullOrWhiteSpace(user.Email))
    //        throw new ArgumentException("Email is required.", nameof(user.Email));
    //    if (string.IsNullOrWhiteSpace(password))
    //        throw new ArgumentException("Password is required.", nameof(password));

    //    var exists = await _context.Users
    //        .AsNoTracking()
    //        .AnyAsync(u => u.Email == user.Email, cancellationToken);
    //    if (exists)
    //        throw new InvalidOperationException("Email is already registered.");

    //    //user.PasswordHash = _passwordHasher.HashPassword(user, password);       
    //    //user.CreatedAt = DateTime.UtcNow;

    //    await _context.Users.AddAsync(user, cancellationToken);
    //    await _context.SaveChangesAsync(cancellationToken);

    //    return user;
    //}

    public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        //user.PasswordResetToken = token;
        //user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

        await _context.SaveChangesAsync();
        return token;
    }
}
