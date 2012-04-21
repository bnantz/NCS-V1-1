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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents the base design node for all authentication providers.
    /// </summary>
    [Image(typeof(AuthorizationProviderNode))]
    public abstract class AuthorizationProviderNode : ConfigurationNode
    {
        private AuthorizationProviderData authorizationProviderData;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationProviderNode"/> class from
        /// the specified display name and <see cref="AuthorizationProviderData"/>.
        /// </summary>
        /// <param name="authorizationProviderData">An <see cref="AuthorizationProviderData"/>.</param>
        protected AuthorizationProviderNode(AuthorizationProviderData authorizationProviderData) : base()
        {
            if (authorizationProviderData == null)
            {
                throw new ArgumentNullException("data");
            }
            this.authorizationProviderData = authorizationProviderData;
        }

        /// <summary>
        /// Gets or sets the type name of a class that implements
        /// <see cref="IAuthorizationProvider"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IAuthorizationProvider))]
        [Required]
        [SRDescription(SR.Keys.ProviderTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [TypeValidation]
        public virtual string TypeName
        {
            get { return this.authorizationProviderData.TypeName; }
            set { this.authorizationProviderData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual AuthorizationProviderData AuthorizationProviderData
        {
            get { return this.authorizationProviderData; }
        }

        /// <summary>
        /// <para>Sets the name for the node based on the configuration name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = authorizationProviderData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the configuration data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            authorizationProviderData.Name = e.Node.Name;
        }
    }
}