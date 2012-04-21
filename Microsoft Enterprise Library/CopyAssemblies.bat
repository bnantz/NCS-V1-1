@echo off
@REM  ----------------------------------------------------------------------------
@REM  CopyAssemblies.bat file
@REM
@REM  This batch file copies the Enterprise Library assemblies from their build
@REM  location to a common destination folder.
@REM  
@REM  Optional arguments for this batch file:
@REM   1 - The build output folder (Release, Debug, etc. Defaults to Debug)
@REM   2 - The destination folder (i.e. where the build will be dropped.
@REM       Defaults to ..\bin)
@REM  
@REM  ----------------------------------------------------------------------------

echo.
echo =========================================================
echo   CopyAssemblies                                         
echo      Copies EnterpriseLibrary assemblies to a single    
echo      destination directory                              
echo =========================================================
echo.

set solutionDir=.
set buildType=Debug
set binDir=..\bin
set pause=true

if "%1"=="/?" goto HELP

if not Exist %solutionDir%\enterpriselibrary.sln goto HELP

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------

if /i "%1"=="/q" (
 set pause=false
 SHIFT
)

@REM  ------------------------------------------------------
@REM  User can override default build type by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Release).
@REM  ------------------------------------------------------

if not "%1"=="" set buildType=%1

@REM  ---------------------------------------------------------------
@REM  User can override default destination directory by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Debug c:\bin).
@REM  ---------------------------------------------------------------

if not "%2"=="" set binDir=%2

@REM  ----------------------------------------
@REM  Shorten the command prompt for output
@REM  ----------------------------------------
set savedPrompt=%prompt%
set prompt=*$g


@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Started
@ECHO ----------------------------------------
@ECHO.

@REM -------------------------------------------------------
@REM Change to the directory where the solution file resides
@REM -------------------------------------------------------

pushd %solutionDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Create destination folder 
@ECHO ----------------------------------------
@ECHO.

if not Exist %binDir% mkdir %binDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Caching files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll copy /V Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml copy /V Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll copy /V Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml copy /V Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll copy /V Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml copy /V Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll copy /V Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml copy /V Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll copy /V Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml copy /V Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll copy /V Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml copy /V Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Common files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll copy /V Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml copy /V Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Configuration files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Configuration\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.dll copy /V Configuration\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Configuration\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.xml copy /V Configuration\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll copy /V Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml copy /V Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Data Access files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll copy /V Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml copy /V Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll copy /V Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml copy /V Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Data\DB2\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.DB2.dll copy /V Data\DB2\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.DB2.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Data\DB2\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.DB2.xml copy /V Data\DB2\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.DB2.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Exception Handling files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll copy /V ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml copy /V ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll copy /V ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml copy /V ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll copy /V ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml copy /V ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll copy /V ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml copy /V ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Logging files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll copy /V Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml copy /V Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll copy /V Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml copy /V Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Logging\MSMQDistributor\bin\%buildType%\MSMQDistributor.exe copy /V Logging\MSMQDistributor\bin\%buildType%\MSMQDistributor.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\MSMQDistributor\bin\%buildType%\MsmqDistributor.exe.config copy /V Logging\MSMQDistributor\bin\%buildType%\MsmqDistributor.exe.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\MSMQDistributor\bin\%buildType%\loggingDistributorConfiguration.config copy /V Logging\MSMQDistributor\bin\%buildType%\loggingDistributorConfiguration.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\MSMQDistributor\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml copy /V Logging\MSMQDistributor\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Logging\Sinks\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.dll copy /V Logging\Sinks\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\Sinks\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.xml copy /V Logging\Sinks\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Logging\Sinks\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design.dll copy /V Logging\Sinks\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Logging\Sinks\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design.xml copy /V Logging\Sinks\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Security files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll copy /V Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml copy /V Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\ActiveDirectory\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.dll copy /V Security\ActiveDirectory\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\ActiveDirectory\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.xml copy /V Security\ActiveDirectory\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\ActiveDirectory\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design.dll copy /V Security\ActiveDirectory\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\ActiveDirectory\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design.xml copy /V Security\ActiveDirectory\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll copy /V Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml copy /V Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll copy /V Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml copy /V Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll copy /V Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml copy /V Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.dll copy /V Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.xml copy /V Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll copy /V Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml copy /V Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll copy /V Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml copy /V Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll copy /V Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml copy /V Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.dll copy /V Security\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.xml copy /V Security\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Database\Authentication\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.dll copy /V Security\Database\Authentication\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Database\Authentication\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.xml copy /V Security\Database\Authentication\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Database\Authentication\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design.dll copy /V Security\Database\Authentication\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Database\Authentication\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design.xml copy /V Security\Database\Authentication\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Security\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design.dll copy /V Security\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Security\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design.xml copy /V Security\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------
@ECHO Copy Configuration Tool files with verification
@ECHO -----------------------------------------------
@ECHO.

if Exist Tools\ConfigurationConsole\bin\%buildType%\EntLibConfig.exe copy /V Tools\ConfigurationConsole\bin\%buildType%\EntLibConfig.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\ConfigurationConsole\bin\%buildType%\EntLibConfig.exe.config copy /V Tools\ConfigurationConsole\bin\%buildType%\EntLibConfig.exe.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\ConfigurationConsole\bin\%buildType%\ConfigurationConsole.exe.manifest copy /V Tools\ConfigurationConsole\bin\%buildType%\ConfigurationConsole.exe.manifest %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------------------
@ECHO Copy Security Database Console Tool files with verification
@ECHO -----------------------------------------------------------
@ECHO.

if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe.config copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe.manifest copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\SecurityDatabaseConsole.exe.manifest %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\App.config copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\App.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\dataConfiguration.config copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\dataConfiguration.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\securityConfiguration.config copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\securityConfiguration.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Tools\SecurityDatabaseConsole\bin\%buildType%\securityCryptographyConfiguration.config copy /V Tools\SecurityDatabaseConsole\bin\%buildType%\securityCryptographyConfiguration.config %binDir%\.
@if errorlevel 1 goto :error


@ECHO.
@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Completed
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
  @ECHO An error occured in CopyAssemblies.bat - %errorLevel%

if %pause%==true PAUSE
@exit errorLevel

:HELP
echo Usage: CopyAssemblies [/q] [build output folder] [destination dir]
echo.
echo CopyAssemblies is to be executed in the directory where EnterpriseLibrary.sln resides
echo The default build output folder is Debug
echo The default destintation directory is ..\bin
echo.
echo Examples:
echo.
echo    "CopyAssemblies" - copies Debug build assemblies to ..\bin      
echo    "CopyAssemblies Release" - copies Release build assemblies to ..\bin
echo    "CopyAssemblies Release C:\temp" - copies Release build assemblies to C:\temp
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit
popd
set pause=
set solutionDir=
set buildType=
set prompt=%savedPrompt%
set savedPrompt=
echo on