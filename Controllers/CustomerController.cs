using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchTerm = "", bool? activeOnly = null)
        {
            try
            {
            _logger.LogInformation("Customers list accessed at {Time}", DateTime.UtcNow);
            var query = _context.Customers.Include(c => c.LibraryBranch).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm) || c.Email.Contains(searchTerm));

            if (activeOnly == true)
                query = query.Where(c => c.IsActive);

            var customers = await query.OrderBy(c => c.LastName).ToListAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.ActiveOnly = activeOnly;

            return View(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers list");
            return RedirectToAction("Error", "Home");
        }
        }   

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.LibraryBranch)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: Customer/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var vm = new CustomerViewModel
            {
                Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName")
            };
            return View(vm);
        }

        // POST: Customer/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    Phone = vm.Phone,
                    Address = vm.Address,
                    MemberSince = vm.MemberSince,
                    IsActive = vm.IsActive,
                    LibraryBranchId = vm.LibraryBranchId
                };
                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Customer '{customer.FullName}' registered successfully!";
                return RedirectToAction(nameof(Index));
            }

            vm.Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", vm.LibraryBranchId);
            return View(vm);
        }

        // GET: Customer/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var vm = new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                MemberSince = customer.MemberSince,
                IsActive = customer.IsActive,
                LibraryBranchId = customer.LibraryBranchId,
                Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", customer.LibraryBranchId)
            };

            return View(vm);
        }

        // POST: Customer/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                customer.FirstName = vm.FirstName;
                customer.LastName = vm.LastName;
                customer.Email = vm.Email;
                customer.Phone = vm.Phone;
                customer.Address = vm.Address;
                customer.MemberSince = vm.MemberSince;
                customer.IsActive = vm.IsActive;
                customer.LibraryBranchId = vm.LibraryBranchId;

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Customer '{customer.FullName}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(c => c.Id == id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            vm.Branches = new SelectList(await _context.LibraryBranches.OrderBy(b => b.BranchName).ToListAsync(), "Id", "BranchName", vm.LibraryBranchId);
            return View(vm);
        }

        // GET: Customer/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .Include(c => c.LibraryBranch)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customer/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Customer '{customer.FullName}' removed successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
