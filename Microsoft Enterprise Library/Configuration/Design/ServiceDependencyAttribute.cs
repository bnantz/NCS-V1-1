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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Specifies the dependencies on other services that must be available at run-time to ensure proper functioning.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class ServiceDependencyAttribute : Attribute
    {
        private readonly Type serviceType;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ServiceDependencyAttribute"/> class with a service type.</para>.
        /// </summary>
        /// <param name="serviceType">
        /// <para>The service <see cref="Type"/> required by the component.</para>
        /// </param>
        public ServiceDependencyAttribute(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        /// <summary>
        /// <para>Gets the service <see cref="Type"/> of the dependency.</para>
        /// </summary>
        /// <value>
        /// <para>The service <see cref="Type"/> of the dependency.</para>
        /// </value>
        public Type ServiceType
        {
            get { return serviceType; }
        }
    }
}