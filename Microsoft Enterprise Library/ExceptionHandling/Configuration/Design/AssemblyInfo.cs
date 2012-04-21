//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
[assembly : ConfigurationDesignManager(typeof(ExceptionHandlingConfigurationDesignManager))]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum)]
[assembly : EventLogPermission(SecurityAction.RequestMinimum)]

[assembly : AssemblyTitle("Enterprise Library Exception Handling Application Block Design")]
[assembly : AssemblyDescription("Exception Handling Configuration Design")]
[assembly : AssemblyDelaySign(false)]
[assembly : AssemblyKeyFile("")]
[assembly : AssemblyKeyName("")]
