//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    /// <summary>
    /// Provides base functionality for Logging Configuraiton Design Manager
    /// </summary>
    public abstract class LoggingConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Creates a <c>LoggingConfigurationDesignManager</c>.
        /// </summary>
        protected LoggingConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="LoggingSettings"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public virtual void Register(IServiceProvider serviceProvider)
        {
            CreateCommands(serviceProvider);
        }
       
        /// <summary>
        /// <para>Opens the configuration settings and registers them with the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public abstract void Open(IServiceProvider serviceProvider);

        /// <summary>
        /// <para>Saves the configuration settings created for the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public abstract void Save(IServiceProvider serviceProvider);

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
        public abstract void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary);

        protected LoggingSettingsNode GetLoggingSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchyService hierarchyService = serviceProvider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            LoggingSettingsNode node = (LoggingSettingsNode)currentHierarchy.FindNodeByType(typeof(LoggingSettingsNode));
            if (node == null)
            {
                node = new LoggingSettingsNode();
                currentHierarchy.RootNode.Nodes.Add(node);
            }
            return node;
        }

        private static void CreateCommands(IServiceProvider provider)
        {
            IUIHierarchyService hierarchyService = provider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(LoggingSettingsNode));
            IMenuContainerService menuService = provider.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Debug.Assert(menuService != null, "Could not get the IMenuContainerService");
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.LogSettingsCmd,
                new AddChildNodeCommand(provider, typeof(LoggingSettingsNode)),
                ServiceHelper.GetCurrentRootNode(provider),
                Shortcut.None,
                SR.GenericCreateStatusText(SR.LogSettingsCmd),
                InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);
        }
    }
}