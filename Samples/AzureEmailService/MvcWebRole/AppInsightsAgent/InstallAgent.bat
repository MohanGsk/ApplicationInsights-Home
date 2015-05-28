REM /*********************************************************
REM *                                                        *
REM *    Copyright (C) Microsoft. All rights reserved.       *
REM *                                                        *
REM *********************************************************/

@ECHO OFF

REM Check if this task is running on the compute emulator.
IF "%RoleEnvironment.IsEmulated%" == "true" (
    EXIT /B 0
)

ECHO %DATE% %TIME% - Start executing script under %USERNAME% >> "%ROLEROOT%\install.agent.info.log" 2>&1

ECHO ................................................ >> "%ROLEROOT%\install.agent.info.log" 2>&1

REM Log the environment variables
ECHO ApplicationInsightsAgent.DownloadLink=%ApplicationInsightsAgent.DownloadLink% >> "%ROLEROOT%\install.agent.info.log" 2>&1
ECHO RoleEnvironment.IsEmulated=%RoleEnvironment.IsEmulated% >> "%ROLEROOT%\install.agent.info.log" 2>&1

ECHO ................................................ >> "%ROLEROOT%\install.agent.info.log" 2>&1

REM Following sets appropriate execution policy on the host machine
SET ExecutionPolicyLevel=RemoteSigned
FOR /F "usebackq" %%i IN (`powershell -noprofile -command "Get-ExecutionPolicy"`) DO (
    SET ExecutionPolicy=%%i
    IF /I "%%i"=="Unrestricted" GOTO :InstallAgent
    IF /I "%%i"=="RemoteSigned" GOTO :InstallAgent
    Powershell.exe -NoProfile -Command "Set-ExecutionPolicy RemoteSigned" < NUL >> NUL 2>> NUL
)

:InstallAgent

ECHO %DATE% %TIME% - Start powershell InstallAgent.ps1 >> "%ROLEROOT%\install.agent.info.log" 2>&1
Powershell.exe -NoProfile -Command "& '%~dp0InstallAgent.ps1'" < NUL >> NUL 2>> NUL
IF %ERRORLEVEL% EQU 0 (
    ECHO %DATE% %TIME% - Completed executing InstallAgent.ps1 normally. >> "%ROLEROOT%\install.agent.info.log" 2>&1
) ELSE (
    ECHO %DATE% %TIME% - Failed executing InstallAgent.ps1 with ERRORLEVEL = %ERRORLEVEL%. >> "%ROLEROOT%\install.agent.error.log" 2>&1
)

REM If an error occurred, return the errorlevel.
EXIT /B %ERRORLEVEL%