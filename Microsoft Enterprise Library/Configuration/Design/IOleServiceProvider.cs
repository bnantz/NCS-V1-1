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
    /// <para>The <see cref="IOleServiceProvider"/> interface is a generic access mechanism to locate a globally unique identifier (GUID) identified service. </para>
    /// </summary>
    /// <remarks>
    /// <para>This maps to the definition of IServiceProvider defined in servprov.idl in COM. We have to rename to avoid the collision with <see cref="IServiceProvider"/> </para>
    /// </remarks>
    [ComImport]
    [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleServiceProvider
    {
        /// <summary>
        /// <para>Acts as the factory method for any services exposed through an implementation of <see cref="IOleServiceProvider"/>.</para>
        /// </summary>
        /// <param name="guidService"><para>Unique identifier of the service (an SID).</para></param>
        /// <param name="riid"><para>Unique identifier of the interface the caller wishes to receive for the service.</para></param>
        /// <param name="ppvObject"><para>Address of the caller-allocated variable to receive the interface pointer of the service on successful return from this function. The caller becomes responsible for calling Release through this interface pointer when the service is no longer needed.</para></param>
        /// <returns>
        /// <para>
        /// Returns one of the following values:
        /// <list type="table">
        /// <item>
        /// <term>S_OK</term> 
        /// <description>The service was successfully created or retrieved. The caller is responsible for calling ((IUnknown *)*ppv)->Release();</description>
        /// </item>
        /// <item>
        /// <term>E_OUTOFMEMORY</term> 
        /// <description>There is insufficient memory to create the service.</description>
        /// </item>
        /// <item>
        /// <term>E_UNEXPECTED</term>
        /// <description>An unknown error occurred.</description>
        /// </item>
        /// <item>
        /// <term>E_NOINTERFACE</term>
        /// <description>The service exists, but the interface requested does not exist on that service.</description>
        /// </item>
        /// <item>
        /// <term>SVC_E_UNKNOWNSERVICE</term>
        /// <description>The service identified with guidService is not recognized.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </returns>
        [PreserveSig]
        int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
    }
}