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
    /// <see cref="CustomAuthorizationProviderData"/>.
    /// </summary>
    public class CustomAuthorizationProviderNode : AuthorizationProviderNode
    {
        private CustomAuthorizationProviderData customAuthorizationProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthorizationProviderNode"/>
        /// class.
        /// </summary>
        public CustomAuthorizationProviderNode() : this(new CustomAuthorizationProviderData(SR.CustomAuthorizationProviderCommandName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthorizationProviderNode"/>
        /// class from existing configuration data.
        /// </summary>
        /// <param name="customAuthorizationProviderData">Configuration data for a generic authentication provider</param>
        public CustomAuthorizationProviderNode(CustomAuthorizationProviderData customAuthorizationProviderData) : base(customAuthorizationProviderData)
        {
            this.customAuthorizationProviderData = customAuthorizationProviderData;
        }

        /// <summary>
        /// Gets or sets a list of name-value pairs.
        /// </summary>
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Extensions
        {
            get { return this.customAuthorizationProviderData.Extensions; }
        }
    }
}