using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using secret_santa.Models.DTO;

namespace secret_santa.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{   
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);
        await _userManager.AddToRoleAsync(user, "USER");
        await _signInManager.SignInAsync(user, isPersistent: false);
        return Ok(new { message = "User registered successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok(new { message = "User logged in successfully!" });
        }
        else
        {
            return Unauthorized(new { message = "Invalid login attempt." });
        }
    }
}