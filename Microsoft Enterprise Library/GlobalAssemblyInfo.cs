//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Runtime.InteropServices;
[assembly : ComVisible(false)]
[assembly : CLSCompliant(true)]
[assembly : AssemblyProduct("Microsoft Enterprise Library for .NET")]
[assembly : AssemblyCompany("Microsoft Corporation")]
[assembly : AssemblyVersion("1.0.0.0")]
#if DEBUG
#if  DEBUG && LONG_RUNNING_TESTS
[assembly : AssemblyConfiguration("Debug Long")]
#else
[assembly : AssemblyConfiguration("Debug")]
#endif
#else
#if UNIT_TESTS
[assembly : AssemblyConfiguration("Release Test")]
#elif FINAL
[assembly : AssemblyConfiguration("Release")]
#else 
[assembly : AssemblyConfiguration("Release Unchecked")]
#endif
#endif
[assembly : AssemblyCopyright("")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]