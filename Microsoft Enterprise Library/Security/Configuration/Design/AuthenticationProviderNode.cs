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
    [Image(typeof(AuthenticationProviderNode))]
    public abstract class AuthenticationProviderNode : ConfigurationNode
    {
        private AuthenticationProviderData authenticationProviderData;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthenticationProviderNode"/> class from
        /// the specified display name and <see cref="AuthenticationProviderData"/>.
        /// </summary>
        /// <param name="authenticationProviderData">An <see cref="AuthenticationProviderData"/>.</param>
        protected AuthenticationProviderNode(AuthenticationProviderData authenticationProviderData) : base( /*SR.AuthenticationProviderNodeName*/)
        {
            if (authenticationProviderData == null)
            {
                throw new ArgumentNullException("data");
            }
            this.authenticationProviderData = authenticationProviderData;
        }

        /// <summary>
        /// Gets or sets the type name of a class that implements
        /// <see cref="IAuthenticationProvider"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IAuthenticationProvider))]
        [Required]
        [SRDescription(SR.Keys.ProviderTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return this.authenticationProviderData.TypeName; }
            set { this.authenticationProviderData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual AuthenticationProviderData AuthenticationProviderData
        {
            get { return this.authenticationProviderData; }
        }

        /// <summary>
        /// <para>Sets the name for the <see cref="AuthenticationProviderNode"/></para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = authenticationProviderData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the provider.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            authenticationProviderData.Name = e.Node.Name;
        }

    }
}