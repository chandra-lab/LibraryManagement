using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Library Branches
            modelBuilder.Entity<LibraryBranch>().HasData(
                new LibraryBranch { Id = 1, BranchName = "Downtown Central Library", Address = "100 Main Street", City = "Vancouver", Phone = "604-555-0101", Email = "downtown@library.ca", OpeningHours = "Mon-Fri 9am-8pm, Sat-Sun 10am-5pm", ManagerName = "Sarah Johnson", IsOpen = true },
                new LibraryBranch { Id = 2, BranchName = "West End Branch", Address = "250 Robson Street", City = "Vancouver", Phone = "604-555-0102", Email = "westend@library.ca", OpeningHours = "Mon-Sat 10am-7pm", ManagerName = "Michael Chen", IsOpen = true },
                new LibraryBranch { Id = 3, BranchName = "East Vancouver Branch", Address = "515 Kingsway Ave", City = "Vancouver", Phone = "604-555-0103", Email = "eastvan@library.ca", OpeningHours = "Mon-Fri 10am-6pm, Sat 10am-4pm", ManagerName = "Priya Patel", IsOpen = true },
                new LibraryBranch { Id = 4, BranchName = "North Shore Library", Address = "1 Lonsdale Ave", City = "North Vancouver", Phone = "604-555-0104", Email = "northshore@library.ca", OpeningHours = "Mon-Sun 9am-9pm", ManagerName = "Robert Wilson", IsOpen = true },
                new LibraryBranch { Id = 5, BranchName = "Burnaby Central Branch", Address = "200 Willingdon Ave", City = "Burnaby", Phone = "604-555-0105", Email = "burnaby@library.ca", OpeningHours = "Mon-Fri 9am-7pm, Sat 10am-5pm", ManagerName = "Lisa Nguyen", IsOpen = true },
                new LibraryBranch { Id = 6, BranchName = "Richmond Branch", Address = "7700 Minoru Blvd", City = "Richmond", Phone = "604-555-0106", Email = "richmond@library.ca", OpeningHours = "Mon-Sat 9am-8pm", ManagerName = "David Park", IsOpen = true },
                new LibraryBranch { Id = 7, BranchName = "Surrey City Centre", Address = "10350 University Dr", City = "Surrey", Phone = "604-555-0107", Email = "surrey@library.ca", OpeningHours = "Mon-Sun 10am-8pm", ManagerName = "Amanda Torres", IsOpen = true },
                new LibraryBranch { Id = 8, BranchName = "Kitsilano Branch", Address = "2425 MacDonald St", City = "Vancouver", Phone = "604-555-0108", Email = "kits@library.ca", OpeningHours = "Mon-Sat 10am-6pm", ManagerName = "James Murphy", IsOpen = true },
                new LibraryBranch { Id = 9, BranchName = "Coquitlam Branch", Address = "575 Poirier St", City = "Coquitlam", Phone = "604-555-0109", Email = "coquitlam@library.ca", OpeningHours = "Mon-Fri 9am-7pm, Sun 12pm-5pm", ManagerName = "Helen Zhang", IsOpen = true },
                new LibraryBranch { Id = 10, BranchName = "Langley Township Branch", Address = "20382 64A Ave", City = "Langley", Phone = "604-555-0110", Email = "langley@library.ca", OpeningHours = "Mon-Sat 10am-7pm", ManagerName = "Kevin Brown", IsOpen = false },
                new LibraryBranch { Id = 11, BranchName = "UBC Campus Branch", Address = "1956 Main Mall", City = "Vancouver", Phone = "604-555-0111", Email = "ubc@library.ca", OpeningHours = "Mon-Sun 8am-10pm", ManagerName = "Dr. Emily Ross", IsOpen = true },
                new LibraryBranch { Id = 12, BranchName = "Mount Pleasant Branch", Address = "1 Kingsway", City = "Vancouver", Phone = "604-555-0112", Email = "mtpleasant@library.ca", OpeningHours = "Tue-Sat 10am-6pm", ManagerName = "Carlos Rivera", IsOpen = true },
                new LibraryBranch { Id = 13, BranchName = "New Westminster Branch", Address = "716 6th Ave", City = "New Westminster", Phone = "604-555-0113", Email = "newwest@library.ca", OpeningHours = "Mon-Fri 9am-8pm", ManagerName = "Sandra Kim", IsOpen = true },
                new LibraryBranch { Id = 14, BranchName = "Port Moody Branch", Address = "100 Newport Dr", City = "Port Moody", Phone = "604-555-0114", Email = "portmoody@library.ca", OpeningHours = "Mon-Sat 10am-7pm", ManagerName = "Nathan Scott", IsOpen = true },
                new LibraryBranch { Id = 15, BranchName = "Steveston Branch", Address = "4111 Moncton St", City = "Richmond", Phone = "604-555-0115", Email = "steveston@library.ca", OpeningHours = "Mon-Sun 10am-5pm", ManagerName = "Grace Lee", IsOpen = true },
                new LibraryBranch { Id = 16, BranchName = "Dunbar Branch", Address = "4515 Dunbar St", City = "Vancouver", Phone = "604-555-0116", Email = "dunbar@library.ca", OpeningHours = "Mon-Sat 9am-6pm", ManagerName = "Frank Mitchell", IsOpen = true },
                new LibraryBranch { Id = 17, BranchName = "Capilano Branch", Address = "4885 Capilano Rd", City = "North Vancouver", Phone = "604-555-0117", Email = "capilano@library.ca", OpeningHours = "Mon-Fri 10am-7pm", ManagerName = "Maria Garcia", IsOpen = true },
                new LibraryBranch { Id = 18, BranchName = "Maple Ridge Branch", Address = "130 - 22470 Dewdney Trunk Rd", City = "Maple Ridge", Phone = "604-555-0118", Email = "mapleridge@library.ca", OpeningHours = "Mon-Sat 9am-7pm", ManagerName = "Paul Thompson", IsOpen = true },
                new LibraryBranch { Id = 19, BranchName = "White Rock Branch", Address = "15342 Russell Ave", City = "White Rock", Phone = "604-555-0119", Email = "whiterock@library.ca", OpeningHours = "Tue-Sat 10am-6pm", ManagerName = "Susan Walker", IsOpen = true },
                new LibraryBranch { Id = 20, BranchName = "Delta Branch", Address = "4695 Clarence Taylor Cres", City = "Delta", Phone = "604-555-0120", Email = "delta@library.ca", OpeningHours = "Mon-Fri 9am-8pm, Sat 10am-4pm", ManagerName = "Victor Hernandez", IsOpen = true }
            );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "J.K.", LastName = "Rowling", Nationality = "British", BirthYear = 1965, Biography = "Best known for the Harry Potter fantasy series." },
                new Author { Id = 2, FirstName = "George", LastName = "Orwell", Nationality = "British", BirthYear = 1903, Biography = "English novelist known for Nineteen Eighty-Four and Animal Farm." },
                new Author { Id = 3, FirstName = "Toni", LastName = "Morrison", Nationality = "American", BirthYear = 1931, Biography = "Nobel Prize-winning author known for Beloved and Song of Solomon." },
                new Author { Id = 4, FirstName = "Gabriel García", LastName = "Márquez", Nationality = "Colombian", BirthYear = 1927, Biography = "Nobel laureate, master of magical realism." },
                new Author { Id = 5, FirstName = "Haruki", LastName = "Murakami", Nationality = "Japanese", BirthYear = 1949, Biography = "International bestselling author of surrealist fiction." },
                new Author { Id = 6, FirstName = "Chimamanda Ngozi", LastName = "Adichie", Nationality = "Nigerian", BirthYear = 1977, Biography = "Author known for Americanah and Half of a Yellow Sun." },
                new Author { Id = 7, FirstName = "Yuval Noah", LastName = "Harari", Nationality = "Israeli", BirthYear = 1976, Biography = "Historian and author of Sapiens." },
                new Author { Id = 8, FirstName = "Stephen", LastName = "King", Nationality = "American", BirthYear = 1947, Biography = "Prolific author of horror and suspense novels." },
                new Author { Id = 9, FirstName = "Agatha", LastName = "Christie", Nationality = "British", BirthYear = 1890, Biography = "The Queen of Crime, creator of Hercule Poirot." },
                new Author { Id = 10, FirstName = "Ernest", LastName = "Hemingway", Nationality = "American", BirthYear = 1899, Biography = "Nobel laureate known for minimalist prose." },
                new Author { Id = 11, FirstName = "Jane", LastName = "Austen", Nationality = "British", BirthYear = 1775, Biography = "Regency-era novelist known for Pride and Prejudice." },
                new Author { Id = 12, FirstName = "Fyodor", LastName = "Dostoevsky", Nationality = "Russian", BirthYear = 1821, Biography = "Russian novelist known for Crime and Punishment." },
                new Author { Id = 13, FirstName = "Leo", LastName = "Tolstoy", Nationality = "Russian", BirthYear = 1828, Biography = "Author of War and Peace and Anna Karenina." },
                new Author { Id = 14, FirstName = "Maya", LastName = "Angelou", Nationality = "American", BirthYear = 1928, Biography = "Poet and memoirist known for I Know Why the Caged Bird Sings." },
                new Author { Id = 15, FirstName = "Kazuo", LastName = "Ishiguro", Nationality = "British-Japanese", BirthYear = 1954, Biography = "Nobel laureate, author of The Remains of the Day." },
                new Author { Id = 16, FirstName = "Cormac", LastName = "McCarthy", Nationality = "American", BirthYear = 1933, Biography = "Author of No Country for Old Men and The Road." },
                new Author { Id = 17, FirstName = "Margaret", LastName = "Atwood", Nationality = "Canadian", BirthYear = 1939, Biography = "Canadian author known for The Handmaid's Tale." },
                new Author { Id = 18, FirstName = "Paulo", LastName = "Coelho", Nationality = "Brazilian", BirthYear = 1947, Biography = "Author of The Alchemist, sold over 150 million copies worldwide." },
                new Author { Id = 19, FirstName = "Albert", LastName = "Camus", Nationality = "French-Algerian", BirthYear = 1913, Biography = "Philosopher and author of The Stranger." },
                new Author { Id = 20, FirstName = "Frank", LastName = "Herbert", Nationality = "American", BirthYear = 1920, Biography = "Science fiction author known for the Dune series." },
                new Author { Id = 21, FirstName = "Ursula K.", LastName = "Le Guin", Nationality = "American", BirthYear = 1929, Biography = "Pioneer of feminist science fiction." },
                new Author { Id = 22, FirstName = "Neil", LastName = "Gaiman", Nationality = "British", BirthYear = 1960, Biography = "Author of American Gods and Good Omens." }
            );

            // Seed Books (20+ genuine books)
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone", AuthorId = 1, ISBN = "978-0439708180", Genre = "Fantasy", PublishedYear = 1997, Publisher = "Bloomsbury", TotalCopies = 5, AvailableCopies = 3, LibraryBranchId = 1, Description = "The first book in the Harry Potter series." },
                new Book { Id = 2, Title = "Harry Potter and the Chamber of Secrets", AuthorId = 1, ISBN = "978-0439064873", Genre = "Fantasy", PublishedYear = 1998, Publisher = "Bloomsbury", TotalCopies = 4, AvailableCopies = 4, LibraryBranchId = 2, Description = "Harry's second year at Hogwarts." },
                new Book { Id = 3, Title = "Nineteen Eighty-Four", AuthorId = 2, ISBN = "978-0451524935", Genre = "Dystopian Fiction", PublishedYear = 1949, Publisher = "Secker & Warburg", TotalCopies = 3, AvailableCopies = 2, LibraryBranchId = 1, Description = "A chilling dystopian novel about totalitarianism." },
                new Book { Id = 4, Title = "Animal Farm", AuthorId = 2, ISBN = "978-0451526342", Genre = "Political Satire", PublishedYear = 1945, Publisher = "Secker & Warburg", TotalCopies = 3, AvailableCopies = 3, LibraryBranchId = 3, Description = "An allegorical novella about the Russian Revolution." },
                new Book { Id = 5, Title = "Beloved", AuthorId = 3, ISBN = "978-1400033416", Genre = "Historical Fiction", PublishedYear = 1987, Publisher = "Alfred A. Knopf", TotalCopies = 2, AvailableCopies = 1, LibraryBranchId = 1, Description = "A Pulitzer Prize-winning novel about slavery's aftermath." },
                new Book { Id = 6, Title = "One Hundred Years of Solitude", AuthorId = 4, ISBN = "978-0060883287", Genre = "Magical Realism", PublishedYear = 1967, Publisher = "Harper & Row", TotalCopies = 3, AvailableCopies = 2, LibraryBranchId = 2, Description = "A landmark of magical realism and world literature." },
                new Book { Id = 7, Title = "Norwegian Wood", AuthorId = 5, ISBN = "978-0375704024", Genre = "Literary Fiction", PublishedYear = 1987, Publisher = "Kodansha", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 4, Description = "A nostalgic story of loss and sexuality." },
                new Book { Id = 8, Title = "Americanah", AuthorId = 6, ISBN = "978-0307455925", Genre = "Contemporary Fiction", PublishedYear = 2013, Publisher = "Knopf", TotalCopies = 3, AvailableCopies = 3, LibraryBranchId = 5, Description = "A story of identity, race, and love." },
                new Book { Id = 9, Title = "Sapiens: A Brief History of Humankind", AuthorId = 7, ISBN = "978-0062316097", Genre = "Non-Fiction", PublishedYear = 2011, Publisher = "Harper", TotalCopies = 4, AvailableCopies = 2, LibraryBranchId = 1, Description = "A sweeping history of the human species." },
                new Book { Id = 10, Title = "The Shining", AuthorId = 8, ISBN = "978-0385121675", Genre = "Horror", PublishedYear = 1977, Publisher = "Doubleday", TotalCopies = 3, AvailableCopies = 3, LibraryBranchId = 3, Description = "A terrifying psychological horror novel." },
                new Book { Id = 11, Title = "Murder on the Orient Express", AuthorId = 9, ISBN = "978-0062693662", Genre = "Mystery", PublishedYear = 1934, Publisher = "Collins Crime Club", TotalCopies = 3, AvailableCopies = 2, LibraryBranchId = 6, Description = "Hercule Poirot investigates a murder on a train." },
                new Book { Id = 12, Title = "The Old Man and the Sea", AuthorId = 10, ISBN = "978-0684801223", Genre = "Literary Fiction", PublishedYear = 1952, Publisher = "Charles Scribner's Sons", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 7, Description = "A Pulitzer Prize-winning story of perseverance." },
                new Book { Id = 13, Title = "Pride and Prejudice", AuthorId = 11, ISBN = "978-0141040349", Genre = "Classic Romance", PublishedYear = 1813, Publisher = "T. Egerton", TotalCopies = 4, AvailableCopies = 4, LibraryBranchId = 8, Description = "A classic novel of manners and marriage." },
                new Book { Id = 14, Title = "Crime and Punishment", AuthorId = 12, ISBN = "978-0140449136", Genre = "Psychological Fiction", PublishedYear = 1866, Publisher = "The Russian Messenger", TotalCopies = 2, AvailableCopies = 1, LibraryBranchId = 9, Description = "A study of the criminal mind." },
                new Book { Id = 15, Title = "War and Peace", AuthorId = 13, ISBN = "978-0199232765", Genre = "Historical Fiction", PublishedYear = 1869, Publisher = "The Russian Messenger", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 10, Description = "An epic tale of Russia during the Napoleonic era." },
                new Book { Id = 16, Title = "I Know Why the Caged Bird Sings", AuthorId = 14, ISBN = "978-0345514400", Genre = "Autobiography", PublishedYear = 1969, Publisher = "Random House", TotalCopies = 3, AvailableCopies = 3, LibraryBranchId = 11, Description = "An autobiography of Maya Angelou's early years." },
                new Book { Id = 17, Title = "Never Let Me Go", AuthorId = 15, ISBN = "978-1400078776", Genre = "Dystopian Fiction", PublishedYear = 2005, Publisher = "Faber and Faber", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 12, Description = "A haunting dystopian novel about cloning." },
                new Book { Id = 18, Title = "The Road", AuthorId = 16, ISBN = "978-0307387899", Genre = "Post-Apocalyptic", PublishedYear = 2006, Publisher = "Alfred A. Knopf", TotalCopies = 3, AvailableCopies = 2, LibraryBranchId = 13, Description = "A father and son survive in a post-apocalyptic world." },
                new Book { Id = 19, Title = "The Handmaid's Tale", AuthorId = 17, ISBN = "978-0385490818", Genre = "Dystopian Fiction", PublishedYear = 1985, Publisher = "McClelland & Stewart", TotalCopies = 4, AvailableCopies = 3, LibraryBranchId = 1, Description = "A Canadian dystopian novel about a totalitarian society." },
                new Book { Id = 20, Title = "The Alchemist", AuthorId = 18, ISBN = "978-0062315007", Genre = "Philosophical Fiction", PublishedYear = 1988, Publisher = "HarperOne", TotalCopies = 5, AvailableCopies = 4, LibraryBranchId = 2, Description = "A journey of self-discovery and following one's dream." },
                new Book { Id = 21, Title = "The Stranger", AuthorId = 19, ISBN = "978-0679720201", Genre = "Philosophical Fiction", PublishedYear = 1942, Publisher = "Gallimard", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 3, Description = "An exploration of existentialism and absurdism." },
                new Book { Id = 22, Title = "Dune", AuthorId = 20, ISBN = "978-0441013593", Genre = "Science Fiction", PublishedYear = 1965, Publisher = "Chilton Books", TotalCopies = 4, AvailableCopies = 3, LibraryBranchId = 4, Description = "An epic science fiction saga set in the far future." },
                new Book { Id = 23, Title = "The Left Hand of Darkness", AuthorId = 21, ISBN = "978-0441478125", Genre = "Science Fiction", PublishedYear = 1969, Publisher = "Ace Books", TotalCopies = 2, AvailableCopies = 2, LibraryBranchId = 5, Description = "A groundbreaking feminist science fiction novel." },
                new Book { Id = 24, Title = "American Gods", AuthorId = 22, ISBN = "978-0380973651", Genre = "Fantasy", PublishedYear = 2001, Publisher = "William Morrow", TotalCopies = 3, AvailableCopies = 3, LibraryBranchId = 6, Description = "A fantasy novel about old gods vs. new American gods." },
                new Book { Id = 25, Title = "Kafka on the Shore", AuthorId = 5, ISBN = "978-1400079278", Genre = "Magical Realism", PublishedYear = 2002, Publisher = "Shinchosha", TotalCopies = 2, AvailableCopies = 1, LibraryBranchId = 7, Description = "A surreal parallel narrative." }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FirstName = "Alice", LastName = "Thompson", Email = "alice.thompson@email.com", Phone = "604-555-1001", Address = "101 Oak St, Vancouver", MemberSince = new DateTime(2020, 3, 15), IsActive = true, LibraryBranchId = 1 },
                new Customer { Id = 2, FirstName = "Bob", LastName = "Martinez", Email = "bob.martinez@email.com", Phone = "604-555-1002", Address = "202 Maple Ave, Vancouver", MemberSince = new DateTime(2019, 7, 20), IsActive = true, LibraryBranchId = 2 },
                new Customer { Id = 3, FirstName = "Carol", LastName = "Williams", Email = "carol.williams@email.com", Phone = "604-555-1003", Address = "303 Pine Rd, Burnaby", MemberSince = new DateTime(2021, 1, 10), IsActive = true, LibraryBranchId = 5 },
                new Customer { Id = 4, FirstName = "David", LastName = "Chen", Email = "david.chen@email.com", Phone = "604-555-1004", Address = "404 Cedar Dr, Richmond", MemberSince = new DateTime(2018, 11, 5), IsActive = true, LibraryBranchId = 6 },
                new Customer { Id = 5, FirstName = "Eva", LastName = "Rodriguez", Email = "eva.rodriguez@email.com", Phone = "604-555-1005", Address = "505 Birch Blvd, Surrey", MemberSince = new DateTime(2022, 6, 1), IsActive = true, LibraryBranchId = 7 },
                new Customer { Id = 6, FirstName = "Frank", LastName = "Lee", Email = "frank.lee@email.com", Phone = "604-555-1006", Address = "606 Spruce St, Vancouver", MemberSince = new DateTime(2020, 9, 14), IsActive = false, LibraryBranchId = 1 },
                new Customer { Id = 7, FirstName = "Grace", LastName = "Kim", Email = "grace.kim@email.com", Phone = "604-555-1007", Address = "707 Willow Way, North Vancouver", MemberSince = new DateTime(2021, 4, 22), IsActive = true, LibraryBranchId = 4 },
                new Customer { Id = 8, FirstName = "Henry", LastName = "Nguyen", Email = "henry.nguyen@email.com", Phone = "604-555-1008", Address = "808 Elm St, Coquitlam", MemberSince = new DateTime(2019, 12, 30), IsActive = true, LibraryBranchId = 9 },
                new Customer { Id = 9, FirstName = "Isla", LastName = "Patel", Email = "isla.patel@email.com", Phone = "604-555-1009", Address = "909 Aspen Ave, Vancouver", MemberSince = new DateTime(2023, 2, 8), IsActive = true, LibraryBranchId = 8 },
                new Customer { Id = 10, FirstName = "James", LastName = "Brown", Email = "james.brown@email.com", Phone = "604-555-1010", Address = "1010 Cherry Cres, Vancouver", MemberSince = new DateTime(2017, 5, 19), IsActive = true, LibraryBranchId = 1 },
                new Customer { Id = 11, FirstName = "Karen", LastName = "Wilson", Email = "karen.wilson@email.com", Phone = "604-555-1011", Address = "111 Fir St, Langley", MemberSince = new DateTime(2022, 8, 3), IsActive = true, LibraryBranchId = 10 },
                new Customer { Id = 12, FirstName = "Liam", LastName = "Anderson", Email = "liam.anderson@email.com", Phone = "604-555-1012", Address = "222 Alder Rd, New Westminster", MemberSince = new DateTime(2020, 10, 27), IsActive = true, LibraryBranchId = 13 },
                new Customer { Id = 13, FirstName = "Mia", LastName = "Garcia", Email = "mia.garcia@email.com", Phone = "604-555-1013", Address = "333 Poplar Pl, Port Moody", MemberSince = new DateTime(2021, 3, 16), IsActive = true, LibraryBranchId = 14 },
                new Customer { Id = 14, FirstName = "Noah", LastName = "Taylor", Email = "noah.taylor@email.com", Phone = "604-555-1014", Address = "444 Sycamore St, White Rock", MemberSince = new DateTime(2019, 6, 11), IsActive = false, LibraryBranchId = 19 },
                new Customer { Id = 15, FirstName = "Olivia", LastName = "Moore", Email = "olivia.moore@email.com", Phone = "604-555-1015", Address = "555 Magnolia Ave, Delta", MemberSince = new DateTime(2023, 1, 25), IsActive = true, LibraryBranchId = 20 },
                new Customer { Id = 16, FirstName = "Peter", LastName = "Jackson", Email = "peter.jackson@email.com", Phone = "604-555-1016", Address = "666 Hickory Rd, Vancouver", MemberSince = new DateTime(2018, 4, 9), IsActive = true, LibraryBranchId = 11 },
                new Customer { Id = 17, FirstName = "Quinn", LastName = "Davis", Email = "quinn.davis@email.com", Phone = "604-555-1017", Address = "777 Walnut Way, Maple Ridge", MemberSince = new DateTime(2022, 11, 14), IsActive = true, LibraryBranchId = 18 },
                new Customer { Id = 18, FirstName = "Rachel", LastName = "Scott", Email = "rachel.scott@email.com", Phone = "604-555-1018", Address = "888 Juniper St, Vancouver", MemberSince = new DateTime(2020, 7, 6), IsActive = true, LibraryBranchId = 3 },
                new Customer { Id = 19, FirstName = "Samuel", LastName = "White", Email = "samuel.white@email.com", Phone = "604-555-1019", Address = "999 Cypress Blvd, Burnaby", MemberSince = new DateTime(2021, 9, 30), IsActive = true, LibraryBranchId = 5 },
                new Customer { Id = 20, FirstName = "Tina", LastName = "Harris", Email = "tina.harris@email.com", Phone = "604-555-1020", Address = "1111 Redwood Ave, Richmond", MemberSince = new DateTime(2022, 4, 18), IsActive = true, LibraryBranchId = 6 },
                new Customer { Id = 21, FirstName = "Uma", LastName = "Clark", Email = "uma.clark@email.com", Phone = "604-555-1021", Address = "1212 Rosewood Rd, Surrey", MemberSince = new DateTime(2023, 5, 7), IsActive = true, LibraryBranchId = 7 },
                new Customer { Id = 22, FirstName = "Victor", LastName = "Lewis", Email = "victor.lewis@email.com", Phone = "604-555-1022", Address = "1313 Larch St, Vancouver", MemberSince = new DateTime(2019, 2, 14), IsActive = true, LibraryBranchId = 12 }
            );
        }
    }
}
