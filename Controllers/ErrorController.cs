using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using LibraryManagement.Exceptions;
using LibraryManagement.Models;
using System.Diagnostics;

namespace LibraryManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features
                .Get<IExceptionHandlerPathFeature>();

            var exception = exceptionFeature?.Error;

            _logger.LogError(exception, "Unhandled exception at path: {Path}",
                exceptionFeature?.Path);

            // Route to specific view based on exception type
            return exception switch
            {
                BookNotAvailableException ex => View("BookNotAvailable",
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = ex.Message,
                        BookTitle = ex.BookTitle
                    }),

                BranchNotFoundException ex => View("BranchNotFound",
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = ex.Message,
                        BranchId = ex.BranchId
                    }),

                _ => View("General",
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = "An unexpected error occurred. Please try again."
                    })
            };
        }
    }
}