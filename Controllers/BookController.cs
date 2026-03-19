using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Exceptions;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookController> _logger;

        public BookController(ApplicationDbContext context, ILogger<BookController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Book
        public async Task<IActionResult> Index(string searchTerm = "", string genre = "")
        {
            try
            {
            _logger.LogInformation("Books list accessed at {Time}", DateTime.UtcNow);

            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(b => b.Title.Contains(searchTerm) || b.Author!.LastName.Contains(searchTerm));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre == genre);

            var books = await query.OrderBy(b => b.Title).ToListAsync();

            var genres = await _context.Books
                .Where(b => b.Genre != null)
                .Select(b => b.Genre!)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            ViewBag.Genres = new SelectList(genres);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedGenre = genre;

            return View(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving books list");
            return RedirectToAction("Error", "Home");
        }
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.Id == id) ?? throw new BookNotAvailableException("Unknown");

            if (book.AvailableCopies == 0)
            throw new BookNotAvailableException(book.Title);

            return View(book);
        }

        // GET: Book/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var vm = new BookViewModel
            {
                Authors = new SelectList(await _context.Authors.OrderBy(a => a.LastName).ToListAsync(), "Id", "FullName"),
                Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName")
            };
            return View(vm);
        }

        // POST: Book/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = vm.Title,
                    AuthorId = vm.AuthorId,
                    ISBN = vm.ISBN,
                    Genre = vm.Genre,
                    PublishedYear = vm.PublishedYear,
                    Publisher = vm.Publisher,
                    TotalCopies = vm.TotalCopies,
                    AvailableCopies = vm.AvailableCopies,
                    Description = vm.Description,
                    LibraryBranchId = vm.LibraryBranchId
                };
                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Book '{book.Title}' created successfully!";
                return RedirectToAction(nameof(Index));
            }

            vm.Authors = new SelectList(await _context.Authors.OrderBy(a => a.LastName).ToListAsync(), "Id", "FullName", vm.AuthorId);
            vm.Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", vm.LibraryBranchId);
            return View(vm);
        }

        // GET: Book/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var vm = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                ISBN = book.ISBN,
                Genre = book.Genre,
                PublishedYear = book.PublishedYear,
                Publisher = book.Publisher,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Description = book.Description,
                LibraryBranchId = book.LibraryBranchId,
                Authors = new SelectList(await _context.Authors.OrderBy(a => a.LastName).ToListAsync(), "Id", "FullName", book.AuthorId),
                Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", book.LibraryBranchId)
            };

            return View(vm);
        }

        // POST: Book/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null) return NotFound();

                book.Title = vm.Title;
                book.AuthorId = vm.AuthorId;
                book.ISBN = vm.ISBN;
                book.Genre = vm.Genre;
                book.PublishedYear = vm.PublishedYear;
                book.Publisher = vm.Publisher;
                book.TotalCopies = vm.TotalCopies;
                book.AvailableCopies = vm.AvailableCopies;
                book.Description = vm.Description;
                book.LibraryBranchId = vm.LibraryBranchId;

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Book '{book.Title}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(b => b.Id == id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            vm.Authors = new SelectList(await _context.Authors.OrderBy(a => a.LastName).ToListAsync(), "Id", "FullName", vm.AuthorId);
            vm.Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", vm.LibraryBranchId);
            return View(vm);
        }

        // GET: Book/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Book/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Book '{book.Title}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
