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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor
{
    /// <summary>
    /// Configuraiton design manager for distributor settings.
    /// </summary>
    public class DistributorLoggingConfigurationDesignManager : LoggingConfigurationDesignManager
    {

        public override void Register(IServiceProvider serviceProvider)
        {
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
            if (configurationContext.IsValidSection(DistributorSettings.SectionName))
            {
                DistributorSettings distributorSettings = null;
                DistributorSettingsNode distributorSettingsNode = null;
                try
                {
                    distributorSettings = (DistributorSettings)configurationContext.GetConfiguration(DistributorSettings.SectionName);
                    distributorSettingsNode = new DistributorSettingsNode(distributorSettings);
                    ConfigurationNode configurationNode = GetLoggingSettingsNode(serviceProvider);
                    configurationNode.Nodes.Add(distributorSettingsNode);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, distributorSettingsNode, e);
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
            if (configurationContext.IsValidSection(DistributorSettings.SectionName))
            {
                DistributorSettingsNode distributorSettingsNode = null;
                try
                {
                    IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
                    distributorSettingsNode = hierarchy.FindNodeByType(typeof(DistributorSettingsNode)) as DistributorSettingsNode;
                    if (distributorSettingsNode == null)
                    {
                        return;
                    }
                    DistributorSettings distributorSettings = distributorSettingsNode.DistributorSettings;
                    configurationContext.WriteConfiguration(DistributorSettings.SectionName, distributorSettings);
                }
                catch (ConfigurationException e)
                {
                    ServiceHelper.LogError(serviceProvider, distributorSettingsNode, e);
                }
                catch (InvalidOperationException e)
                {
                    ServiceHelper.LogError(serviceProvider, distributorSettingsNode, e);
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
            DistributorSettingsNode node = GetDistributorSettingsNode(serviceProvider);
            if (node != null)
            {
                DistributorSettings settings = node.DistributorSettings;
                configurationDictionary[DistributorSettings.SectionName] = settings;
            }
        }

        private static DistributorSettingsNode GetDistributorSettingsNode(IServiceProvider serviceProvider)
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            if (hierarchy == null) return null;

            return hierarchy.FindNodeByType(typeof(DistributorSettingsNode)) as DistributorSettingsNode;
        }

        private static void RegisterNodeMaps(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

            Type nodeType = typeof(CustomSinkNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomSinkData), SR.CustomSink);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(EmailSinkNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(EmailSinkData), SR.EmailSink);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(EventLogSinkNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(EventLogSinkData), SR.EventLogSink);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(FlatFileSinkNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(FlatFileSinkData), SR.FlatFileSink);
            nodeCreationService.AddNodeCreationEntry(entry);

           nodeType = typeof(RollingFlatFileSinkNode);
           entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(RollingFlatFileSinkData), SR.RollingFlatFileSink);
           nodeCreationService.AddNodeCreationEntry(entry);
   
           nodeType = typeof(ConsoleSinkNode);
           entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(ConsoleSinkData), SR.ConsoleSink);
           nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(MsmqSinkNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(MsmqSinkData), SR.MsmqSink);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(WmiLogSinkNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(WMILogSinkData), SR.WmiLogSink);
            nodeCreationService.AddNodeCreationEntry(entry);

           nodeType = typeof(WSSinkNode);
           entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(WSSinkNode), SR.WSSink);
           nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(CustomFormatterNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(CustomFormatterData), SR.CustomFormatter);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(TextFormatterNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryWithMultiples(new AddChildNodeCommand(serviceProvider, nodeType), nodeType, typeof(TextFormatterData), SR.TextFormatter);
            nodeCreationService.AddNodeCreationEntry(entry);
        }
    }
}