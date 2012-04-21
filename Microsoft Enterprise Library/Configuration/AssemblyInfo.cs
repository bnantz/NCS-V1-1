//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
[assembly : FileIOPermission(SecurityAction.RequestMinimum)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, Flags=ReflectionPermissionFlag.MemberAccess)]
[assembly : AssemblyTitle("Enterprise Library Configuration Application Block")]
[assembly : AssemblyDescription("Configuration Management")]
[assembly : AssemblyDelaySign(false)]
[assembly : AssemblyKeyFile("")]
[assembly : AssemblyKeyName("")]