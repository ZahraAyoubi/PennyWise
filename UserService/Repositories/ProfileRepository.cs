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

    public async Task<bool> CreateProfileAsync(User user, CancellationToken cancellationToken = default)
    {
        var profile = new Profile
        {
            User = user
        };

        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Profile> GetProfileByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var profile = await _context.Profiles.Where(p => p.User.Email.Equals(email)).FirstOrDefaultAsync();
        return profile;
    }

    public async Task<Profile> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _context.Profiles.Where(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        return profile;
    }

    public async Task<bool> UpdateProfileAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        var existingProfile = await _context.Profiles.FindAsync(profile.Id);

        if (existingProfile == null)
        {
            return false;
        }

        existingProfile.Name = profile.Name;
        existingProfile.Description = profile.Description;

        _context.Profiles.Update(existingProfile);

        await _context.SaveChangesAsync();

        return true;
    }
}
