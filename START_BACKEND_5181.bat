@echo off
REM Start Backend on Port 5181
cd /d "C:\C_Sharp\FitnessTracker\FitnessTracker.Api"

echo.
echo ======================================
echo  FitnessTracker API - Port 5181
echo ======================================
echo.

echo Restoring packages...
dotnet restore >nul 2>&1

echo Applying database migrations...
dotnet ef database update --verbose

echo.
echo Starting Backend on http://localhost:5181
echo.
dotnet run --launch-profile http-5181

pause
