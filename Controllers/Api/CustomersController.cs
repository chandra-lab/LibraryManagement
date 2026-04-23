using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers.Api
{
    /// <summary>
    /// CRUD operations for Customers.
    /// All endpoints require a valid JWT bearer token.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>Retrieve all customers. Optionally filter by active status.</summary>
        /// <param name="isActive">true = active members only, false = inactive only.</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var customers = await _customerService.GetAllAsync(isActive);
            return Ok(ApiResponse<IEnumerable<CustomerDto>>.Ok(customers));
        }

        /// <summary>Retrieve a single customer by ID.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(ApiResponse<object>.Fail($"Customer with ID {id} not found."));
            return Ok(ApiResponse<CustomerDto>.Ok(customer));
        }

        /// <summary>Create a new customer.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var customer = await _customerService.CreateAsync(dto);
            _logger.LogInformation("Customer created: {Id} '{Name}'", customer.Id, customer.FullName);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id },
                ApiResponse<CustomerDto>.Ok(customer, "Customer created successfully."));
        }

        /// <summary>Update an existing customer.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid payload."));

            var customer = await _customerService.UpdateAsync(id, dto);
            if (customer == null)
                return NotFound(ApiResponse<object>.Fail($"Customer with ID {id} not found."));

            return Ok(ApiResponse<CustomerDto>.Ok(customer, "Customer updated successfully."));
        }

        /// <summary>Delete a customer.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _customerService.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<object>.Fail($"Customer with ID {id} not found."));

            return Ok(ApiResponse<object>.Ok(null, "Customer deleted successfully."));
        }
    }
}
