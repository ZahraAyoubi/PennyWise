using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.Models;

namespace UserService.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly UserDbContext _context;
    public ProfileRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Profile> GetProfileByEmail(string email)
    {
        var profile = await _context.Profiles.Where(p => p.User.Email.Equals(email)).FirstOrDefaultAsync();
        return profile;
    }
}
