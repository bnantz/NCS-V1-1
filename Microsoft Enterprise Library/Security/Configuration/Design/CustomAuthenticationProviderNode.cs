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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the 
    /// <see cref="CustomAuthenticationProviderData"/>.
    /// </summary>
    public class CustomAuthenticationProviderNode : AuthenticationProviderNode
    {
        private CustomAuthenticationProviderData customAuthenticationProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthenticationProviderNode"/> class.
        /// </summary>
        public CustomAuthenticationProviderNode() : this(new CustomAuthenticationProviderData(SR.CustomAuthenticationProviderCommandName, String.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthenticationProviderNode"/>
        /// class from existing configuration data.
        /// </summary>
        /// <param name="customAuthenticationProviderData">Configuration data for a generic authentication provider</param>
        public CustomAuthenticationProviderNode(CustomAuthenticationProviderData customAuthenticationProviderData) : base(customAuthenticationProviderData)
        {
            this.customAuthenticationProviderData = customAuthenticationProviderData;
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
            get { return this.customAuthenticationProviderData.Extensions; }
        }
    }
}