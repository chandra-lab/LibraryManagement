using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.ViewModels
{
    public class LibraryBranchViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string? City { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Display(Name = "Opening Hours")]
        [StringLength(100)]
        public string? OpeningHours { get; set; }

        [Display(Name = "Manager Name")]
        [StringLength(100)]
        public string? ManagerName { get; set; }

        [Display(Name = "Currently Open")]
        public bool IsOpen { get; set; } = true;

        [Display(Name = "Total Books")]
        public int BookCount { get; set; }

        [Display(Name = "Registered Customers")]
        public int CustomerCount { get; set; }
    }
}
