@echo off
@REM  ---------------------------------------------------------------------------------
@REM  InstallServices.bat file
@REM
@REM  This batch file installs/uninstalls various services for the Enterprise Library 
@REM  application blocks.
@REM  
@REM  Optional arguments for this batch file:
@REM    1 - /u to unstall. Otherwise it is installed.
@REM  ----------------------------------------------------------------------------------

echo.
echo ==========================================================================
echo   InstallServices.bat                                                    
echo      Installs/uninstalls services for the Enterprise Library  
echo ==========================================================================
echo.

set visualStudioDir=C:\Program Files\Microsoft Visual Studio .NET 2003\Common7\IDE\
set binDir="..\bin"
set pause=true

@REM  ---------------------------------------------------------------
@REM  User can override default directory containing the
@REM  the Enterprise Library assemblies by supplying 
@REM  a parameter to batch file (e.g. InstallServices C:\bin).
@REM  ---------------------------------------------------------------

if "%1"=="/?" goto HELP

if "%1"=="" goto RUN

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------

if /i "%1"=="/q" (
 set pause=false
 SHIFT
)

@REM  ----------------------------------------------------
@REM  If the first parameter is /u, uninstall.
@REM  ----------------------------------------------------

if /i "%1"=="/u" goto RUN

goto HELP

:RUN

@REM  ------------------------------------------------
@REM  Shorten the command prompt for making the output
@REM  easier to read.
@REM  ------------------------------------------------
set savedPrompt=%prompt%
set prompt=*$g

@ECHO ----------------------------------------
@ECHO InstallServices.bat Started
@ECHO ----------------------------------------
@ECHO.

@REM  ----------------------------------------
@REM  Register VS.NET environment variables
@REM  (required to call installutil)
@REM  ----------------------------------------

if not Exist "%visualStudioDir%..\Tools\vsvars32.bat" goto HELPVS

@call "%visualStudioDir%..\Tools\vsvars32.bat"

@REM -------------------------------------------------------
@REM Change to the directory where the assemblies reside
@REM -------------------------------------------------------

pushd %binDir%

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the Common Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Common.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Common.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the Caching Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Caching.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Caching.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the ConfigurationApplication Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Configuration.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Configuration.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the Cryptography Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the Data Access Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Data.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Data.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------------
@ECHO Installing Services for the Exception Handling Application Block
@ECHO -----------------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO ---------------------------------------------------------------------------------
@ECHO Installing Services for the Logging and Instrumentation Application Block
@ECHO ---------------------------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Logging.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Logging.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------------
@ECHO Installing Services for the Security Application Block
@ECHO -----------------------------------------------------------------
@ECHO.

if Exist Microsoft.Practices.EnterpriseLibrary.Security.dll installutil %1 Microsoft.Practices.EnterpriseLibrary.Security.dll
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO InstallServices.bat Completed
@ECHO ----------------------------------------
@ECHO.

@REM  ----------------------------------------
@REM  Restore the command prompt and exit
@REM  ----------------------------------------
@goto :exit

@REM  -------------------------------------------
@REM  Handle errors
@REM
@REM  Use the following after any call to exit
@REM  and return an error code when errors occur
@REM
@REM  if errorlevel 1 goto :error	
@REM  -------------------------------------------
:error
@ECHO An error occured in InstallServices.bat - %errorLevel%
if %pause%==true PAUSE
@exit errorLevel

:HELPVS
echo Error: Unable to locate Visual Studio.NET
echo.
echo InstallServices.bat assumes Visual Studio.NET 2003 has been installed in its default location 
echo ("%visualStudioDir%".) 
echo.
echo If you have installed Visual Studio.NET 2003 to a different location, you will need 
echo to update this batch file to reflect that location.
echo.
goto exit

:HELP
echo Usage: InstallServices.bat [/q] [/u]   
echo.
echo Examples:
echo.
echo    "InstallServices" - installs services for Enterprise Library assemblies       
echo    "InstallServices /u" - uninstalls services for Enterprise Library assemblies 
echo    "InstallServices /q" - installs services, no pause when error occurs (quiet mode)     
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit

popd
set pause=
set binDir=
set prompt=%savedPrompt%
set savedPrompt=

echo on

