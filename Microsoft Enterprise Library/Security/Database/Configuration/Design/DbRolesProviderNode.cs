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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime 
    /// <see cref="DbRolesProviderNode"/> class.
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DbRolesProviderNode : RolesProviderNode
    {
        private DbRolesProviderData dbRolesProviderData;
        private InstanceNode database;
        private ConfigurationNodeChangedEventHandler onDatabaseRemoved;
        private ConfigurationNodeChangedEventHandler onDatabaseRenamed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbRolesProviderNode"/> class.
        /// </summary>
        public DbRolesProviderNode() : this(new DbRolesProviderData(SR.DbRolesProviderDisplayName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbRolesProviderNode"/>
        /// class with the specified <see cref="DbRolesProviderData"/>.
        /// </summary>
        /// <param name="dbRolesProviderData">A <see cref="DbRolesProviderData"/> object.</param>
        public DbRolesProviderNode(DbRolesProviderData dbRolesProviderData) : base(dbRolesProviderData)
        {
            this.dbRolesProviderData = dbRolesProviderData;
            this.onDatabaseRemoved = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRemoved);
            this.onDatabaseRenamed = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRenamed);
        }

        /// <summary>
        /// Gets or sets the database instance that will be used to lookup
        /// roles. 
        /// </summary>
        /// <value>An <see cref="InstanceNode"/> object.</value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InstanceNode))]
        [SRDescription(SR.Keys.DbRolesProviderDatabasePropertyDescription)]
        [Required]
        public InstanceNode Database
        {
            get { return this.database; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.database = (InstanceNode)service.CreateReference(database, value, onDatabaseRemoved, onDatabaseRenamed);
                this.dbRolesProviderData.Database = string.Empty;
                if (this.database != null)
                {
                    this.dbRolesProviderData.Database = this.database.Name;
                }
            }
        }

        /// <summary>
        /// See <see cref="RolesProviderNode.TypeName"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Browsable(false)]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
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
            this.Database = Hierarchy.FindNodeByName(instanceCollectionNode, this.dbRolesProviderData.Database) as InstanceNode;

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
            this.dbRolesProviderData.Database = args.Node.Name;
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