# User Directory Backend

A backend-only User Directory REST API built with .NET 8, ASP.NET Core Web API, Entity Framework Core, and SQLite.

## Requirements

- .NET 8 SDK

## Run locally

```bash
cd UserDirectory.Api
dotnet restore
dotnet build
dotnet run
```

Open the Swagger URL printed in the terminal, then append `/swagger` if needed.

The SQLite database is created automatically at `UserDirectory.Api/data/app.db`.

## Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/users` | List all users |
| GET | `/api/users/{id}` | Get one user |
| POST | `/api/users` | Create a user |
| PUT | `/api/users/{id}` | Update a user |
| DELETE | `/api/users/{id}` | Delete a user |

## Example POST body

```json
{
  "name": "Rahul Sharma",
  "age": 28,
  "city": "Bengaluru",
  "state": "Karnataka",
  "pincode": "560001"
}
```

## Behavior

- Request DTOs use Data Annotations for validation.
- Invalid request bodies return `400 Bad Request`.
- Missing users return `404 Not Found`.
- Successful creation returns `201 Created`.
- Successful deletion returns `204 No Content`.
- SQLite provides local persistent storage.

## GitHub publishing

Create a repository named `user-directory-backend` on GitHub, then from this folder run:

```bash
git init
git add .
git commit -m "Add User Directory backend API"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/user-directory-backend.git
git push -u origin main
```

Replace `YOUR-USERNAME` with your GitHub username. Do not commit the generated database file.
