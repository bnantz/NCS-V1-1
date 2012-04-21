@echo off

echo ==================================================
echo Uninstalling Data Instrumentation
echo ==================================================
installutil /u ..\..\..\Data\bin\%1\Microsoft.Practices.EnterpriseLibrary.Data.dll

echo ==================================================
echo Uninstalling Caching Instrumentation
echo ==================================================
installutil /u ..\..\..\Caching\bin\%1\Microsoft.Practices.EnterpriseLibrary.Caching.dll

echo ==================================================
echo Uninstalling Logging Instrumentation
echo ==================================================
installutil /u ..\..\..\Logging\bin\%1\Microsoft.Practices.EnterpriseLibrary.Logging.dll

echo ==================================================
echo Installing Logging Instrumentation Tests
echo ==================================================
installutil /u ..\..\..\Logging\Sinks\WMISchema\bin\%1\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.WMISchema.Tests.dll

echo ==================================================
echo Uninstalling ExceptionHandling Instrumentation
echo ==================================================
installutil /u ..\..\..\ExceptionHandling\bin\%1\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll

echo ==================================================
echo Uninstalling Security Instrumentation
echo ==================================================
installutil /u ..\..\..\Security\bin\%1\Microsoft.Practices.EnterpriseLibrary.Security.dll

echo ==================================================
echo Uninstalling Security Cryptography Instrumentation
echo ==================================================
installutil /u ..\..\..\Security\Cryptography\bin\%1\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll
