using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace secret_santa.Models
{
    public class Wishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Id is auto-generated
        public int Id { get; set; }

        [Required]
        public string UserEmail { get; set; } 

        [Required]
        public string ItemName { get; set; } 
    }
}