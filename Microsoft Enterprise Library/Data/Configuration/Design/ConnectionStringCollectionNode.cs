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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a container for the connection string nodes in the data settings for an application.
    /// </para>
    /// </summary>    
    [Image(typeof(ConnectionStringCollectionNode))]
    public class ConnectionStringCollectionNode : ConfigurationNode
    {
        private ConnectionStringDataCollection connectionStringDataCollection;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ConnectionStringCollectionNode"/> class with a <see cref="ConnectionStringDataCollection"/>.
        /// </para>
        /// </summary>
        /// <param name="connectionStringDataCollection">
        /// <para>The <see cref="ConnectionStringDataCollection"/> from configuration.</para>
        /// </param>
        public ConnectionStringCollectionNode(ConnectionStringDataCollection connectionStringDataCollection) : base()
        {
            if (connectionStringDataCollection == null)
            {
                throw new ArgumentNullException("connectionStringDataCollection");
            }
            this.connectionStringDataCollection = connectionStringDataCollection;

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
        /// <para>Gets the <see cref="ConnectionStringData"/> objects for this collection.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConnectionStringData"/> objects for this collection.</para>
        /// </value>
        [Browsable(false)]
        public virtual ConnectionStringDataCollection ConnectionStringDataCollection
        {
            get
            {
                connectionStringDataCollection.Clear();
                foreach (ConnectionStringNode connectionStringNode in Nodes)
                {
                    connectionStringDataCollection[connectionStringNode.ConnectionStringData.Name] = connectionStringNode.ConnectionStringData;
                }
                return connectionStringDataCollection;
            }
        }

        /// <summary>
        /// <para>
        /// Add the default child nodes for the current node.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will add the default parameter nodes for the connection string.
        /// </para>
        /// </remarks>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CreateDefaultConnectionStringNode();
        }

        /// <summary>
        /// <para>Adds the default menu items.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            AddMenuItem(ConfigurationMenuItem.CreateValidateNodeCommand(Site, this));
            CreateDynamicMenuItems(typeof(ConnectionStringNode));
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultConnectionStringCollectionNodeName;
            CreateDynamicNodes(connectionStringDataCollection);
        }

        private void CreateDefaultConnectionStringNode()
        {
            ConnectionStringNode node = new ConnectionStringNode(new ConnectionStringData(SR.DefaultNewConnectionStringName));
            Nodes.AddWithDefaultChildren(node);
        }
        
    }
}