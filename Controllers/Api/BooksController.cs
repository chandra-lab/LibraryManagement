using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers.Api
{
    /// <summary>
    /// CRUD operations for Books.
    /// GET endpoints are public; POST / PUT / DELETE require a valid JWT bearer token.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>Retrieve all books, optionally filtered by search term or genre.</summary>
        /// <param name="searchTerm">Part of the title or author's last name.</param>
        /// <param name="genre">Exact genre string.</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] string? genre)
        {
            _logger.LogInformation("GET /api/books searchTerm={S} genre={G}", searchTerm, genre);
            var books = await _bookService.GetAllAsync(searchTerm, genre);
            return Ok(ApiResponse<IEnumerable<BookDto>>.Ok(books));
        }

        /// <summary>Retrieve a single book by ID.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound(ApiResponse<object>.Fail($"Book with ID {id} not found."));
            return Ok(ApiResponse<BookDto>.Ok(book));
        }

        /// <summary>Create a new book. Requires authentication.</summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var book = await _bookService.CreateAsync(dto);
            _logger.LogInformation("Book created: {Id} '{Title}'", book.Id, book.Title);
            return CreatedAtAction(nameof(GetById), new { id = book.Id },
                ApiResponse<BookDto>.Ok(book, "Book created successfully."));
        }

        /// <summary>Update an existing book. Requires authentication.</summary>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var book = await _bookService.UpdateAsync(id, dto);
            if (book == null)
                return NotFound(ApiResponse<object>.Fail($"Book with ID {id} not found."));

            return Ok(ApiResponse<BookDto>.Ok(book, "Book updated successfully."));
        }

        /// <summary>Delete a book. Requires authentication.</summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookService.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<object>.Fail($"Book with ID {id} not found."));

            return Ok(ApiResponse<object>.Ok(null, "Book deleted successfully."));
        }
    }
}
