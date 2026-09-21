# Hotel Management System

A hotel management REST API built with **ASP.NET Core** and **PostgreSQL**. The system provides hotel and room management, reservations, food and drink ordering, services, authentication and role-based authorization.

## Features

* User registration and JWT authentication
* Role-based authorization
* Hotel, Room and city management
* Orders and order items
* Automatic factor generation
* Automatic customer role management
* PostgreSQL database running in Docker

## Tech Stack

* **ASP.NET Core Web API**
* **Entity Framework Core**
* **PostgreSQL**
* **ASP.NET Core Identity**
* **JWT Bearer Authentication**
* **AutoMapper**
* **Swagger / OpenAPI**
* **Docker & Docker Compose**

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

## Database schema

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

### 1. Configure environment variables

Create `.env` from the example:

```bash
cp .env.example .env
```

Set the PostgreSQL values in `.env`:

```env
POSTGRES_DB=hotelapi
POSTGRES_USER=your_username
POSTGRES_PASSWORD=your_password
```

### 2. Build and start the application

```bash
docker compose up --build -d
```

This starts:

* **Hotel API** — `http://localhost:8080`
* **PostgreSQL** — port `5432`

The API waits for PostgreSQL to become healthy before starting.

### 3. Check containers

```bash
docker compose ps
```

### 4. View API logs

```bash
docker compose logs -f api
```

PostgreSQL logs:

```bash
docker compose logs -f postgres
```

### 5. Open Swagger

Open:

```text
http://localhost:8080/swagger
```

### 6. Stop the application

```bash
docker compose down
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
