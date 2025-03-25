using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.IRepositories;
using UserService.IServices;
using UserService.Models;

namespace UserService.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _repository;

    public UserServices(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await _repository.Login(email, password);

        return user;
    }
    public async Task<List<User>> GetAll()
    {
        var user = await _repository.GetAll();
        return user;
    }
    public async Task<bool> Register(User user)
    {
       if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }



        var existingUser = await _repository.GetUserByEmail(user.Email);
        if (existingUser != null)
        {
            return false;
        }

        await _repository.Register(user);

        await _repository.CreateProfile(user);

        //UserEventPublisher rabbit = new UserEventPublisher();
        //await rabbit.PublishUserCreatedEvent(user.Id, user.Email);

        return true;
    }
}
