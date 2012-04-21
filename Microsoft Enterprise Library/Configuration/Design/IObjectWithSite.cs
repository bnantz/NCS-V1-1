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
    /// <para>The IObjectWithSite interface provides a simple way to support communication between an object and its site in the container.</para>
    /// </summary>
    [ComImport]
    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectWithSite
    {
        /// <summary>
        /// <para>Provides the site's IUnknown pointer to the object. The object should hold onto this pointer, calling IUnknown::AddRef in doing so. If the object already has a site, it should call that existing site's IUnknown::Release, save the new site pointer, and call the new site's IUnknown::AddRef.</para>
        /// </summary>
        /// <param name="pUnkSite">
        /// <para>Pointer to the IUnknown interface pointer of the site managing this object. If NULL, the object should call IUnknown::Release on any existing site at which point the object no longer knows its site.</para>
        /// </param>
        void SetSite([MarshalAs(UnmanagedType.Interface)] object pUnkSite);

        /// <summary>
        /// <para>Retrieves the last site set with IObjectWithSite::SetSite. If there's no known site, the object return a failure code.</para>
        /// </summary>
        /// <param name="riid">
        /// <para>The IID of the interface pointer that should be returned in ppvSite.</para>
        /// </param>
        /// <param name="ppvSite"><para>Address of pointer variable that receives the interface pointer requested in riid. Upon successful return, *ppvSite contains the requested interface pointer to the site last seen in IObjectWithSite::SetSite. The specific interface returned depends on the riid argument—in essence, the two arguments act identically to those in IUnknown::QueryInterface. If the appropriate interface pointer is available, the object must call IUnknown::AddRef on that pointer before returning successfully. If no site is available, or the requested interface is not supported, this method must *ppvSite to NULL and return a failure code.</para></param>
        void GetSite([In] ref Guid riid, [Out]
        [MarshalAs(UnmanagedType.LPArray)] object[] ppvSite);
    }
}