//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design
{
    /// <summary>
    /// Represents the <see cref="IConfigurationDesignManager"/> for 
    /// the Active Directory Roles provider.
    /// </summary>
    public class SecurityActiveDirectoryConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecurityActiveDirectoryConfigurationDesignManager"/> class.
        /// </summary>
        public SecurityActiveDirectoryConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="ActiveDirectoryRolesProviderNode"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Register(IServiceProvider serviceProvider)
        {
            RegisterNodeTypes(serviceProvider);
            RegisterXmlIncludeTypes(serviceProvider);
        }

        /// <summary>
        /// <para>Opens the configuration settings and registers them with the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Open(IServiceProvider serviceProvider)
        {
        }

        /// <summary>
        /// <para>Saves the configuration settings created for the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Save(IServiceProvider serviceProvider)
        {
        }

        /// <summary>
        /// <para>Adds to the dictionary configuration data for 
        /// the enterpriselibrary.configurationSettings configuration section.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="configurationDictionary">
        /// <para>A <see cref="ConfigurationDictionary"/> to add 
        /// configuration data to.</para></param>
        public void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
        {
        }

        private static void RegisterXmlIncludeTypes(IServiceProvider serviceProvider)
        {
            IXmlIncludeTypeService xmlIncludeTypeService = serviceProvider.GetService(typeof(IXmlIncludeTypeService)) as IXmlIncludeTypeService;
            Debug.Assert(xmlIncludeTypeService != null, "Could not find the IXmlIncludeTypeService");
            xmlIncludeTypeService.AddXmlIncludeType(SecuritySettings.SectionName, typeof(AdRolesProviderData));
        }

        private static void RegisterNodeTypes(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(ActiveDirectoryRolesProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(AdRolesProviderData), SR.ActiveDirectoryProvider);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}