using UserService.Models;

namespace UserService.IServices;

public interface IProfileService
{
    Task<Profile> GetProfileByEmail(string email);
    Task<bool> UpdateProfile(Profile profile);
}
