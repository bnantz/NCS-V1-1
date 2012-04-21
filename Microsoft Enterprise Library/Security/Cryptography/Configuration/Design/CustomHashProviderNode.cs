//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="CustomHashProviderData"/>.
    /// </summary>
    public class CustomHashProviderNode : HashProviderNode
    {
        private CustomHashProviderData customHashProviderData;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CustomHashProviderNode() : this(new CustomHashProviderData(SR.CustomHashProviderNodeName, string.Empty))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="customHashProviderData">The corresponding runtime configuration data.</param>
        public CustomHashProviderNode(CustomHashProviderData customHashProviderData) : base(customHashProviderData)
        {
            this.customHashProviderData = customHashProviderData;
        }

        /// <summary>
        /// Gets or sets fully qualified assembly name of the <see cref="IHashProvider"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IHashProvider))]
        [SRDescription(SR.Keys.HashProviderTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string TypeName
        {
            get { return customHashProviderData.TypeName; }
            set { customHashProviderData.TypeName = value; }
        }

        /// <summary>
        /// See <see cref="CustomHashProviderData.Extensions"/>.
        /// </summary>
        [SRDescription(SR.Keys.CustomHashProviderNodeExtensionsDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Extensions
        {
            get { return customHashProviderData.Extensions; }
        }
    }
}