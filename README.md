# AntecLMS

A modern Learning Management System built with ASP.NET Core 10 and PostgreSQL.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Setup

### 1. Start the database (in project root)

```bash
docker compose up -d
```

This starts a PostgreSQL 18 container with the database `anteclms` and user `antec`.

### 2. Install EF Core tools

```bash
dotnet tool install -g dotnet-ef
```

### 3. Apply EF Core migrations

Navigate to `src/AntecLMS.API` and run:

```bash
dotnet ef database update
```

### 4. Seed an admin user

```bash
docker exec -it antec-postgres psql -U antec -d anteclms
```

Inside the PostgreSQL prompt, run:

```sql
CREATE EXTENSION IF NOT EXISTS pgcrypto;

INSERT INTO users
(
    name,
    surname,
    email,
    password,
    role,
    status,
    created_at
)
VALUES
(
    'Admin',
    'Two',
    'admin2@school.com',
    crypt('admin123', gen_salt('bf')),
    'Admin',
    'Active',
    NOW()
);
```

Exit with `\q`.

### 5. Run the application

```bash
cd src/AntecLMS.API
dotnet run
```

The API will be available at `https://localhost:5014` (or the port shown in the console output).
