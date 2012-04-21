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
    /// <para>This wraps the IOleServiceProvider interface and provides an easy COM+ way to get at services.</para>
    /// </summary>
    public class ServiceProvider : IServiceProvider, IObjectWithSite
    {
        private static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

        private IOleServiceProvider oleServiceProvider;

        /// <summary>
        /// <para>Creates a new ServiceProvider object and uses the given interface to resolve services.</para>
        /// </summary>
        /// <param name="oleServiceProvider">
        /// <para>The IOleServiceProvider interface to use.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="oleServiceProvider"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ServiceProvider(IOleServiceProvider oleServiceProvider)
        {
            if (null == oleServiceProvider)
            {
                throw new ArgumentNullException("oleServiceProvider");
            }
            this.oleServiceProvider = oleServiceProvider;
        }

        /// <summary>
        /// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceProvider"/> is reclaimed by garbage collection. </para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceContainer"/> is reclaimed by garbage collection. </para>
        /// </summary>
        /// <param name="disposing"><para><c>true</c> if we are disposing; otherwise, <c>false</c> which means the call is coming from our finalizer.</para></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (oleServiceProvider != null)
                {
                    oleServiceProvider = null;
                }
            }
        }

        /// <summary>
        /// <para>Retrieves the requested service.</para>
        /// </summary>
        /// <param name="serviceClass">
        /// <para>The type of the service to retrieve.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the service or <c>null</c> if no such service exists.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceClass"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        public virtual object GetService(Type serviceClass)
        {
            if (null == serviceClass)
            {
                throw new ArgumentNullException("serviceClass");
            }
            return GetService(serviceClass.GUID);
        }

        /// <summary>
        /// <para>Retrieves the requested service.</para>
        /// </summary>
        /// <param name="guid">
        /// <para>The GUID of the service to retrieve.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the service or null if no such service exists.</para>
        /// </returns>
        public virtual object GetService(Guid guid)
        {
            object service = null;
            // No valid guid on the passed in class, so there is no service for it.
            if (guid.Equals(Guid.Empty))
            {
                return null;
            }
            // We provide a couple of services of our own.
            if (guid.Equals(typeof(IOleServiceProvider).GUID))
            {
                return oleServiceProvider;
            }
            if (guid.Equals(typeof(IObjectWithSite).GUID))
            {
                return this;
            }
            IntPtr pUnk;
            int hr = oleServiceProvider.QueryService(ref guid, ref IID_IUnknown, out pUnk);
            if (Succeeded(hr) && (pUnk != IntPtr.Zero))
            {
                service = Marshal.GetObjectForIUnknown(pUnk);
                Marshal.Release(pUnk);
            }
            return service;
        }

        /// <summary>
        /// <para>Retrieves the last site set with IObjectWithSite::SetSite. If there's no known site, the object return a failure code.</para>
        /// </summary>
        /// <param name="riid">
        /// <para>The IID of the interface pointer that should be returned in ppvSite.</para>
        /// </param>
        /// <param name="ppvSite"><para>Address of pointer variable that receives the interface pointer requested in riid. Upon successful return, *ppvSite contains the requested interface pointer to the site last seen in IObjectWithSite::SetSite. The specific interface returned depends on the riid argument—in essence, the two arguments act identically to those in IUnknown::QueryInterface. If the appropriate interface pointer is available, the object must call IUnknown::AddRef on that pointer before returning successfully. If no site is available, or the requested interface is not supported, this method must *ppvSite to NULL and return a failure code.</para></param>
        public void GetSite(ref Guid riid, object[] ppvSite)
        {
            ppvSite[0] = GetService(riid);
        }

        /// <summary>
        /// <para>Provides the site's IUnknown pointer to the object. The object should hold onto this pointer, calling IUnknown::AddRef in doing so. If the object already has a site, it should call that existing site's IUnknown::Release, save the new site pointer, and call the new site's IUnknown::AddRef.</para>
        /// </summary>
        /// <param name="pUnkSite">
        /// <para>Pointer to the IUnknown interface pointer of the site managing this object. If NULL, the object should call IUnknown::Release on any existing site at which point the object no longer knows its site.</para>
        /// </param>
        public void SetSite(object pUnkSite)
        {
            if (pUnkSite is IOleServiceProvider)
            {
                oleServiceProvider = (IOleServiceProvider)pUnkSite;
            }
        }

        /// <summary>
        /// <para>Returns true if the given HRESULT is a success HRESULT</para>
        /// </summary>
        /// <param name="hr"><para>The HRESULT to test.</para></param>
        /// <returns><para><c>true</c> if the HRESULT is a success; otherwise <c>false</c>.</para></returns>
        public static bool Succeeded(int hr)
        {
            return (hr >= 0);
        }

        /// <summary>
        /// <para>Returns true if the given HRESULT is a failure HRESULT</para>
        /// </summary>
        /// <param name="hr"><para>The HRESULT to test.</para></param>
        /// <returns><para><c>true</c> if the HRESULT is a failure; otherwise <c>false</c>.</para></returns>
        public static bool Failed(int hr)
        {
            return (hr < 0);
        }
    }
}