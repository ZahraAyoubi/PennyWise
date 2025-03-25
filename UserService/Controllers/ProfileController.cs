using Microsoft.AspNetCore.Mvc;
using UserService.IServices;
using UserService.Models;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [Route("{email}")]
    public async Task<IActionResult> GetProfileByEmail(string email)
    {
        var profile = await _profileService.GetProfileByEmail(email);
        //return CreatedAtAction(nameof(GetProfileByEmail), profile);
        return Ok(new { profile });
    }
}
