@echo off

set hh=%TIME:~0,2%
set mm=%TIME:~3,2%
set ss=%TIME:~6,2%
set t=%hh%:%mm%:%ss%

echo Dont forget set: Set-ExecutionPolicy RemoteSigned -Scope CurrentUser

echo Run Mirroring Projects
powershell ./XConductor-Mirroring-Tool-W8.ps1

goto end

:error
echo ERROR EXECUTING SCRIPT %0
exit /b %errorlevel%

:end
powershell Write-Host '=============== Done! was runing'([datetime]::Parse('%t%') - [datetime]::Now).ToString('hh\:mm\:ss\.fff')'===============' -ForegroundColor Green
pause
rem 



