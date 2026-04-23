# Library Management System
### CSCI 6809 · Fairleigh Dickinson University Vancouver · Winter 2026

---

## Table of Contents

1. [Project Overview](#1-project-overview)
2. [Prerequisites & Setup](#2-prerequisites--setup)
3. [Full Folder Structure](#3-full-folder-structure)
4. [How the API Is Mapped — Complete Explanation](#4-how-the-api-is-mapped--complete-explanation)
5. [Running the Application](#5-running-the-application)
6. [Swagger UI — Step-by-Step Testing Guide](#6-swagger-ui--step-by-step-testing-guide)
7. [Manual API Testing with curl](#7-manual-api-testing-with-curl)
8. [Detailed Testing Steps — Unit & Integration Tests](#8-detailed-testing-steps--unit--integration-tests)
9. [All Test Cases Explained](#9-all-test-cases-explained)
10. [JSON Payload Reference](#10-json-payload-reference)
11. [API Response Format](#11-api-response-format)
12. [Security Design](#12-security-design)
13. [Configuration Reference](#13-configuration-reference)

---

## 1. Project Overview

This is the **Project C** (final) extension of the Library Management Application. Building on the MVC web application from Projects A and B, this deliverable adds:

| Feature | What was added |
|---|---|
| REST API | 4 resource controllers + 1 auth controller, all returning JSON |
| JWT Security | Bearer token authentication on all write operations |
| Swagger UI | Auto-generated interactive API documentation at `/swagger` |
| Service Layer | `IBookService`, `IAuthorService`, etc. — interfaces that sit between controllers and the database |
| Unit Tests | ~42 tests using in-memory EF Core and Moq |
| Integration Tests | ~20 tests using `WebApplicationFactory` — full HTTP request pipeline |

The original MVC web interface (Views, Razor Pages, cookie login) from Projects A and B is fully preserved and still works at the root URL.

---

## 2. Prerequisites & Setup

### Required

- **.NET 10 SDK** — https://dotnet.microsoft.com/download/dotnet/10.0

### Verify Installation

```bash
dotnet --version
# Should print 10.x.x
```

### No database setup needed

The SQLite database file (`Database/library.db`) is already included in the project with seed data: 22 authors, 40+ books, 20 library branches, and sample customers.

---

## 3. Full Folder Structure

```
LibraryManagement.sln                          ← Solution file (2 projects)
│
├── LibraryManagement/                         ← Main ASP.NET Core web application
│   │
│   ├── Controllers/
│   │   ├── AuthorController.cs                ← MVC: /Author (HTML views)
│   │   ├── BookController.cs                  ← MVC: /Book (HTML views)
│   │   ├── CustomerController.cs              ← MVC: /Customer (HTML views)
│   │   ├── LibraryBranchController.cs         ← MVC: /LibraryBranch (HTML views)
│   │   ├── HomeController.cs                  ← MVC: / (home page)
│   │   ├── ErrorController.cs                 ← MVC: custom error pages
│   │   └── Api/                               ← ★ NEW: REST API controllers
│   │       ├── AuthController.cs              ← POST /api/auth/register + /login
│   │       ├── BooksController.cs             ← CRUD /api/books
│   │       ├── AuthorsController.cs           ← CRUD /api/authors
│   │       ├── CustomersController.cs         ← CRUD /api/customers (all auth-guarded)
│   │       └── LibraryBranchesController.cs   ← CRUD /api/librarybranches
│   │
│   ├── Services/                              ← ★ NEW: service layer
│   │   ├── IServices.cs                       ← Interfaces (IBookService, etc.)
│   │   └── Services.cs                        ← Concrete implementations
│   │
│   ├── DTOs/                                  ← ★ NEW: API data shapes
│   │   └── ApiDTOs.cs                         ← BookDto, CreateBookDto, LoginDto, etc.
│   │
│   ├── Models/                                ← EF Core entity classes (from Projects A/B)
│   │   ├── Author.cs
│   │   ├── Book.cs
│   │   ├── Customer.cs
│   │   └── LibraryBranch.cs
│   │
│   ├── Data/
│   │   └── ApplicationDbContext.cs            ← EF Core DbContext + seed data
│   │
│   ├── Migrations/                            ← EF Core migration history
│   ├── ViewModels/                            ← MVC form view-models
│   ├── Views/                                 ← Razor HTML views (Projects A/B)
│   ├── Areas/Identity/                        ← ASP.NET Identity login/register pages
│   ├── Database/
│   │   └── library.db                         ← SQLite database file (included)
│   ├── Program.cs                             ← ★ UPDATED: JWT + Swagger registration
│   ├── appsettings.json                       ← ★ UPDATED: JWT config added
│   └── LibraryManagement.csproj              ← ★ UPDATED: JWT + Swagger packages added
│
└── LibraryManagement.Tests/                   ← ★ NEW: dedicated test project
    ├── Unit/
    │   ├── BookServiceTests.cs                ← 12 service-layer unit tests
    │   ├── AuthorServiceTests.cs              ← 10 service-layer unit tests
    │   ├── BooksControllerTests.cs            ← 10 controller unit tests (Moq)
    │   └── CustomerAndBranchServiceTests.cs   ← 20 service-layer unit tests
    ├── Integration/
    │   ├── LibraryWebApplicationFactory.cs    ← Test host configuration
    │   ├── BooksApiIntegrationTests.cs        ← 8 full HTTP pipeline tests
    │   └── AuthorsAndBranchesApiIntegrationTests.cs  ← 12 HTTP pipeline tests
    └── LibraryManagement.Tests.csproj
```

---

## 4. How the API Is Mapped — Complete Explanation

This section explains the exact mechanism by which HTTP requests reach API actions — from URL to method.

### 4.1 The Two Controller Families

The project has **two types of controllers** that coexist:

| Type | Base Class | Purpose | URL Pattern |
|---|---|---|---|
| MVC Controllers | `Controller` | Serve HTML pages | `/Book`, `/Author`, `/Customer` |
| API Controllers | `ControllerBase` | Return JSON only | `/api/books`, `/api/authors` |

The key difference is that `ControllerBase` (used by API controllers) has no view-rendering support, keeping the API layer clean and lightweight.

---

### 4.2 How Route Attributes Work

Every API controller has two key attributes at the class level:

```csharp
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
```

**`[ApiController]`** activates several API-specific behaviours:
- Automatic `400 Bad Request` if model validation fails (you don't need to check `ModelState` manually)
- Infers that parameters come from the request body (`[FromBody]`) for POST/PUT automatically
- Problem Details responses for errors

**`[Route("api/[controller]")]`** defines the URL prefix. The `[controller]` token is replaced at runtime with the controller's class name minus the word "Controller":
- `BooksController` → `/api/books`
- `AuthorsController` → `/api/authors`
- `CustomersController` → `/api/customers`
- `LibraryBranchesController` → `/api/librarybranches`
- `AuthController` → `/api/auth`

---

### 4.3 How HTTP Methods Map to Actions

Inside each controller, individual methods are decorated with HTTP verb attributes that complete the route:

```csharp
// Maps to:  GET  /api/books
[HttpGet]
public async Task<IActionResult> GetAll(...)

// Maps to:  GET  /api/books/5
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)

// Maps to:  POST /api/books
[HttpPost]
[Authorize]
public async Task<IActionResult> Create([FromBody] CreateBookDto dto)

// Maps to:  PUT  /api/books/5
[HttpPut("{id:int}")]
[Authorize]
public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)

// Maps to:  DELETE /api/books/5
[HttpDelete("{id:int}")]
[Authorize]
public async Task<IActionResult> Delete(int id)
```

The `{id:int}` part is a **route constraint** — ASP.NET Core only matches this route if the segment is a valid integer, and automatically binds it to the `int id` parameter.

---

### 4.4 Complete URL Routing Table

| HTTP Method | URL | Controller | Action | Auth Required |
|---|---|---|---|---|
| POST | `/api/auth/register` | AuthController | Register | No |
| POST | `/api/auth/login` | AuthController | Login | No |
| GET | `/api/books` | BooksController | GetAll | No |
| GET | `/api/books?searchTerm=Harry` | BooksController | GetAll | No |
| GET | `/api/books?genre=Fantasy` | BooksController | GetAll | No |
| GET | `/api/books/{id}` | BooksController | GetById | No |
| POST | `/api/books` | BooksController | Create | **Yes (JWT)** |
| PUT | `/api/books/{id}` | BooksController | Update | **Yes (JWT)** |
| DELETE | `/api/books/{id}` | BooksController | Delete | **Yes (JWT)** |
| GET | `/api/authors` | AuthorsController | GetAll | No |
| GET | `/api/authors/{id}` | AuthorsController | GetById | No |
| POST | `/api/authors` | AuthorsController | Create | **Yes (JWT)** |
| PUT | `/api/authors/{id}` | AuthorsController | Update | **Yes (JWT)** |
| DELETE | `/api/authors/{id}` | AuthorsController | Delete | **Yes (JWT)** |
| GET | `/api/customers` | CustomersController | GetAll | **Yes (JWT)** |
| GET | `/api/customers?isActive=true` | CustomersController | GetAll | **Yes (JWT)** |
| GET | `/api/customers/{id}` | CustomersController | GetById | **Yes (JWT)** |
| POST | `/api/customers` | CustomersController | Create | **Yes (JWT)** |
| PUT | `/api/customers/{id}` | CustomersController | Update | **Yes (JWT)** |
| DELETE | `/api/customers/{id}` | CustomersController | Delete | **Yes (JWT)** |
| GET | `/api/librarybranches` | LibraryBranchesController | GetAll | No |
| GET | `/api/librarybranches?isOpen=true` | LibraryBranchesController | GetAll | No |
| GET | `/api/librarybranches/{id}` | LibraryBranchesController | GetById | No |
| POST | `/api/librarybranches` | LibraryBranchesController | Create | **Yes (JWT)** |
| PUT | `/api/librarybranches/{id}` | LibraryBranchesController | Update | **Yes (JWT)** |
| DELETE | `/api/librarybranches/{id}` | LibraryBranchesController | Delete | **Yes (JWT)** |

---

### 4.5 How the Service Layer Is Wired (Dependency Injection)

The API controllers never touch the database directly. They depend on service interfaces:

```
HTTP Request
     │
     ▼
BooksController            ← depends on IBookService (injected by DI)
     │
     ▼
BookService                ← implements IBookService
     │
     ▼
ApplicationDbContext       ← EF Core context
     │
     ▼
library.db (SQLite)        ← actual data
```

The wiring happens in `Program.cs`:

```csharp
builder.Services.AddScoped<IBookService,          BookService>();
builder.Services.AddScoped<IAuthorService,        AuthorService>();
builder.Services.AddScoped<ICustomerService,      CustomerService>();
builder.Services.AddScoped<ILibraryBranchService, LibraryBranchService>();
```

`AddScoped` means one instance is created per HTTP request, then disposed. This is the standard pattern for database-touching services.

This architecture is what makes unit testing possible: in tests, `IBookService` is replaced by a Moq mock, so the controller tests never need a real database.

---

### 4.6 How DTOs Protect the Data Layer

Models (e.g., `Book`) are EF Core entities — they map 1:1 to database tables. DTOs (Data Transfer Objects) are separate classes that define exactly what comes in and goes out of the API.

```
POST /api/books  →  CreateBookDto  →  BookService.CreateAsync()  →  Book (entity saved to DB)
                                                                         │
GET  /api/books  ←  BookDto        ←  BookService.GetAllAsync()  ←  Book (entity read from DB)
```

This means:
- The API never accidentally exposes internal fields (like foreign key IDs without names)
- Input validation (`[Required]`, `[StringLength]`) lives on the DTO, not the entity
- `BookDto` enriches the response with computed fields like `AuthorName` and `BranchName`

---

### 4.7 Request Flow — Tracing a Single Request End-to-End

Example: `GET https://localhost:7147/api/books?searchTerm=Harry`

```
1. HTTPS request arrives at the ASP.NET Core Kestrel server

2. Middleware pipeline runs in order (Program.cs):
   UseHttpsRedirection → UseStaticFiles → UseRouting
   → UseAuthentication → UseAuthorization

3. Router matches the URL to BooksController.GetAll()
   because: route = "api/[controller]" + [HttpGet] = "api/books"

4. No [Authorize] on GetAll → skips JWT check

5. ASP.NET Core constructs BooksController:
   - Injects IBookService (resolves to BookService instance)
   - Injects ILogger<BooksController>

6. BooksController.GetAll("Harry", null) is called

7. BookService.GetAllAsync("Harry", null) queries the DB:
   SELECT * FROM Books WHERE Title LIKE '%Harry%' OR Author.LastName LIKE '%Harry%'

8. Results mapped from Book entities → List<BookDto>

9. Controller wraps in ApiResponse<IEnumerable<BookDto>> { Success = true, Data = [...] }

10. OkObjectResult serialized to JSON, sent back with 200 OK
```

---

## 5. Running the Application

### Step 1 — Navigate to the main project

```bash
cd LibraryManagement
```

### Step 2 — Restore NuGet packages

```bash
dotnet restore
```

This downloads all packages including `Swashbuckle.AspNetCore` (Swagger), `Microsoft.AspNetCore.Authentication.JwtBearer`, and `Moq` for tests.

### Step 3 — Run the application

```bash
dotnet run
```

You will see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7147
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5035
```

### Step 4 — Open the app

| URL | What it is |
|---|---|
| `https://localhost:7147` | Main MVC web application (home page) |
| `https://localhost:7147/swagger` | **Swagger API documentation UI** |
| `https://localhost:7147/api/books` | Raw JSON API response |

> If you get an HTTPS certificate error, run: `dotnet dev-certs https --trust`

---

## 6. Swagger UI — Step-by-Step Testing Guide

Swagger UI lets you test every API endpoint directly in the browser without any extra tools.

### Step 1 — Open Swagger

Navigate to: `https://localhost:7147/swagger`

You will see all 5 groups of endpoints: **Auth**, **Books**, **Authors**, **Customers**, **LibraryBranches**.

---

### Step 2 — Register a New User

1. Expand **Auth** → `POST /api/auth/register`
2. Click **Try it out**
3. Paste this into the request body:
   ```json
   {
     "username": "admin",
     "email": "admin@library.ca",
     "password": "Admin123"
   }
   ```
4. Click **Execute**
5. You should see `200 OK` with a `token` in the response

---

### Step 3 — Login and Copy the Token

1. Expand **Auth** → `POST /api/auth/login`
2. Click **Try it out**
3. Enter:
   ```json
   {
     "username": "admin",
     "password": "Admin123"
   }
   ```
4. Click **Execute**
5. In the response, find the `"token"` field — copy the entire value (it starts with `eyJ`)

---

### Step 4 — Authorize in Swagger

1. Click the **Authorize** button (🔒) at the top right of the Swagger page
2. In the dialog, type: `Bearer ` (with a space), then paste your token
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
3. Click **Authorize**, then **Close**

All protected endpoints will now use your token automatically.

---

### Step 5 — Test GET All Books (Public)

1. Expand **Books** → `GET /api/books`
2. Click **Try it out**
3. Leave filters empty, click **Execute**
4. You should see `200 OK` with a list of books from the seeded database

**With a filter:**
- Enter `Fantasy` in the `genre` field, click Execute → returns only Fantasy books
- Enter `Harry` in `searchTerm`, click Execute → returns books with "Harry" in title or author name

---

### Step 6 — Test GET Book by ID

1. Expand `GET /api/books/{id}`
2. Click **Try it out**
3. Enter `1` in the `id` field
4. Click **Execute** → returns the first book
5. Enter `9999` → returns `404 Not Found`

---

### Step 7 — Test POST Create a Book (Requires Auth)

1. Expand `POST /api/books`
2. Click **Try it out**
3. Enter this payload:
   ```json
   {
     "title": "The Great Gatsby",
     "authorId": 1,
     "isbn": "978-0743273565",
     "genre": "Classic Fiction",
     "publishedYear": 1925,
     "publisher": "Scribner",
     "totalCopies": 4,
     "availableCopies": 4,
     "description": "A story of the Jazz Age.",
     "libraryBranchId": 1
   }
   ```
4. Click **Execute**
5. Expected: `201 Created` with the new book in the response (including the assigned `id`)

**Without authorization:** Log out of Swagger (click Authorize → Logout) and repeat — you will get `401 Unauthorized`.

---

### Step 8 — Test PUT Update a Book

1. Expand `PUT /api/books/{id}`
2. Click **Try it out**
3. Enter `1` in the `id` field
4. Modify a field in the body (e.g., change `availableCopies` from 3 to 2):
   ```json
   {
     "title": "Harry Potter and the Philosopher's Stone",
     "authorId": 1,
     "isbn": "978-0439708180",
     "genre": "Fantasy",
     "publishedYear": 1997,
     "publisher": "Bloomsbury",
     "totalCopies": 5,
     "availableCopies": 2,
     "description": "The first book in the Harry Potter series.",
     "libraryBranchId": 1
   }
   ```
5. Click **Execute** → `200 OK` with updated book

---

### Step 9 — Test DELETE a Book

1. Expand `DELETE /api/books/{id}`
2. Click **Try it out**
3. Enter an `id` you created in Step 7
4. Click **Execute** → `200 OK` with success message

---

### Step 10 — Test Customers (All Endpoints Require Auth)

1. Expand **Customers** → `GET /api/customers`
2. Click **Try it out** → Execute
3. Returns all customers (requires you to be authorized from Step 4)

**Filter by active status:**
- Set `isActive` to `true` → only active members
- Set `isActive` to `false` → only inactive members

---

### Step 11 — Test Library Branches

1. `GET /api/librarybranches?isOpen=true` → only open branches (public endpoint)
2. `GET /api/librarybranches/1` → Downtown Central Library details with book count
3. `POST /api/librarybranches` (requires auth):
   ```json
   {
     "branchName": "Test Branch",
     "address": "999 Test Ave",
     "city": "Vancouver",
     "phone": "604-555-0200",
     "email": "test@library.ca",
     "openingHours": "Mon-Fri 9am-5pm",
     "managerName": "Test Manager",
     "isOpen": true
   }
   ```

---

## 7. Manual API Testing with curl

If you prefer the terminal, here are ready-to-run curl commands.

### Register

```bash
curl -k -X POST https://localhost:7147/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","email":"admin@library.ca","password":"Admin123"}'
```

### Login — save the token

```bash
TOKEN=$(curl -k -s -X POST https://localhost:7147/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123"}' \
  | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
echo $TOKEN
```

### GET all books

```bash
curl -k https://localhost:7147/api/books
```

### GET books filtered

```bash
curl -k "https://localhost:7147/api/books?searchTerm=Harry&genre=Fantasy"
```

### POST create a book (with auth)

```bash
curl -k -X POST https://localhost:7147/api/books \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "New Book Title",
    "authorId": 1,
    "genre": "Fiction",
    "totalCopies": 3,
    "availableCopies": 3
  }'
```

### PUT update a book

```bash
curl -k -X PUT https://localhost:7147/api/books/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Harry Potter and the Philosopher'\''s Stone",
    "authorId": 1,
    "genre": "Fantasy",
    "totalCopies": 5,
    "availableCopies": 2,
    "publishedYear": 1997,
    "publisher": "Bloomsbury"
  }'
```

### DELETE a book

```bash
curl -k -X DELETE https://localhost:7147/api/books/5 \
  -H "Authorization: Bearer $TOKEN"
```

### GET customers (requires auth)

```bash
curl -k https://localhost:7147/api/customers \
  -H "Authorization: Bearer $TOKEN"
```

---

## 8. Detailed Testing Steps — Unit & Integration Tests

### 8.1 Navigate to the Test Project

```bash
cd LibraryManagement.Tests
```

### 8.2 Restore Test Dependencies

```bash
dotnet restore
```

This downloads: `xunit`, `Moq`, `FluentAssertions`, `Microsoft.AspNetCore.Mvc.Testing`, and `Microsoft.EntityFrameworkCore.InMemory`.

### 8.3 Run All Tests

```bash
dotnet test
```

Expected output:
```
Passed!  - Failed: 0, Passed: 72, Skipped: 0, Total: 72
```

---

### 8.4 Run with Detailed Output (See Each Test Name)

```bash
dotnet test --logger "console;verbosity=detailed"
```

Sample output:
```
[PASS] BookServiceTests.GetAllAsync_NoFilter_ReturnsAllBooks
[PASS] BookServiceTests.GetAllAsync_SearchByTitle_ReturnsMatchingBooks
[PASS] BookServiceTests.GetByIdAsync_ExistingId_ReturnsCorrectBook
[PASS] BooksControllerTests.GetById_ExistingId_ReturnsOkWithBook
[PASS] BooksApiIntegrationTests.GET_Books_ReturnsOkWithSeededBooks
...
```

---

### 8.5 Run Only Unit Tests

```bash
dotnet test --filter "FullyQualifiedName~Unit"
```

### 8.6 Run Only Integration Tests

```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

### 8.7 Run Tests for a Specific Class

```bash
# Only BookService tests
dotnet test --filter "ClassName=LibraryManagement.Tests.Unit.BookServiceTests"

# Only controller tests
dotnet test --filter "ClassName=LibraryManagement.Tests.Unit.BooksControllerTests"

# Only integration tests for books
dotnet test --filter "ClassName=LibraryManagement.Tests.Integration.BooksApiIntegrationTests"
```

### 8.8 Run a Single Specific Test

```bash
dotnet test --filter "FullyQualifiedName=LibraryManagement.Tests.Unit.BookServiceTests.GetAllAsync_NoFilter_ReturnsAllBooks"
```

### 8.9 Generate a Test Results Report

```bash
dotnet test --logger "trx;LogFileName=TestResults.trx"
# Results saved to: TestResults/TestResults.trx (XML format, viewable in Visual Studio)
```

---

## 9. All Test Cases Explained

### Unit Tests — BookService (`BookServiceTests.cs`)

These 12 tests use an **in-memory EF Core database** (unique per test via `Guid.NewGuid()`) so they are fully isolated and never touch `library.db`.

| Test Name | What it verifies |
|---|---|
| `GetAllAsync_NoFilter_ReturnsAllBooks` | Seeding 3 books → GetAll returns 3 |
| `GetAllAsync_SearchByTitle_ReturnsMatchingBooks` | searchTerm="Harry" → returns 1 matching book |
| `GetAllAsync_FilterByGenre_ReturnsMatchingBooks` | genre="Fantasy" → returns only Fantasy books |
| `GetAllAsync_NoMatchFound_ReturnsEmptyList` | searchTerm="Nonexistent XYZ" → empty list, no crash |
| `GetByIdAsync_ExistingId_ReturnsCorrectBook` | GetById(1) → correct title and ID |
| `GetByIdAsync_NonExistingId_ReturnsNull` | GetById(999) → null (controller will 404) |
| `CreateAsync_ValidDto_CreatesAndReturnsBook` | Create with valid DTO → new record in DB |
| `CreateAsync_SetsPropertiesCorrectly` | All DTO fields copied to saved entity correctly |
| `UpdateAsync_ExistingBook_UpdatesAndReturnsBook` | Update(1, dto) → title and copies changed |
| `UpdateAsync_NonExistingBook_ReturnsNull` | Update(999, dto) → null (controller will 404) |
| `DeleteAsync_ExistingBook_ReturnsTrueAndRemovesBook` | Delete(1) → true, DB count drops from 3 to 2 |
| `DeleteAsync_NonExistingBook_ReturnsFalse` | Delete(999) → false, DB count unchanged |

---

### Unit Tests — AuthorService (`AuthorServiceTests.cs`)

10 tests validating the author business logic and computed properties.

| Test Name | What it verifies |
|---|---|
| `GetAllAsync_ReturnsAllAuthors` | 3 seeded authors → 3 returned |
| `GetAllAsync_ReturnsAuthorsOrderedByLastName` | Result sorted: Morrison, Orwell, Rowling |
| `GetAllAsync_IncludesBookCount` | Rowling has 1 seeded book → BookCount = 1 |
| `GetByIdAsync_ExistingId_ReturnsAuthor` | GetById(1) → firstName = "J.K." |
| `GetByIdAsync_NonExistingId_ReturnsNull` | GetById(999) → null |
| `CreateAsync_ValidDto_PersistsAndReturnsAuthor` | New author saved, ID assigned |
| `CreateAsync_FullNameComputedCorrectly` | "Paulo" + "Coelho" → FullName = "Paulo Coelho" |
| `UpdateAsync_ExistingAuthor_UpdatesFields` | Biography field updated correctly |
| `UpdateAsync_NonExistingAuthor_ReturnsNull` | Update(999, dto) → null |
| `DeleteAsync_ExistingAuthor_ReturnsTrueAndRemoves` | Delete(2) → true, count drops |

---

### Unit Tests — BooksController (`BooksControllerTests.cs`)

These 10 tests use **Moq** to mock `IBookService` completely. The controller is tested in pure isolation — no database, no HTTP server.

```csharp
// How the mock is set up in each test:
_mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);
// Then the controller is called directly:
var actionResult = await _sut.GetById(1);
// Then the response is inspected:
actionResult.Should().BeOfType<OkObjectResult>();
```

| Test Name | What it verifies |
|---|---|
| `GetAll_ReturnsOkWithBooks` | 200 OK with 2 books when service returns 2 |
| `GetAll_WithSearchTerm_PassesTermToService` | Controller passes searchTerm="Harry" to service unchanged |
| `GetById_ExistingId_ReturnsOkWithBook` | 200 OK, body contains correct title |
| `GetById_NonExistingId_ReturnsNotFound` | 404 Not Found, body.Success = false, message contains "999" |
| `Create_ValidDto_ReturnsCreatedWithBook` | 201 Created, Location header set, body.Data.Id = 5 |
| `Create_InvalidModelState_ReturnsBadRequest` | 400 Bad Request when ModelState has an error |
| `Update_ExistingBook_ReturnsOkWithUpdatedBook` | 200 OK, body contains updated title |
| `Update_NonExistingBook_ReturnsNotFound` | 404 when service returns null |
| `Delete_ExistingBook_ReturnsOk` | 200 OK, body.Success = true |
| `Delete_NonExistingBook_ReturnsNotFound` | 404 when service returns false |
| `Delete_CallsServiceExactlyOnce` | Verifies service.DeleteAsync was called exactly 1 time |

---

### Unit Tests — Customer & Branch Services (`CustomerAndBranchServiceTests.cs`)

20 combined tests covering both services with their filter parameters.

**CustomerService tests (10):**

| Test Name | What it verifies |
|---|---|
| `GetAllAsync_NoFilter_ReturnsAllCustomers` | 3 customers returned without filter |
| `GetAllAsync_FilterActive_ReturnsOnlyActiveCustomers` | isActive=true → 2 results |
| `GetAllAsync_FilterInactive_ReturnsOnlyInactiveCustomers` | isActive=false → 1 result |
| `GetByIdAsync_ExistingId_ReturnsCustomer` | Returns correct email |
| `GetByIdAsync_NonExistingId_ReturnsNull` | Returns null for missing ID |
| `CreateAsync_ValidDto_PersistsCustomer` | New customer saved, ID assigned |
| `CreateAsync_SetsMemberSinceToToday` | MemberSince equals DateTime.Today |
| `UpdateAsync_ExistingCustomer_UpdatesEmail` | Email field updated |
| `UpdateAsync_NonExistingCustomer_ReturnsNull` | Null for missing ID |
| `DeleteAsync_ExistingCustomer_RemovesRecord` | Record removed from DB |

**LibraryBranchService tests (10):**

| Test Name | What it verifies |
|---|---|
| `GetAllAsync_NoFilter_ReturnsAllBranches` | 3 branches returned |
| `GetAllAsync_FilterOpen_ReturnsOnlyOpenBranches` | isOpen=true → 2 results |
| `GetAllAsync_FilterClosed_ReturnsOnlyClosedBranches` | isOpen=false → 1 result |
| `GetByIdAsync_ExistingId_ReturnsBranchWithBookCount` | BookCount = 1 for seeded branch |
| `GetByIdAsync_NonExistingId_ReturnsNull` | Null for missing ID |
| `CreateAsync_ValidDto_CreatesBranch` | New branch saved |
| `UpdateAsync_ExistingBranch_UpdatesManagerName` | ManagerName field updated |
| `UpdateAsync_NonExistingBranch_ReturnsNull` | Null for missing ID |
| `DeleteAsync_ExistingBranch_ReturnsTrueAndRemoves` | Deleted, count drops |
| `DeleteAsync_NonExistingBranch_ReturnsFalse` | False for missing ID |

---

### Integration Tests — Books API (`BooksApiIntegrationTests.cs`)

These 8 tests spin up a **real in-process HTTP server** using `WebApplicationFactory`. The SQLite database is replaced with `UseInMemoryDatabase`. Every test sends real HTTP requests and reads real HTTP responses.

| Test Name | What it verifies |
|---|---|
| `GET_Books_ReturnsOkWithSeededBooks` | 200 OK, response body is valid JSON with books list |
| `GET_Books_WithSearchTerm_ReturnsFilteredResults` | Filter works end-to-end through the full pipeline |
| `GET_Books_WithGenreFilter_ReturnsFilteredResults` | Genre filter works end-to-end |
| `GET_BookById_ExistingId_ReturnsBook` | Title matches seeded data |
| `GET_BookById_NonExistingId_Returns404` | 404 with `success: false` body |
| `POST_Book_WithoutAuth_Returns401` | Cannot create without a token |
| `PUT_Book_WithoutAuth_Returns401` | Cannot update without a token |
| `DELETE_Book_WithoutAuth_Returns401` | Cannot delete without a token |

---

### Integration Tests — Authors & Branches (`AuthorsAndBranchesApiIntegrationTests.cs`)

12 tests covering the same pattern for the other two resource types.

**Authors (6):** GET all, GET by ID (found + not found), POST/PUT/DELETE all return 401 without auth.

**Library Branches (6):** GET all, filter by isOpen, GET by ID (found + not found), POST/DELETE return 401 without auth.

---

## 10. JSON Payload Reference

### Register / Login

```json
POST /api/auth/register
{
  "username": "string",
  "email": "user@example.com",
  "password": "minimum 6 chars, 1 digit"
}

POST /api/auth/login
{
  "username": "string (username or email)",
  "password": "string"
}
```

### Book

```json
POST /api/books  (or PUT /api/books/{id})
{
  "title": "string (required, max 200)",
  "authorId": 1,
  "isbn": "978-0000000000",
  "genre": "Fiction",
  "publishedYear": 2024,
  "publisher": "Publisher Name",
  "totalCopies": 3,
  "availableCopies": 3,
  "description": "Optional description.",
  "libraryBranchId": 1
}
```

### Author

```json
POST /api/authors  (or PUT /api/authors/{id})
{
  "firstName": "string (required)",
  "lastName": "string (required)",
  "biography": "optional",
  "nationality": "Canadian",
  "birthYear": 1975
}
```

### Customer

```json
POST /api/customers  (or PUT /api/customers/{id})
{
  "firstName": "string (required)",
  "lastName": "string (required)",
  "email": "valid@email.com (required)",
  "phone": "604-555-0100",
  "address": "123 Street, City",
  "isActive": true,
  "libraryBranchId": 1
}
```

### Library Branch

```json
POST /api/librarybranches  (or PUT /api/librarybranches/{id})
{
  "branchName": "string (required)",
  "address": "string (required)",
  "city": "Vancouver",
  "phone": "604-555-0100",
  "email": "branch@library.ca",
  "openingHours": "Mon-Fri 9am-8pm",
  "managerName": "Manager Name",
  "isOpen": true
}
```

---

## 11. API Response Format

All API responses use a consistent wrapper:

```json
{
  "success": true,
  "message": "Optional success message.",
  "data": { ... }
}
```

On error:
```json
{
  "success": false,
  "message": "Book with ID 999 not found.",
  "data": null
}
```

| HTTP Status | Meaning |
|---|---|
| 200 OK | GET, PUT, DELETE succeeded |
| 201 Created | POST succeeded, `Location` header points to new resource |
| 400 Bad Request | Validation failed (missing required field, etc.) |
| 401 Unauthorized | No token or invalid/expired token |
| 404 Not Found | Resource with given ID does not exist |

---

## 12. Security Design

### JWT Token Lifecycle

```
1. Client calls POST /api/auth/login with username + password
2. Server validates credentials via ASP.NET Core Identity
3. Server builds a JWT token containing:
   - sub  (user ID from Identity)
   - unique_name (username)
   - email
   - jti (unique token ID for revocation tracking)
   - exp (expiry: 8 hours from issue time)
4. Token is signed with HMAC-SHA256 using the key from appsettings.json
5. Client stores the token and sends it in the Authorization header:
   Authorization: Bearer eyJhbGci...
6. On each protected request, JwtBearerMiddleware validates:
   - Signature is valid
   - Issuer = "LibraryManagementAPI"
   - Audience = "LibraryManagementClients"
   - Token has not expired
7. If valid, the request proceeds. If not, 401 is returned immediately.
```

### Authorization Policy

Public endpoints (no `[Authorize]`): Books GET, Authors GET, Library Branches GET.

JWT-required endpoints (`[Authorize]` attribute): All POST/PUT/DELETE, and all Customer endpoints.

The `CustomersController` has `[Authorize]` at the class level, applying it to every method.

---

## 13. Configuration Reference

### appsettings.json — Key Sections

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Database/library.db"
  },
  "Jwt": {
    "Key": "LibraryMgmt_SuperSecret_JWT_Key_2026_CSCI6809_FDU!",
    "Issuer": "LibraryManagementAPI",
    "Audience": "LibraryManagementClients",
    "ExpiryHours": "8"
  }
}
```

**Jwt:Key** — The HMAC secret used to sign tokens. Must be at least 32 characters. In a real system this would be stored in environment variables, not in the config file.

**Jwt:Issuer** — Validated on every incoming token. Must match what was used when the token was created.

**Jwt:Audience** — Identifies the intended recipient. Must also match.

**Jwt:ExpiryHours** — How long a token stays valid. Default is 8 hours.

### NuGet Packages Added for Project C

| Package | Purpose |
|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT Bearer middleware |
| `Microsoft.IdentityModel.Tokens` | Token validation primitives |
| `System.IdentityModel.Tokens.Jwt` | JWT token reading/writing |
| `Swashbuckle.AspNetCore` | Swagger / OpenAPI generation |
| `xunit` | Test framework |
| `Moq` | Mocking framework for unit tests |
| `FluentAssertions` | Readable assertion syntax |
| `Microsoft.AspNetCore.Mvc.Testing` | In-process test server |
| `Microsoft.EntityFrameworkCore.InMemory` | In-memory DB for tests |
