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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a node for a <see cref="ConfigurationSectionData"/> of the current configuration.</para>
    /// </summary>
    [ServiceDependency(typeof(INodeCreationService))]
    [Image(typeof(ConfigurationSectionNode))]
    public class ConfigurationSectionNode : ConfigurationNode
    {
        private ConfigurationSectionData configurationSectionData;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionNode"/> class.</para>
        /// </summary>
        public ConfigurationSectionNode() : this(new ConfigurationSectionData(SR.DefaultConfigurationSectionNodeName))
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionNode"/> class with the runtime configuration.</para>
        /// </summary>
        /// <param name="configurationSectionData">
        /// <para>The runtime version of the configuration data.</para>
        /// </param>        
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="configurationSectionData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ConfigurationSectionNode(ConfigurationSectionData configurationSectionData) : base()
        {
            if (configurationSectionData == null)
            {
                throw new ArgumentNullException("configurationSection");
            }
            this.configurationSectionData = configurationSectionData;
        }

        /// <summary>
        /// <para>Gets or sets whether or not this section is encrypted or not.</para>
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the section is encrypted; otherwise, <see langword="false"/>.
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.ConfigurationSectionNodeEncryptDescription)]
        public bool Encrypt
        {
            get { return configurationSectionData.Encrypt; }
            set { configurationSectionData.Encrypt = value; }
        }

        /// <summary>
        /// <para>Gets the runtime configuration data.</para>
        /// </summary>
        /// <value><para>The runtime configuration data.</para></value>
        [Browsable(false)]
        public virtual ConfigurationSectionData ConfigurationSection
        {
            get
            {
                this.configurationSectionData.Transformer = GetTransformer();
                this.configurationSectionData.StorageProvider = GetStorageProvider();
                return this.configurationSectionData;
            }
        }

        /// <summary>
        /// <para>Selects the child <see cref="StorageProviderNode"/> if it exists.</para>
        /// </summary>
        /// <returns><para>The <see cref="StorageProviderNode"/> or <see langword="null"/> (Nothing in Visual Basic) if one doe not exist.</para></returns>
        public StorageProviderNode SelectStorageProviderNode()
        {
            return Hierarchy.FindNodeByType(this, typeof(StorageProviderNode)) as StorageProviderNode;
        }

        /// <summary>
        /// <para>Selects the child <see cref="TransformerNode"/> if it exists.</para>
        /// </summary>
        /// <returns><para>The <see cref="TransformerNode"/> or <see langword="null"/> (Nothing in Visual Basic) if one doe not exist.</para></returns>
        public TransformerNode SelectTransformerNode()
        {
            return Hierarchy.FindNodeByType(this, typeof(TransformerNode)) as TransformerNode;
        }

        /// <devdoc>
        /// Creates a new ConfigurationSectionNode with storage and transformer.
        /// </devdoc>
        internal static ReadOnlyConfigurationSectionNode CreateReadOnlyDefault(string sectionName, ConfigurationNode parent)
        {
            ReadOnlyConfigurationSectionNode sectionNode = new ReadOnlyConfigurationSectionNode(new ReadOnlyConfigurationSectionData(sectionName));
            parent.Nodes.Add(sectionNode);
            XmlFileStorageProviderNode storageNode = new XmlFileStorageProviderNode(new XmlFileStorageProviderData(SR.XMLStorageProviderNodeFriendlyName, sectionName + ".config"));
            sectionNode.Nodes.Add(storageNode);
            XmlSerializerTransformerNode transformerNode = new XmlSerializerTransformerNode();
            sectionNode.Nodes.Add(transformerNode);
            return sectionNode;
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name and adds the <see cref="StorageProviderNode"/> and <see cref="TransformerNode"/> based on any active configuration data.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = configurationSectionData.Name;
            INodeCreationService service = ServiceHelper.GetNodeCreationService(Site);
            
            if (this.configurationSectionData.StorageProvider != null)
            {
                StorageProviderNode storageProviderNode = service.CreateNode(configurationSectionData.StorageProvider.GetType(), new object[] {configurationSectionData.StorageProvider}) as StorageProviderNode;
                Debug.Assert(storageProviderNode != null, "The storage provider type was not registered succesfully.");
                this.Nodes.Add(storageProviderNode);
            }
            if (this.configurationSectionData.Transformer != null)
            {
                TransformerNode transformerNode = service.CreateNode(configurationSectionData.Transformer.GetType(), new object[] {this.configurationSectionData.Transformer}) as TransformerNode;
                Debug.Assert(transformerNode != null, "The transformer type was not registered succesfully.");
                this.Nodes.Add(transformerNode);
            }
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the underlying configuration section runtime data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            configurationSectionData.Name = e.Node.Name;
        }

        /// <summary>
        /// <para>Add menu items to the user interface for creating <see cref="StorageProviderNode"/>s and <see cref="TransformerNode"/>s.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(StorageProviderNode));
            CreateDynamicMenuItems(typeof(TransformerNode));
        }

        private TransformerData GetTransformer()
        {
            TransformerNode transformerNode = this.SelectTransformerNode();
            if (transformerNode != null)
            {
                return transformerNode.TransformerData;
            }
            else
            {
                return null;
            }
        }

        private StorageProviderData GetStorageProvider()
        {
            StorageProviderNode storageProviderNode = this.SelectStorageProviderNode();
            if (storageProviderNode != null)
            {
                return storageProviderNode.StorageProvider;
            }
            else
            {
                return null;
            }
        }
    }
}