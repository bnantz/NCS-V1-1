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

using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the 
    /// <see cref="CustomRolesProviderData"/>.
    /// </summary>
    public class CustomRolesProviderNode : RolesProviderNode
    {
        private CustomRolesProviderData customRolesProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRolesProviderNode"/>
        /// class.
        /// </summary>
        public CustomRolesProviderNode() : this(new CustomRolesProviderData(SR.CustomRolesProviderCommandName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRolesProviderNode"/>
        /// class from existing configuration data.
        /// </summary>
        /// <param name="data">Configuration data for a generic Roles provider</param>
        public CustomRolesProviderNode(CustomRolesProviderData data) : base(data)
        {
            this.customRolesProviderData = data;
        }

        /// <summary>
        /// <para>Gets the set of custom attributes for the node.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="NameValueItemCollection"/> representing the custom attributes.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Extensions
        {
            get { return this.customRolesProviderData.Extensions; }
        }
    }
}