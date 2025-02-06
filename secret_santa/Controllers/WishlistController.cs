using Microsoft.AspNetCore.Mvc;
using secret_santa.Models;
using secret_santa.Context;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using secret_santa.Models.DTO;


namespace secret_santa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        public async Task<IActionResult> DeleteWishlistItem(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Item deleted from wishlist!" });
        }
    }
}