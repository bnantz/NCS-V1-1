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
    /// The design time representation of the <see cref="CustomProfileProviderData"/>.
    /// </summary>
    public class CustomProfileProviderNode : ProfileProviderNode
    {
        private CustomProfileProviderData customProfileProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomProfileProviderNode"/> class.
        /// </summary>
        public CustomProfileProviderNode() : this(new CustomProfileProviderData(SR.CustomProfileProviderCommandName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomProfileProviderNode"/> class from existing configuration data.
        /// </summary>
        /// <param name="customProfileProviderData">Configuration data for a generic profile provider</param>
        public CustomProfileProviderNode(CustomProfileProviderData customProfileProviderData) : base(customProfileProviderData)
        {
            this.customProfileProviderData = customProfileProviderData;
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
            get { return this.customProfileProviderData.Extensions; }
        }
    }
}