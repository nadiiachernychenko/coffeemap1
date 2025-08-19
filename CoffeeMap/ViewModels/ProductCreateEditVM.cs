using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoffeeMap.ViewModels
{
    public class ProductCreateEditVM
    {
        public int Id { get; set; }

        [Display(Name = "Кав'ярня")]
        [Required]
        public int CoffeeShopId { get; set; }

        [Display(Name = "Назва")]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Категорія")]
        public string? Category { get; set; }

        [Display(Name = "Ціна")]
        [Required]
        public string Price { get; set; } = string.Empty; // <--- строка

        [Display(Name = "Опис")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
