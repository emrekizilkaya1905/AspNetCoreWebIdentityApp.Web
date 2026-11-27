# ASP.NET Core Web Identity App

An ASP.NET Core MVC learning project focused on authentication and authorization with ASP.NET Core Identity. The application is being organized into separate Core, Repository, Service, and Web layers.

## Features

- Account registration, email/password sign-in, remember me, and logout.
- Password changes and email-based password reset.
- Profile editing, birth date, city, gender, and profile picture upload.
- User and role management in the Admin area.
- Assigning and removing user roles.
- Role and permission claims, a claims overview page, and role badges through a custom Tag Helper.
- Policy-based access checks for city, age, exchange membership, and permissions.
- Bootstrap views with a shared green theme.

## Technology

- .NET 10 and ASP.NET Core MVC.
- ASP.NET Core Identity and Entity Framework Core.
- SQL Server / SQL Server LocalDB.
- Razor views, Tag Helpers, and Bootstrap.
- SMTP email delivery using STARTTLS on port 587.

## Solution structure

| Project | Responsibility |
| --- | --- |
| `AspNetCoreWebIdentityApp.Core` | Shared view models, enums, configuration models, and permission constants. |
| `AspNetCoreWebIdentityApp.Repository` | Identity entities, database context, migrations, and role/permission seeding. |
| `AspNetCoreWebIdentityApp.Service` | Member operations, password/profile updates, file uploads, and email delivery. |
| `AspNetCoreWebIdentityApp.Web` | MVC controllers, Razor views, authorization policies, claim transformation, and application startup. |

Project references follow `Web -> Service -> Repository -> Core`. Some authentication and administration logic still lives in Web while the service extraction is in progress.

## Prerequisites

- .NET 10 SDK.
- SQL Server or SQL Server LocalDB on Windows.
- Visual Studio with the ASP.NET and web development workload, or another .NET editor.
- An SMTP account to use the password reset email feature.
- The `dotnet-ef` CLI tool if you use the migration commands below.

## Configuration

Configure `AspNetCoreWebIdentityApp.Web/appsettings.Development.json` for your local environment. This example uses placeholders rather than real credentials:

```json
{
  "ConnectionStrings": {
    "SqlCon": "Server=(localdb)\\IdentityLocalDB;Database=identityDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "EmailSettings": {
    "Host": "smtp.example.com",
    "Email": "your-email@example.com",
    "Password": "your-smtp-password"
  }
}
```

Use your own SQL Server instance name. The `IdentityLocalDB` instance must exist if you use the example connection string. For a standard LocalDB installation, you can use `(localdb)\\MSSQLLocalDB` in JSON instead.

Keep real SMTP passwords out of source control. Environment variables such as `EmailSettings__Password` can override the configuration values.

## Build and run

Run these commands from the solution directory:

```powershell
dotnet restore AspNetCoreWebIdentityApp.Web.slnx
dotnet build AspNetCoreWebIdentityApp.Web.slnx
```

Apply the database migrations before launching the application:

```powershell
dotnet ef database update --project AspNetCoreWebIdentityApp.Repository --startup-project AspNetCoreWebIdentityApp.Web -- --environment Development
```

Then run the Web project:

```powershell
dotnet run --project AspNetCoreWebIdentityApp.Web --launch-profile https
```

The HTTPS launch profile uses `https://localhost:7228`. If necessary, trust the development certificate with `dotnet dev-certs https --trust`.

The startup code runs the permission seed, so the Identity database tables must already exist.

## Migrations

The database context and migrations are in Repository. Web is the startup project and configures the connection string and migration assembly.

List migrations:

```powershell
dotnet ef migrations list --project AspNetCoreWebIdentityApp.Repository --startup-project AspNetCoreWebIdentityApp.Web -- --environment Development
```

Create a migration:

```powershell
dotnet ef migrations add YourMigrationName --project AspNetCoreWebIdentityApp.Repository --startup-project AspNetCoreWebIdentityApp.Web -- --environment Development
```

In Visual Studio Package Manager Console, select Repository as the default project and Web as the startup project:

```powershell
Get-Migration -Project AspNetCoreWebIdentityApp.Repository -StartupProject AspNetCoreWebIdentityApp.Web
```

## Roles and permissions

The startup seed creates the following roles when they do not already exist:

| Role | Stock, Order, and Catalog permissions |
| --- | --- |
| `BasicRole` | Read |
| `AdvancedRole` | Read, Create, Update |
| `AdminRole` | Read, Create, Update, Delete |

Permission values follow names such as `Permissions.Order.Read`. The current seed only adds claims when creating a role; adding permission lines later does not update an existing role automatically. The seed does not create an administrator account or assign these roles to users.

`AdminRole` is separate from the `admin` role used by some role-based checks. Assigning permission claims does not automatically satisfy an `[Authorize(Roles = "admin")]` check.

Role and permission changes are reflected in a user's authentication cookie after it is refreshed or the user signs out and signs in again.

## Authorization examples

| Policy | Requirement |
| --- | --- |
| `StockholmPolicy` | A `city` claim with the value `Stockholm`. |
| `ExchangePolicy` | An `ExchangeExpire` claim containing an unexpired date. |
| `ViolencePolicy` | A `birthdate` claim meeting the configured minimum age of 18. |
| `OrderPermissionReadAndDelete` | Order Read, Order Delete, and Stock Delete permission claims. |

The Order view also uses `IAuthorizationService` to show individual buttons based on policies. Those buttons currently demonstrate UI visibility rather than executing order operations.

## Useful pages

| Route | Page |
| --- | --- |
| `/Home/SignUp` | Register |
| `/Home/SignIn` | Sign in |
| `/Home/ForgetPassword` | Request a password reset email |
| `/Member/Index` | Profile |
| `/Member/UserEdit` | Edit profile |
| `/Member/PasswordChange` | Change password |
| `/Member/Claims` | Current session claims |
| `/Member/StockholmPage` | City-restricted page |
| `/Member/ExchangePage` | Exchange membership page |
| `/Member/Violence` | Age-restricted page |
| `/Admin/Home/UserList` | User administration |
| `/Admin/Roles/Index` | Role administration |
| `/Order/Index` | Permission-protected order page |

## Development notes

- Account lockout is configured for three failed password attempts and a three-minute lockout period, for users with lockout enabled.
- Profile pictures are stored under `AspNetCoreWebIdentityApp.Web/wwwroot/userpictures/`. This folder is excluded from Git; supply the default profile picture separately when setting up a fresh checkout.
- Local notes under `Mds/` are excluded from Git.
- Existing lowercase migration class names can produce `CS8981` compiler warnings.
- This is an evolving learning project; authorization coverage and service boundaries should be reviewed before deployment.