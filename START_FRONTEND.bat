@echo off
cd /d "C:\C_Sharp\FitnessTracker\fitnesstracker.web"

echo.
echo ======================================
echo  FitnessTracker Frontend - Port 5173
echo ======================================
echo.

set /p port="Enter backend port (5179, 5180, or 5181) [default: 5179]: "
if "%port%"=="" set port=5179

echo Using backend port: %port%
echo.
echo Starting frontend on http://localhost:5173
echo Frontend will proxy API calls to http://localhost:%port%
echo.

set BACKEND_PORT=%port%
npm run dev

pause
