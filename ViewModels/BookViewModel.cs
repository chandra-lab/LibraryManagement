using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.ViewModels
{
    public class BookViewModel
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
        [Range(1, 1000)]
        public int TotalCopies { get; set; } = 1;

        [Display(Name = "Available Copies")]
        [Range(0, 1000)]
        public int AvailableCopies { get; set; } = 1;

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Library Branch")]
        public int? LibraryBranchId { get; set; }

        // For display
        public string? AuthorName { get; set; }
        public string? BranchName { get; set; }

        // For dropdowns
        public SelectList? Authors { get; set; }
        public SelectList? Branches { get; set; }
    }
}
