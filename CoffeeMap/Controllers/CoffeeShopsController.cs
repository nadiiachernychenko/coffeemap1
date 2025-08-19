using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMap.Data;
using CoffeeMap.Models;
using CoffeeMap.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMap.Controllers
{
    public class CoffeeShopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CoffeeShopsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var shops = await _context.CoffeeShops.ToListAsync();
            return View(shops);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var shop = await _context.CoffeeShops
                .Include(c => c.Products)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (shop == null) return NotFound();
            return View(shop);
        }

        public IActionResult Create()
        {
            return View(new CoffeeShopCreateEditVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoffeeShopCreateEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string? imageUrl = vm.ImageUrl;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                imageUrl = await SaveImage(vm.ImageFile);
            }

            var shop = new CoffeeShop
            {
                Name = vm.Name,
                Address = vm.Address,
                Lat = vm.Lat,
                Lng = vm.Lng,
                OpeningHours = vm.OpeningHours,
                Phone = vm.Phone,
                ImageUrl = imageUrl
            };

            _context.Add(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var shop = await _context.CoffeeShops.FindAsync(id);
            if (shop == null) return NotFound();

            var vm = new CoffeeShopCreateEditVM
            {
                Id = shop.Id,
                Name = shop.Name,
                Address = shop.Address,
                Lat = shop.Lat,
                Lng = shop.Lng,
                OpeningHours = shop.OpeningHours,
                Phone = shop.Phone,
                ImageUrl = shop.ImageUrl
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoffeeShopCreateEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var shop = await _context.CoffeeShops.FindAsync(id);
            if (shop == null) return NotFound();

            shop.Name = vm.Name;
            shop.Address = vm.Address;
            shop.Lat = vm.Lat;
            shop.Lng = vm.Lng;
            shop.OpeningHours = vm.OpeningHours;
            shop.Phone = vm.Phone;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                shop.ImageUrl = await SaveImage(vm.ImageFile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var shop = await _context.CoffeeShops.FindAsync(id);
            if (shop == null) return NotFound();
            return View(shop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.CoffeeShops.FindAsync(id);
            if (shop != null)
            {
                _context.CoffeeShops.Remove(shop);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "shops");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fname = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fname);

            using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/shops/{fname}";
        }
    }
}
