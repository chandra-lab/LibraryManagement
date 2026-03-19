using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        // Inject ILogger (Lecture 6)
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
            _logger.LogInformation("Dashboard visited at {Time}", DateTime.UtcNow);

            ViewBag.BookCount = await _context.Books.CountAsync();
            ViewBag.AuthorCount = await _context.Authors.CountAsync();
            ViewBag.CustomerCount = await _context.Customers.CountAsync();
            ViewBag.BranchCount = await _context.LibraryBranches.CountAsync();
            ViewBag.AvailableBooks = await _context.Books.Where(b => b.AvailableCopies > 0).CountAsync();
            return View();
        }   
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dashboard");
            return RedirectToAction("Error", "Home");
        }
        } 

        // Error handler action (Lecture 6)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error,
                    "Unhandled exception at path: {Path}", exceptionFeature.Path);
            }

            ViewBag.StatusCode = statusCode;
            ViewBag.ErrorMessage = statusCode switch
            {
                404 => "Page not found.",
                403 => "You are not authorized to access this page.",
                500 => "An internal server error occurred.",
                _ => "An unexpected error occurred."
            };

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}