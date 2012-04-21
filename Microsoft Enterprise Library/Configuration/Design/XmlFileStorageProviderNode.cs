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

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a storage provider for the meta-configuration of a specific section of configuration for an application.</para>
    /// </summary>
    public class XmlFileStorageProviderNode : StorageProviderNode
    {
        private XmlFileStorageProviderData xmlFileStorageProviderData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="XmlFileStorageProviderNode"/> class.</para>
        /// </summary>               
        public XmlFileStorageProviderNode() : this(new XmlFileStorageProviderData(SR.XMLStorageProviderNodeFriendlyName))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="XmlFileStorageProviderNode"/> class.</para>
        /// </summary>       
        /// <param name="data">
        /// <para>The runtime configuration data.</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="data"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public XmlFileStorageProviderNode(XmlFileStorageProviderData data) : base(data)
        {
            this.xmlFileStorageProviderData = data;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the file name of the external configuration file.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>
        /// The file name of the configuration file.
        /// </para>
        /// </value>
        [Required]
        [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
        [FilteredFileNameEditor("Configuration files (*.config)|*.config|All files|*.*")]
        [SRDescription(SR.Keys.XmlFileStorageProviderNodeFileNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string FileName
        {
            get { return this.xmlFileStorageProviderData.Path; }
            set
            {
                Hierarchy.StorageTable.Remove(xmlFileStorageProviderData.Path);
                this.xmlFileStorageProviderData.Path = value;
                Hierarchy.StorageTable.Add(new XmlFileStorageCreationCommand(value, Site));
            }
        }

        /// <summary>
        /// <para>
        /// Gets or sets the fully qualified type name of the instance.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>
        /// The name of the connection string.
        /// </para>
        /// </value>
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.XmlFileStorageProviderNodeTypeNameDescription)]
        [ReadOnly(true)]
        public string TypeName
        {
            get { return this.xmlFileStorageProviderData.TypeName; }
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name and adds a <see cref="XmlFileStorageCreationCommand"/> to the current storage table if the file name is set.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited ();
            if ((FileName.Length > 0) && (!Hierarchy.StorageTable.Contains(FileName)))
            {
                Hierarchy.StorageTable.Add(new XmlFileStorageCreationCommand(FileName, Site));
            }
        }

        /// <summary>
        /// <para>Adds a a <see cref="ValidateNodeCommand"/> and <see cref="RemoveNodeCommand"/> if the parent node is not readonly.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            if (!Parent.GetType().Equals(typeof(ReadOnlyConfigurationSectionNode)))
            {
                AddMenuItem(ConfigurationMenuItem.CreateRemoveNodeCommand(Site, this));
            }
            AddMenuItem(ConfigurationMenuItem.CreateValidateNodeCommand(Site, this));
        }

        /// <summary>
        /// <para>Removes the document from the document table.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRemoving(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRemoving(e);
            if (FileName.Length > 0)
            {
                if (Hierarchy != null) Hierarchy.StorageTable.Remove(FileName);
            }
        }

    }
}