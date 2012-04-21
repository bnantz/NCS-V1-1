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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a host for all the services for the configuration design.</para>
    /// </summary>
    public class ConfigurationDesignHost : ServiceContainer, IContainer, IComponentChangeService, IDictionaryService 
    {
        private ArrayList components;
        private Hashtable dictionary;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationDesignHost"/> class.</para>
        /// </summary>
        public ConfigurationDesignHost() : this(null)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationDesignHost"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The parent <see cref="IServiceProvider"/>.</para>
        /// </param>
        public ConfigurationDesignHost(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            components = new ArrayList();
            dictionary = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            AddService(typeof(IContainer), this);
            AddService(typeof(IComponentChangeService), this);
            AddService(typeof(INodeNameCreationService), new NodeNameCreationService());
            AddService(typeof(INameCreationService), new NodeNameCreationService());
            AddService(typeof(IUIHierarchyService), new UIHierarchyService());
            AddService(typeof(IDictionaryService), this);
            AddService(typeof(IConfigurationErrorLogService), new ConfigurationErrorLogService());
            AddService(typeof(INodeCreationService), new NodeCreationService());
            AddService(typeof(IXmlIncludeTypeService), new XmlIncludeTypeService());
            AddService(typeof(ILinkNodeService), new LinkNodeService(this));
            AddService(typeof(IMenuContainerService), new MenuContainerService());
        }

        /// <summary>
        /// Occurs when a component has been added.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentAdded"/>
        public event ComponentEventHandler ComponentAdded;

        /// <summary>
        /// Occurs when a component is in the process of being added.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentAdding"/>
        public event ComponentEventHandler ComponentAdding;

        /// <summary>
        /// Occurs when a component has been changed.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentChanged"/>
        public event ComponentChangedEventHandler ComponentChanged;

        /// <summary>
        /// Occurs when a component is in the process of being changed.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentChanging"/>
        public event ComponentChangingEventHandler ComponentChanging;

        /// <summary>
        /// Occurs when a component has been removed.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentRemoved"/>
        public event ComponentEventHandler ComponentRemoved;

        /// <summary>
        /// Occurs when a component is in the process of being removed.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentRemoving"/>
        public event ComponentEventHandler ComponentRemoving;

        /// <summary>
        /// Occurs when a component is renamed.
        /// </summary>
        /// <seealso cref="IComponentChangeService.ComponentRename"/>
        public event ComponentRenameEventHandler ComponentRename;

        /// <summary>
        /// <para>Gets all the components in the <see cref="IContainer"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The components in the <see cref="IContainer"/>.</para>
        /// </value>
        /// <seealso cref="IContainer.Components"/>
        public ComponentCollection Components
        {
            get
            {
                if (components.Count == 0)
                {
                    return new ComponentCollection(new IComponent[] {});
                }
                IComponent[] componentsArray = new IComponent[components.Count];
                components.CopyTo(componentsArray, 0);
                return new ComponentCollection(componentsArray);
            }
        }

        /// <summary>
        /// <para>Adds the specified <see cref="IComponent"/> to the <see cref="IContainer"/> at the end of the list.</para>
        /// </summary>
        /// <param name="component">
        /// <para>The <see cref="IComponent"/> to add.</para>
        /// </param>
        /// <seealso cref="IContainer.Add(IComponent)"/>
        public void Add(IComponent component)
        {
            Add(component, null);
        }

        /// <summary>
        /// <para>Adds the specified <see cref="IComponent"/> to the <see cref="IContainer"/> at the end of the list.</para>
        /// </summary>
        /// <param name="component">
        /// <para>The <see cref="IComponent"/> to add.</para>
        /// </param>
        /// <param name="name">
        /// <para>The unique, case-insensitive name to assign to the component.</para>
        /// <para>- or -</para>
        /// <para>A null reference (Nothing in Visual Basic) that leaves the component unnamed.</para>
        /// </param>
        /// <seealso cref="IContainer.Add(IComponent, string)"/>
        public void Add(IComponent component, string name)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            RemoveComponentSite(component);
            CreateComponentName(component, ref name);
            SetComponentSite(component, name);
            OnComponentAdding(new ComponentEventArgs(component));
            AddAsExtenderProvider(component);
            components.Add(component);
            OnComponentAdded(new ComponentEventArgs(component));
        }

        /// <summary>
        /// <para>Removes a component from the <see cref="IContainer"/>.</para>
        /// </summary>
        /// <param name="component">
        /// <para>The <see cref="IComponent"/> to remove.</para>
        /// </param>
        /// <seealso cref="IContainer.Remove"/>
        public void Remove(IComponent component)
        {
            if (component == null)
            {
                return;
            }
            if (component.Site == null || component.Site.Container != this)
            {
                return;
            }
            OnComponentRemoving(new ComponentEventArgs(component));
            RemoveAsExtenderProvider(component);
            components.Remove(component);
            OnComponentRemoved(new ComponentEventArgs(component));
            component.Site = null;
        }

        /// <summary>
        /// <para>Announces to the component change service that a particular component has changed.</para>
        /// </summary>
        /// <param name="component">
        /// <para>The component that has changed.</para>
        /// </param>
        /// <param name="member">
        /// <para>The member that has changed. This is a <see langword="null"/> reference (Nothing in Visual Basic) if this change is not related to a single member.</para>
        /// </param>
        /// <param name="oldValue">
        /// <para>The old value of the member. This is valid only if the member is not a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <param name="newValue">
        /// <para>The new value of the member. This is valid only if the member is not a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <seealso cref="IComponentChangeService.OnComponentChanged"/>
        public void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
        {
            if (ComponentChanged != null)
            {
                ComponentChanged(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
            }
        }

        /// <summary>
        /// <para>Announces to the component change service that a particular component is changing.</para>
        /// </summary>
        /// <param name="component">
        /// <para>The component that is about to change.</para>
        /// </param>
        /// <param name="member">
        /// <para>The member that is changing. This is a <see langword="null"/> reference (Nothing in Visual Basic) if this change is not related to a single member.</para>
        /// </param>
        /// <seealso cref="IComponentChangeService.OnComponentChanging"/>
        public void OnComponentChanging(object component, MemberDescriptor member)
        {
            if (ComponentChanging != null)
            {
                ComponentChanging(this, new ComponentChangingEventArgs(component, member));
            }
        }

        /// <summary>
        /// <para>Gets the key corresponding to the specified value.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The value to look up in the dictionary.</para>
        /// </param>
        /// <returns>
        /// <para>The associated key, or a <see langword="null"/> reference (Nothing in Visual Basic) if no key exists.</para>
        /// </returns>
        /// <seealso cref="IDictionaryService.GetKey"/>
        public object GetKey(object value)
        {
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                return GetKeyFromValue(value);
            }
        }

        /// <summary>
        /// <para>Gets the value corresponding to the specified key.</para>
        /// </summary>
        /// <param name="key">
        /// <para>The key to look up the value for.</para>
        /// </param>
        /// <returns>
        /// <para>The associated value, or a <see langword="null"/> reference (Nothing in Visual Basic) if no value exists.</para>
        /// </returns>
        /// <seealso cref="IDictionaryService.GetValue"/>
        public object GetValue(object key)
        {
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                return dictionary[key];
            }
        }

        /// <summary>
        /// <para>Sets the specified key-value pair.</para>
        /// </summary>
        /// <param name="key">
        /// <para>An object to use as the key to associate the value with.</para>
        /// </param>
        /// <param name="value">
        /// <para>The value to store.</para>
        /// </param>
        /// <seealso cref="IDictionaryService.SetValue"/>
        public void SetValue(object key, object value)
        {
            // Remove if we're setting to null
            if (value == null)
            {
                dictionary.Remove(key);
                return;
            }

            // Set value
            dictionary[key] = value;
        }

        /// <summary>
        /// <para>Raises the <see cref="ComponentRename"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ComponentRenameEventArgs"/> object containing data for the event.</para>
        /// </param>
        public virtual void OnComponentRename(ComponentRenameEventArgs e)
        {
            if (ComponentRename != null)
            {
                ComponentRename(this, e);
            }
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ProviderFactory"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IComponent[] componentArray = new IComponent[components.Count];
                components.CopyTo(componentArray, 0);
                foreach (IComponent component in componentArray)
                {
                    components.Remove(component);
                    component.Dispose();
                }
                components.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <para>Raises the <see cref="ComponentAdding"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ComponentEventArgs"/> object containing data for the event.</para>
        /// </param>
        protected virtual void OnComponentAdding(ComponentEventArgs e)
        {
            if (ComponentAdding != null)
            {
                ComponentAdding(this, e);
            }
        }

        /// <summary>
        /// <para>Raises the <see cref="ComponentAdded"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ComponentEventArgs"/> object containing data for the event.</para>
        /// </param>
        protected virtual void OnComponentAdded(ComponentEventArgs e)
        {
            if (ComponentAdded != null)
            {
                ComponentAdded(this, e);
            }
        }

        /// <summary>
        /// <para>Raises the <see cref="ComponentRemoving"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ComponentEventArgs"/> object containing data for the event.</para>
        /// </param>
        protected virtual void OnComponentRemoving(ComponentEventArgs e)
        {
            if (ComponentRemoving != null)
            {
                ComponentRemoving(this, e);
            }
        }

        /// <summary>
        /// <para>Raises the <see cref="ComponentRemoved"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ComponentEventArgs"/> object containing data for the event.</para>
        /// </param>
        protected virtual void OnComponentRemoved(ComponentEventArgs e)
        {
            if (ComponentRemoved != null)
            {
                ComponentRemoved(this, e);
            }
        }

        private object GetKeyFromValue(object value)
        {
            IDictionaryEnumerator e = dictionary.GetEnumerator();

            while (e.MoveNext())
            {
                if (e.Value == value)
                {
                    return e.Key;
                }
            }

            return null;
        }

        private void AddAsExtenderProvider(IComponent component)
        {
            if (component is IExtenderProvider)
            {
                IExtenderProviderService e = (IExtenderProviderService)GetService(typeof(IExtenderProviderService));
                e.AddExtenderProvider((IExtenderProvider)component);
            }
        }

        private void RemoveAsExtenderProvider(IComponent component)
        {
            if (component is IExtenderProvider)
            {
                IExtenderProviderService e = (IExtenderProviderService)GetService(typeof(IExtenderProviderService));
                e.RemoveExtenderProvider((IExtenderProvider)component);
            }
        }

        private void SetComponentSite(IComponent component, string name)
        {
            ConfigurationNodeSite site = new ConfigurationNodeSite(this, component, name);
            component.Site = site;
        }

        private void RemoveComponentSite(IComponent component)
        {
            if (component.Site != null && component.Site.Container != this)
            {
                component.Site.Container.Remove(component);
            }
        }

        private void CreateComponentName(IComponent component, ref string name)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
            PropertyDescriptor find = properties.Find("Name", true);
            if (find != null)
            {
                string currentName = find.GetValue(component) as string;
                if (currentName != null && currentName.Length > 0)
                {
                    name = currentName;
                    return;
                }
            }
            if (name == null)
            {
                INameCreationService nameService = (INameCreationService)GetService(typeof(INameCreationService));
                name = nameService.CreateName(this, component.GetType());
            }
        }
    }
}