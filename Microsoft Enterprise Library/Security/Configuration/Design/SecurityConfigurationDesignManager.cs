//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents the block manager for the Authentication 
    /// configuration data.
    /// </summary>
    public class SecurityConfigurationDesignManager : IConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationDesignManager"/> class.
        /// </summary>
        public SecurityConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="SecuritySettings"/> in the application.</para>
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
            if (configurationContext.IsValidSection(SecuritySettings.SectionName))
            {
                SecuritySettings securitySettings = null;
                SecuritySettingsNode securitySettingsNode = null;
                try
                {
                    securitySettings = (SecuritySettings)configurationContext.GetConfiguration(SecuritySettings.SectionName);
                    securitySettingsNode = new SecuritySettingsNode(securitySettings);
                    ConfigurationNode configurationNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
                    configurationNode.Nodes.Add(securitySettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, securitySettingsNode, e);
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
            if (configurationContext.IsValidSection(SecuritySettings.SectionName))
            {
                SecuritySettingsNode securitySettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    securitySettingsNode = hierarchy.FindNodeByType(typeof(SecuritySettingsNode)) as SecuritySettingsNode;
                    if (securitySettingsNode == null)
                    {
                        return;
                    }
                    SecuritySettings securitySettings = securitySettingsNode.SecuritySettings;
                    configurationContext.WriteConfiguration(SecuritySettings.SectionName, securitySettings);
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
            SecuritySettingsNode node = GetSecuritySettingsNode(serviceProvider);
            if (node != null)
            {
                SecuritySettings settings = node.SecuritySettings;
                configurationDictionary[SecuritySettings.SectionName] = settings;
            }
        }

        private static SecuritySettingsNode GetSecuritySettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(SecuritySettingsNode)) as SecuritySettingsNode;
        }

        private static void CreateCommands(IServiceProvider provider)
        {
            IUIHierarchyService hierarchyService = provider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            IUIHierarchy currentHierarchy = hierarchyService.SelectedHierarchy;
            bool containsNode = currentHierarchy.ContainsNodeType(typeof(SecuritySettingsNode));
            IMenuContainerService menuService = provider.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Debug.Assert(menuService != null, "Could not get the IMenuContainerService");
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.AddConfigurationSectionCommandName,
                                                                   new AddConfigurationSectionCommand(provider, typeof(SecuritySettingsNode), SecuritySettings.SectionName),
                                                                   ServiceHelper.GetCurrentRootNode(provider),
                                                                   Shortcut.None,
                                                                   SR.GenericCreateStatusText(SR.AddConfigurationSectionCommandName),
                                                                   InsertionPoint.New);
            item.Enabled = !containsNode;
            menuService.MenuItems.Add(item);

        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(CustomAuthenticationProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomAuthenticationProviderData), SR.CustomAuthenticationProviderCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomRolesProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomRolesProviderData), SR.CustomRolesProviderCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomAuthorizationProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomAuthorizationProviderData), SR.CustomAuthorizationProviderCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomProfileProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomProfileProviderData), SR.CustomProfileProviderCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomSecurityCacheProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomSecurityCacheProviderData), SR.CustomSecurityCacheNodeCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(AuthorizationRuleProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(AuthorizationRuleProviderData), SR.AuthorizationRuleProviderCommandName);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}