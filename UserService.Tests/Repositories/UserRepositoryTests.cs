using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Data;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Test.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<UserDbContext> _mockContext;
    private readonly UserRepository _repo;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UserDbContext(options);

        _mockContext = new Mock<UserDbContext>(options);

        _repo = new UserRepository(context);
    }

    [Fact]
    public async Task Register_ShouldAddUserWithHashedPassword()
    {
        //Arrange
        var user = new User { Email = "test@example.com", Password = "MyPass123" };

        //Act
        var result = await _repo.RegisterAsync(user);
        var savedUser = await _repo.GetUserByEmailAsync("test@example.com");

        //Assert
        Assert.True(result);
        Assert.NotNull(savedUser);
        Assert.NotEqual("MyPass123", savedUser.PasswordHash); // ensure password was hashed
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        //Arrange
        await _repo.RegisterAsync(new User { Email = "a@b.com", Password = "123" });
        await _repo.RegisterAsync(new User { Email = "b@c.com", Password = "123" });

        //Act
        var users = await _repo.GetAsync();

        //Assert
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnCorrectUser()
    {
        //Arrange
        await _repo.RegisterAsync(new User { Email = "email@host.com", Password = "123" });

        //Act
        var user = await _repo.GetUserByEmailAsync("email@host.com");

        //Assert
        Assert.NotNull(user);
        Assert.Equal("email@host.com", user.Email);
    }
}
