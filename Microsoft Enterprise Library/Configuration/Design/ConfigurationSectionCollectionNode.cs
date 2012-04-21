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
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>The design-time representation of the <see cref="ConfigurationSectionDataCollection"/> class.</para>
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    [Image(typeof(ConfigurationSectionCollectionNode))]
    public class ConfigurationSectionCollectionNode : ConfigurationNode
    {
        private ConfigurationSettings configurationSettings;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionCollectionNode"/> class.</para>
        /// </summary>
        public ConfigurationSectionCollectionNode() : base()
        {
            this.configurationSettings = new ConfigurationSettings();

        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionCollectionNode"/> class with the runtime configuration.</para>
        /// </summary>
        /// <param name="configurationSettings">
        /// <para>The runtime configuration </para>
        /// </param>
        public ConfigurationSectionCollectionNode(ConfigurationSettings configurationSettings) : this()
        {
            if (null == configurationSettings)
            {
                throw new ArgumentNullException("configurationSettings");
            }
            this.configurationSettings = configurationSettings;
        }

        /// <summary>
        /// <para>Gets or sets the name of the node.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// <para>Get the runtime configuration settings.</para>
        /// </summary>
        /// <value>
        /// <para>The runtime configuration settings.</para>
        /// </value>
        [Browsable(false)]
        public virtual ConfigurationSettings ConfigurationSettings
        {
            get
            {
                this.configurationSettings.ConfigurationSections.Clear();
                foreach (ConfigurationNode node in Nodes)
                {
                    ConfigurationSectionNode sectionNode = node as ConfigurationSectionNode;
                    if (sectionNode != null)
                    {
                        this.configurationSettings.ConfigurationSections.Add(sectionNode.ConfigurationSection);
                        continue;
                    }

                    EncryptionSettingsNode encryptionNode = node as EncryptionSettingsNode;
                    if (encryptionNode != null)
                    {
                        this.configurationSettings.KeyAlgorithmPairStorageProviderData = encryptionNode.KeyAlgorithmPairStorageProviderData;
                        continue;
                    }
                }
                return this.configurationSettings;
            }
        }

        /// <summary>
        /// <para>Adds a new menu item to the user iterface to create a <see cref="ConfigurationSectionNode"/>.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            AddMenuItem(new ConfigurationMenuItem(SR.ConfigurationSectionMenuItemText, new AddChildNodeCommand(Site, typeof(ConfigurationSectionNode)), this, Shortcut.None, SR.ConfigurationSectionCollectionStatusText, InsertionPoint.New));
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name and adds the <see cref="EncryptionSettingsNode"/> that applies to the configuration sections.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultConfigurationSectionCollectionNodeName;
            if (configurationSettings.KeyAlgorithmPairStorageProviderData != null)
            {
                Nodes.Add(new EncryptionSettingsNode(configurationSettings.KeyAlgorithmPairStorageProviderData));
            }
            else
            {
                Nodes.Add(new EncryptionSettingsNode());
            }
            CreateDynamicNodes(configurationSettings.ConfigurationSections);
        }
    }
}