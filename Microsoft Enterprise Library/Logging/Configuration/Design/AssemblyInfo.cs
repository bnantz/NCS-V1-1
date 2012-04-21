//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor;

[assembly : ReflectionPermission(SecurityAction.RequestMinimum)]

[assembly : ConfigurationDesignManager(typeof(ClientLoggingConfigurationDesignManager))]
[assembly : ConfigurationDesignManager(typeof(DistributorLoggingConfigurationDesignManager))]

[assembly : AssemblyTitle("Enterprise Library Logging and Instrumentation Application Block Design")]
[assembly : AssemblyDescription("Logging Configuration Design")]
[assembly : AssemblyDelaySign(false)]
[assembly : AssemblyKeyFile("")]
[assembly : AssemblyKeyName("")]
