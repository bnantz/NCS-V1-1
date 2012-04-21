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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Represents the controller for IConfigurationDesignManager objects for an application.
    /// </devdoc>
    internal sealed class ConfigurationDesignManagerDomain
    {
        private IServiceProvider serviceProvider;
        internal ArrayList managers;

        internal ConfigurationDesignManagerDomain(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.managers = new ArrayList();
        }

        /// <devdoc>
        /// Searches the specified assembly for ConfigurationDesignManagerAttribute and loads the  specified IConfigurationDesignManager objects into the domain.
        /// </devdoc>
        internal void Load(Assembly assembly)
        {
            object[] typeAttributes = assembly.GetCustomAttributes(typeof(ConfigurationDesignManagerAttribute), true);
            foreach (ConfigurationDesignManagerAttribute typeAttribute in typeAttributes)
            {
                Load(typeAttribute);
            }
        }

        /// <devdoc>
        /// Invokes IConfigurationDesignManager.Open on all of the contained IConfigurationDesignManager objects.
        /// </devdoc>
        internal void Open()
        {
            foreach (IConfigurationDesignManager manager in managers)
            {
                manager.Open(serviceProvider);
            }
        }

        /// <devdoc>
        /// Invokes IConfigurationDesignManager.Save on all of the contained IConfigurationDesignManager objects
        /// </devdoc>
        internal void Save()
        {
            foreach (IConfigurationDesignManager manager in managers)
            {
                manager.Save(serviceProvider);
            }
        }

        /// <devdoc>
        /// Build the context for the design.
        /// </devdoc>
        internal ConfigurationContext BuildContext()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            foreach (IConfigurationDesignManager manager in managers)
            {
                manager.BuildContext(serviceProvider, dictionary);
            }
            return ConfigurationManager.CreateContext(dictionary);
        }

        /// <devdoc>
        /// Invokes IConfigurationDesignManager.Register on all of the contained IConfigurationDesignManager objects.
        /// </devdoc>
        internal void Register()
        {
            foreach (IConfigurationDesignManager manager in managers)
            {
                try
                {
                    manager.Register(serviceProvider);
                }
                catch (IOException e)
                {
                    IUIHierarchyService uiHiearchyService = serviceProvider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
                    Debug.Assert(uiHiearchyService != null, "Could not load the IUIHiearchyService.");
                    IConfigurationErrorLogService configurationErrorLogService = serviceProvider.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
                    Debug.Assert(configurationErrorLogService != null, "Could not load the IErrorService.");
                    configurationErrorLogService.LogError(new ConfigurationError(uiHiearchyService.SelectedHierarchy.SelectedNode, e.Message));
                }
            }
        }

        /// <devdoc>
        /// Loads the IConfigurationDesignManager object from the specified ConfigurationDesignManagerAttribute.
        /// </devdoc>
        internal void Load(ConfigurationDesignManagerAttribute typeAttribute)
        {
            this.Load(typeAttribute.ConfigurationDesignManager);
        }

        /// <devdoc>
        /// Loads the IConfigurationDesignManager object from the specified Type.
        /// </devdoc>
        internal void Load(Type type)
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(type);
            Load(proxy);
        }

        internal void Load(ConfigurationDesignManagerProxy proxy)
        {
            if (!managers.Contains(proxy))
            {
                // our manager should allways be the fist in the list
                if (proxy.ConfigurationDesignManager.GetType().Equals(typeof(ConfigurationDesignManager)))
                {
                    this.managers.Insert(0, proxy);
                }
                else
                {
                    this.managers.Add(proxy);    
                }
                
            }
            
        }

        /// <devdoc>
        /// Loads all of the assemblies located at the specified path and searches them for ConfigurationDesignManagerAttribute attributes.
        /// </devdoc>
        internal void LoadFrom(string path)
        {
            string[] files = Directory.GetFiles(path, "*.dll");

            foreach (string file in files)
            {
                Assembly assembly = null;

                try
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
                    assembly = Assembly.Load(assemblyName);
                }
                catch (BadImageFormatException)
                {
                }
                if (assembly != null)
                {
                    this.Load(assembly);
                }
            }
        }

        /// <devdoc>
        /// Loads all of the assemblies located at the current AppDomain.BaseDirectory and searches them for ConfigurationDesignManagerAttribute attributes.
        /// </devdoc>
        internal void Load()
        {
            LoadFrom(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}