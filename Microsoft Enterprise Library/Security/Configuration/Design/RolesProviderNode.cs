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
    /// Represents the base design node for all role providers.
    /// </summary>
    [Image(typeof(RolesProviderNode))]
    public abstract class RolesProviderNode : ConfigurationNode
    {
        private RolesProviderData rolesProviderData;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="RolesProviderNode"/> class from
        /// the specified display name and <see cref="RolesProviderData"/>.
        /// </summary>
        /// <param name="rolesProviderData">An <see cref="RolesProviderData"/>.</param>
        protected RolesProviderNode(RolesProviderData rolesProviderData) : base( /*SR.RolesProviderNodeName*/)
        {
            if (rolesProviderData == null)
            {
                throw new ArgumentNullException("rolesProviderData");
            }
            this.rolesProviderData = rolesProviderData;
        }

        /// <summary>
        /// Gets or sets the type name of a class that implements
        /// <see cref="IRolesProvider"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IRolesProvider))]
        [Required]
        [SRDescription(SR.Keys.ProviderTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return this.rolesProviderData.TypeName; }
            set { this.rolesProviderData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual RolesProviderData RolesProviderData
        {
            get { return this.rolesProviderData; }
        }

        /// <summary>
        /// <para>Sets the name for the node based on the configuration name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = rolesProviderData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the configuration data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            rolesProviderData.Name = e.Node.Name;
        }

    }
}