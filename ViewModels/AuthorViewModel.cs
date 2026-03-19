using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Biography { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [Display(Name = "Birth Year")]
        public int? BirthYear { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Number of Books")]
        public int BookCount { get; set; }
    }
}
