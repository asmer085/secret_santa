using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using secret_santa.Context;

using secret_santa.Models;


namespace secret_santa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PairingController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("pairUsers")]
        public async Task<IActionResult> PairUsers()
        {
            // Fetch all users from the database
            var allUsers = _userManager.Users.ToList();

            // Filter out the admin user and get only regular users
            var regularUsers = allUsers
                .Where(user => !user.UserName.Contains("admin")) 
                .Select(user => user.UserName)
                .ToList();

            if (regularUsers.Count < 2)
                return BadRequest("At least two regular users are required for pairing.");

            var shuffledUsers = Shuffle(regularUsers);

            // Delete previous pairs
            _context.Pairs.RemoveRange(_context.Pairs);
            await _context.SaveChangesAsync();

            var pairs = new List<Pair>();
            for (int i = 0; i < shuffledUsers.Count; i++)
            {
                var giver = shuffledUsers[i];
                var receiver = shuffledUsers[(i + 1) % shuffledUsers.Count]; // Wrap around to the first user
                pairs.Add(new Pair { Giver = giver, Receiver = receiver });
            }

            _context.Pairs.AddRange(pairs);
            await _context.SaveChangesAsync();

            var pairStrings = pairs.Select(p => $"{p.Giver} -> {p.Receiver}").ToList();
            return Ok(pairStrings);
        }

        [HttpGet("getPairs")]
        public async Task<IActionResult> GetPairs()
        {
            var pairs = _context.Pairs.ToList();

            var pairStrings = pairs.Select(p => $"{p.Giver} -> {p.Receiver}").ToList();
            return Ok(pairStrings);
        }

        private List<string> Shuffle(List<string> users)
        {
            var random = new System.Random();
            return users.OrderBy(_ => random.Next()).ToList();
        }
    }
}