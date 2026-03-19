namespace LibraryManagement.Exceptions
{
    // Custom Exception 1: Book not available for borrowing
    public class BookNotAvailableException : Exception
    {
        public string BookTitle { get; }

        public BookNotAvailableException(string bookTitle)
            : base($"The book '{bookTitle}' has no available copies for borrowing.")
        {
            BookTitle = bookTitle;
        }
    }

    // Custom Exception 2: Library branch not found
    public class BranchNotFoundException : Exception
    {
        public int BranchId { get; }

        public BranchNotFoundException(int branchId)
            : base($"Library branch with ID {branchId} could not be found.")
        {
            BranchId = branchId;
        }
    }
}