# RunnerMeets

> A web platform that helps local runners connect and organize runs in Victoria, BC.

I find it hard to gather scattered running activities happening in the city. Facebook, Instagram, newsletters—it’s difficult to keep track of everything. That's why I am building this all-in-one platform for running clubs, user-initiated running events, and sharing information.

## Project Status

This is a work in progress.

**Working in the UI:** registration, login, and profile editing, including running preferences (experience level, training goals, preferred distance, pace range).

**Backend only, no UI yet:** creating, updating, and deleting events and event locations. These are implemented as authenticated REST endpoints, and you can try them through Swagger.

## Tech Stack

- **Frontend:** React, TypeScript, Vite
- **Backend:** ASP.NET Core 10, Entity Framework Core, PostgreSQL
- **Auth:** ASP.NET Core Identity with cookie-based sessions
- **API docs:** NSwag/OpenAPI, served at `/swagger` in development

I built the backend in ASP.NET Core because it’s what I use professionally, which let me focus on learning the rest of the full-stack development. 

## Technical Highlights

- Full-stack architecture with a React client and ASP.NET Core REST API
- Cookie-based authentication and protected API endpoints
- PostgreSQL database modeled and migrated with Entity Framework Core
- Interactive API documentation through Swagger

## Getting Started

### Prerequisites

Install the following before running the project:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 24.18.0](https://nodejs.org/en/download), as specified in `myapp.client/.nvmrc`
- [PostgreSQL](https://www.postgresql.org/download/)
- The `dotnet-ef` command-line tool:

  ```bash
  dotnet tool install --global dotnet-ef --version "10.*"
  ```

### 1. Clone the repository

```bash
git clone https://github.com/megleemeg/RunnerMeets.git
cd RunnerMeets
```

### 2. Configure the database

Create a PostgreSQL database for the application, then store its connection string with [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets). Replace the placeholder values with your local database settings:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=runner_meets;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
```

User Secrets are stored outside the repository and should not be committed to source control.

Apply the existing database migrations:

```bash
dotnet ef database update
```

### 3. Start the backend

From the repository root, run:

```bash
dotnet run --launch-profile https
```

The API will run at `https://localhost:7078`. In development, its Swagger UI is available at `https://localhost:7078/swagger`.

If your browser does not trust the local HTTPS certificate, run:

```bash
dotnet dev-certs https --trust
```

### 4. Start the frontend

In a second terminal, run:

```bash
cd myapp.client
nvm use          # If you manage Node.js with nvm
npm install
npm run dev
```

Open the local URL shown by Vite, normally `http://localhost:5173`.

## Development Commands

From `myapp.client`:

```bash
npm run dev      # Start the frontend development server
npm run build    # Type-check and create a production frontend build
npm run lint     # Run ESLint
```

From the repository root:

```bash
dotnet build                 # Build the backend
dotnet ef database update    # Apply pending database migrations
```

## Roadmap

- Add dedicated pages for running clubs
- Improve event discovery and filtering
- Match runners based on goals, pace, and preferred distance
- Add automated backend and frontend tests

## Repository Structure

```text
RunnerMeets/
├── Controllers/       # API controllers
├── Migrations/        # Entity Framework Core migrations
├── Models/            # Database entities and application context
├── Services/          # Application services
├── myapp.client/      # React and TypeScript frontend
└── Program.cs         # ASP.NET Core application configuration
```
