//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents the configuration design manger for the database settings.
    /// </para>
    /// </summary>    
    public class DataConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="DataConfigurationDesignManager"/> class.
        /// </para>
        /// </summary>
        public DataConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="DatabaseSettings"/> in the application.</para>
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
            if (configurationContext.IsValidSection(DatabaseSettings.SectionName))
            {
                DatabaseSettings databaseSettings = null;
                DatabaseSettingsNode databaseSettingsNode = null;
                try
                {
                    databaseSettings = (DatabaseSettings)configurationContext.GetConfiguration(DatabaseSettings.SectionName);
                    databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
                    ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
                    configurationNode.Nodes.Add(databaseSettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, databaseSettingsNode, e);
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
            if (configurationContext.IsValidSection(DatabaseSettings.SectionName))
            {
                DatabaseSettingsNode databaseSettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    databaseSettingsNode = hierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
                    if (databaseSettingsNode == null)
                    {
                        return;
                    }
                    DatabaseSettings databaseSettings = databaseSettingsNode.DatabaseSettings;
                    configurationContext.WriteConfiguration(DatabaseSettings.SectionName, databaseSettings);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, databaseSettingsNode, e);
                }
                catch (InvalidOperationException e)
                {
                    ServiceHelper.LogError(serviceProvider, databaseSettingsNode, e);
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
            DatabaseSettingsNode node = GetDatabaseSettingsNode(serviceProvider);
            if (node != null)
            {
                DatabaseSettings settings = node.DatabaseSettings;
                configurationDictionary[DatabaseSettings.SectionName] = settings;
            }
        }

        private static DatabaseSettingsNode GetDatabaseSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
        }

        private static void CreateCommands(IServiceProvider serviceProvider)
        {
            IUIHierarchyService hierarchyService = ServiceHelper.GetUIHierarchyService(serviceProvider);
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(DatabaseSettingsNode));

            IMenuContainerService menuService = ServiceHelper.GetMenuContainerService(serviceProvider);
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.DataConfigurationMenuText, new AddConfigurationSectionCommand(serviceProvider, typeof(DatabaseSettingsNode), DatabaseSettings.SectionName), ServiceHelper.GetCurrentRootNode(serviceProvider), Shortcut.None, SR.DataConfigurationMenuText, InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);
            
            Type nodeType = typeof(OracleConnectionStringNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(OracleConnectionStringData), SR.OracleConnectionStringNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(ConnectionStringNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ConnectionStringData), SR.ConnectionStringNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}