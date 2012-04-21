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

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design
{
    /// <summary>
    /// The design time representation for <see cref="DbProfileProviderData"/>.
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DbProfileProviderNode : ProfileProviderNode
    {
        private DbProfileProviderData dbProfileProviderData;
        private InstanceNode database;
        private ConfigurationNodeChangedEventHandler onDatabaseRemoved;
        private ConfigurationNodeChangedEventHandler onDatabaseRenamed;

        /// <summary>
        /// Initializes the node with default configuration data.
        /// </summary>
        public DbProfileProviderNode() : this(new DbProfileProviderData(SR.DbProfileProviderDisplayName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes the node with configuration data.
        /// </summary>
        /// <param name="dbProfileProviderData">The configuration data.</param>
        public DbProfileProviderNode(DbProfileProviderData dbProfileProviderData) : base(dbProfileProviderData)
        {
            this.dbProfileProviderData = dbProfileProviderData;
            this.onDatabaseRemoved = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRemoved);
            this.onDatabaseRenamed = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRenamed);
        }

        /// <summary>
        /// Gets or sets the database instance that will be used for the profile.
        /// </summary>
        /// <value>An <see cref="InstanceNode"/> object.</value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InstanceNode))]
        [SRDescription(SR.Keys.DbProfileProviderPropertyDescription)]
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public InstanceNode Database
        {
            get { return this.database; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.database = (InstanceNode)service.CreateReference(database, value, onDatabaseRemoved, onDatabaseRenamed);
                this.dbProfileProviderData.Database = string.Empty;
                if (this.database != null)
                {
                    this.dbProfileProviderData.Database = this.database.Name;
                }
            }
        }

        /// <summary>
        /// Resolves the database node reference.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            DatabaseSettingsNode databaseSettingsNode = Hierarchy.FindNodeByType(typeof(DatabaseSettingsNode)) as DatabaseSettingsNode;
            Debug.Assert(databaseSettingsNode != null, "How is it that the database settings are not there?");
            InstanceCollectionNode instanceCollectionNode = Hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            this.Database = Hierarchy.FindNodeByName(instanceCollectionNode, this.dbProfileProviderData.Database) as InstanceNode;
        }

        /// <summary>
        /// See <see cref="ProfileProviderNode.TypeName"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return typeof(DbProfileProvider).AssemblyQualifiedName; }
        }

        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CreateDatabaseSettingsNode();
        }

        private void OnDatabaseRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.Database = null;
        }

        private void OnDatabaseRenamed(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.dbProfileProviderData.Database = args.Node.Name;
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