===========================
REBUILDING WebPanelBar DLLs
===========================

Before building the WebPanelBar dll's , you have to create a strong name file. To create this file, you must execute the following command: sn -k StrongKey.snk and copy the newly generated StrongKey.snk file to the Project directory. The Sn.exe utility is located in your FrameworkSDK binary directory.
Please read the 'Strong-Named Assemblies' topic in MSDN for more information.