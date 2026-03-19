using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Biography = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BirthYear = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BranchName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    OpeningHours = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ManagerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsOpen = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBranches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PublishedYear = table.Column<int>(type: "INTEGER", nullable: true),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TotalCopies = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableCopies = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LibraryBranchId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_LibraryBranches_LibraryBranchId",
                        column: x => x.LibraryBranchId,
                        principalTable: "LibraryBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    MemberSince = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LibraryBranchId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_LibraryBranches_LibraryBranchId",
                        column: x => x.LibraryBranchId,
                        principalTable: "LibraryBranches",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "BirthYear", "FirstName", "LastName", "Nationality" },
                values: new object[,]
                {
                    { 1, "Best known for the Harry Potter fantasy series.", 1965, "J.K.", "Rowling", "British" },
                    { 2, "English novelist known for Nineteen Eighty-Four and Animal Farm.", 1903, "George", "Orwell", "British" },
                    { 3, "Nobel Prize-winning author known for Beloved and Song of Solomon.", 1931, "Toni", "Morrison", "American" },
                    { 4, "Nobel laureate, master of magical realism.", 1927, "Gabriel García", "Márquez", "Colombian" },
                    { 5, "International bestselling author of surrealist fiction.", 1949, "Haruki", "Murakami", "Japanese" },
                    { 6, "Author known for Americanah and Half of a Yellow Sun.", 1977, "Chimamanda Ngozi", "Adichie", "Nigerian" },
                    { 7, "Historian and author of Sapiens.", 1976, "Yuval Noah", "Harari", "Israeli" },
                    { 8, "Prolific author of horror and suspense novels.", 1947, "Stephen", "King", "American" },
                    { 9, "The Queen of Crime, creator of Hercule Poirot.", 1890, "Agatha", "Christie", "British" },
                    { 10, "Nobel laureate known for minimalist prose.", 1899, "Ernest", "Hemingway", "American" },
                    { 11, "Regency-era novelist known for Pride and Prejudice.", 1775, "Jane", "Austen", "British" },
                    { 12, "Russian novelist known for Crime and Punishment.", 1821, "Fyodor", "Dostoevsky", "Russian" },
                    { 13, "Author of War and Peace and Anna Karenina.", 1828, "Leo", "Tolstoy", "Russian" },
                    { 14, "Poet and memoirist known for I Know Why the Caged Bird Sings.", 1928, "Maya", "Angelou", "American" },
                    { 15, "Nobel laureate, author of The Remains of the Day.", 1954, "Kazuo", "Ishiguro", "British-Japanese" },
                    { 16, "Author of No Country for Old Men and The Road.", 1933, "Cormac", "McCarthy", "American" },
                    { 17, "Canadian author known for The Handmaid's Tale.", 1939, "Margaret", "Atwood", "Canadian" },
                    { 18, "Author of The Alchemist, sold over 150 million copies worldwide.", 1947, "Paulo", "Coelho", "Brazilian" },
                    { 19, "Philosopher and author of The Stranger.", 1913, "Albert", "Camus", "French-Algerian" },
                    { 20, "Science fiction author known for the Dune series.", 1920, "Frank", "Herbert", "American" },
                    { 21, "Pioneer of feminist science fiction.", 1929, "Ursula K.", "Le Guin", "American" },
                    { 22, "Author of American Gods and Good Omens.", 1960, "Neil", "Gaiman", "British" }
                });

            migrationBuilder.InsertData(
                table: "LibraryBranches",
                columns: new[] { "Id", "Address", "BranchName", "City", "Email", "IsOpen", "ManagerName", "OpeningHours", "Phone" },
                values: new object[,]
                {
                    { 1, "100 Main Street", "Downtown Central Library", "Vancouver", "downtown@library.ca", true, "Sarah Johnson", "Mon-Fri 9am-8pm, Sat-Sun 10am-5pm", "604-555-0101" },
                    { 2, "250 Robson Street", "West End Branch", "Vancouver", "westend@library.ca", true, "Michael Chen", "Mon-Sat 10am-7pm", "604-555-0102" },
                    { 3, "515 Kingsway Ave", "East Vancouver Branch", "Vancouver", "eastvan@library.ca", true, "Priya Patel", "Mon-Fri 10am-6pm, Sat 10am-4pm", "604-555-0103" },
                    { 4, "1 Lonsdale Ave", "North Shore Library", "North Vancouver", "northshore@library.ca", true, "Robert Wilson", "Mon-Sun 9am-9pm", "604-555-0104" },
                    { 5, "200 Willingdon Ave", "Burnaby Central Branch", "Burnaby", "burnaby@library.ca", true, "Lisa Nguyen", "Mon-Fri 9am-7pm, Sat 10am-5pm", "604-555-0105" },
                    { 6, "7700 Minoru Blvd", "Richmond Branch", "Richmond", "richmond@library.ca", true, "David Park", "Mon-Sat 9am-8pm", "604-555-0106" },
                    { 7, "10350 University Dr", "Surrey City Centre", "Surrey", "surrey@library.ca", true, "Amanda Torres", "Mon-Sun 10am-8pm", "604-555-0107" },
                    { 8, "2425 MacDonald St", "Kitsilano Branch", "Vancouver", "kits@library.ca", true, "James Murphy", "Mon-Sat 10am-6pm", "604-555-0108" },
                    { 9, "575 Poirier St", "Coquitlam Branch", "Coquitlam", "coquitlam@library.ca", true, "Helen Zhang", "Mon-Fri 9am-7pm, Sun 12pm-5pm", "604-555-0109" },
                    { 10, "20382 64A Ave", "Langley Township Branch", "Langley", "langley@library.ca", false, "Kevin Brown", "Mon-Sat 10am-7pm", "604-555-0110" },
                    { 11, "1956 Main Mall", "UBC Campus Branch", "Vancouver", "ubc@library.ca", true, "Dr. Emily Ross", "Mon-Sun 8am-10pm", "604-555-0111" },
                    { 12, "1 Kingsway", "Mount Pleasant Branch", "Vancouver", "mtpleasant@library.ca", true, "Carlos Rivera", "Tue-Sat 10am-6pm", "604-555-0112" },
                    { 13, "716 6th Ave", "New Westminster Branch", "New Westminster", "newwest@library.ca", true, "Sandra Kim", "Mon-Fri 9am-8pm", "604-555-0113" },
                    { 14, "100 Newport Dr", "Port Moody Branch", "Port Moody", "portmoody@library.ca", true, "Nathan Scott", "Mon-Sat 10am-7pm", "604-555-0114" },
                    { 15, "4111 Moncton St", "Steveston Branch", "Richmond", "steveston@library.ca", true, "Grace Lee", "Mon-Sun 10am-5pm", "604-555-0115" },
                    { 16, "4515 Dunbar St", "Dunbar Branch", "Vancouver", "dunbar@library.ca", true, "Frank Mitchell", "Mon-Sat 9am-6pm", "604-555-0116" },
                    { 17, "4885 Capilano Rd", "Capilano Branch", "North Vancouver", "capilano@library.ca", true, "Maria Garcia", "Mon-Fri 10am-7pm", "604-555-0117" },
                    { 18, "130 - 22470 Dewdney Trunk Rd", "Maple Ridge Branch", "Maple Ridge", "mapleridge@library.ca", true, "Paul Thompson", "Mon-Sat 9am-7pm", "604-555-0118" },
                    { 19, "15342 Russell Ave", "White Rock Branch", "White Rock", "whiterock@library.ca", true, "Susan Walker", "Tue-Sat 10am-6pm", "604-555-0119" },
                    { 20, "4695 Clarence Taylor Cres", "Delta Branch", "Delta", "delta@library.ca", true, "Victor Hernandez", "Mon-Fri 9am-8pm, Sat 10am-4pm", "604-555-0120" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AvailableCopies", "Description", "Genre", "ISBN", "LibraryBranchId", "PublishedYear", "Publisher", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 1, 1, 3, "The first book in the Harry Potter series.", "Fantasy", "978-0439708180", 1, 1997, "Bloomsbury", "Harry Potter and the Philosopher's Stone", 5 },
                    { 2, 1, 4, "Harry's second year at Hogwarts.", "Fantasy", "978-0439064873", 2, 1998, "Bloomsbury", "Harry Potter and the Chamber of Secrets", 4 },
                    { 3, 2, 2, "A chilling dystopian novel about totalitarianism.", "Dystopian Fiction", "978-0451524935", 1, 1949, "Secker & Warburg", "Nineteen Eighty-Four", 3 },
                    { 4, 2, 3, "An allegorical novella about the Russian Revolution.", "Political Satire", "978-0451526342", 3, 1945, "Secker & Warburg", "Animal Farm", 3 },
                    { 5, 3, 1, "A Pulitzer Prize-winning novel about slavery's aftermath.", "Historical Fiction", "978-1400033416", 1, 1987, "Alfred A. Knopf", "Beloved", 2 },
                    { 6, 4, 2, "A landmark of magical realism and world literature.", "Magical Realism", "978-0060883287", 2, 1967, "Harper & Row", "One Hundred Years of Solitude", 3 },
                    { 7, 5, 2, "A nostalgic story of loss and sexuality.", "Literary Fiction", "978-0375704024", 4, 1987, "Kodansha", "Norwegian Wood", 2 },
                    { 8, 6, 3, "A story of identity, race, and love.", "Contemporary Fiction", "978-0307455925", 5, 2013, "Knopf", "Americanah", 3 },
                    { 9, 7, 2, "A sweeping history of the human species.", "Non-Fiction", "978-0062316097", 1, 2011, "Harper", "Sapiens: A Brief History of Humankind", 4 },
                    { 10, 8, 3, "A terrifying psychological horror novel.", "Horror", "978-0385121675", 3, 1977, "Doubleday", "The Shining", 3 },
                    { 11, 9, 2, "Hercule Poirot investigates a murder on a train.", "Mystery", "978-0062693662", 6, 1934, "Collins Crime Club", "Murder on the Orient Express", 3 },
                    { 12, 10, 2, "A Pulitzer Prize-winning story of perseverance.", "Literary Fiction", "978-0684801223", 7, 1952, "Charles Scribner's Sons", "The Old Man and the Sea", 2 },
                    { 13, 11, 4, "A classic novel of manners and marriage.", "Classic Romance", "978-0141040349", 8, 1813, "T. Egerton", "Pride and Prejudice", 4 },
                    { 14, 12, 1, "A study of the criminal mind.", "Psychological Fiction", "978-0140449136", 9, 1866, "The Russian Messenger", "Crime and Punishment", 2 },
                    { 15, 13, 2, "An epic tale of Russia during the Napoleonic era.", "Historical Fiction", "978-0199232765", 10, 1869, "The Russian Messenger", "War and Peace", 2 },
                    { 16, 14, 3, "An autobiography of Maya Angelou's early years.", "Autobiography", "978-0345514400", 11, 1969, "Random House", "I Know Why the Caged Bird Sings", 3 },
                    { 17, 15, 2, "A haunting dystopian novel about cloning.", "Dystopian Fiction", "978-1400078776", 12, 2005, "Faber and Faber", "Never Let Me Go", 2 },
                    { 18, 16, 2, "A father and son survive in a post-apocalyptic world.", "Post-Apocalyptic", "978-0307387899", 13, 2006, "Alfred A. Knopf", "The Road", 3 },
                    { 19, 17, 3, "A Canadian dystopian novel about a totalitarian society.", "Dystopian Fiction", "978-0385490818", 1, 1985, "McClelland & Stewart", "The Handmaid's Tale", 4 },
                    { 20, 18, 4, "A journey of self-discovery and following one's dream.", "Philosophical Fiction", "978-0062315007", 2, 1988, "HarperOne", "The Alchemist", 5 },
                    { 21, 19, 2, "An exploration of existentialism and absurdism.", "Philosophical Fiction", "978-0679720201", 3, 1942, "Gallimard", "The Stranger", 2 },
                    { 22, 20, 3, "An epic science fiction saga set in the far future.", "Science Fiction", "978-0441013593", 4, 1965, "Chilton Books", "Dune", 4 },
                    { 23, 21, 2, "A groundbreaking feminist science fiction novel.", "Science Fiction", "978-0441478125", 5, 1969, "Ace Books", "The Left Hand of Darkness", 2 },
                    { 24, 22, 3, "A fantasy novel about old gods vs. new American gods.", "Fantasy", "978-0380973651", 6, 2001, "William Morrow", "American Gods", 3 },
                    { 25, 5, 1, "A surreal parallel narrative.", "Magical Realism", "978-1400079278", 7, 2002, "Shinchosha", "Kafka on the Shore", 2 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsActive", "LastName", "LibraryBranchId", "MemberSince", "Phone" },
                values: new object[,]
                {
                    { 1, "101 Oak St, Vancouver", "alice.thompson@email.com", "Alice", true, "Thompson", 1, new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1001" },
                    { 2, "202 Maple Ave, Vancouver", "bob.martinez@email.com", "Bob", true, "Martinez", 2, new DateTime(2019, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1002" },
                    { 3, "303 Pine Rd, Burnaby", "carol.williams@email.com", "Carol", true, "Williams", 5, new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1003" },
                    { 4, "404 Cedar Dr, Richmond", "david.chen@email.com", "David", true, "Chen", 6, new DateTime(2018, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1004" },
                    { 5, "505 Birch Blvd, Surrey", "eva.rodriguez@email.com", "Eva", true, "Rodriguez", 7, new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1005" },
                    { 6, "606 Spruce St, Vancouver", "frank.lee@email.com", "Frank", false, "Lee", 1, new DateTime(2020, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1006" },
                    { 7, "707 Willow Way, North Vancouver", "grace.kim@email.com", "Grace", true, "Kim", 4, new DateTime(2021, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1007" },
                    { 8, "808 Elm St, Coquitlam", "henry.nguyen@email.com", "Henry", true, "Nguyen", 9, new DateTime(2019, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1008" },
                    { 9, "909 Aspen Ave, Vancouver", "isla.patel@email.com", "Isla", true, "Patel", 8, new DateTime(2023, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1009" },
                    { 10, "1010 Cherry Cres, Vancouver", "james.brown@email.com", "James", true, "Brown", 1, new DateTime(2017, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1010" },
                    { 11, "111 Fir St, Langley", "karen.wilson@email.com", "Karen", true, "Wilson", 10, new DateTime(2022, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1011" },
                    { 12, "222 Alder Rd, New Westminster", "liam.anderson@email.com", "Liam", true, "Anderson", 13, new DateTime(2020, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1012" },
                    { 13, "333 Poplar Pl, Port Moody", "mia.garcia@email.com", "Mia", true, "Garcia", 14, new DateTime(2021, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1013" },
                    { 14, "444 Sycamore St, White Rock", "noah.taylor@email.com", "Noah", false, "Taylor", 19, new DateTime(2019, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1014" },
                    { 15, "555 Magnolia Ave, Delta", "olivia.moore@email.com", "Olivia", true, "Moore", 20, new DateTime(2023, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1015" },
                    { 16, "666 Hickory Rd, Vancouver", "peter.jackson@email.com", "Peter", true, "Jackson", 11, new DateTime(2018, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1016" },
                    { 17, "777 Walnut Way, Maple Ridge", "quinn.davis@email.com", "Quinn", true, "Davis", 18, new DateTime(2022, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1017" },
                    { 18, "888 Juniper St, Vancouver", "rachel.scott@email.com", "Rachel", true, "Scott", 3, new DateTime(2020, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1018" },
                    { 19, "999 Cypress Blvd, Burnaby", "samuel.white@email.com", "Samuel", true, "White", 5, new DateTime(2021, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1019" },
                    { 20, "1111 Redwood Ave, Richmond", "tina.harris@email.com", "Tina", true, "Harris", 6, new DateTime(2022, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1020" },
                    { 21, "1212 Rosewood Rd, Surrey", "uma.clark@email.com", "Uma", true, "Clark", 7, new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1021" },
                    { 22, "1313 Larch St, Vancouver", "victor.lewis@email.com", "Victor", true, "Lewis", 12, new DateTime(2019, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "604-555-1022" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LibraryBranchId",
                table: "Books",
                column: "LibraryBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LibraryBranchId",
                table: "Customers",
                column: "LibraryBranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "LibraryBranches");
        }
    }
}
