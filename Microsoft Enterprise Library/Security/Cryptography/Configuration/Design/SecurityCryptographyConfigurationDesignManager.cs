//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents the block manager for the Authentication 
    /// configuration data.
    /// </summary>
    public class SecurityCryptographyConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCryptographyConfigurationDesignManager"/> class.
        /// </summary>
        public SecurityCryptographyConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="CryptographySettings"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Register(IServiceProvider serviceProvider)
        {
            CreateCommands(serviceProvider);
            RegisterNodeMaps(serviceProvider);
        }

        /// <summary>
        /// <para>Opens the configuration settings and registers them with the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Open(IServiceProvider serviceProvider)
        {
            ConfigurationContext configurationContext = ServiceHelper.GetCurrentConfigurationContext(serviceProvider);
            if (configurationContext.IsValidSection(CryptographySettings.SectionName))
            {
                CryptographySettings cryptographySettings = null;
                CryptographySettingsNode cryptographySettingsNode = null;
                try
                {
                    cryptographySettings = (CryptographySettings)configurationContext.GetConfiguration(CryptographySettings.SectionName);
                    cryptographySettingsNode = new CryptographySettingsNode(cryptographySettings);
                    ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
                    configurationNode.Nodes.Add(cryptographySettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, cryptographySettingsNode, e);
                }
            }
        }

        /// <summary>
        /// <para>Saves the configuration settings created for the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Save(IServiceProvider serviceProvider)
        {
            ConfigurationContext configurationContext = ServiceHelper.GetCurrentConfigurationContext(serviceProvider);
            if (configurationContext.IsValidSection(CryptographySettings.SectionName))
            {
                CryptographySettingsNode securitySettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    securitySettingsNode = hierarchy.FindNodeByType(typeof(CryptographySettingsNode)) as CryptographySettingsNode;
                    if (securitySettingsNode == null)
                    {
                        return;
                    }
                    CryptographySettings securitySettings = securitySettingsNode.CryptographySettings;
                    configurationContext.WriteConfiguration(CryptographySettings.SectionName, securitySettings);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, securitySettingsNode, e);
                }
                catch (InvalidOperationException e)
                {
                    ServiceHelper.LogError(serviceProvider, securitySettingsNode, e);
                }
            }
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
            CryptographySettingsNode node = GetCryptographySettingsNode(serviceProvider);
            if (node != null)
            {
                CryptographySettings settings = node.CryptographySettings;
                configurationDictionary[CryptographySettings.SectionName] = settings;
            }
        }

        private static CryptographySettingsNode GetCryptographySettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(CryptographySettingsNode)) as CryptographySettingsNode;
        }

        private static void CreateCommands(IServiceProvider provider)
        {
            IUIHierarchyService hierarchyService = provider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(CryptographySettingsNode));
            IMenuContainerService menuService = provider.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Debug.Assert(menuService != null, "Could not get the IMenuContainerService");
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.CryptographySectionCommandName,
                                                                   new AddConfigurationSectionCommand(provider, typeof(CryptographySettingsNode), CryptographySettings.SectionName),
                                                                   ServiceHelper.GetCurrentRootNode(provider),
                                                                   Shortcut.None,
                                                                   SR.GenericCreateStatusText(SR.CryptographySectionCommandName),
                                                                   InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(CustomHashProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomHashProviderData), SR.CustomHashProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomSymmetricCryptoProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomSymmetricCryptoProviderData), SR.CustomSymmetricCryptoProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(HashAlgorithmProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(HashAlgorithmProviderData), SR.HashAlgorithmProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(KeyedHashAlgorithmProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(KeyedHashAlgorithmProviderData), SR.KeyedHashProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(SymmetricAlgorithmProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(SymmetricAlgorithmProviderData), SR.SymmetricAlgorithmProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(DpapiSymmetricCryptoProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(DpapiSymmetricCryptoProviderData), SR.DpapiSymmetricCryptoProviderNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}