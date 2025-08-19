using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeMap.Models
{
    public class CoffeeShop
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string OpeningHours { get; set; }
        public string Phone { get; set; }

        // Для загруженной картинки
        public string ImageUrl { get; set; }

        // Связанные сущности
        public ICollection<Product> Products { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}