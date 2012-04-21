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
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents <see cref="IConfigurationDesignManager"/> implementation for the configuration runtime design.</para> 
    /// </summary>
    public class ConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDesignManager"/> class.</para>
        /// </summary>
        public ConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="ConfigurationSettings"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public void Register(IServiceProvider serviceProvider)
        {
            RegisterNodeMaps(serviceProvider);
            CreateCommands(serviceProvider);
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
            ConfigurationSectionCollectionNode sectionsNode = null;
            ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            RemoveCurrentConfigurationSectionCollectionNode(serviceProvider);
            try
            {
                string appName = SR.DefaultApplicationName;
                ConfigurationSettings configurationSettings = configurationContext.GetMetaConfiguration();
                if (null != configurationSettings)
                {
                    appName = configurationSettings.ApplicationName;
                    if (configurationSettings.ConfigurationSections.Count > 0)
                    {
                        sectionsNode = new ConfigurationSectionCollectionNode(configurationSettings);
                        configurationNode.Nodes.Add(sectionsNode);    
                    }
                }
                if (configurationNode is ApplicationConfigurationNode)
                {
                    ((ApplicationConfigurationNode)configurationNode).Name = appName;
                }
            }
            catch (ConfigurationException e)
            {
                ServiceHelper.LogError(serviceProvider, sectionsNode, e);    
                
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
            ConfigurationNode node = ServiceHelper.GetCurrentRootNode(serviceProvider);
            try
            {
                ConfigurationSettings configurationSettings = GetConfigurationSettings(serviceProvider);
                configurationSettings.ApplicationName = node.Name;
                configurationContext.WriteMetaConfiguration(configurationSettings);
            }
            catch (ConfigurationException e)
            {
                ServiceHelper.LogError(serviceProvider, node, e);
            }
            catch (InvalidOperationException e)
            {
                ServiceHelper.LogError(serviceProvider, node, e);
            }
        }

        /// <summary>
        /// <para>Adds the <see cref="ConfigurationSettings"/> to the <see cref="ConfigurationDictionary"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="configurationDictionary">
        /// <para>A <see cref="ConfigurationDictionary"/> object that will contain the configuration data.</para></param>
        public void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
        {
            configurationDictionary[ConfigurationSettings.SectionName] = GetConfigurationSettings(serviceProvider);
        }

        private static void CreateCommands(IServiceProvider serviceProvider)
        {
            IUIHierarchy currentHierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            bool containsNode = currentHierarchy.ContainsNodeType(rootNode, typeof(ConfigurationSectionCollectionNode));
            IMenuContainerService menuService = ServiceHelper.GetMenuContainerService(serviceProvider);
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.ConfigurationSectionCollectionMenuItemText, new AddChildNodeCommand(serviceProvider, typeof(ConfigurationSectionCollectionNode)), rootNode, Shortcut.None, SR.ConfigurationSectionCollectionStatusText, InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RemoveCurrentConfigurationSectionCollectionNode(IServiceProvider serviceProvider)
        {
            ConfigurationSectionCollectionNode currentNode = ServiceHelper.GetCurrentHierarchy(serviceProvider).FindNodeByType(typeof(ConfigurationSectionCollectionNode)) as ConfigurationSectionCollectionNode;
            if (currentNode != null)
            {
                currentNode.Remove();
            }
        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);
            
            Type nodeType = typeof(CustomKeyAlgorithmStorageProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomKeyAlgorithmPairStorageProviderData), SR.CustomKeyAlgorithmPairStorageProviderNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomTransformerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomTransformerData), SR.CustomTransformerNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomStorageProviderData), SR.CustomStorageProviderNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(XmlFileStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(XmlFileStorageProviderData), SR.XMLStorageProviderNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(XmlSerializerTransformerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(XmlSerializerTransformerData), SR.XmlSerializerTransformerNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(ConfigurationSectionNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ConfigurationSectionData), SR.ConfigurationSectionNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(ReadOnlyConfigurationSectionNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ReadOnlyConfigurationSectionData), SR.ReadOnlyConfigurationSectionNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(FileKeyAlgorithmPairStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddFileKeyAlgorithmPairNodeCommand(serviceProvider, nodeType), nodeType, typeof(FileKeyAlgorithmPairStorageProviderData), SR.FileKeyAlgorithmStorageProviderNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }

        private static ConfigurationSettings GetConfigurationSettings(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            ConfigurationSectionCollectionNode sectionsNode = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ConfigurationSectionCollectionNode)) as ConfigurationSectionCollectionNode;
            ConfigurationSettings configurationSettings = null;
            if (sectionsNode == null)
            {
                configurationSettings = new ConfigurationSettings();
            }
            else
            {
                configurationSettings = sectionsNode.ConfigurationSettings;
            }
            return configurationSettings;
        }
    }
}