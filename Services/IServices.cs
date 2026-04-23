using LibraryManagement.DTOs;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync(string? searchTerm, string? genre);
        Task<BookDto?> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(CreateBookDto dto);
        Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto?> GetByIdAsync(int id);
        Task<AuthorDto> CreateAsync(CreateAuthorDto dto);
        Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync(bool? isActive);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public interface ILibraryBranchService
    {
        Task<IEnumerable<LibraryBranchDto>> GetAllAsync(bool? isOpen);
        Task<LibraryBranchDto?> GetByIdAsync(int id);
        Task<LibraryBranchDto> CreateAsync(CreateLibraryBranchDto dto);
        Task<LibraryBranchDto?> UpdateAsync(int id, UpdateLibraryBranchDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
