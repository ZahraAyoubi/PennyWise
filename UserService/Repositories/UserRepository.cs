using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public UserRepository(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<bool> CreateProfile(User user)
    {
        var profile = new Profile
        {
            User = user
        };

        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return true;
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
        //|| !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
        //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
        //if (user == null) return null;

        //return user;

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
            if (user == null) return null;

            return user;
        }
        catch(Exception ex)
        {
            throw new ApplicationException("An error occurred while trying to log in.", ex);
        }
    }

    public async Task<bool> Register(User user)
    {
        string hashedPassword = HashedPassword(user.Password);
        user.PasswordHash = hashedPassword;
        user.CreatedAt = DateTime.UtcNow;

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

    //private string GenerateJwtToken(User user)
    //{
    //    var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, user.Email) }),
    //        Expires = DateTime.UtcNow.AddHours(1),
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
    //        Issuer = _config["JwtSettings:Issuer"],
    //        Audience = _config["JwtSettings:Audience"]
    //    };

    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}
}
