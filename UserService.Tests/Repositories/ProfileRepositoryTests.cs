using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Data;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Test.Repositories;

public class ProfileRepositoryTests
{
    private readonly Mock<UserDbContext> _mockContext;
    private readonly ProfileRepository _profilRrepo;
    private readonly UserRepository _userRepo;

    public ProfileRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UserDbContext(options);

        _mockContext = new Mock<UserDbContext>(options);

        _profilRrepo = new ProfileRepository(context);
        _userRepo = new UserRepository(context);
    }

    [Fact]
    public async Task CreateProfile_ShouldAddProfile()
    {
        var user = new User { Email = "new@user.com" };
        await _userRepo.Register(user); // add user

        var result = await _profilRrepo.CreateProfile(user);

        var profile = await _profilRrepo.GetProfileByEmail("new@user.com");

        Assert.True(result);
        Assert.NotNull(profile);
    }
}
