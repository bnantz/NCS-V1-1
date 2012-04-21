//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    /// <summary>
    /// Manages registration of custom cache data nodes to their respective cache nodes.
    /// </summary>
    public class CachingCryptographyConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingCryptographyConfigurationDesignManager"/> class.
        /// </summary>
        public CachingCryptographyConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="SymmetricStorageEncryptionNode"/> in the application.</para>
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
            xmlIncludeTypeService.AddXmlIncludeType(CacheManagerSettings.SectionName, typeof(SymmetricStorageEncryptionProviderData));
        }

        private static void RegisterNodeTypes(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);
            
            Type nodeType = typeof(SymmetricStorageEncryptionNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(SymmetricStorageEncryptionProviderData), SR.SymmetricStorageEncryption);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}