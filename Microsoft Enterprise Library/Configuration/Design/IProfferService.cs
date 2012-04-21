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

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>This interface provides a general mechanism for objects to offer services to other objects sited on the same host.</para>
    /// </summary>
    [ComImport]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("CB728B20-F786-11CE-92AD-00AA00A74CD0")]
    internal interface IProfferService
    {
        /// <summary>
        /// <para>Makes a service available to other objects cited on the same host.</para>
        /// </summary>
        /// <param name="rguidService">
        /// <para>Value of type GUID that specifies the service being offered.</para>
        /// </param>
        /// <param name="psp">
        /// <para>Pointer to an IOleServiceProvider interface.</para>
        /// </param>
        /// <param name="pdwCookie">
        /// <para>Pointer to a variable of type DWORD that receives an implementation-defined value used for identification purposes. The calling application must keep track of this value in the event it needs to execute IProfferService::RevokeService.</para>
        /// </param>
        /// <returns>
        /// <para>Returns S_OK if successful, or an error value otherwise.</para>
        /// </returns>
        int ProfferService(
            [In] ref Guid rguidService,
            [In]
            [MarshalAs(UnmanagedType.Interface)] IOleServiceProvider psp,
            [Out]
            [MarshalAs(UnmanagedType.U4)] out uint pdwCookie);

        /// <summary>
        /// <para>Makes a service unavailable that was previously available to other objects using <seealso cref="IProfferService.ProfferService"/></para>
        /// </summary>
        /// <param name="dwCookie">Specifies an implementation-defined value used for identification purposes. The calling application receives this value from <seealso cref="IProfferService.ProfferService"/>.</param>
        void RevokeService(
            [In]
            [MarshalAs(UnmanagedType.U4)] uint dwCookie);
    }
}