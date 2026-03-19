using System.Diagnostics;

namespace LibraryManagement.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? Message { get; set; }

        // For BookNotAvailableException
        public string? BookTitle { get; set; }

        // For BranchNotFoundException
        public int? BranchId { get; set; }
    }
}