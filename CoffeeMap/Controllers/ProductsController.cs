using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMap.Data;
using CoffeeMap.Models;
using CoffeeMap.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMap.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var items = await _context.Products
                .Include(p => p.CoffeeShop)
                .OrderBy(p => p.Title)
                .ToListAsync();
            return View(items);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.CoffeeShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name");
            return View(new ProductCreateEditVM());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateEditVM vm)
        {
            if (!TryParsePrice(vm.Price, out var price))
            {
                ModelState.AddModelError("Price", "Введіть коректну ціну (наприклад: 12,5 або 12.5)");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", vm.CoffeeShopId);
                return View(vm);
            }

            string? imageUrl = null;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                imageUrl = await SaveImage(vm.ImageFile);
            }

            var product = new Product
            {
                CoffeeShopId = vm.CoffeeShopId,
                Title = vm.Title,
                Category = vm.Category,
                Price = price, // decimal
                Description = vm.Description,
                ImageUrl = imageUrl
            };

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            var vm = new ProductCreateEditVM
            {
                Id = p.Id,
                CoffeeShopId = p.CoffeeShopId,
                Title = p.Title,
                Category = p.Category,
                Price = p.Price.ToString(CultureInfo.InvariantCulture),
                Description = p.Description,
                ImageUrl = p.ImageUrl
            };

            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", p.CoffeeShopId);
            return View(vm);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (!TryParsePrice(vm.Price, out var price))
            {
                ModelState.AddModelError("Price", "Введіть коректну ціну (наприклад: 12,5 або 12.5)");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", vm.CoffeeShopId);
                return View(vm);
            }

            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            p.CoffeeShopId = vm.CoffeeShopId;
            p.Title = vm.Title;
            p.Category = vm.Category;
            p.Price = price;
            p.Description = vm.Description;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                p.ImageUrl = await SaveImage(vm.ImageFile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.CoffeeShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fname = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fname);

            using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fname}";
        }

        private bool TryParsePrice(string? input, out decimal price)
        {
            price = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;

            // заменяем запятую на точку
            var normalized = input.Replace(",", ".");
            return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out price);
        }
    }
}
