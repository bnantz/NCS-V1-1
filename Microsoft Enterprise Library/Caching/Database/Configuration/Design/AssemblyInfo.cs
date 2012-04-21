//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess=true)]
[assembly : ConfigurationDesignManager(typeof(CachingDatabaseConfigurationDesignManager))]

[assembly : AssemblyTitle("Enterprise Library Caching Data Provider Design")]
[assembly : AssemblyDescription("")]
[assembly : AssemblyDelaySign(false)]
[assembly : AssemblyKeyFile("")]
[assembly : AssemblyKeyName("")]