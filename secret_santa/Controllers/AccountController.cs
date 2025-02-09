using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using secret_santa.Models.DTO;


namespace secret_santa.Controllers
{
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
            return Ok(new { email = model.Email, role = "USER" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            var role = "";
            if (model.Email.Contains("admin"))
                role = "ADMIN";
            else role = "USER";

            if (result.Succeeded)
            {
                return Ok(new { email = model.Email, role = role });
            }
            else
            {
                return Unauthorized(new { message = "Invalid login attempt." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User logged out successfully!" });
        }
        
        [HttpGet("check-session")]
        public async Task<IActionResult> CheckSession()
        {
            var user = await _userManager.GetUserAsync(User);  
            if (user == null)
            {
                return Unauthorized(); 
            }

            var roles = await _userManager.GetRolesAsync(user); 
            var role = roles.Contains("ADMIN") ? "ADMIN" : "USER"; 

            return Ok(new { email = user.Email, role }); 
        }
    }
}