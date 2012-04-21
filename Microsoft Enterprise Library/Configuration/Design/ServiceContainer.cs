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
using System.Collections;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a container for services.
    /// </summary>
    public abstract class ServiceContainer : IServiceContainer, IServiceProvider, IOleServiceProvider, IDisposable
    {
        internal IDictionary profferedServices;
        internal IDictionary services;
        internal IServiceProvider parentServiceProvider;
        private IServiceContainer serviceContainer;
        private IProfferService profferService;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ServiceContainer"/> class.</para>.
        /// </summary>
        protected ServiceContainer()
        {
            profferedServices = new Hashtable();
            services = new Hashtable();
        }

        /// <summary>
        /// <para>Initializes a new instance of the ServiceContainer class using the specified parent service provider.</para>.
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>A parent service provider.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceProvider"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected ServiceContainer(IServiceProvider serviceProvider) : this()
        {
            if (null == serviceProvider)
            {
                return;
            }
            this.parentServiceProvider = serviceProvider;
            // Try to retrieve a parent container.
            serviceContainer = serviceProvider.GetService(typeof(IServiceContainer)) as IServiceContainer;
            // Try to retrieve the proffer service.
            profferService = serviceProvider.GetService(typeof(IProfferService)) as IProfferService;
        }

        /// <summary>
        /// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceContainer"/> is reclaimed by garbage collection. </para>
        /// </summary>
        ~ServiceContainer()
        {
            Dispose(false);
        }

        /// <summary>
        /// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="ServiceContainer"/> is reclaimed by garbage collection. </para>
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
            // Remove all services promoted to VS.
            if (disposing)
            {
                foreach (Type svc in profferedServices.Keys)
                {
                    RemovePromotedService(svc);
                }
                DisposeServices();
            }
        }

        /// <summary>
        /// <para>Adds the specified service to the service container, and optionally promotes the service to parent service containers.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to add.</para></param>
        /// <param name="callback"><para>A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</para></param>
        /// <param name="promote"><para><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</para></param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="callback"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><paramref name="serviceType"/> already exists.</para>
        /// </exception>
        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            AddService(serviceType, callback);
            if (promote)
            {
                AddPromotedService(serviceType, callback);
            }
        }

        /// <summary>
        /// <para>Adds the specified service to the service container.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to add.</para></param>
        /// <param name="callback"><para>A callback object that is used to create the service. This allows a service to be declared as available, but delays the creation of the object until the service is requested.</para></param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="callback"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><paramref name="serviceType"/> already exists.</para>
        /// </exception>
        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            if (null == serviceType)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }

            if (services.Contains(serviceType))
            {
                throw new InvalidOperationException(SR.ExceptionServiceExists(serviceType.Name));
            }
            services[serviceType] = callback;
        }

        /// <summary>
        /// <para>Adds the specified service to the service container, and optionally promotes the service to any parent service containers.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to add.</para></param>
        /// <param name="serviceInstance"><para>An instance of the service type to add. This object must implement or inherit from the type indicated by the serviceType parameter.</para></param>
        /// <param name="promote"><para><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</para></param>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="serviceType"/> is invalid.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="serviceInstance"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><paramref name="serviceType"/> already exists.</para>
        /// </exception>
        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            AddService(serviceType, serviceInstance);
            if (promote)
            {
                AddPromotedService(serviceType, serviceInstance);
            }
        }

        /// <summary>
        /// <para>Adds the specified service to the service container.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to add.</para></param>
        /// <param name="serviceInstance"><para>An instance of the service type to add. This object must implement or inherit from the type indicated by the serviceType parameter.</para></param>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="serviceType"/> is invalid.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="serviceInstance"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><paramref name="serviceType"/> already exists.</para>
        /// </exception>
        public void AddService(Type serviceType, object serviceInstance)
        {
            if (null == serviceType)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (null == serviceInstance)
            {
                throw new ArgumentNullException("serviceInstance");
            }

            // __ComObjects are assignable anyway. We can't check that. 
            // This is what System.ComponentModel.Design.ServiceContainer does.
            if ((null == (serviceInstance as ServiceCreatorCallback)) &&
                ((!serviceInstance.GetType().IsCOMObject && !serviceType.IsAssignableFrom(serviceInstance.GetType()))))
            {
                throw new ArgumentException(SR.ExceptionInvalidServiceInstance, "serviceInstance");
            }

            if (services.Contains(serviceType))
            {
                throw new InvalidOperationException(SR.ExceptionServiceExists(serviceType.Name));
            }

            services[serviceType] = serviceInstance;
        }

        /// <summary>
        /// <para>Gets the requested service.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to retrieve.</para></param>
        /// <returns><para>An instance of the service if it could be found, or a null reference (Nothing in Visual Basic) if it could not be found.</para></returns>
        public virtual object GetService(Type serviceType)
        {
            object serviceInstance = null;

            if (serviceType == typeof(IServiceContainer))
            {
                return this;
            }
            else
            {
                serviceInstance = services[serviceType];
            }

            if (null != (serviceInstance as ServiceCreatorCallback))
            {
                ServiceCreatorCallback cbk = (ServiceCreatorCallback)serviceInstance;
                // Create the instance through the callback.
                serviceInstance = cbk(this, serviceType);

                // __ComObjects are assignable anyway. We can't check that. 
                // This is what System.ComponentModel.Design.ServiceContainer does.
                if ((serviceInstance != null) && (!serviceInstance.GetType().IsCOMObject &&
                    !serviceType.IsAssignableFrom(serviceInstance.GetType())))
                {
                    services.Remove(serviceType);
                    serviceInstance = null;
                }
                else
                {
                    services[serviceType] = serviceInstance;
                }
            }

            // Propagate request to parents.
            if (serviceInstance == null && parentServiceProvider != null)
            {
                return parentServiceProvider.GetService(serviceType);
            }

            return serviceInstance;
        }

        /// <summary>
        /// <para>Removes the specified service type from the service container.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to remove.</para></param>
        /// <param name="promote"><c>true</c> to promote this request to any parent service containers; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void RemoveService(Type serviceType, bool promote)
        {
            RemoveService(serviceType);
            if (promote)
            {
                RemovePromotedService(serviceType);
            }
        }

        /// <summary>
        /// <para>Removes the specified service type from the service container.</para>
        /// </summary>
        /// <param name="serviceType">The type of service to remove.</param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceType"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void RemoveService(Type serviceType)
        {
            if (null == serviceType)
            {
                throw new ArgumentNullException("serviceType");
            }
            services.Remove(serviceType);
        }

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
        public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            foreach (DictionaryEntry entry in services)
            {
                if (((Type)entry.Key).GUID == guidService)
                {
                    object service = GetService((Type)entry.Key);
                    IntPtr pUnk = Marshal.GetIUnknownForObject(service);
                    int hr = Marshal.QueryInterface(pUnk, ref riid, out ppvObject);
                    Marshal.Release(pUnk);

                    return hr;
                }
            }

            ppvObject = (IntPtr)0;
            return 0;
        }

        private void DisposeServices()
        {
            foreach (object service in services.Values)
            {
                if (service == this) continue;
                IDisposable disposable = service as IDisposable;
                if (disposable != null) disposable.Dispose();
            }
        }

        private void AddPromotedService(Type serviceType, object serviceInstanceOrCallback)
        {
            if ((null == serviceContainer) && (null == profferService))
            {
                return;
            }

            // Regular service promotion.
            if (null != serviceContainer)
            {
                serviceContainer.AddService(serviceType, serviceInstanceOrCallback, true);
                return;
            }

            // Proffered services promotion.
            if (null != profferService)
            {
                ProfferedService svc = new ProfferedService();
                svc.Instance = serviceInstanceOrCallback;
                uint cookie;
                Guid sg = serviceType.GUID;
                int hr = profferService.ProfferService(ref sg, this, out cookie);
                svc.Cookie = cookie;
                // If there're failures, throw?
                if (hr < 0)
                {
                    throw new COMException(SR.ExceptionProfferServiceCOMFailure(serviceType.FullName), hr);
                }
                profferedServices[serviceType] = svc;
            }
        }

        private void RemovePromotedService(Type serviceType)
        {
            if (serviceContainer == null && profferService == null)
            {
                return;
            }

            // Regular service demotion.
            if (serviceContainer != null)
            {
                serviceContainer.RemoveService(serviceType, true);
                return;
            }

            // We have a proffered service at hand.
            ProfferedService svc = (ProfferedService)profferedServices[serviceType];
            if (svc != null)
            {
                if (svc.Cookie != 0)
                {
                    profferService.RevokeService(svc.Cookie);
                    // Dispose if appropriate, but don't dispose ourselves again.
                    if (svc.Instance is IDisposable && svc.Instance != this)
                    {
                        ((IDisposable)svc.Instance).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// This class contains a service that is being promoted to VS.  
        /// </summary>
        private sealed class ProfferedService
        {
            public object Instance;
            public uint Cookie;
        }
    }
}