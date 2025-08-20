using CoffeeMap.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC + Razor
builder.Services.AddControllersWithViews();

// EF Core + SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ������������� ��
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles(); // �����: ��� wwwroot/uploads

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CoffeeShops}/{action=Index}/{id?}");

// ---- ����� ������ ----
//using (var scope = app.Services.CreateScope())
//{
//var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  //  CoffeeMap.Data.AppDbSeeder.SeedAsync(db).GetAwaiter().GetResult();
//

app.Run();
