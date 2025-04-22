using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.Models;
using System.Text;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAll()
    {
        var user = await _context.Users.ToListAsync();

        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();

        return user;
    }

    public async Task<User> Login(string email, string password)
    {
        //try
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        //    if (user == null) return null;

        //    var hashedInput = HashedPassword(password);

        //    if (hashedInput == user.PasswordHash)
        //    {
        //        return user;
        //    }

        //    return null;
        //}
        //catch (Exception ex)
        //{
        //    throw new ApplicationException("An error occurred while trying to log in.", ex);
        //}
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return user;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while trying to log in.", ex);
        }
    }

    public async Task<bool> Register(User user)
    {
        //string hashedPassword = HashedPassword(user.Password);
        //user.PasswordHash = hashedPassword;
        //user.CreatedAt = DateTime.UtcNow;

        //await _context.Users.AddAsync(user);
        //await _context.SaveChangesAsync();

        //return true;

        // Generate a random salt
        var salt = GenerateSalt();

        // Hash the password with the salt
        string hashedPassword = HashPasswordWithSalt(user.Password, salt);

        // Set properties
        user.PasswordHash = hashedPassword;
        user.Salt = salt;
        user.CreatedAt = DateTime.UtcNow;

        // Store in DB
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }


    private string HashedPassword(string password)
    {
        byte[] salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var hashedInput = HashPasswordWithSalt(password, storedSalt);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(hashedInput),
            Encoding.UTF8.GetBytes(storedHash)
        );
    }

    public static string HashPasswordWithSalt(string password, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 100000, HashAlgorithmName.SHA256);
        var hashBytes = pbkdf2.GetBytes(32); // 256-bit hash

        return Convert.ToBase64String(hashBytes);
    }

    private string GenerateSalt(int size = 32)
    {
        var rng = new RNGCryptoServiceProvider();
        var buffer = new byte[size];
        rng.GetBytes(buffer);
        return Convert.ToBase64String(buffer);
    }
}
