using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs
{
    // ── Book DTOs ──────────────────────────────────────────────────────────────
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? Genre { get; set; }
        public int? PublishedYear { get; set; }
        public string? Publisher { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int? LibraryBranchId { get; set; }
        public string? BranchName { get; set; }
    }

    public class CreateBookDto
    {
        [Required] [StringLength(200)] public string Title { get; set; } = string.Empty;
        [Required] public int AuthorId { get; set; }
        [StringLength(20)] public string? ISBN { get; set; }
        [StringLength(100)] public string? Genre { get; set; }
        public int? PublishedYear { get; set; }
        [StringLength(100)] public string? Publisher { get; set; }
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;
        [StringLength(500)] public string? Description { get; set; }
        public int? LibraryBranchId { get; set; }
    }

    public class UpdateBookDto
    {
        [Required] [StringLength(200)] public string Title { get; set; } = string.Empty;
        [Required] public int AuthorId { get; set; }
        [StringLength(20)] public string? ISBN { get; set; }
        [StringLength(100)] public string? Genre { get; set; }
        public int? PublishedYear { get; set; }
        [StringLength(100)] public string? Publisher { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        [StringLength(500)] public string? Description { get; set; }
        public int? LibraryBranchId { get; set; }
    }

    // ── Author DTOs ────────────────────────────────────────────────────────────
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? Biography { get; set; }
        public string? Nationality { get; set; }
        public int? BirthYear { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateAuthorDto
    {
        [Required] [StringLength(100)] public string FirstName { get; set; } = string.Empty;
        [Required] [StringLength(100)] public string LastName { get; set; } = string.Empty;
        [StringLength(500)] public string? Biography { get; set; }
        [StringLength(50)] public string? Nationality { get; set; }
        public int? BirthYear { get; set; }
    }

    public class UpdateAuthorDto
    {
        [Required] [StringLength(100)] public string FirstName { get; set; } = string.Empty;
        [Required] [StringLength(100)] public string LastName { get; set; } = string.Empty;
        [StringLength(500)] public string? Biography { get; set; }
        [StringLength(50)] public string? Nationality { get; set; }
        public int? BirthYear { get; set; }
    }

    // ── Customer DTOs ──────────────────────────────────────────────────────────
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime MemberSince { get; set; }
        public bool IsActive { get; set; }
        public int? LibraryBranchId { get; set; }
        public string? BranchName { get; set; }
    }

    public class CreateCustomerDto
    {
        [Required] [StringLength(100)] public string FirstName { get; set; } = string.Empty;
        [Required] [StringLength(100)] public string LastName { get; set; } = string.Empty;
        [Required] [EmailAddress] [StringLength(150)] public string Email { get; set; } = string.Empty;
        [Phone] [StringLength(20)] public string? Phone { get; set; }
        [StringLength(250)] public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public int? LibraryBranchId { get; set; }
    }

    public class UpdateCustomerDto
    {
        [Required] [StringLength(100)] public string FirstName { get; set; } = string.Empty;
        [Required] [StringLength(100)] public string LastName { get; set; } = string.Empty;
        [Required] [EmailAddress] [StringLength(150)] public string Email { get; set; } = string.Empty;
        [Phone] [StringLength(20)] public string? Phone { get; set; }
        [StringLength(250)] public string? Address { get; set; }
        public bool IsActive { get; set; }
        public int? LibraryBranchId { get; set; }
    }

    // ── LibraryBranch DTOs ─────────────────────────────────────────────────────
    public class LibraryBranchDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? OpeningHours { get; set; }
        public string? ManagerName { get; set; }
        public bool IsOpen { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateLibraryBranchDto
    {
        [Required] [StringLength(150)] public string BranchName { get; set; } = string.Empty;
        [Required] [StringLength(250)] public string Address { get; set; } = string.Empty;
        [StringLength(100)] public string? City { get; set; }
        [Phone] [StringLength(20)] public string? Phone { get; set; }
        [EmailAddress] [StringLength(150)] public string? Email { get; set; }
        [StringLength(100)] public string? OpeningHours { get; set; }
        [StringLength(100)] public string? ManagerName { get; set; }
        public bool IsOpen { get; set; } = true;
    }

    public class UpdateLibraryBranchDto
    {
        [Required] [StringLength(150)] public string BranchName { get; set; } = string.Empty;
        [Required] [StringLength(250)] public string Address { get; set; } = string.Empty;
        [StringLength(100)] public string? City { get; set; }
        [Phone] [StringLength(20)] public string? Phone { get; set; }
        [EmailAddress] [StringLength(150)] public string? Email { get; set; }
        [StringLength(100)] public string? OpeningHours { get; set; }
        [StringLength(100)] public string? ManagerName { get; set; }
        public bool IsOpen { get; set; }
    }

    // ── Auth DTOs ──────────────────────────────────────────────────────────────
    public class LoginDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] [MinLength(6)] public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
    }

    // ── Generic API response wrapper ───────────────────────────────────────────
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
