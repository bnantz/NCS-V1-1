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

using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime configuration data
    /// for an Roles provider.
    /// </summary>
    [Image(typeof(RolesProviderCollectionNode))]
    public class RolesProviderCollectionNode : ConfigurationNode
    {
        private RolesProviderDataCollection rolesProviderDataCollection;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="RolesProviderCollectionNode"/> class.
        /// </summary>
        public RolesProviderCollectionNode() : this(new RolesProviderDataCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="RolesProviderCollectionNode"/>
        /// class from the specified data.
        /// </summary>
        /// <param name="rolesProviderDataCollection">The configuration data for an Roles provider collection.</param>
        public RolesProviderCollectionNode(RolesProviderDataCollection rolesProviderDataCollection) : base()
        {
            if (rolesProviderDataCollection == null)
            {
                throw new ArgumentNullException("rolesProviderDataCollection");
            }
            this.rolesProviderDataCollection = rolesProviderDataCollection;
        }

        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual RolesProviderDataCollection RolesProviderDataCollection
        {
            get
            {
                rolesProviderDataCollection.Clear();
                foreach (RolesProviderNode node in this.Nodes)
                {
                    rolesProviderDataCollection[node.RolesProviderData.Name] = node.RolesProviderData;
                }
                return rolesProviderDataCollection;
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.RolesProviderCollectionNodeName;
            CreateDynamicNodes(rolesProviderDataCollection);
        }

        protected override void OnAddMenuItems()
        {
            ConfigurationMenuItem.CreateValidateNodeCommand(Site, this);
            CreateDynamicMenuItems(typeof(RolesProviderNode));
        }
    }
}