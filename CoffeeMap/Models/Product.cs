using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeMap.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int CoffeeShopId { get; set; }
        public CoffeeShop? CoffeeShop { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; } = "";

        [StringLength(60)]
        public string? Category { get; set; }

        [Range(0, 10000)]
        [Column(TypeName = "decimal(10,2)")] // в SQLite будет REAL, нам ок
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // <<< поле-«файл»: храним ссылку (URL) на загруженную картинку в wwwroot/uploads
        public string? ImageUrl { get; set; }
    }
}
