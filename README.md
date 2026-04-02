# FitnessTracker Template

Full-stack starter template:
- Frontend: React (Vite)
- Backend: ASP.NET Core Web API (.NET 10)
- Database: SQLite (local SQL database) with Entity Framework Core migrations

## Project Structure

- FitnessTracker.Api - backend API + EF Core + migrations
- fitnesstracker.web - React frontend
- FitnessTracker.slnx - solution file

## First-Time Setup

From the root folder:

```powershell
dotnet restore .\FitnessTracker.Api\FitnessTracker.Api.csproj
Set-Location .\fitnesstracker.web
npm install
```

## Run Backend

```powershell
Set-Location .\FitnessTracker.Api
dotnet run
```

Backend URL:
- http://localhost:5179

## Run Frontend

Open a second terminal:

```powershell
Set-Location .\fitnesstracker.web
npm run dev
```

Frontend URL:
- http://localhost:5173

Vite proxy is configured so frontend calls to /api/* go to backend http://localhost:5179.

## Database (Local SQL via SQLite)

The database file is created in FitnessTracker.Api/fitnesstracker.db.

### Apply migrations

```powershell
Set-Location .\FitnessTracker.Api
dotnet ef database update
```

### Create a new migration later

```powershell
Set-Location .\FitnessTracker.Api
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Starter API Endpoints

- GET /api/workouts
- GET /api/workouts/{id}
- POST /api/workouts
- PUT /api/workouts/{id}
- DELETE /api/workouts/{id}
