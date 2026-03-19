using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.Exceptions;

namespace LibraryManagement.Controllers
{
    public class LibraryBranchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LibraryBranchController> _logger;

        public LibraryBranchController(ApplicationDbContext context, ILogger<LibraryBranchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: LibraryBranch
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            try
            {
            _logger.LogInformation("Library branches list accessed at {Time}", DateTime.UtcNow);
            var query = _context.LibraryBranches
                .Include(lb => lb.Books)
                .Include(lb => lb.Customers)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(lb => lb.BranchName.Contains(searchTerm) || (lb.City != null && lb.City.Contains(searchTerm)));

            var branches = await query.OrderBy(lb => lb.BranchName).ToListAsync();
            ViewBag.SearchTerm = searchTerm;

            return View(branches);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving library branches list");
            return RedirectToAction("Error", "Home");
        }
        }

        // GET: LibraryBranch/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var branch = await _context.LibraryBranches
                .Include(lb => lb.Books).ThenInclude(b => b.Author)
                .Include(lb => lb.Customers)
                .FirstOrDefaultAsync(lb => lb.Id == id) ?? throw new BranchNotFoundException(id.Value);

            return View(branch);
        }

        // GET: LibraryBranch/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new LibraryBranchViewModel());
        }

        // POST: LibraryBranch/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryBranchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var branch = new LibraryBranch
                {
                    BranchName = vm.BranchName,
                    Address = vm.Address,
                    City = vm.City,
                    Phone = vm.Phone,
                    Email = vm.Email,
                    OpeningHours = vm.OpeningHours,
                    ManagerName = vm.ManagerName,
                    IsOpen = vm.IsOpen
                };
                _context.Add(branch);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Branch '{branch.BranchName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: LibraryBranch/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var branch = await _context.LibraryBranches.FindAsync(id) ?? throw new BranchNotFoundException(id.Value);
            if (branch == null) return NotFound();

            var vm = new LibraryBranchViewModel
            {
                Id = branch.Id,
                BranchName = branch.BranchName,
                Address = branch.Address,
                City = branch.City,
                Phone = branch.Phone,
                Email = branch.Email,
                OpeningHours = branch.OpeningHours,
                ManagerName = branch.ManagerName,
                IsOpen = branch.IsOpen
            };

            return View(vm);
        }

        // POST: LibraryBranch/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibraryBranchViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var branch = await _context.LibraryBranches.FindAsync(id);
                if (branch == null) return NotFound();

                branch.BranchName = vm.BranchName;
                branch.Address = vm.Address;
                branch.City = vm.City;
                branch.Phone = vm.Phone;
                branch.Email = vm.Email;
                branch.OpeningHours = vm.OpeningHours;
                branch.ManagerName = vm.ManagerName;
                branch.IsOpen = vm.IsOpen;

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Branch '{branch.BranchName}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.LibraryBranches.Any(lb => lb.Id == id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: LibraryBranch/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var branch = await _context.LibraryBranches
                .Include(lb => lb.Books)
                .Include(lb => lb.Customers)
                .FirstOrDefaultAsync(lb => lb.Id == id);

            if (branch == null) return NotFound();

            return View(branch);
        }

        // POST: LibraryBranch/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.LibraryBranches.FindAsync(id);
            if (branch != null)
            {
                _context.LibraryBranches.Remove(branch);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Branch '{branch.BranchName}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
