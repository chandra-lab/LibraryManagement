using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers.Api
{
    /// <summary>
    /// CRUD operations for Authors.
    /// GET endpoints are public; POST / PUT / DELETE require a valid JWT bearer token.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        /// <summary>Retrieve all authors.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AuthorDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<AuthorDto>>.Ok(authors));
        }

        /// <summary>Retrieve a single author by ID (includes book count).</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
                return NotFound(ApiResponse<object>.Fail($"Author with ID {id} not found."));
            return Ok(ApiResponse<AuthorDto>.Ok(author));
        }

        /// <summary>Create a new author. Requires authentication.</summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var author = await _authorService.CreateAsync(dto);
            _logger.LogInformation("Author created: {Id} '{Name}'", author.Id, author.FullName);
            return CreatedAtAction(nameof(GetById), new { id = author.Id },
                ApiResponse<AuthorDto>.Ok(author, "Author created successfully."));
        }

        /// <summary>Update an existing author. Requires authentication.</summary>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var author = await _authorService.UpdateAsync(id, dto);
            if (author == null)
                return NotFound(ApiResponse<object>.Fail($"Author with ID {id} not found."));

            return Ok(ApiResponse<AuthorDto>.Ok(author, "Author updated successfully."));
        }

        /// <summary>Delete an author. Requires authentication.</summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _authorService.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<object>.Fail($"Author with ID {id} not found."));

            return Ok(ApiResponse<object>.Ok(null, "Author deleted successfully."));
        }
    }
}
