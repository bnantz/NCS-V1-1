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
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a connection string node in the data settings for an application.
    /// </para>
    /// </summary>
    [Image(typeof(ConnectionStringNode))]
    public class ConnectionStringNode : ConfigurationNode
    {
        private ConnectionStringData connectionStringData;       

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ConnectionStringNode"/> class.
        /// </para>
        /// </summary>
        public ConnectionStringNode() : this(new ConnectionStringData(SR.DefaultConnectionStringNodeName))
        {
        }

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ConnectionStringNode"/> class with a <see cref="ConnectionStringData"/>.
        /// </para>
        /// </summary>
        /// <param name="connectionStringData">
        /// <para>The <see cref="ConnectionStringData"/> that this node represents.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="connectionStringData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ConnectionStringNode(ConnectionStringData connectionStringData) : base()
        {
            if (connectionStringData == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            this.connectionStringData = connectionStringData;
        }

        /// <summary>
        /// <para>Gets the <see cref="ConnectionStringData"/> object that this node represents.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConnectionStringData"/> object that this node represents.</para>
        /// </value>        
        [Browsable(false)]
        public virtual ConnectionStringData ConnectionStringData
        {
            get
            {
                this.connectionStringData.Parameters.Clear();
                foreach (ConfigurationNode childNode in Nodes)
                {
                    ParameterNode parameterNode = childNode as ParameterNode;
                    if (parameterNode != null)
                    {
                        this.connectionStringData.Parameters.Add(parameterNode.Parameter);
                    }
                }
                return this.connectionStringData;
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
            this.CreateDefaultParameterNodes();
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = connectionStringData.Name;
            foreach (ParameterData parameter in this.connectionStringData.Parameters)
            {
                if (parameter.IsSensitive)
                {
                    Nodes.Add(new SensitiveParameterNode(parameter));
                }
                else
                {
                    Nodes.Add(new ParameterNode(parameter));
                }
            }
        }

        /// <summary>
        /// <para>Add menus for the encryption settings node.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            AddMenuItem(new ConfigurationMenuItem(SR.ParameterMenuText, new AddChildNodeCommand(Site, typeof(ParameterNode)), this, Shortcut.None, SR.ParameterStatusText, InsertionPoint.New));
            AddMenuItem(new ConfigurationMenuItem(SR.SensitiveParameterMenuText, new AddChildNodeCommand(Site, typeof(SensitiveParameterNode)), this, Shortcut.None, SR.SensitiveParameterStatusText, InsertionPoint.New));
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            connectionStringData.Name = e.Node.Name;
        }

        private void CreateDefaultParameterNodes()
        {
            AddDefaultParameter(new ParameterData(SR.DefaultServerParameterName, SR.DefaultServerParameterName));
            AddDefaultParameter(new ParameterData(SR.DefaultDatabaseParameterName, SR.DefaultDatabaseParameterName));
            AddDefaultParameter(new ParameterData(SR.DefaultSecurityParameterName, true.ToString(CultureInfo.InvariantCulture)));
        }

        private void AddDefaultParameter(ParameterData data)
        {
            ParameterNode node = new ParameterNode(data);
            Nodes.AddWithDefaultChildren(node);
        }
    }
}