@echo off
FOR /R %%i IN (*.*) DO (
echo %%~ni%%~xi
certutil.exe -hashfile %%i MD5 | find /i /v "md5" | find /i /v "certutil" | find /i /v "\n"
rem > tmp.txt
rem SETLOCAL EnableDelayedExpansion
rem for /f "Tokens=* Delims=" %%x in (tmp.txt) do set hashsum=!hashsum!%%x
rem for /f "delims=" %%x in (tmp.txt) do set hashsum=%%x
rem SET /P hashsum=< tmp.txt
rem echo %%~ni%%~xi
rem echo dffr %hashsum%

)
@pause