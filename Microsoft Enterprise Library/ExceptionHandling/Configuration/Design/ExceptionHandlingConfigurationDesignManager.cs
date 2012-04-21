//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// The configuration block manager that handles reading and writing configuration
    /// for Exception Handling.
    /// </summary>
    public class ExceptionHandlingConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Create's an instance of the block manager.
        /// </summary>
        public ExceptionHandlingConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="ExceptionHandlingSettings"/> in the application.</para>
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
            if (configurationContext.IsValidSection(ExceptionHandlingSettings.SectionName))
            {
                ExceptionHandlingSettings exceptionHandlingSettings = null;
                ExceptionHandlingSettingsNode exceptionHandlingSettingsNode = null;
                try
                {
                    exceptionHandlingSettings = (ExceptionHandlingSettings)configurationContext.GetConfiguration(ExceptionHandlingSettings.SectionName);
                    exceptionHandlingSettingsNode = new ExceptionHandlingSettingsNode(exceptionHandlingSettings);
                    ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
                    configurationNode.Nodes.Add(exceptionHandlingSettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, exceptionHandlingSettingsNode, e);
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
            if (configurationContext.IsValidSection(ExceptionHandlingSettings.SectionName))
            {
                ExceptionHandlingSettingsNode exceptionHandlingSettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    exceptionHandlingSettingsNode = hierarchy.FindNodeByType(typeof(ExceptionHandlingSettingsNode)) as ExceptionHandlingSettingsNode;
                    if (exceptionHandlingSettingsNode == null)
                    {
                        return;
                    }
                    ExceptionHandlingSettings exceptionHandlingSettings = exceptionHandlingSettingsNode.ExceptionHandlingSettings;
                    configurationContext.WriteConfiguration(ExceptionHandlingSettings.SectionName, exceptionHandlingSettings);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, exceptionHandlingSettingsNode, e);
                }
                catch (InvalidOperationException e)
                {
                    ServiceHelper.LogError(serviceProvider, exceptionHandlingSettingsNode, e);
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
            ExceptionHandlingSettingsNode node = GetExceptionHandlingSettingsNode(serviceProvider);
            if (node != null)
            {
                ExceptionHandlingSettings settings = node.ExceptionHandlingSettings;
                configurationDictionary[ExceptionHandlingSettings.SectionName] = settings;
            }
        }

        private static ExceptionHandlingSettingsNode GetExceptionHandlingSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(ExceptionHandlingSettingsNode)) as ExceptionHandlingSettingsNode;
        }

        private static void CreateCommands(IServiceProvider provider)
        {
            IUIHierarchyService hierarchyService = provider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(ExceptionHandlingSettingsNode));
            IMenuContainerService menuService = provider.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Debug.Assert(menuService != null, "Could not get the IMenuContainerService");
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.ExceptionHandlingSettingsNodeMenuText, new AddConfigurationSectionCommand(provider, typeof(ExceptionHandlingSettingsNode), ExceptionHandlingSettings.SectionName), ServiceHelper.GetCurrentRootNode(provider), Shortcut.None, SR.ExceptionHandlingSettingsNodeStatusText, InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(CustomHandlerNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomHandlerData), SR.DefaultCustomHandlerNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(WrapHandlerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(WrapHandlerData), SR.DefaultWrapHandlerNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(ReplaceHandlerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ReplaceHandlerData), SR.DefaultReplaceHandlerNodeName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}