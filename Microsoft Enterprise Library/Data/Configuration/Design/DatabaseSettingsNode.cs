//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents the database settings for an application.
    /// </para>
    /// </summary>
    [Image(typeof(DatabaseSettingsNode))]
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DatabaseSettingsNode : ConfigurationNode
    {
        private DatabaseSettings databaseSettings;
        private InstanceNode instanceNode;
        private ConfigurationNodeChangedEventHandler instanceRemovedHandler;
        private ConfigurationNodeChangedEventHandler instanceRenameHandler;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseSettingsNode"/> class.</para>
        /// </summary>
        public DatabaseSettingsNode() : this(new DatabaseSettings())
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseSettingsNode"/> class with a <see cref="DatabaseSettings"/> object.</para>
        /// </summary>
        /// <param name="databaseSettings">
        /// <para>The <see cref="DatabaseSettings"/> runtime configuration.</para>
        /// </param>
        public DatabaseSettingsNode(DatabaseSettings databaseSettings) : base()
        {
            if (databaseSettings == null)
            {
                throw new ArgumentNullException("databaseSettings");
            }
            this.databaseSettings = databaseSettings;
            this.instanceRemovedHandler = new ConfigurationNodeChangedEventHandler(OnInstanceNodeRemoved);
            this.instanceRenameHandler = new ConfigurationNodeChangedEventHandler(OnInstanceNodeRenamed);
        }

        /// <summary>
        /// <para>
        /// Gets or sets the <see cref="DatabaseTypeNode"/> for the instance.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>
        /// The <see cref="DatabaseTypeNode"/> for the instance.
        /// </para>
        /// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(InstanceNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [Required]
        [SRDescription(SR.Keys.DefaultInstanceTypeDescription)]
        public InstanceNode DefaultInstanceNode
        {
            get { return this.instanceNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.instanceNode = (InstanceNode)service.CreateReference(instanceNode, value, instanceRemovedHandler, instanceRenameHandler);
                databaseSettings.DefaultInstance = string.Empty;
                if (this.instanceNode != null)
                {
                    databaseSettings.DefaultInstance = this.instanceNode.Name;
                }
            }
        }

        /// <summary>
        /// <para>Gets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// <para>Gets the runtime configuration data for the database configuration.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="DatabaseSettings"/> reference.</para>
        /// </value>
        [Browsable(false)]
        public virtual DatabaseSettings DatabaseSettings
        {
            get
            {
                GetInstanceCollectionData();
                GetDatabaseTypeDataCollection();
                GetConnectionStringCollectionData();
                return this.databaseSettings;
            }
        }

        private void GetConnectionStringCollectionData()
        {
            ConnectionStringCollectionNode node = Hierarchy.FindNodeByType(typeof(ConnectionStringCollectionNode)) as ConnectionStringCollectionNode;
            if (node == null) return;

            ConnectionStringDataCollection data = node.ConnectionStringDataCollection;
            if (Object.ReferenceEquals(databaseSettings.ConnectionStrings, data)) return;

            databaseSettings.ConnectionStrings.Clear();
            foreach (ConnectionStringData connectionStringData in data)
            {
                databaseSettings.ConnectionStrings[connectionStringData.Name] = connectionStringData;
            }
        }

        private void GetDatabaseTypeDataCollection()
        {
            DatabaseTypeCollectionNode node = Hierarchy.FindNodeByType(typeof(DatabaseTypeCollectionNode)) as DatabaseTypeCollectionNode;
            if (node == null) return;

            DatabaseTypeDataCollection data = node.DatabaseTypeDataCollection;
            if (Object.ReferenceEquals(databaseSettings.DatabaseTypes, data)) return;

            databaseSettings.DatabaseTypes.Clear();
            foreach (DatabaseTypeData databseTypeData in data)
            {
                databaseSettings.DatabaseTypes[databseTypeData.Name] = databseTypeData;
            }
        }

        private void GetInstanceCollectionData()
        {
            InstanceCollectionNode node = Hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            if (node == null) return;

            InstanceDataCollection data = node.InstanceDataCollection;
            if (Object.ReferenceEquals(databaseSettings.Instances, data)) return;

            databaseSettings.Instances.Clear();
            foreach (InstanceData instanceData in data)
            {
                databaseSettings.Instances[instanceData.Name] = instanceData;
            }
        }

        /// <summary>
        /// <para>
        /// Add the default child nodes for the current node.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will add the default <see cref="DatabaseTypeCollectionNode"/>, <see cref="ConnectionStringCollectionNode"/>, and <see cref="InstanceCollectionNode"/>.
        /// </para>
        /// </remarks>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            AddDefaultDatabaseTypeCollectionNode();
            AddDefaultConnectionStringCollectionNode();
            AddDefaultInstanceNode();
            InstanceCollectionNode nodes = (InstanceCollectionNode)Hierarchy.FindNodeByType(this, typeof(InstanceCollectionNode));
            if (nodes.Nodes.Count > 0)
            {
                this.DefaultInstanceNode = nodes.Nodes[0] as InstanceNode;
            }
        }

        /// <summary>
        /// <para>After the node is loaded, allows child nodes to resolve references to sibling nodes in configuration.</para>
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            InstanceCollectionNode node = Hierarchy.FindNodeByType(this, typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            if (node == null) return;
            DefaultInstanceNode = Hierarchy.FindNodeByName(node, databaseSettings.DefaultInstance) as InstanceNode;
            if (instanceNode == null)
            {
                throw new InvalidOperationException(SR.ExceptionInstanceNodeNotFound(databaseSettings.DefaultInstance));
            }
        }


        /// <summary>
        /// <para>Sets the default name of the node.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultDatabaseSettingsName;
            if (databaseSettings.ConnectionStrings.Count > 0)
            {
                Nodes.Add(new ConnectionStringCollectionNode(this.databaseSettings.ConnectionStrings));
            }
            if (databaseSettings.DatabaseTypes.Count > 0)
            {
                Nodes.Add(new DatabaseTypeCollectionNode(this.databaseSettings.DatabaseTypes));
            }
            if (databaseSettings.Instances.Count > 0)
            {
                Nodes.Add(new InstanceCollectionNode(this.databaseSettings.Instances));
            }
        }

        /// <summary>
        /// <para>Adds the base menu items and menu items for creating <see cref="ConnectionStringCollectionNode"/>, <see cref="DatabaseTypeCollectionNode"/>, <see cref="InstanceCollectionNode"/> objects.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();            
            AddNodeMenu(typeof(ConnectionStringCollectionNode), SR.ConnectionStringColectionNodeMenuText, SR.ConnectionStringColectionNodeStatusText);
            AddNodeMenu(typeof(DatabaseTypeCollectionNode), SR.DatabaseTypeColectionNodeMenuText, SR.DatabaseTypeColectionNodeStatusText);
            AddNodeMenu(typeof(InstanceCollectionNode), SR.InstanceColectionNodeMenuText, SR.InstanceColectionNodeStatusText);
        }

        private void AddNodeMenu(Type type, string menuText, string statusText)
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem(menuText, 
                                                                   new AddChildNodeCommand(Site, type), 
                                                                   this, 
                                                                   Shortcut.None, 
                                                                   statusText, InsertionPoint.New);
            item.Enabled = !DoesChildNodeExist(typeof(ConnectionStringCollectionNode));
            AddMenuItem(item);
        }

        private bool DoesChildNodeExist(Type type)
        {
            if (Hierarchy.FindNodeByType(this, type) != null)
            {
                return true;
            }
            return false;
        }


        private void AddDefaultInstanceNode()
        {
            InstanceCollectionNode node = new InstanceCollectionNode(databaseSettings.Instances);
            Nodes.AddWithDefaultChildren(node);            
        }

        private void AddDefaultConnectionStringCollectionNode()
        {
            ConnectionStringCollectionNode node = new ConnectionStringCollectionNode(databaseSettings.ConnectionStrings);
            Nodes.AddWithDefaultChildren(node);            
        }

        private void AddDefaultDatabaseTypeCollectionNode()
        {
            DatabaseTypeCollectionNode node = new DatabaseTypeCollectionNode(databaseSettings.DatabaseTypes);
            Nodes.AddWithDefaultChildren(node);            
        }

        /// <devdoc>
        /// Handles the remove of a instnce node.
        /// </devdoc>                
        private void OnInstanceNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.instanceNode = null;
            databaseSettings.DefaultInstance = string.Empty;
        }

        /// <devdoc>
        /// Handles the rename of a database node.
        /// </devdoc>                
        private void OnInstanceNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            databaseSettings.DefaultInstance = e.Node.Name;
        }
    }
}