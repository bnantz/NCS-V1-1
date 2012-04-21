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

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
    /// <summary>
    /// Node representing a Data Cache Storage
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    [ServiceDependency(typeof(IXmlIncludeTypeService))]
    [ServiceDependency(typeof(INodeCreationService))]
    public class DataCacheStorageNode : CacheStorageNode
    {
        private DataCacheStorageData dataCacheStorageData;
        private InstanceNode instanceNode;
        private ConfigurationNodeChangedEventHandler onInstanceNodeRemoved;
        private ConfigurationNodeChangedEventHandler onInstanceNodeRenamed;

        /// <summary>
        /// Creates node with initial data.
        /// </summary>
        public DataCacheStorageNode() : this(new DataCacheStorageData(SR.DataCacheStorage))
        {
        }

        /// <summary>
        /// Creates node with specified data.
        /// </summary>
        /// <param name="dataCacheStorageData">Configuration data.</param>
        public DataCacheStorageNode(DataCacheStorageData dataCacheStorageData) : base(dataCacheStorageData)
        {
            this.dataCacheStorageData = dataCacheStorageData;
            this.onInstanceNodeRemoved += new ConfigurationNodeChangedEventHandler(OnInstanceNodeRemoved);
            this.onInstanceNodeRenamed += new ConfigurationNodeChangedEventHandler(OnInstanceNodeRenamed);
        }

        /// <summary>
        /// Read only.  Returns the type name for a DataBackingStore.
        /// </summary>
        [Browsable(false)]
        public override string Type
        {
            get { return dataCacheStorageData.TypeName; }
        }

        /// <summary>
        /// The configured database instance node reference.
        /// </summary>
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InstanceNode))]    
        [SRCategory(SR.Keys.CategoryGeneral)]
        public InstanceNode DatabaseInstance
        {
            get { return instanceNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                instanceNode = service.CreateReference(instanceNode, value, onInstanceNodeRemoved, onInstanceNodeRenamed) as InstanceNode;
                this.dataCacheStorageData.DatabaseInstanceName = string.Empty;
                if (instanceNode != null)
                {
                    dataCacheStorageData.DatabaseInstanceName = instanceNode.Name;
                }
            }
        }

		/// <summary>
		/// The name of the segments in the caching store to use for this data.
		/// </summary>
        [Required]
        [SRDescription(SR.Keys.DatabasePartitionNameDesciption)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string PartitionName
        {
            get { return dataCacheStorageData.PartitionName; }
            set { dataCacheStorageData.PartitionName = value; }
        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            InstanceCollectionNode instanceCollectionNode = Hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            DatabaseInstance = Hierarchy.FindNodeByName(instanceCollectionNode, this.dataCacheStorageData.DatabaseInstanceName) as InstanceNode;
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        public override CacheStorageData CacheStorageData
        {
            get
            {
                return base.CacheStorageData;
            }
        }

        /// <summary>
        /// Initializes the data configuration section of the configuration tree
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CreateDatabaseSettingsNode();
        }

        private void OnInstanceNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.instanceNode = null;
        }

        private void OnInstanceNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.dataCacheStorageData.DatabaseInstanceName = e.Node.Name;
        }

        private void CreateDatabaseSettingsNode()
        {
            if (!DatabaseSettingsNodeExists())
            {
                AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Site, typeof(DatabaseSettingsNode), DatabaseSettings.SectionName);
                cmd.Execute(Hierarchy.RootNode);
            }
        }

        private bool DatabaseSettingsNodeExists()
        {
            DatabaseSettingsNode node = Hierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
            return (node != null);
        }
    }
}