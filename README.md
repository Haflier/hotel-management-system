# Hotel Management System

A hotel management REST API built with **ASP.NET Core** and **PostgreSQL**. The system provides hotel and room management, reservations, food and drink ordering, services, authentication and role-based authorization.

## Tech Stack

* **ASP.NET Core Web API**
* **Entity Framework Core**
* **PostgreSQL**
* **ASP.NET Core Identity**
* **JWT Bearer Authentication**
* **AutoMapper**
* **Swagger / OpenAPI**
* **Docker & Docker Compose**

## Features

* User registration and JWT authentication
* Role-based authorization
* Hotel and city management
* Room management
* Room reservations
* Food and drink management
* Room services
* Orders and order items
* Automatic factor generation
* Automatic customer role management
* Entity Framework Core migrations
* PostgreSQL database running in Docker

## Architecture

The application uses a **layered architecture** that separates HTTP endpoints, application services, data access, and persistence.

```text
Client
  │
  ▼
ASP.NET Core Web API
  │
  ├── Controllers
  │
  ├── Services
  │
  └── Repositories
          │
          ▼
   Entity Framework Core
          │
          ▼
      PostgreSQL
```

### Main Components

* **Controllers** — Expose REST API endpoints and handle HTTP requests.
* **Services** — Handle application-level and background operations.
* **Repositories** — Encapsulate database access and persistence operations.
* **Entity Framework Core** — Provides ORM functionality and database migrations.
* **ASP.NET Core Identity** — Manages users, passwords, and roles.
* **JWT** — Provides stateless authentication for API requests.
* **PostgreSQL** — Stores application and Identity data.

## Database

The database contains entities for:

* Cities
* Hotels
* Rooms
* Reservations
* Users
* Food and drinks
* Services
* Orders and order items
* Factors
* Room-service relationships

![Database Schema](docs/database-schema.png)

The database schema is managed using **Entity Framework Core migrations**.

## Authentication & Authorization

The API uses **ASP.NET Core Identity** for user management and **JWT Bearer tokens** for API authentication.

Three roles are supported:

* **User** — Registered users who have not yet made a reservation.
* **Customer** — Users who have made a reservation and can perform customer operations.
* **Administrator** — Full administrative access to management operations.

Authorization policies are used to restrict protected endpoints according to the user's role.

Swagger includes Bearer authentication support, allowing authenticated endpoints to be tested directly from the Swagger UI.

### Customer Role Lifecycle

A newly registered account is assigned the `User` role.

After creating a reservation, the account is promoted to the `Customer` role. A background service is responsible for customer-role lifecycle management.

## Docker

The application can be run as a multi-container application using Docker Compose.

```text
                    Docker Compose
                         │
              ┌──────────┴──────────┐
              │                     │
              ▼                     ▼
       ┌─────────────┐       ┌───────────────┐
       │  hotel-api  │──────▶│ hotel-postgres│
       │ ASP.NET Core│       │ PostgreSQL 18 │
       │    :8080    │       │    :5432      │
       └─────────────┘       └───────────────┘
```

### Requirements

* Docker
* Docker Compose

### Configuration

Create the environment file from the provided example:

```bash
cp .env.example .env
```

Configure the required PostgreSQL credentials in `.env`.

> `.env` is ignored by Git.

### Start the Application

Build and start the containers:

```bash
docker compose up --build -d
```

Check the container status:

```bash
docker compose ps
```

The API and PostgreSQL containers should be running, with PostgreSQL reporting a healthy status.

### Access the API

API:

```text
http://localhost:8080
```

Swagger / OpenAPI:

```text
http://localhost:8080/swagger
```

### View Logs

API logs:

```bash
docker compose logs api
```

PostgreSQL logs:

```bash
docker compose logs postgres
```

### Stop the Application

```bash
docker compose down
```

PostgreSQL data and ASP.NET Core Data Protection keys are stored in Docker named volumes and persist when containers are recreated.

## Database Migrations

Entity Framework Core migrations are applied automatically when the API starts:

```csharp
dbContext.Database.Migrate();
```

Migration history is maintained in:

```text
__EFMigrationsHistory
```

This allows the PostgreSQL schema to be initialized and updated automatically when the application starts.

## Project Structure

```text
HotelManagementSystem/
│
├── api/
│   ├── Configuration/
│   ├── Controllers/
│   ├── Data/
│   ├── Exceptions/
│   ├── Interfaces/
│   ├── Middleware/
│   ├── Migrations/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Program.cs
│   └── api.csproj
│
├── docs/
│   └── database-schema.png
│
├── Dockerfile
├── docker-compose.yml
├── .env.example
├── .gitignore
└── README.md
```

## Running Without Docker

The API can also be run directly using the .NET SDK.

```bash
cd api
dotnet restore
dotnet run
```

Make sure PostgreSQL is running and the `DefaultConnection` connection string is configured correctly.

## License

This project is intended for educational and portfolio purposes.

