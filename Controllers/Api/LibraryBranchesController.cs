using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers.Api
{
    /// <summary>
    /// CRUD operations for Library Branches.
    /// GET endpoints are public; POST / PUT / DELETE require a valid JWT bearer token.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LibraryBranchesController : ControllerBase
    {
        private readonly ILibraryBranchService _branchService;
        private readonly ILogger<LibraryBranchesController> _logger;

        public LibraryBranchesController(ILibraryBranchService branchService, ILogger<LibraryBranchesController> logger)
        {
            _branchService = branchService;
            _logger = logger;
        }

        /// <summary>Retrieve all library branches, optionally filtered by open status.</summary>
        /// <param name="isOpen">true = open branches only, false = closed branches only.</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LibraryBranchDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isOpen)
        {
            var branches = await _branchService.GetAllAsync(isOpen);
            return Ok(ApiResponse<IEnumerable<LibraryBranchDto>>.Ok(branches));
        }

        /// <summary>Retrieve a single branch by ID (includes book count).</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<LibraryBranchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _branchService.GetByIdAsync(id);
            if (branch == null)
                return NotFound(ApiResponse<object>.Fail($"Branch with ID {id} not found."));
            return Ok(ApiResponse<LibraryBranchDto>.Ok(branch));
        }

        /// <summary>Create a new library branch. Requires authentication.</summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LibraryBranchDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateLibraryBranchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var branch = await _branchService.CreateAsync(dto);
            _logger.LogInformation("Branch created: {Id} '{Name}'", branch.Id, branch.BranchName);
            return CreatedAtAction(nameof(GetById), new { id = branch.Id },
                ApiResponse<LibraryBranchDto>.Ok(branch, "Branch created successfully."));
        }

        /// <summary>Update an existing library branch. Requires authentication.</summary>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LibraryBranchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLibraryBranchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var branch = await _branchService.UpdateAsync(id, dto);
            if (branch == null)
                return NotFound(ApiResponse<object>.Fail($"Branch with ID {id} not found."));

            return Ok(ApiResponse<LibraryBranchDto>.Ok(branch, "Branch updated successfully."));
        }

        /// <summary>Delete a library branch. Requires authentication.</summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _branchService.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<object>.Fail($"Branch with ID {id} not found."));

            return Ok(ApiResponse<object>.Ok(null, "Branch deleted successfully."));
        }
    }
}
