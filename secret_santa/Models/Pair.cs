using System.ComponentModel.DataAnnotations;

namespace secret_santa.Models
{
    public class Pair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Giver { get; set; }

        [Required]
        public string Receiver { get; set; }
    }
}