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

    public async Task<Profile> GetProfileByEmail(string email)
    {
        var profile = await _context.Profiles.Where(p => p.User.Email.Equals(email)).FirstOrDefaultAsync();
        return profile;
    }

    public async Task<Profile> GetProfileByyId(Guid id)
    {
        var profile = await _context.Profiles.Where(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        return profile;
    }

    public async Task<bool> UpdateProfile(Profile profile)
    {
        // Retrieve the existing profile from the database
        var existingProfile = await _context.Profiles.FindAsync(profile.Id);

        if (existingProfile == null)
        {
            // Profile not found, return false or handle accordingly
            return false;
        }

        // Update the properties of the existing profile with the new values
        existingProfile.Name = profile.Name;
        existingProfile.Description = profile.Description;

        // Update the profile in the database
        _context.Profiles.Update(existingProfile);

        // Save changes
        await _context.SaveChangesAsync();

        return true;
    }
}
