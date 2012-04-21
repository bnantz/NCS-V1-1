//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Node that represents a CacheStorageData
    /// </summary>
    [Image(typeof(CacheStorageNode))]
    public abstract class CacheStorageNode : ConfigurationNode
    {
        private CacheStorageData cacheStorageData;

        /// <summary>
        /// Creates node with sepecifed display name and configuration data.
        /// </summary>
        /// <param name="cacheStorageData">The configuration data.</param>
        protected CacheStorageNode(CacheStorageData cacheStorageData) : base()
        {
            if (cacheStorageData == null)
            {
                throw new ArgumentNullException("cacheStorageData");
            }
            this.cacheStorageData = cacheStorageData;
        }

        /// <summary>
        /// Name of the type used to implement this behavior
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.CacheStorageNodeTypeDescription)]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IBackingStore))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string Type
        {
            get { return cacheStorageData.TypeName; }
            set { cacheStorageData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CacheStorageData CacheStorageData
        {
            get
            {
                cacheStorageData.StorageEncryption = GetStorageEncryptionData();
                return cacheStorageData;
            }
        }

        /// <summary>
        /// <para>Sets the name based on the data name and adds the encyrption node if one exists.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = cacheStorageData.Name;
            if (cacheStorageData.StorageEncryption != null && cacheStorageData.StorageEncryption.Name != null)
            {
                INodeCreationService service = GetService(typeof(INodeCreationService)) as INodeCreationService;
                Debug.Assert(service != null, "Could not get INodeCreationService");
                ConfigurationNode node = service.CreateNode(cacheStorageData.StorageEncryption.GetType(), new object[] {cacheStorageData.StorageEncryption});
                Nodes.Add(node);
            }
        }

        /// <summary>
        /// <para>Adds the default menu items.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(StorageEncryptionNode));
        }

        /// <summary>
        /// <para>Renames the underlying data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            cacheStorageData.Name = e.Node.Name;
        }

        private StorageEncryptionProviderData GetStorageEncryptionData()
        {
            StorageEncryptionProviderData storageEncryptionData = null;

            if (Nodes.Count == 1)
            {
                storageEncryptionData = ((StorageEncryptionNode)Nodes[0]).StorageEncryptionProviderData;
            }

            return storageEncryptionData;
        }
    }
}