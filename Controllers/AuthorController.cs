using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ApplicationDbContext context, ILogger<AuthorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Author
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            try
            {
            _logger.LogInformation("Authors list accessed at {Time}", DateTime.UtcNow);

            var query = _context.Authors.Include(a => a.Books).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(a => a.FirstName.Contains(searchTerm) || a.LastName.Contains(searchTerm));

            var authors = await query.OrderBy(a => a.LastName).ToListAsync();
            ViewBag.SearchTerm = searchTerm;

            return View(authors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving authors list");
            return RedirectToAction("Error", "Home");
        }
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.Books)
                .ThenInclude(b => b.LibraryBranch)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            return View(author);
        }

        // GET: Author/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new AuthorViewModel());
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var author = new Author
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Biography = vm.Biography,
                    Nationality = vm.Nationality,
                    BirthYear = vm.BirthYear
                };
                _context.Add(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Author '{author.FullName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Author/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            var vm = new AuthorViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Biography = author.Biography,
                Nationality = author.Nationality,
                BirthYear = author.BirthYear
            };

            return View(vm);
        }

        // POST: Author/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null) return NotFound();

                author.FirstName = vm.FirstName;
                author.LastName = vm.LastName;
                author.Biography = vm.Biography;
                author.Nationality = vm.Nationality;
                author.BirthYear = vm.BirthYear;

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Author '{author.FullName}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Authors.Any(a => a.Id == id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Author/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            return View(author);
        }

        // POST: Author/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Author '{author.FullName}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
