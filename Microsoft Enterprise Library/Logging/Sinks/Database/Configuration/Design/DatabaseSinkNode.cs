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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design
{
    /// <summary>
    ///  Represents a node for a "<see cref="DatabaseSinkData"/>".
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DatabaseSinkNode : SinkNode
    {
        private const string DatabaseSinkStoredProcedure = "WriteLog";

        private DatabaseSinkData databaseSinkData;
        private InstanceNode instanceNode;
        private ConfigurationNodeChangedEventHandler onInstanceNodeRemoved;
        private ConfigurationNodeChangedEventHandler onInstanceNodeRenamed;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public DatabaseSinkNode() : this(new DatabaseSinkData(SR.DatabaseSink, DatabaseSinkStoredProcedure))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="databaseSinkData">Configuration data.</param>
        public DatabaseSinkNode(DatabaseSinkData databaseSinkData) : base(databaseSinkData)
        {
            this.databaseSinkData = databaseSinkData;
            this.onInstanceNodeRemoved = new ConfigurationNodeChangedEventHandler(OnInstanceNodeRemoved);
            this.onInstanceNodeRenamed = new ConfigurationNodeChangedEventHandler(OnInstanceNodeRenamed);
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.DatabaseSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return databaseSinkData.TypeName; }
        }

        /// <summary>
        /// The configured database instance to use for this sink.
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
                this.instanceNode = (InstanceNode)service.CreateReference(instanceNode, value, onInstanceNodeRemoved, onInstanceNodeRenamed);
                databaseSinkData.DatabaseInstanceName = (instanceNode == null) ? String.Empty : instanceNode.Name;

            }
        }

        /// <summary>
        /// See <see cref="DatabaseSinkData.StoredProcName"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.StoredProcNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string StoredProcName
        {
            get { return databaseSinkData.StoredProcName; }
            set { databaseSinkData.StoredProcName = value; }
        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            DatabaseSettingsNode databaseSettingsNode = Hierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
            Debug.Assert(databaseSettingsNode != null, "How is it that the database settings are not there?");
            InstanceCollectionNode instanceCollectionNode = Hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            DatabaseInstance = Hierarchy.FindNodeByName(instanceCollectionNode, this.databaseSinkData.DatabaseInstanceName) as InstanceNode;
        }

        /// <summary>
        /// Creates a Data configuration section if required.
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
            this.databaseSinkData.DatabaseInstanceName = e.Node.Name;
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