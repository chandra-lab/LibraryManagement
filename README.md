# Library Management System
## CSCI 6809 - Project Deliverable A
**Fairleigh Dickinson University - Vancouver | Winter 2026**

---

## Overview
An ASP.NET Core MVC web application for library management built with SQLite database via Entity Framework Core. The application includes full CRUD operations, user authentication with ASP.NET Core Identity, Google OAuth login, exception handling, and logging.

---

## Features
- Full CRUD for Books, Authors, Customers, and Library Branches
- SQLite database with Entity Framework Core ORM
- 20+ genuine seeded records in each table
- Search and filter functionality on all list pages
- ASP.NET Core Identity — Register, Login, Logout
- Google OAuth external authentication
- User Profile page with edit and change password
- Global exception handling and custom error pages
- Logging with ILogger (console)
- Bootstrap 5 responsive UI
- [Authorize] protection on Create, Edit, Delete actions

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10)
- Visual Studio Code (with C# Dev Kit extension)
- Internet connection (for Bootstrap CDN and Google OAuth)

---

## How to Run

### 1. Extract the zip
```bash
unzip LibraryManagement.zip
cd LibraryManagement
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Set up Google OAuth secrets (required for Google login)
```bash
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_CLIENT_ID.apps.googleusercontent.com"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_CLIENT_SECRET"
```
> If you don't have Google credentials, the app still works — you can register and login with a local account instead.

### 4. Trust the HTTPS certificate (first time only)
```bash
dotnet dev-certs https --trust
```

### 5. Run the application
```bash
dotnet run
```

### 6. Open your browser
Navigate to: `https://localhost:5001`

> Use HTTPS — Google OAuth requires it. The database is created automatically on first run via EnsureCreated().

---

## Project Structure

```
LibraryManagement/
├── Areas/
│   └── Identity/
│       └── Pages/
│           └── Account/
│               ├── Register.cshtml         # Register page (scaffolded)
│               ├── Login.cshtml            # Login page with Google button
│               ├── Logout.cshtml
│               └── Manage/
│                   ├── Index.cshtml        # Profile page
│                   ├── ChangePassword.cshtml
│                   └── Email.cshtml
├── Controllers/
│   ├── HomeController.cs                   # Dashboard + Error handler
│   ├── BookController.cs                   # CRUD for Books
│   ├── AuthorController.cs                 # CRUD for Authors
│   ├── CustomerController.cs               # CRUD for Customers
│   ├── LibraryBranchController.cs          # CRUD for Library Branches
│   └── ProfileController.cs               # User profile management
├── Models/
│   ├── Book.cs
│   ├── Author.cs
│   ├── Customer.cs
│   ├── LibraryBranch.cs
│   ├── ErrorViewModel.cs                   # For error page
│   └── UserProfileViewModel.cs            # For profile + change password
├── ViewModels/
│   ├── BookViewModel.cs
│   ├── AuthorViewModel.cs
│   ├── CustomerViewModel.cs
│   └── LibraryBranchViewModel.cs
├── Data/
│   └── ApplicationDbContext.cs            # EF DbContext + seed data
├── Migrations/                            # EF Core migrations
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml                   # Dashboard
│   │   └── Error.cshtml                   # Custom error page
│   ├── Profile/
│   │   ├── Index.cshtml                   # View profile
│   │   ├── Edit.cshtml                    # Edit profile
│   │   └── ChangePassword.cshtml          # Change password
│   ├── Book/{Index,Details,Create,Edit,Delete}.cshtml
│   ├── Author/{Index,Details,Create,Edit,Delete}.cshtml
│   ├── Customer/{Index,Details,Create,Edit,Delete}.cshtml
│   ├── LibraryBranch/{Index,Details,Create,Edit,Delete}.cshtml
│   └── Shared/
│       └── _Layout.cshtml                 # Navbar with Login/Logout/Profile
├── Properties/
│   └── launchSettings.json               # HTTPS configuration
├── Database/                              # SQLite DB file created here
├── appsettings.json                       # Connection string + settings
└── Program.cs                            # App configuration + middleware
```

---

## Database

- **Database:** SQLite (`Database/library.db`)
- **ORM:** Entity Framework Core 10
- **Connection string:** configured in `appsettings.json`
- **Created automatically** on first run via `EnsureCreated()`

### Tables & Seed Data
| Table           | Records | Notes                      |
|-----------------|---------|----------------------------|
| Authors         | 22      | Real published authors     |
| Books           | 25      | Real published books       |
| Customers       | 22      | Sample library members     |
| LibraryBranches | 20      | Metro Vancouver branches   |
| AspNetUsers     | —       | Created via Register/Login |
| AspNetRoles     | —       | Identity roles table       |

---

## Security

### ASP.NET Core Identity
- Register and login with email/password
- Passwords are hashed automatically by Identity
- [Authorize] attribute protects Create, Edit, Delete actions
- Unauthenticated users are redirected to Login page

### Google OAuth
- External login via Google account
- Credentials stored securely using .NET User Secrets (never in code)
- Requires HTTPS

### Authorization Rules
| Action       | Access         |
|--------------|----------------|
| Index (list) | Everyone       |
| Details      | Everyone       |
| Create       | Logged in only |
| Edit         | Logged in only |
| Delete       | Logged in only |
| Profile      | Logged in only |

---

## Exception Handling

- **Development:** Full developer exception page with stack trace
- **Production:** Custom friendly error page at `/Home/Error`
- **Status code pages:** 404, 403, 500 handled with UseStatusCodePagesWithReExecute
- **Try-Catch blocks:** Used in all controller actions
- **ILogger:** Logs information, warnings and errors to console

### Logging Levels Used
```
LogInformation  — page visits, successful actions
LogWarning      — unexpected but recoverable situations
LogError        — exceptions and failures
```

---

## Key Commands

```bash
# Restore packages
dotnet restore

# Run the app
dotnet run

# Install EF tools
dotnet tool install --global dotnet-ef

# Create a new migration after model changes
dotnet ef migrations add MigrationName

# Apply migrations to DB
dotnet ef database update

# Reset the database (delete and recreate)
del Database\library.db
dotnet run

# Set Google OAuth secrets
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_SECRET"

# View stored secrets
dotnet user-secrets list

# Trust HTTPS certificate
dotnet dev-certs https --trust

# Scaffold Identity pages
dotnet aspnet-codegenerator identity -dc LibraryManagement.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.Manage.Index;Account.Manage.ChangePassword"
```

---

## Technologies Used
| Technology                    | Purpose                    |
|-------------------------------|----------------------------|
| ASP.NET Core MVC (.NET 10)    | Web framework              |
| Entity Framework Core 10      | ORM / database access      |
| SQLite                        | Database                   |
| ASP.NET Core Identity         | Authentication & user mgmt |
| Google OAuth 2.0              | External login             |
| Bootstrap 5 + Bootstrap Icons | UI styling                 |
| ILogger                       | Logging                    |
| C# 13                         | Programming language       |

---

## References
- [ASP.NET Core MVC Documentation](https://learn.microsoft.com/en-us/aspnet/core/mvc)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Google External Login](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins)
- [Handle Errors in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)