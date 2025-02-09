using Microsoft.AspNetCore.Mvc;
using secret_santa.Models;
using secret_santa.Context;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using secret_santa.Models.DTO;

using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Authorization;


namespace secret_santa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist(string userEmail)
        {
            var wishlist = await _context.Wishlists
                .Where(w => w.UserEmail == userEmail)
                .ToListAsync();

            return Ok(wishlist);
        }

        [HttpPost]
        [Authorize]  
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserEmail = User.Identity.Name; 
            var isAdmin = User.IsInRole("ADMIN"); 

            if (request.UserEmail != currentUserEmail && !isAdmin)
            {
                return Forbid(); 
            }

            var wishlistItem = new Wishlist
            {
                UserEmail = request.UserEmail,
                ItemName = request.ItemName
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to wishlist!" });
        }

        [HttpDelete("{id}")]
        [Authorize] 
        public async Task<IActionResult> DeleteWishlistItem(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            var currentUserEmail = User.Identity.Name; 
            var isAdmin = User.IsInRole("ADMIN"); 

            if (wishlistItem.UserEmail != currentUserEmail && !isAdmin)
            {
                return Forbid(); 
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Item deleted from wishlist!" });
        }
    }
}