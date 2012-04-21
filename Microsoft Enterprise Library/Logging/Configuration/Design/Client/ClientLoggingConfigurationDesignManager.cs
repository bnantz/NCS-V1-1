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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client
{
    /// <summary>
    /// Configuration design manager for client logging settings.
    /// </summary>
    public class ClientLoggingConfigurationDesignManager : LoggingConfigurationDesignManager
    {

        public override void Register(IServiceProvider serviceProvider)
        {
            base.Register (serviceProvider);
            RegisterNodeMaps(serviceProvider);
        }

        /// <summary>
        /// <para>Opens the configuration settings and registers them with the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public override void Open(IServiceProvider serviceProvider)
        {
            ConfigurationContext configurationContext = ServiceHelper.GetCurrentConfigurationContext(serviceProvider);
            if (configurationContext.IsValidSection(LoggingSettings.SectionName))
            {
                LoggingSettings loggingSettings = null;
                ClientSettingsNode clientSettingsNode = null;
                try
                {
                    loggingSettings = (LoggingSettings)configurationContext.GetConfiguration(LoggingSettings.SectionName);
                    clientSettingsNode = new ClientSettingsNode(loggingSettings);
                    ConfigurationNode configurationNode = GetLoggingSettingsNode(serviceProvider);
                    configurationNode.Nodes.Add(clientSettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, clientSettingsNode, e);
                }
            }
        }

        /// <summary>
        /// <para>Saves the configuration settings created for the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public override void Save(IServiceProvider serviceProvider)
        {
            ConfigurationContext configurationContext = ServiceHelper.GetCurrentConfigurationContext(serviceProvider);
            if (configurationContext.IsValidSection(LoggingSettings.SectionName))
            {
                ClientSettingsNode clientSettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    clientSettingsNode = hierarchy.FindNodeByType(typeof(ClientSettingsNode)) as ClientSettingsNode;
                    if (clientSettingsNode == null)
                    {
                        return;
                    }
                    LoggingSettings loggingSettings = clientSettingsNode.LoggingSettings;
                    configurationContext.WriteConfiguration(LoggingSettings.SectionName, loggingSettings);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, clientSettingsNode, e);
                }
                catch (InvalidOperationException e)
                {
                    ServiceHelper.LogError(serviceProvider, clientSettingsNode, e);
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
        public override void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
        {
            ClientSettingsNode node = GetClientSettingsNode(serviceProvider);
            if (node != null)
            {
                LoggingSettings settings = node.LoggingSettings;
                configurationDictionary[LoggingSettings.SectionName] = settings;
            }
        }

        private static ClientSettingsNode GetClientSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(ClientSettingsNode)) as ClientSettingsNode;
        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(CustomDistributionStrategyNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomDistributionStrategyData), SR.CustomDistributionStrategy);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(InProcDistributionStrategyNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(InProcDistributionStrategyData), SR.InProcDistributionStrategy);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(MsmqDistributionStrategyNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(MsmqDistributionStrategyData), SR.MsmqDistributionStrategy);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}