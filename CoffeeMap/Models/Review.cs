using System.ComponentModel.DataAnnotations;

namespace CoffeeMap.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
