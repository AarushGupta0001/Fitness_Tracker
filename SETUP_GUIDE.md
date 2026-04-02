## FitnessTracker Setup Guide - SQL Server Version

### Prerequisites
- SQL Server LocalDB (comes with Visual Studio)
- .NET 10.0 SDK
- Node.js & npm

---

## PART 1: DATABASE SETUP

### Step 1: Create Database in SQL Server
Open **SQL Server Management Studio (SSMS)** and run:

```sql
-- Create database
CREATE DATABASE FitnessTrackerDb;

-- You can now see it in SSMS:
-- (localdb)\mssqllocaldb > Databases > FitnessTrackerDb
```

The database will be automatically populated with tables when you run migrations.

---

## PART 2: BACKEND SETUP

### Step 2: Restore NuGet Packages & Build Backend

Open **PowerShell** or **Command Prompt** and run:

```powershell
cd "C:\C_Sharp\FitnessTracker\FitnessTracker.Api"

# Restore NuGet packages (installs SQL Server EF Core)
dotnet restore

# Build the project
dotnet build
```

### Step 3: Apply Database Migrations (Creates Tables)

```powershell
# This creates all tables in SQL Server
dotnet ef database update
```

You should see output:
```
Build started...
Build succeeded.
Applying migration '20260401052046_InitialCreate'.
Applying migration '20260401052913_AddExerciseAndWorkoutSession'.
Done. Verifying the solution.
```

### Step 4: Verify Database in SSMS

1. Open **SSMS**
2. Connect to `(localdb)\mssqllocaldb`
3. Expand **Databases** > **FitnessTrackerDb** > **Tables**
4. You should see:
   - `Workouts`
   - `Exercises`
   - `WorkoutSessions`

---

## PART 3: RUN THE BACKEND

### Choose Your Port (5179, 5180, or 5181)

**Option A: Port 5179 (Default)**
```powershell
cd "C:\C_Sharp\FitnessTracker\FitnessTracker.Api"
dotnet run --launch-profile http
```

**Option B: Port 5180**
```powershell
dotnet run --launch-profile http-5180
```

**Option C: Port 5181**
```powershell
dotnet run --launch-profile http-5181
```

You should see:
```
Now listening on: http://localhost:5179
Application started. Press Ctrl+C to quit.
```

---

## PART 4: RUN THE FRONTEND

### In a NEW terminal window:

```powershell
cd "C:\C_Sharp\FitnessTracker\fitnesstracker.web"

# If using port 5179 (default):
npm run dev

# If using port 5180:
BACKEND_PORT=5180 npm run dev

# If using port 5181:
BACKEND_PORT=5181 npm run dev
```

Frontend starts at: `http://localhost:5173`

---

## QUICK START CHECKLIST

- [ ] Backend running on one of: 5179, 5180, 5181
- [ ] See "Now listening on: http://localhost:PORT" in terminal
- [ ] Frontend running on http://localhost:5173
- [ ] Database created in SSMS as `FitnessTrackerDb`
- [ ] Tables visible in SSMS: Workouts, Exercises, WorkoutSessions

---

## TROUBLESHOOTING

**"Port already in use"?**
→ Use a different port (5179, 5180, or 5181)

**"Cannot connect to database"?**
→ Run: `dotnet ef database update` again

**"No tables in database"?**
→ Run: `dotnet ef database update` to apply migrations

**"Frontend can't connect to backend"?**
→ Make sure BACKEND_PORT env var matches backend port
→ Check browser console for CORS errors

---

## MANUAL DATABASE MANAGEMENT

### View/Edit Data in SSMS:
1. Open SSMS
2. Connect to `(localdb)\mssqllocaldb`
3. Right-click table → "Edit Top 200 Rows"
4. Add/modify exercise data directly

### Add Sample Data (Optional):
```sql
-- Connect to FitnessTrackerDb and run:
INSERT INTO Exercises (Name, Weight, Sets, Reps, Date, MuscleGroup, Username)
VALUES
('Bench Press', 80, 4, 6, CAST(GETDATE() AS DATE), 0, 'Aarush'),
('Squat', 100, 4, 8, CAST(GETDATE() AS DATE), 5, 'Aarush');
```

---

## CONNECTION STRING (Already Updated)

Located in: `C:\C_Sharp\FitnessTracker\FitnessTracker.Api\appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FitnessTrackerDb;Integrated Security=true;"
}
```

This connects to SQL Server LocalDB, not SQLite.
