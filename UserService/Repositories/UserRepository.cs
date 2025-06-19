using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;


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

    public async Task<ApplicationUser> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required", nameof(request.Email));
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required", nameof(request.Password));

        var user =await _userManager.FindByEmailAsync(request.Email);
        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (result == false)
            return null;

        return user;
    }

    public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user;
    }

    public async Task<string> GenerateResetTokenAsync(ApplicationUser user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {

       var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return result;
    }
}
