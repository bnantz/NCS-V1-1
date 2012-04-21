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
    /// <see cref="CustomSecurityCacheProviderData"/>.
    /// </summary>
    public class CustomSecurityCacheProviderNode : SecurityCacheProviderNode
    {
        private CustomSecurityCacheProviderData customSecurityCacheProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSecurityCacheProviderNode"/>
        /// class.
        /// </summary>
        public CustomSecurityCacheProviderNode() : this(new CustomSecurityCacheProviderData(SR.CustomSecurityCacheNodeDefaultName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSecurityCacheProviderNode"/>
        /// class from existing configuration data.
        /// </summary>
        /// <param name="customSecurityCacheProviderData">Configuration data for a generic security cache provider</param>
        public CustomSecurityCacheProviderNode(CustomSecurityCacheProviderData customSecurityCacheProviderData) : base(customSecurityCacheProviderData)
        {
            this.customSecurityCacheProviderData = customSecurityCacheProviderData;
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
            get { return this.customSecurityCacheProviderData.Extensions; }
        }
    }
}