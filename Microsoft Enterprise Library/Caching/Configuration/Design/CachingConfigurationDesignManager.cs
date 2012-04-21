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
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Caching Block Manager
    /// </summary>
    public class CachingConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Create's an instance of the block manager.
        /// </summary>
        public CachingConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="CacheManagerSettings"/> in the application.</para>
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
            if (configurationContext.IsValidSection(CacheManagerSettings.SectionName))
            {
                CacheManagerSettingsNode cacheManagerSettingsNode = null;
                try
                {
                    CacheManagerSettings settings = configurationContext.GetConfiguration(CacheManagerSettings.SectionName) as CacheManagerSettings;
                    if (settings != null)
                    {
                        cacheManagerSettingsNode = new CacheManagerSettingsNode(settings);
                        ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
                        configurationNode.Nodes.Add(cacheManagerSettingsNode);
                    }
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, cacheManagerSettingsNode, e);
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
            if (configurationContext.IsValidSection(CacheManagerSettings.SectionName))
            {
                CacheManagerSettingsNode cacheManagerSettingsNode = GetCacheManagerSettingsNode(serviceProvider);
                CacheManagerSettings settings = cacheManagerSettingsNode.CacheManagerSettings;
                if (settings != null)
                {
                    try
                    {
                        configurationContext.WriteConfiguration(CacheManagerSettings.SectionName, settings);
                    }
                    catch (InvalidOperationException e)
                    {
                        ServiceHelper.LogError(serviceProvider, cacheManagerSettingsNode, e);
                    }
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
            CacheManagerSettingsNode node = GetCacheManagerSettingsNode(serviceProvider);
            if (node != null)
            {
                CacheManagerSettings settings = node.CacheManagerSettings;
                configurationDictionary[CacheManagerSettings.SectionName] = settings;
            }
        }

        private static CacheManagerSettingsNode GetCacheManagerSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode)) as CacheManagerSettingsNode;
        }

        private static void CreateCommands(IServiceProvider serviceProvider)
        {
            IUIHierarchyService hierarchyService = ServiceHelper.GetUIHierarchyService(serviceProvider);
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(CacheManagerSettingsNode));
            IMenuContainerService menuService = ServiceHelper.GetMenuContainerService(serviceProvider);
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.CacheSettingsMenuText, new AddConfigurationSectionCommand(serviceProvider, typeof(CacheManagerSettingsNode), CacheManagerSettings.SectionName), ServiceHelper.GetCurrentRootNode(serviceProvider), Shortcut.None, SR.CacheSettingsStatusText, InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);
            
            Type nodeType = typeof(IsolatedStorageCacheStorageNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(IsolatedStorageCacheStorageData), SR.DefaultIsolatedStorageNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(CustomCacheStorageNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomCacheStorageData), SR.DefaultCustomCacheStorageNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}