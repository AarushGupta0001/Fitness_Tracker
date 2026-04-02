@echo off
cd /d "C:\C_Sharp\FitnessTracker\FitnessTracker.Api"

echo Applying database migrations...
dotnet ef database update

echo.
echo Starting FitnessTracker API on port 5179...
dotnet run --launch-profile https
pause
