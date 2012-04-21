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
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Basic <see cref="ISite"/> implementation. Passes all service requests to the parent service provider.</para>
    /// </summary>
    public class ConfigurationNodeSite : ISite
    {
        private readonly IServiceProvider provider;
        private readonly IComponent component;
        private readonly IContainer container;
        private string name;

        /// <summary>
        /// Constructs a site.
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="component"><para>The component this site is being associated with.</para></param>
        /// <param name="name"><para>A name for the site.</para></param>
        public ConfigurationNodeSite(IServiceProvider serviceProvider, IComponent component, string name)
        {
            ArgumentValidation.CheckForNullReference(serviceProvider, "serviceProvider");
            ArgumentValidation.CheckForNullReference(component, "component");

            this.component = component;
            // Pull the container from the service provider (if any).
            container = (IContainer)serviceProvider.GetService(typeof(IContainer));
            this.provider = serviceProvider;
            this.name = name;
        }

        /// <summary>
        /// <para>Gets the component associated with the <see cref="ISite"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IComponent"/> instance associated with the <see cref="ISite"/>.</para>
        /// </value>
        /// <seealso cref="ISite.Component"/>
        public IComponent Component
        {
            get { return component; }
        }

        /// <summary>
        /// <para>Gets the container associated with the <see cref="ISite"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IContainer"/> instance associated with the <see cref="ISite"/>.</para>
        /// </value>
        /// <seealso cref="ISite.Container"/>
        public IContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// <para>Determines whether the component is in design mode.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the component is in design mode; otherwise, <see langword="false"/>.</para>
        /// </value>
        /// <seealso cref="ISite.DesignMode"/>
        public bool DesignMode
        {
            get { return false; }
        }

        /// <summary>
        /// <para>Gets the name of the component associated with the <see cref="ISite"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the component associated with the <see cref="ISite"/>; or a null reference (Nothing in Visual Basic), if no name is assigned to the component.</para>
        /// </value>
        /// <seealso cref="ISite.Name"/>
        public string Name
        {
            get { return name; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Cannot set a component's name to a null value.");
                }
                if (value == name)
                {
                    return;
                }
                string oldName = name;
                IComponentChangeService componentChangeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (null != componentChangeService)
                {
                    // Apply new name
                    MemberDescriptor md = TypeDescriptor.CreateProperty(component.GetType(), "Name", typeof(string), new Attribute[] {});
                    componentChangeService.OnComponentChanging(component, md);
                    name = value;
                    OnComponentRename(component, oldName, name);
                    componentChangeService.OnComponentChanged(component, md, oldName, name);
                }
            }
        }

        /// <summary>
        /// <para>Gets the service object of the specified type.</para>
        /// </summary>
        /// <param name="serviceType">
        /// <para>An object that specifies the type of service object to get.</para>
        /// </param>
        /// <returns>
        /// <para>A service object of type <paramref name="serviceType"/>.</para>
        /// <para>-or-</para>
        /// <para>A <c>null</c> reference (Nothing in Visual Basic) if there is no service object of type <paramref name="serviceType"/>.</para>
        /// </returns>
        public virtual object GetService(Type serviceType)
        {
            if (serviceType != typeof(ISite))
            {
                return provider.GetService(serviceType);
            }
            return this;
        }

        private void OnComponentRename(object component, string oldName, string newName)
        {
            ConfigurationDesignHost host = provider as ConfigurationDesignHost;
            if (host != null)
            {
                host.OnComponentRename(new ComponentRenameEventArgs(component, oldName, newName));
            }
        }
    }
}