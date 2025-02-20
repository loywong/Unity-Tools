@echo off
echo IP Address
ipconfig|find "IPv4"
echo.
echo Computer Name
hostname
pause>nul
exit