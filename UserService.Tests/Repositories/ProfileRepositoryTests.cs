using Microsoft.AspNetCore.Identity;
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
    private readonly IPasswordHasher<User> _passwordHasher;

    public ProfileRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UserDbContext(options);

        _mockContext = new Mock<UserDbContext>(options);

        _profilRrepo = new ProfileRepository(context);
        _passwordHasher = new PasswordHasher<User>();
        _userRepo = new UserRepository(context, _passwordHasher);
    }

    [Fact]
    public async Task CreateProfile_ShouldAddProfile()
    {
        var user = new User { Email = "new@user.com", Password= "123" };
        await _userRepo.RegisterAsync(user); // add user

        var result = await _profilRrepo.CreateProfileAsync(user);

        var profile = await _profilRrepo.GetProfileByEmailAsync("new@user.com");

        Assert.True(result);
        Assert.NotNull(profile);
    }
}
