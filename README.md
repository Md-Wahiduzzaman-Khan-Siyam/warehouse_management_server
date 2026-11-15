# Warehouse Management Server

ASP.NET Core Web API for managing products, warehouses, inventory, stock movements, users, roles, and authentication.

## Table of contents

- [Overview](#overview)
- [Features](#features)
- [Technology stack](#technology-stack)
- [Architecture](#architecture)
- [Requirements](#requirements)
- [Configuration](#configuration)
- [Running locally](#running-locally)
- [Database migrations](#database-migrations)
- [Authentication](#authentication)
- [API reference](#api-reference)
- [Data model](#data-model)
- [Error responses](#error-responses)
- [Troubleshooting](#troubleshooting)
- [Development workflow](#development-workflow)
- [Known considerations](#known-considerations)
- [License](#license)

## Overview

Warehouse Management Server is a backend service for warehouse and inventory operations. It exposes a RESTful HTTP API backed by PostgreSQL and Entity Framework Core.

The solution follows **Clean Architecture**. Business rules live in the Application and Domain layers, while database access and HTTP concerns stay in the Infrastructure and API layers.

## Features

- Product and SKU management
- Warehouse management
- Inventory tracking by product and warehouse
- Stock quantity creation, updates, and adjustments
- Stock movement history
- User registration and account management
- Role creation and user-role assignment
- JWT bearer authentication
- Refresh-token rotation and revocation
- Password hashing with PBKDF2-SHA256
- PostgreSQL persistence through Entity Framework Core
- Optional automatic migrations on startup
- Swagger/OpenAPI documentation in Development
- Centralized exception-to-HTTP error handling (RFC 7807 problem details)
- Automatic audit timestamps for persisted entities

## Technology stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- PostgreSQL (local or hosted, for example Neon)
- Npgsql Entity Framework Core provider
- JWT bearer authentication
- Swashbuckle ASP.NET Core

## Architecture

This project uses a Clean Architecture-style structure. Dependencies point inward: the Domain layer depends on nothing, and the Application layer depends only on the Domain. Infrastructure implements the interfaces that Application defines, and the API layer wires everything together.

```text
API ──────────────► Application ──────────► Domain
 │                       ▲                     ▲
 │                       │                     │
 └────► Infrastructure ──┴─────────────────────┘
```

### Solution structure

```text
WarehouseManagement/
├── WarehouseManagement.sln
├── WarehouseManagement.API/
│   ├── Controllers/
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── WarehouseManagement.Application/
│   ├── Dtos/
│   ├── Interfaces/
│   ├── Security/
│   ├── Services/
│   └── Settings/
├── WarehouseManagement.Domain/
│   ├── Entities/
│   └── Enum/
└── WarehouseManagement.Infrastructure/
    ├── Data/
    ├── Data/Migrations/
    └── Repositories/
```

### Layer responsibilities

| Layer | References | Responsibility |
|---|---|---|
| **Domain** | Nothing | Core entities, relationships, shared audit fields, and domain enumerations |
| **Application** | Domain | Use cases, business rules, DTOs, service and repository interfaces, password hashing, and token generation |
| **Infrastructure** | Application, Domain | EF Core `DbContext`, PostgreSQL integration, migrations, and repository implementations |
| **API** | Application, Infrastructure | HTTP endpoints, authentication setup, authorization middleware, Swagger, dependency injection, and exception handling |

The API layer is the composition root. It registers the Application services and the Infrastructure implementations at startup, so Application code depends only on abstractions.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A PostgreSQL database (local installation or a hosted service such as Neon)
- A secure JWT signing key of at least 32 characters
- EF Core CLI tools for creating and applying migrations manually:

```bash
dotnet tool install --global dotnet-ef
```

Verify the SDK:

```bash
dotnet --version
```

## Configuration

Runtime configuration is located in:

- `WarehouseManagement/WarehouseManagement.API/appsettings.json`
- `WarehouseManagement/WarehouseManagement.API/appsettings.Development.json`

The committed `appsettings.json` contains placeholder values. Provide a real PostgreSQL connection string and JWT key before starting the API. The API will not start if the `Jwt` section is missing.

```json
{
  "Jwt": {
    "Key": "replace-with-a-long-random-secret",
    "Issuer": "WarehouseManagement.API",
    "Audience": "WarehouseManagement.Client",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WarehouseDB;Username=postgres;Password=your-password;SSL Mode=Prefer"
  },
  "Database": {
    "ApplyMigrationsOnStartup": true
  }
}
```

### Settings reference

| Setting | Description |
|---|---|
| `Jwt:Key` | Secret used to sign tokens. Use at least 32 random characters. |
| `Jwt:Issuer` / `Jwt:Audience` | Must match between token issuing and validation. |
| `Jwt:AccessTokenExpirationMinutes` | Access token lifetime. Default: 15. |
| `Jwt:RefreshTokenExpirationDays` | Refresh token lifetime. Default: 7. |
| `ConnectionStrings:DefaultConnection` | Npgsql connection string for PostgreSQL. |
| `Database:ApplyMigrationsOnStartup` | When `true` (the project default), pending migrations are applied at startup. Set it to `false` to skip this and run migrations manually. See [Database migrations](#database-migrations). |

### Keeping secrets out of source control

Do not commit real database passwords or JWT secrets. For local development, use .NET user secrets:

```bash
cd WarehouseManagement/WarehouseManagement.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your-connection-string>"
dotnet user-secrets set "Jwt:Key" "<a-long-random-secret>"
```

Alternatively, use environment variables (double underscores map to `:` in configuration keys):

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WarehouseDB;Username=postgres;Password=your-password"
export Jwt__Key="replace-with-a-long-random-secret"
export Database__ApplyMigrationsOnStartup=true
```

In production, use a secret manager provided by your hosting platform.

### Using Neon PostgreSQL

Neon connection strings work with Npgsql. Keep these points in mind:

- Use `SSL Mode=Require` (Neon requires TLS).
- Use the **direct** connection string (host without `-pooler`) for migrations. The pooled endpoint runs PgBouncer in transaction mode, which does not work reliably with EF Core's migration locking.
- Idle Neon compute is suspended, so the first connection after a pause is slower.
- Every database round trip is affected by the distance between you and the Neon region, which makes startup migrations noticeably slower than on a local database.

## Running locally

From the repository root:

```bash
cd WarehouseManagement
dotnet restore
dotnet build
dotnet run --project WarehouseManagement.API
```

Launch profiles are defined in `WarehouseManagement/WarehouseManagement.API/Properties/launchSettings.json`. By default `dotnet run` uses the `http` profile. To also serve HTTPS, use the `https` profile:

```bash
dotnet run --project WarehouseManagement.API --launch-profile https
```

The API is available at:

```text
HTTP:    http://localhost:5002
HTTPS:   https://localhost:7149
Swagger: http://localhost:5002/swagger
```

Swagger is enabled only when `ASPNETCORE_ENVIRONMENT=Development`, which the launch profiles set automatically.

## Database migrations

The repository includes EF Core migrations in `WarehouseManagement.Infrastructure/Data/Migrations`.

### Create a migration

Run this whenever you change an entity or the model configuration in `AppDbContext`:

```bash
cd WarehouseManagement
dotnet ef migrations add MigrationName \
  --project WarehouseManagement.Infrastructure \
  --startup-project WarehouseManagement.API \
  --output-dir Data/Migrations
```

The `--output-dir` option keeps migrations in `WarehouseManagement.Infrastructure/Data/Migrations`, where the initial migration (`InitialCreate`) was created. This command only generates migration files. It does not change the database.

### Apply migrations manually

```bash
dotnet ef database update \
  --project WarehouseManagement.Infrastructure \
  --startup-project WarehouseManagement.API
```

### Apply migrations on startup

Set `Database:ApplyMigrationsOnStartup` to `true` to have the API apply pending migrations when it starts. This option applies existing migrations only. You still need to create them with `dotnet ef migrations add`.

Things to know:

- If no migrations are pending, the API logs that the database is already up to date.
- Against a remote database the check takes a few round trips. If the migration runs before the web server begins listening, the first browser request to Swagger can fail. See [Troubleshooting](#troubleshooting).
- Applying migrations automatically is convenient for development. For production, prefer running `dotnet ef database update` as an explicit deployment step.
- Never edit or delete a migration that has already been applied to a shared database. Add a new migration instead.

## Authentication

All endpoints require authentication except the registration and token endpoints listed below.

Send the access token using the bearer scheme:

```http
Authorization: Bearer <access-token>
```

In Swagger UI, click **Authorize** and paste only the access token. Swagger adds the `Bearer` prefix itself.

Access tokens are validated for issuer, audience, lifetime, and signing key, with no clock-skew tolerance. A token stops working exactly at its expiry time. Use a refresh token to obtain a new one.

### Authentication endpoints

Base route: `/api/Auth`

| Method | Endpoint | Authentication | Description |
|---|---|---|---|
| `POST` | `/api/Auth/register` | Anonymous | Register a user and issue access and refresh tokens |
| `POST` | `/api/Auth/login` | Anonymous | Log in with username or email |
| `POST` | `/api/Auth/refresh` | Anonymous | Exchange an active refresh token for new tokens |
| `POST` | `/api/Auth/revoke` | Anonymous | Revoke a refresh token |
| `GET` | `/api/Auth/me` | Required | Return the authenticated user |

Passwords are hashed using PBKDF2-SHA256 with a random salt, a 32-byte derived key, and 100,000 iterations.

Request and response schemas are documented in Swagger.

## API reference

All routes below require a valid JWT access token unless explicitly marked as anonymous.

### Products

Base route: `/api/Product`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Product` | Create a product |
| `GET` | `/api/Product/{id}` | Get a product by ID |
| `GET` | `/api/Product/by-sku/{sku}` | Get a product by SKU |
| `GET` | `/api/Product/search?name={name}` | Search products by name |
| `GET` | `/api/Product` | Get all products |
| `GET` | `/api/Product?ProductIds={id}` | Get selected products |
| `PUT` | `/api/Product/{id}` | Update a product |
| `DELETE` | `/api/Product/{id}` | Delete a product |

Product SKUs are unique, and unit prices cannot be negative.

### Warehouses

Base route: `/api/Warehouse`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Warehouse` | Create a warehouse |
| `GET` | `/api/Warehouse/{id}` | Get a warehouse |
| `GET` | `/api/Warehouse` | Get all warehouses |
| `GET` | `/api/Warehouse?warehouseIds={id}` | Get selected warehouses |
| `PUT` | `/api/Warehouse/{id}` | Update a warehouse |
| `DELETE` | `/api/Warehouse/{id}` | Delete a warehouse |

### Stock

Base route: `/api/Stock`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Stock` | Create stock for a product and warehouse |
| `GET` | `/api/Stock/{id}` | Get stock by ID |
| `GET` | `/api/Stock/by-product/{productId}` | Get stock by product |
| `GET` | `/api/Stock/by-warehouse/{warehouseId}` | Get stock by warehouse |
| `GET` | `/api/Stock` | Get all stock records |
| `GET` | `/api/Stock?stockId={id}` | Get selected stock records |
| `PUT` | `/api/Stock/{id}` | Update stock |
| `PATCH` | `/api/Stock/{id}/adjust` | Adjust stock using a quantity delta |
| `DELETE` | `/api/Stock/{id}` | Delete stock |

Stock quantity cannot become negative. A product can have only one stock record per warehouse.

### Stock movements

Base route: `/api/StockMovement`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/StockMovement` | Create a stock movement |
| `GET` | `/api/StockMovement/{id}` | Get a stock movement |
| `GET` | `/api/StockMovement/by-product/{productId}` | Get movements by product |
| `GET` | `/api/StockMovement/by-warehouse/{warehouseId}` | Get movements by warehouse |
| `GET` | `/api/StockMovement` | Get all movements |
| `GET` | `/api/StockMovement?stockMovementIds={id}` | Get selected movements |
| `PUT` | `/api/StockMovement/{id}` | Update a movement |
| `DELETE` | `/api/StockMovement/{id}` | Delete a movement |

Supported movement types (stored as strings in the database):

```text
Receipt
Issue
Adjust
```

Creating a stock movement records the movement but does not automatically update the related stock quantity.

### Users

Base route: `/api/User`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/User` | Create a user |
| `GET` | `/api/User/{id}` | Get a user |
| `GET` | `/api/User` | Get all users |
| `GET` | `/api/User?userIds={id}` | Get selected users |
| `PUT` | `/api/User/{id}` | Update a user |
| `PATCH` | `/api/User/{id}/change-password` | Change a password |
| `PATCH` | `/api/User/{id}/active` | Activate or deactivate a user |
| `GET` | `/api/User/{id}/roles` | Get roles assigned to a user |
| `POST` | `/api/User/{id}/roles/{roleId}` | Assign a role |
| `DELETE` | `/api/User/{id}/roles/{roleId}` | Remove a role |
| `DELETE` | `/api/User/{id}` | Delete a user |

### Roles

Base route: `/api/Role`

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Role` | Create a role |
| `GET` | `/api/Role/{id}` | Get a role |
| `GET` | `/api/Role/{id}/users` | Get users assigned to a role |
| `GET` | `/api/Role` | Get all roles |
| `GET` | `/api/Role?roleIds={id}` | Get selected roles |
| `PUT` | `/api/Role/{id}` | Update a role |
| `DELETE` | `/api/Role/{id}` | Delete a role |

## Data model

| Entity | Description |
|---|---|
| `Product` | Product name, description, SKU, and unit price |
| `Warehouse` | Warehouse name and location |
| `Stock` | Quantity for one product in one warehouse |
| `StockMovement` | Receipt, issue, or adjustment record |
| `User` | Username, email, password hash, and active status |
| `Role` | Named application role |
| `UserRoles` | Many-to-many relationship between users and roles |
| `RefreshToken` | Token value, expiration, revocation, and replacement data |

All entities inherit `Id`, `CreatedAt`, `UpdatedAt`, `CreatedBy`, and `UpdatedBy` from `BaseEntity`. `Id` (a GUID), `CreatedAt`, and `UpdatedAt` are set by `AppDbContext` during save operations, using UTC time.

Products belong to warehouses through `Stock` records, not through a direct relationship.

### Database constraints

- Product SKU is unique (maximum 100 characters).
- Username and email are unique.
- Role name is unique.
- Refresh-token value is unique.
- A user cannot have the same role more than once.
- A product can have only one stock record per warehouse.

### Cascade deletes

- Deleting a product or warehouse deletes its related stock and stock-movement records.
- Deleting a user deletes their user-role assignments and refresh tokens.
- Deleting a role deletes its user-role assignments.

## Error responses

The API returns RFC 7807 problem-details responses for handled exceptions:

| Exception | HTTP status |
|---|---:|
| `KeyNotFoundException` | `404 Not Found` |
| `ArgumentException` | `400 Bad Request` |
| `UnauthorizedAccessException` | `401 Unauthorized` |
| `InvalidOperationException` | `409 Conflict` |
| `DbUpdateException` | `409 Conflict` |
| Other exceptions | `500 Internal Server Error` |

For `DbUpdateException`, the response uses a generic message about conflicting data (for example a duplicate SKU, username, or email). For unexpected `500` errors, the response contains only a generic message and no exception details.

## Troubleshooting

### Swagger shows "Failed to load API definition" on first load

```text
Failed to fetch /swagger/v1/swagger.json
```

Your IDE opens the browser as soon as the process launches. If `ApplyMigrationsOnStartup` is `true` and the migration runs before the web server starts listening, the first request arrives while nothing is listening. This is most noticeable with a remote database such as Neon, where each round trip takes hundreds of milliseconds.

Wait until the console prints `Now listening on: http://localhost:5002`, then refresh the page. If the console shows an exception or the process exits, the cause is in the exception message instead (see below).

### The API exits at startup

Read the first exception in the console. Common causes:

| Symptom | Likely cause |
|---|---|
| `Jwt configuration section is missing` | `Jwt` section is missing from the loaded configuration |
| Npgsql connection or authentication errors | Wrong host, database, username, or password, or the database is unreachable |
| `relation "..." already exists` | The schema was created outside EF migrations. Drop the development database and re-run migrations, or repair `__EFMigrationsHistory` |
| Pending model changes error | The model was changed without a new migration. Run `dotnet ef migrations add` |

### Every request returns 401 Unauthorized

- Confirm the access token has not expired (15 minutes by default).
- In Swagger, paste only the token. Do not include the `Bearer ` prefix.
- Make sure `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:Key` match the values used when the token was issued.

### A create or update request returns 409 Conflict

The change violates a unique constraint, such as a duplicate product SKU, username, email, or role name. It can also come from a business-rule `InvalidOperationException`.

## Development workflow

Build the complete solution:

```bash
cd WarehouseManagement
dotnet build
```

Run the API:

```bash
dotnet run --project WarehouseManagement.API
```

Before opening a pull request, verify the following:

- The solution builds successfully.
- Database configuration is available.
- New schema changes include an EF Core migration.
- New endpoints are protected appropriately.
- Secrets are not committed.
- Swagger reflects the implemented API behavior.

### Package versions

The solution targets .NET 10. Keep the EF Core, EF Core Design, and Npgsql provider packages on the same major version (10.x), and avoid release-candidate packages. Upgrade Swashbuckle major versions deliberately, because newer majors change the `Microsoft.OpenApi` types used in `Program.cs`.

## Known considerations

- Controllers use general authentication rather than role-based authorization policies.
- Stock movement creation does not update stock quantities automatically.
- Collection endpoints do not implement pagination.
- CORS is not configured, so browser clients on other origins are blocked.
- Automated unit and integration test projects are not included in the solution.
- Creation and deletion endpoints return the response shapes implemented by the controllers rather than consistently using `201 Created` and `204 No Content`.
- The API layer's exception handler references an EF Core type (`DbUpdateException`), a small, deliberate exception to the layer boundaries.
- Migrating on startup is intended for development. Use an explicit migration step in production.

## License

No license has been specified for this repository.