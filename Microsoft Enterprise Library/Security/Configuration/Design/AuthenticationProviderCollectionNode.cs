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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime configuration data
    /// for an authentication provider.
    /// </summary>
    [Image(typeof(AuthenticationProviderCollectionNode))]
    public class AuthenticationProviderCollectionNode : ConfigurationNode
    {
        private AuthenticationProviderDataCollection authenticationProviderDataCollection;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthenticationProviderCollectionNode"/> class.
        /// </summary>
        public AuthenticationProviderCollectionNode() : this(new AuthenticationProviderDataCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthenticationProviderCollectionNode"/>
        /// class from the specified data.
        /// </summary>
        /// <param name="authenticationProviderDataCollection">The configuration data for an authentication provider collection.</param>
        public AuthenticationProviderCollectionNode(AuthenticationProviderDataCollection authenticationProviderDataCollection) : base()
        {
            if (authenticationProviderDataCollection == null)
            {
                throw new ArgumentNullException("authenticationProviderDataCollection");
            }
            this.authenticationProviderDataCollection = authenticationProviderDataCollection;
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
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual AuthenticationProviderDataCollection AuthenticationProviderDataCollection
        {
            get
            {
                authenticationProviderDataCollection.Clear();
                foreach (AuthenticationProviderNode node in this.Nodes)
                {
                    authenticationProviderDataCollection[node.AuthenticationProviderData.Name] = node.AuthenticationProviderData;
                }
                return authenticationProviderDataCollection;
            }
        }

        /// <summary>
        /// <para>Sets the name of the node and creates the child <see cref="AuthenticationProviderNode"/> objects based on the configuration data provided.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.AuthenticationProviderCollectionNodeName;
            CreateDynamicNodes(authenticationProviderDataCollection);
        }

        /// <summary>
        /// <para>Adds a <see cref="ValidateNodeCommand"/> menu and menu items for creating derived <see cref="AuthenticationProviderNode"/> objects.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            ConfigurationMenuItem.CreateValidateNodeCommand(Site, this);
            CreateDynamicMenuItems(typeof(AuthenticationProviderNode));
        }
    }
}