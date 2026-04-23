using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    // ── BookService ────────────────────────────────────────────────────────────
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _db;
        public BookService(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<BookDto>> GetAllAsync(string? searchTerm, string? genre)
        {
            var query = _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(b => b.Title.Contains(searchTerm)
                                      || (b.Author != null && b.Author.LastName.Contains(searchTerm)));

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(b => b.Genre == genre);

            return await query.OrderBy(b => b.Title)
                .Select(b => MapToDto(b))
                .ToListAsync();
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var b = await _db.Books
                .Include(x => x.Author)
                .Include(x => x.LibraryBranch)
                .FirstOrDefaultAsync(x => x.Id == id);
            return b == null ? null : MapToDto(b);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                ISBN = dto.ISBN,
                Genre = dto.Genre,
                PublishedYear = dto.PublishedYear,
                Publisher = dto.Publisher,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies,
                Description = dto.Description,
                LibraryBranchId = dto.LibraryBranchId
            };
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            await _db.Entry(book).Reference(b => b.Author).LoadAsync();
            await _db.Entry(book).Reference(b => b.LibraryBranch).LoadAsync();
            return MapToDto(book);
        }

        public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return null;
            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;
            book.ISBN = dto.ISBN;
            book.Genre = dto.Genre;
            book.PublishedYear = dto.PublishedYear;
            book.Publisher = dto.Publisher;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;
            book.Description = dto.Description;
            book.LibraryBranchId = dto.LibraryBranchId;
            await _db.SaveChangesAsync();
            await _db.Entry(book).Reference(b => b.Author).LoadAsync();
            await _db.Entry(book).Reference(b => b.LibraryBranch).LoadAsync();
            return MapToDto(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return false;
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }

        private static BookDto MapToDto(Book b) => new()
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            Genre = b.Genre,
            PublishedYear = b.PublishedYear,
            Publisher = b.Publisher,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies,
            Description = b.Description,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.FullName,
            LibraryBranchId = b.LibraryBranchId,
            BranchName = b.LibraryBranch?.BranchName
        };
    }

    // ── AuthorService ──────────────────────────────────────────────────────────
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _db;
        public AuthorService(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<AuthorDto>> GetAllAsync() =>
            await _db.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.LastName)
                .Select(a => MapToDto(a))
                .ToListAsync();

        public async Task<AuthorDto?> GetByIdAsync(int id)
        {
            var a = await _db.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
            return a == null ? null : MapToDto(a);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Biography = dto.Biography,
                Nationality = dto.Nationality,
                BirthYear = dto.BirthYear
            };
            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
            return MapToDto(author);
        }

        public async Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorDto dto)
        {
            var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return null;
            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Biography = dto.Biography;
            author.Nationality = dto.Nationality;
            author.BirthYear = dto.BirthYear;
            await _db.SaveChangesAsync();
            return MapToDto(author);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author == null) return false;
            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();
            return true;
        }

        private static AuthorDto MapToDto(Author a) => new()
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Biography = a.Biography,
            Nationality = a.Nationality,
            BirthYear = a.BirthYear,
            BookCount = a.Books?.Count ?? 0
        };
    }

    // ── CustomerService ────────────────────────────────────────────────────────
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;
        public CustomerService(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(bool? isActive)
        {
            var query = _db.Customers.Include(c => c.LibraryBranch).AsQueryable();
            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);
            return await query.OrderBy(c => c.LastName).Select(c => MapToDto(c)).ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var c = await _db.Customers.Include(x => x.LibraryBranch).FirstOrDefaultAsync(x => x.Id == id);
            return c == null ? null : MapToDto(c);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = dto.IsActive,
                LibraryBranchId = dto.LibraryBranchId,
                MemberSince = DateTime.Today
            };
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            await _db.Entry(customer).Reference(c => c.LibraryBranch).LoadAsync();
            return MapToDto(customer);
        }

        public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _db.Customers.Include(c => c.LibraryBranch).FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null) return null;
            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            customer.IsActive = dto.IsActive;
            customer.LibraryBranchId = dto.LibraryBranchId;
            await _db.SaveChangesAsync();
            return MapToDto(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return false;
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return true;
        }

        private static CustomerDto MapToDto(Customer c) => new()
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            MemberSince = c.MemberSince,
            IsActive = c.IsActive,
            LibraryBranchId = c.LibraryBranchId,
            BranchName = c.LibraryBranch?.BranchName
        };
    }

    // ── LibraryBranchService ───────────────────────────────────────────────────
    public class LibraryBranchService : ILibraryBranchService
    {
        private readonly ApplicationDbContext _db;
        public LibraryBranchService(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<LibraryBranchDto>> GetAllAsync(bool? isOpen)
        {
            var query = _db.LibraryBranches.Include(b => b.Books).AsQueryable();
            if (isOpen.HasValue)
                query = query.Where(b => b.IsOpen == isOpen.Value);
            return await query.OrderBy(b => b.BranchName).Select(b => MapToDto(b)).ToListAsync();
        }

        public async Task<LibraryBranchDto?> GetByIdAsync(int id)
        {
            var b = await _db.LibraryBranches.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
            return b == null ? null : MapToDto(b);
        }

        public async Task<LibraryBranchDto> CreateAsync(CreateLibraryBranchDto dto)
        {
            var branch = new LibraryBranch
            {
                BranchName = dto.BranchName,
                Address = dto.Address,
                City = dto.City,
                Phone = dto.Phone,
                Email = dto.Email,
                OpeningHours = dto.OpeningHours,
                ManagerName = dto.ManagerName,
                IsOpen = dto.IsOpen
            };
            _db.LibraryBranches.Add(branch);
            await _db.SaveChangesAsync();
            return MapToDto(branch);
        }

        public async Task<LibraryBranchDto?> UpdateAsync(int id, UpdateLibraryBranchDto dto)
        {
            var branch = await _db.LibraryBranches.Include(b => b.Books).FirstOrDefaultAsync(b => b.Id == id);
            if (branch == null) return null;
            branch.BranchName = dto.BranchName;
            branch.Address = dto.Address;
            branch.City = dto.City;
            branch.Phone = dto.Phone;
            branch.Email = dto.Email;
            branch.OpeningHours = dto.OpeningHours;
            branch.ManagerName = dto.ManagerName;
            branch.IsOpen = dto.IsOpen;
            await _db.SaveChangesAsync();
            return MapToDto(branch);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _db.LibraryBranches.FindAsync(id);
            if (branch == null) return false;
            _db.LibraryBranches.Remove(branch);
            await _db.SaveChangesAsync();
            return true;
        }

        private static LibraryBranchDto MapToDto(LibraryBranch b) => new()
        {
            Id = b.Id,
            BranchName = b.BranchName,
            Address = b.Address,
            City = b.City,
            Phone = b.Phone,
            Email = b.Email,
            OpeningHours = b.OpeningHours,
            ManagerName = b.ManagerName,
            IsOpen = b.IsOpen,
            BookCount = b.Books?.Count ?? 0
        };
    }
}
