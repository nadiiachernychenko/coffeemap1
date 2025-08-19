using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoffeeMap.ViewModels
{
    public class CoffeeShopCreateEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язково")]
        public string Name { get; set; }

        public string? Address { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? OpeningHours { get; set; }
        public string? Phone { get; set; }

        // ссылка на сохранённую картинку
        public string? ImageUrl { get; set; }

        // файл, который пользователь загружает
        public IFormFile? ImageFile { get; set; }
    }
}
