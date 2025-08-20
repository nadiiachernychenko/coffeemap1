using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMap.Data
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            // Применяем все миграции к базе
            await db.Database.MigrateAsync();

            // Примерная кофейня больше не создается
            // Теперь база останется пустой, если ты сама ничего не добавишь через UI
        }
    }
}
