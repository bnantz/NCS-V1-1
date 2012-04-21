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
    /// Represents the base design node for all security cache providers.
    /// </summary>
    [Image(typeof(SecurityCacheProviderNode))]
    public abstract class SecurityCacheProviderNode : ConfigurationNode
    {
        private SecurityCacheProviderData securityCacheProviderData;

        /// <summary>
        /// Initializes with configuration data and display name.
        /// </summary>
        /// <param name="securityCacheProviderData">The configuration data.</param>
        protected SecurityCacheProviderNode(SecurityCacheProviderData securityCacheProviderData) : base()
        {
            if (securityCacheProviderData == null)
            {
                throw new ArgumentNullException("securityCacheProviderData");
            }
            this.securityCacheProviderData = securityCacheProviderData;
        }

        /// <summary>
        /// Gets or sets the type name of a class that implements
        /// <see cref="ISecurityCacheProvider"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ISecurityCacheProvider))]
        [Required]
        [SRDescription(SR.Keys.ProviderTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return this.securityCacheProviderData.TypeName; }
            set { this.securityCacheProviderData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual SecurityCacheProviderData SecurityCacheProviderData
        {
            get { return this.securityCacheProviderData; }
        }

        /// <summary>
        /// <para>Sets the name for the node based on the configuration name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = securityCacheProviderData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the configuration data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            securityCacheProviderData.Name = e.Node.Name;
        }

    }
}