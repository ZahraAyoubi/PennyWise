using UserService.IRepositories;
using UserService.IServices;
using UserService.Models;

namespace UserService.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _repository;
    public ProfileService(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<Profile> GetProfileByEmail(string email)
    {
        if (email == null)
            return null;

        var profile = await _repository.GetProfileByEmail(email);
        return profile;
    }

    public async Task<bool> UpdateProfile(Profile profile)
    {
        //var oldProfile = await _repository.GetProfileByyId(profile.Id);
        await _repository.UpdateProfile(profile);

        return true;
    }
}
