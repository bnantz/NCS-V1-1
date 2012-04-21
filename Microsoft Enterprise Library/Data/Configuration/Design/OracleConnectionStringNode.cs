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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a connection string node for Oracle in the data settings for an application.
    /// </para>
    /// </summary>    
    public class OracleConnectionStringNode : ConnectionStringNode
    {
        private OracleConnectionStringData oracleConnectionStringData;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="OracleConnectionStringNode"/> class.
        /// </para>
        /// </summary>
        public OracleConnectionStringNode() : this(new OracleConnectionStringData(SR.DefaultOracleConnectionStringName))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="OracleConnectionStringNode"/> class with the runtime version that this node will represent.</para>
        /// </summary>
        /// <param name="oracleConnectionStringData">
        /// <para>The <see cref="OracleConnectionStringData"/> that this node will represent.</para>
        /// </param>
        public OracleConnectionStringNode(OracleConnectionStringData oracleConnectionStringData) : base(oracleConnectionStringData)
        {
            this.oracleConnectionStringData = oracleConnectionStringData;
        }

        /// <summary>
        /// <para>Gets the <see cref="OracleConnectionStringData"/> object that this node represents.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="OracleConnectionStringData"/> object that this node represents.</para>
        /// </value>
        [Browsable(false)]
        public override ConnectionStringData ConnectionStringData
        {
            get
            {
                OracleConnectionStringData oracleConnectionString = (OracleConnectionStringData)base.ConnectionStringData;
                oracleConnectionString.OraclePackages.Clear();
                foreach (ConfigurationNode childNode in Nodes)
                {
                    OraclePackageNode oraclePackageNode = childNode as OraclePackageNode;
                    if (oraclePackageNode != null)
                    {
                        oracleConnectionString.OraclePackages.Add(oraclePackageNode.OraclePackage);
                    }
                }
                return oracleConnectionString;
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
            // don't call base because I want to add my own nodes
            CreateDefaultParameterNode();
            CreateDefaultPackagesNode();
        }

        /// <summary>
        /// <para>Adds the base menu items and a menu item for creating <see cref="OraclePackageNode"/> objects.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.DefaultOraclePackageNodeName, new AddChildNodeCommand(Site, typeof(OraclePackageNode)), this, Shortcut.None, SR.GenericCreateStatusText(SR.DefaultOraclePackageNodeName), InsertionPoint.New);
            AddMenuItem(item);
        }


        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            foreach (OraclePackageData oraclePackage in oracleConnectionStringData.OraclePackages)
            {
                Nodes.Add(new OraclePackageNode(oraclePackage));
            }
        }

        private void CreateDefaultParameterNode()
        {
            ParameterNode node = new ParameterNode(new ParameterData(SR.DefaultServerParameterName, SR.DefaultServerParameterName));
            Nodes.AddWithDefaultChildren(node);
        }

        private void CreateDefaultPackagesNode()
        {
            OraclePackageNode node = new OraclePackageNode();
            Nodes.AddWithDefaultChildren(node);
        }
    }
}