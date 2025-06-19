using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.IServices;
using UserService.Models;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserServices _userService;

    public UserController(IUserServices userService)
    {
        Console.WriteLine("🔹 UserController instantiated.");
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost]
    [Route("Register/{password}")]
    public async Task<IActionResult> Register([FromBody] ApplicationUser user, string password)
    {
        await _userService.Register(user, password);
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.Login(request);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { user });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("user");
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("send-reset-link")]
    public async Task<IActionResult> SendResetLink([FromBody] ApplicationUser request)
    {
        var user = await _userService.FindUserByEmail(request.Email);
        if (user == null)
            return Ok(); 

        var token = await _userService.GenerateResetToken(user);

        var resetLink = $"http://localhost:49222/resetpassword?token={Uri.EscapeDataString(token)}&email={request.Email}";

        var message = $"Click the link to reset your password: {resetLink}";
        await _userService.SendEmailAsync(request, "Reset Your Password", message);

        return Ok();
    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userService.FindUserByEmail(request.Email);
        if (user == null)
            return Ok(); 

         var result = await _userService.ResetPassword(user, request.ResetCode, request.NewPassword);

        if (result.Succeeded)
            return Ok(new { message = "Password reset successful" });

        var error = result.Errors.FirstOrDefault()?.Description;
        return BadRequest(new { message = error });
    }
}
