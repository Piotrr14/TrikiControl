@echo off
setlocal EnableExtensions EnableDelayedExpansion

cd /d "%~dp0"

set LOG=%~dp0install_log.txt
set REPORT=%~dp0CHATGPT_REPORT.txt

echo ========================================== > "%LOG%"
echo TRIKICONTROL ENV INSTALLER LOG >> "%LOG%"
echo ========================================== >> "%LOG%"

echo [START] Instalacja srodowiska TrikiControl...
echo.

:: =========================
:: DOTNET CHECK / INSTALL
:: =========================
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo [INFO] .NET nie znaleziony - instalacja STS...

    echo [.NET INSTALL] missing >> "%LOG%"

    powershell -NoProfile -ExecutionPolicy Bypass -Command ^
    "Invoke-WebRequest https://dot.net/v1/dotnet-install.ps1 -OutFile dotnet-install.ps1"

    powershell -NoProfile -ExecutionPolicy Bypass -File dotnet-install.ps1 -Channel STS >> "%LOG%" 2>&1

    set "DOTNET_ROOT=%USERPROFILE%\.dotnet"
    set "PATH=%DOTNET_ROOT%;%PATH%"

) else (
    echo [OK] dotnet detected >> "%LOG%"
)

echo.
echo [INFO] Checking dotnet...
dotnet --info >> "%LOG%" 2>&1

if %errorlevel% neq 0 (
    goto ERROR
)

:: =========================
:: CHECK PROJECT FILE
:: =========================
if not exist "*.csproj" (
    echo [ERROR] Missing csproj >> "%LOG%"
    goto ERROR
)

:: =========================
:: RESTORE
:: =========================
echo.
echo [INFO] Running dotnet restore...
dotnet restore >> "%LOG%" 2>&1

if %errorlevel% neq 0 (
    echo [ERROR] restore failed >> "%LOG%"
    goto ERROR
)

echo [OK] restore success >> "%LOG%"

echo.
echo ==========================================
echo  INSTALACJA ZAKONCZONA SUKCESEM
echo ==========================================
echo.

pause
exit /b 0


:: =========================
:: ERROR HANDLER
:: =========================
:ERROR

echo.
echo ==========================================
echo  BLAD WYKRYTY
echo ==========================================
echo.

echo [ERROR] Installation failed >> "%LOG%"

echo =============================== > "%REPORT%"
echo TRIKICONTROL DEBUG REPORT >> "%REPORT%"
echo =============================== >> "%REPORT%"
echo. >> "%REPORT%"

echo [DOTNET INFO] >> "%REPORT%"
dotnet --info >> "%REPORT%" 2>&1

echo. >> "%REPORT%"
echo [INSTALL LOG] >> "%REPORT%"
type "%LOG%" >> "%REPORT%"

echo. >> "%REPORT%"
echo =============================== >> "%REPORT%"
echo JAK WYSŁAĆ PROBLEM DO CHATGPT >> "%REPORT%"
echo =============================== >> "%REPORT%"
echo. >> "%REPORT%"
echo 1. Otworz plik CHATGPT_REPORT.txt >> "%REPORT%"
echo 2. Skopiuj CAŁĄ zawartość >> "%REPORT%"
echo 3. Wklej do ChatGPT >> "%REPORT%"
echo 4. Napisz: >> "%REPORT%"
echo    "Przeanalizuj logi instalacji .NET i napraw problem" >> "%REPORT%"
echo. >> "%REPORT%"

echo.
echo ==========================================
echo  PROBLEM WYKRYTY
echo ==========================================
echo.
echo Utworzono plik diagnostyczny:
echo %REPORT%
echo.
echo Skopiuj jego zawartosc i wklej do ChatGPT.
echo.

pause
exit /b 1