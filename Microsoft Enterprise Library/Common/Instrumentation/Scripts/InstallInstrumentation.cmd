@echo off

echo ==================================================
echo Installing Data Instrumentation
echo ==================================================
installutil ..\..\..\Data\bin\%1\Microsoft.Practices.EnterpriseLibrary.Data.dll

echo ==================================================
echo Installing Caching Instrumentation
echo ==================================================
installutil ..\..\..\Caching\bin\%1\Microsoft.Practices.EnterpriseLibrary.Caching.dll

echo ==================================================
echo Installing Logging Instrumentation
echo ==================================================
installutil ..\..\..\Logging\bin\%1\Microsoft.Practices.EnterpriseLibrary.Logging.dll

echo ==================================================
echo Installing Logging Instrumentation Tests
echo ==================================================
installutil ..\..\..\Logging\Sinks\WMISchema\bin\%1\Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.WMISchema.Tests.dll

echo ==================================================
echo Installing ExceptionHandling Instrumentation
echo ==================================================
installutil ..\..\..\ExceptionHandling\bin\%1\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll

echo ==================================================
echo Installing Security Instrumentation
echo ==================================================
installutil ..\..\..\Security\bin\%1\Microsoft.Practices.EnterpriseLibrary.Security.dll

echo ==================================================
echo Installing Security Cryptography Instrumentation
echo ==================================================
installutil ..\..\..\Security\Cryptography\bin\%1\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll
