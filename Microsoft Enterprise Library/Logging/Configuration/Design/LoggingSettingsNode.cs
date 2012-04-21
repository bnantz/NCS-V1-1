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

using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    /// <summary>
    /// Root node for the Logging designtime configuration.
    /// </summary>
    [Image(typeof (LoggingSettingsNode))]
    public class LoggingSettingsNode : ConfigurationNode
    {
        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        public LoggingSettingsNode() : base()
        {
        }

        /// <summary>
        /// Adds Client and Distributor settings nodes.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            new AddConfigurationSectionCommand(Site, typeof(ClientSettingsNode), LoggingSettings.SectionName).Execute(this);
            new AddConfigurationSectionCommand(Site, typeof(DistributorSettingsNode), DistributorSettings.SectionName).Execute(this);
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.ClientSettings,
                new AddConfigurationSectionCommand(Site, typeof(ClientSettingsNode), LoggingSettings.SectionName),
                this,
                Shortcut.None,
                SR.GenericCreateStatusText(SR.LogSettingsCmd),
                InsertionPoint.New);
            bool containsNode = Hierarchy.ContainsNodeType(typeof(ClientSettingsNode));
            item.Enabled = !containsNode;
            AddMenuItem(item);

            item = new ConfigurationMenuItem(SR.DistributorSettings,
                new AddConfigurationSectionCommand(Site, typeof(DistributorSettingsNode), DistributorSettings.SectionName),
                this,
                Shortcut.None,
                SR.GenericCreateStatusText(SR.DistributorSettings),
                InsertionPoint.New);
            containsNode = Hierarchy.ContainsNodeType(typeof(DistributorSettingsNode));
            item.Enabled = !containsNode;
            AddMenuItem(item);
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.LogSettingsNode;
        }

    }
}