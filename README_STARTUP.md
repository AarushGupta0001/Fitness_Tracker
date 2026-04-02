# 🚀 WHAT TO DO NOW - SQL Server Setup

## Summary of Changes Made ✅

1. **Database Changed**: SQLite → SQL Server (LocalDB)
2. **Connection Updated**: Points to `(localdb)\mssqllocaldb`
3. **Database Name**: `FitnessTrackerDb` (visible in SSMS)
4. **Ports Flexible**: Can use 5179, 5180, or 5181
5. **Frontend Updated**: Supports `BACKEND_PORT` environment variable

---

## ⚠️ FIRST TIME SETUP ONLY

### Step 1: Open PowerShell in Administrator Mode

Then copy-paste this command:

```powershell
cd "C:\C_Sharp\FitnessTracker\FitnessTracker.Api" && dotnet restore && dotnet ef database update
```

**What this does:**
- Installs SQL Server EF Core package
- Creates database `FitnessTrackerDb` in SQL Server
- Creates all tables (Workouts, Exercises, WorkoutSessions)

---

## 🎯 Easy Startup (Every Time After Setup)

### **Option 1: Use Batch Files (EASIEST)**

In `C:\C_Sharp\FitnessTracker\` folder, you have:

**Terminal 1 - Backend (Pick ONE):**
- Double-click: `START_BACKEND_5179.bat` (Port 5179)
- OR: `START_BACKEND_5180.bat` (Port 5180)
- OR: `START_BACKEND_5181.bat` (Port 5181)

Wait for: `Now listening on: http://localhost:PORT`

**Terminal 2 - Frontend:**
- Double-click: `START_FRONTEND.bat`
- It will ask which port the backend is using
- Wait for: `Local: http://localhost:5173`

Then open: **http://localhost:5173**

---

### **Option 2: Manual Commands**

**Terminal 1 - Backend (Choose Port):**

```powershell
cd "C:\C_Sharp\FitnessTracker\FitnessTracker.Api"

# Port 5179
dotnet run --launch-profile http

# OR Port 5180
dotnet run --launch-profile http-5180

# OR Port 5181
dotnet run --launch-profile http-5181
```

**Terminal 2 - Frontend:**

```powershell
cd "C:\C_Sharp\FitnessTracker\fitnesstracker.web"

# If backend is on 5179:
npm run dev

# If backend is on 5180:
$env:BACKEND_PORT=5180; npm run dev

# If backend is on 5181:
$env:BACKEND_PORT=5181; npm run dev
```

---

## 📊 Verify Everything Works

### 1. Check Backend Running
Open: **http://localhost:5179/openapi/v1.json**
(or 5180/5181 if using different port)

Should show API documentation ✓

### 2. Check Database in SSMS
1. Open **SQL Server Management Studio**
2. Connect to: `(localdb)\mssqllocaldb`
3. Expand: **Databases** → **FitnessTrackerDb** → **Tables**
4. Should see:
   - Workouts
   - Exercises
   - WorkoutSessions

### 3. Check Frontend
Open: **http://localhost:5173**

Should see the calendar page ✓

### 4. Test Full Flow
- Pick a date on calendar
- Click "Enter"
- Select muscle groups
- Click "Enter"
- Should see exercises page with sidebar

---

## 📝 Manually Edit Database in SSMS

### Add Exercise Records:

1. Open SSMS
2. Connect to `(localdb)\mssqllocaldb`
3. Right-click **FitnessTrackerDb** → **Databases**
4. Right-click **Exercises** table → **Edit Top 200 Rows**
5. Add new rows manually

### Example Insert Query:
```sql
INSERT INTO Exercises (Name, Weight, Sets, Reps, Date, MuscleGroup, Username)
VALUES
('Bench Press', 100, 4, 8, CAST(GETDATE() AS DATE), 0, 'Aarush'),
('Squats', 150, 4, 8, CAST(GETDATE() AS DATE), 5, 'Aarush');
```

---

## 🔧 Files Modified

- ✅ `appsettings.json` - Connection string changed to SQL Server
- ✅ `Program.cs` - Changed from UseSqlite to UseSqlServer
- ✅ `FitnessTracker.Api.csproj` - Swapped SQLite package for SQL Server package
- ✅ `launchSettings.json` - Added 3 port profiles (5179, 5180, 5181)
- ✅ `vite.config.js` - Added BACKEND_PORT environment variable support

---

## ❌ Troubleshooting

| Problem | Solution |
|---------|----------|
| "Port 5179 already in use" | Use 5180 or 5181 instead |
| "No database in SSMS" | Run: `dotnet ef database update` |
| "No tables in database" | Run: `dotnet ef database update --verbose` |
| "Backend won't start" | Make sure LocalDB is installed |
| "Frontend won't connect" | Check BACKEND_PORT matches backend port |
| "Get error 'UseSqlite not found'" | Run: `dotnet restore` |

---

## 📍 Important Paths

- Backend: `C:\C_Sharp\FitnessTracker\FitnessTracker.Api`
- Frontend: `C:\C_Sharp\FitnessTracker\fitnesstracker.web`
- Database: `(localdb)\mssqllocaldb` → Database name: `FitnessTrackerDb`
- Connection String: `appsettings.json` in backend folder

---

## ✨ Next Steps

1. **Run FIRST TIME SETUP command** (One-time only)
2. **Use batch files to start backend + frontend**
3. **Open http://localhost:5173**
4. **View/edit database in SSMS**

That's it! 🎉
