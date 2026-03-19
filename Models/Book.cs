using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [StringLength(20)]
        public string? ISBN { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }

        [Display(Name = "Published Year")]
        public int? PublishedYear { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }

        [Display(Name = "Total Copies")]
        public int TotalCopies { get; set; } = 1;

        [Display(Name = "Available Copies")]
        public int AvailableCopies { get; set; } = 1;

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Library Branch")]
        public int? LibraryBranchId { get; set; }

        // Navigation properties
        public Author? Author { get; set; }
        public LibraryBranch? LibraryBranch { get; set; }
    }
}
