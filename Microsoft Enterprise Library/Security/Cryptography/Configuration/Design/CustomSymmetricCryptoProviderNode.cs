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
    /// Provides designtime configuration for <see cref="CustomSymmetricCryptoProviderData"/>.
    /// </summary>
    public class CustomSymmetricCryptoProviderNode : SymmetricCryptoProviderNode
    {
        private CustomSymmetricCryptoProviderData customSymmetricCryptoProviderData;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CustomSymmetricCryptoProviderNode() : this(new CustomSymmetricCryptoProviderData(SR.CustomSymmetricCryptoProviderNodeName, string.Empty))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="customSymmetricCryptoProviderData">The corresponding runtime configuration data.</param>
        public CustomSymmetricCryptoProviderNode(CustomSymmetricCryptoProviderData customSymmetricCryptoProviderData) : base(customSymmetricCryptoProviderData)
        {
            this.customSymmetricCryptoProviderData = customSymmetricCryptoProviderData;
        }

        /// <summary>
        /// See <see cref="CustomSymmetricCryptoProviderData"/>.
        /// </summary>
        [SRDescription(SR.Keys.CustomSymmetricCryptoProviderExtensionsDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Extensions
        {
            get { return customSymmetricCryptoProviderData.Extensions; }
        }

        /// <summary>
        /// Gets or sets fully qualified assembly name of the <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.CustomSymmetricCryptoProviderTypeNameDescription)]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ISymmetricCryptoProvider))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string TypeName
        {
            get { return customSymmetricCryptoProviderData.TypeName; }
            set { customSymmetricCryptoProviderData.TypeName = value; }
        }
    }
}