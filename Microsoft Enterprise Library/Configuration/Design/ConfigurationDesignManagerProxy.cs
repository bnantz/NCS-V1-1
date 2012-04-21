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
    /// <devdoc>
    /// Represents a proxy for an IConfigurationDesignManager.
    /// This object is comparable so we can use an ArrayList to sort dependencies. 
    /// So in fact it doesn't really work as a compare should since we return 0 to
    /// indicate no dependency, not equality.
    /// </devdoc>
    internal sealed class ConfigurationDesignManagerProxy : IConfigurationDesignManager
    {
        private IConfigurationDesignManager manager;

        public ConfigurationDesignManagerProxy(Type managerType)
        {
            this.manager = Activator.CreateInstance(managerType) as IConfigurationDesignManager;
            if (this.manager == null)
            {
                throw new ArgumentException(SR.ExceptionNotAssignableType(managerType.AssemblyQualifiedName, typeof(IConfigurationDesignManager).AssemblyQualifiedName));
            }
        }

        public IConfigurationDesignManager ConfigurationDesignManager
        {
            get { return this.manager; }
        }

        public void Register(IServiceProvider serviceProvider)
        {
            this.manager.Register(serviceProvider);
        }

        public void Save(IServiceProvider serviceProvider)
        {
            this.manager.Save(serviceProvider);
        }

        public void Open(IServiceProvider serviceProvider)
        {
            this.manager.Open(serviceProvider);
        }

        public void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
        {
            this.manager.BuildContext(serviceProvider, configurationDictionary);
        }

//        public void ResolveDependencies(IServiceProvider serviceProvider)
//        {
//            this.manager.ResolveDependencies(serviceProvider);
//        }
    }
}