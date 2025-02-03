using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace secret_santa.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PairingController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public PairingController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("pairUsers")]
    public async Task<IActionResult> PairUsers()
    {
        // Fetch all users from the database
        var allUsers = _userManager.Users.ToList();

        // Filter out the admin user and get only regular users
        var regularUsers = allUsers
            .Where(user => user.UserName != "admin") // Exclude admin user
            .Select(user => user.UserName) // Extract usernames
            .ToList();

        if (regularUsers.Count < 2)
            return BadRequest("At least two regular users are required for pairing.");

        // Shuffle the list of regular users
        var shuffledUsers = Shuffle(regularUsers);

        // Create pairs
        var pairs = new List<string>();
        for (int i = 0; i < shuffledUsers.Count; i++)
        {
            var giver = shuffledUsers[i];
            var receiver = shuffledUsers[(i + 1) % shuffledUsers.Count]; // Wrap around to the first user
            pairs.Add($"{giver} -> {receiver}");
        }

        return Ok(pairs);
    }

    // Helper method to shuffle a list
    private List<string> Shuffle(List<string> users)
    {
        var random = new System.Random();
        return users.OrderBy(_ => random.Next()).ToList();
    }
}