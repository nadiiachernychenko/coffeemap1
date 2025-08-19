using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeMap.Data;
using CoffeeMap.Models;

namespace CoffeeMap.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var items = await _context.Reviews
                .Include(r => r.CoffeeShop)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(items);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.CoffeeShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null) return NotFound();

            return View(review);
        }

        // GET: Reviews/Create
        // /Reviews/Create?coffeeShopId=3 -> кофейня заранее выбрана
        public IActionResult Create(int? coffeeShopId)
        {
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", coffeeShopId);
            return View(new Review { CoffeeShopId = coffeeShopId ?? 0 });
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoffeeShopId,Rating,Comment")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", review.CoffeeShopId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", review.CoffeeShopId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoffeeShopId,Rating,Comment,CreatedAt")] Review review)
        {
            if (id != review.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "Id", "Name", review.CoffeeShopId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.CoffeeShop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null) return NotFound();

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
