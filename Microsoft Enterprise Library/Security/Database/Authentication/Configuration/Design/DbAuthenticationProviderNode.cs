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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime DbAuthenticationProvider class.
    /// </summary>
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DbAuthenticationProviderNode : AuthenticationProviderNode
    {
        private DbAuthenticationProviderData data;
        private InstanceNode database;
        private HashProviderNode hashProvider;

        private ConfigurationNodeChangedEventHandler onDatabaseRemoved;
        private ConfigurationNodeChangedEventHandler onDatabaseRenamed;

        private ConfigurationNodeChangedEventHandler onHashProviderRemoved;
        private ConfigurationNodeChangedEventHandler onHashProviderRenamed;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="DbAuthenticationProviderNode"/> class.
        /// </summary>
        public DbAuthenticationProviderNode() : this(new DbAuthenticationProviderData(SR.DbAuthenticationProviderDisplayName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="DbAuthenticationProviderNode"/> class with a
        /// specified <see cref="DbAuthenticationProviderData"/> object.
        /// </summary>
        /// <param name="data">A <see cref="DbAuthenticationProviderData"/> object.</param>
        public DbAuthenticationProviderNode(DbAuthenticationProviderData data) : base(data)
        {
            this.data = data;
            this.onDatabaseRemoved = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRemoved);
            this.onDatabaseRenamed = new ConfigurationNodeChangedEventHandler(this.OnDatabaseRenamed);

            this.onHashProviderRemoved = new ConfigurationNodeChangedEventHandler(this.OnHashProviderRemoved);
            this.onHashProviderRenamed = new ConfigurationNodeChangedEventHandler(this.OnHashProviderRenamed);
        }

        /// <summary>
        /// Gets or sets the database instance that will be used to authenticate credentials.
        /// </summary>
        /// <value>An <see cref="InstanceNode"/> object.</value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InstanceNode))]
        [SRDescription(SR.Keys.DbAuthenticationProviderDatabasePropertyDescription)]
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
                this.data.Database = string.Empty;
                if (this.database != null)
                {
                    this.data.Database = this.database.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets the database instance that will be used to authenticate credentials.
        /// </summary>
        /// <value>An <see cref="InstanceNode"/> object.</value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(HashProviderNode))]
        [SRDescription(SR.Keys.DbAuthenticationProviderHashProviderPropertyDescription)]
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public HashProviderNode HashProvider
        {
            get { return this.hashProvider; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.hashProvider = (HashProviderNode)service.CreateReference(database, value, onHashProviderRemoved, onHashProviderRenamed);
                this.data.HashProvider = string.Empty;
                if (this.hashProvider != null)
                {
                    this.data.HashProvider = this.hashProvider.Name;
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

        [Browsable(false)]
        public override AuthenticationProviderData AuthenticationProviderData
        {
            get { return base.AuthenticationProviderData; }
        }

        /// <summary>
        /// Resolves the database node reference.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            InstanceCollectionNode instanceCollectionNode = Hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            Database = Hierarchy.FindNodeByName(instanceCollectionNode, this.data.Database) as InstanceNode;

            HashProviderCollectionNode hashProviderCollectionNode = Hierarchy.FindNodeByType(typeof(HashProviderCollectionNode)) as HashProviderCollectionNode;
            HashProvider = Hierarchy.FindNodeByName(hashProviderCollectionNode, this.data.HashProvider) as HashProviderNode;
        }

        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();

            CreateDatabaseSettingsNode();
            CreateCryptographySettingsNode();
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

        private void CreateCryptographySettingsNode()
        {
            if (!CryptographySettingsNodeExists())
            {
                AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Site, typeof(CryptographySettingsNode), CryptographySettings.SectionName);
                cmd.Execute(Hierarchy.RootNode);
            }
        }

        private bool CryptographySettingsNodeExists()
        {
            CryptographySettingsNode node = Hierarchy.FindNodeByType(typeof(CryptographySettingsNode)) as CryptographySettingsNode;
            return (node != null);
        }

        private void OnDatabaseRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.Database = null;
        }

        private void OnDatabaseRenamed(object sender, ConfigurationNodeChangedEventArgs args)
        {
            if (Hierarchy != null) data.Database = args.Node.Name;
        }

        private void OnHashProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            if (Hierarchy != null) this.HashProvider = null;
        }

        private void OnHashProviderRenamed(object sender, ConfigurationNodeChangedEventArgs args)
        {
            data.HashProvider = args.Node.Name;
        }
    }
}