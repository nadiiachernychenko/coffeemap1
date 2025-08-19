using System;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMap.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMap.Data
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            // Применяем все миграции к базе
            await db.Database.MigrateAsync();

            // Если нет кофейн, добавляем пример
            if (!db.CoffeeShops.Any())
            {
                var shop = new CoffeeShop
                {
                    Name = "Marichka",
                    Address = "Besarabskaya, 1",
                    Lat = 1,
                    Lng = 1,
                    OpeningHours = "8-20",
                    Phone = "0637556233",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3c/Coffee_cup_icon.png" // пример картинки
                };
                db.CoffeeShops.Add(shop);
                await db.SaveChangesAsync();

                // Пример продукта, если сущность Product есть
                if (db.Set<Product>() != null)
                {
                    var prod = new Product
                    {
                        CoffeeShopId = shop.Id,
                        Title = "Капучіно",
                        Category = "Кава",
                        Price = 79.00m,
                        Description = "Класичний капучіно",
                        ImageUrl = null // добавишь через UI загрузкой файла
                    };
                    db.Products.Add(prod);
                }

                // Пример отзыва, если сущность Review есть
                if (db.Set<Review>() != null)
                {
                    var rev = new Review
                    {
                        CoffeeShopId = shop.Id,
                        Rating = 5,
                        Comment = "Вкусно і швидко!",
                        CreatedAt = DateTime.UtcNow
                    };
                    db.Reviews.Add(rev);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
