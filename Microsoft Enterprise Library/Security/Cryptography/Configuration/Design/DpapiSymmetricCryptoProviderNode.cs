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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="DpapiSymmetricCryptoProviderData"/>.
    /// </summary>
    public class DpapiSymmetricCryptoProviderNode : SymmetricCryptoProviderNode
    {
        private DpapiSymmetricCryptoProviderData data;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public DpapiSymmetricCryptoProviderNode() : this(new DpapiSymmetricCryptoProviderData(SR.DpapiSymmetricCryptoProviderNodeName))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="data">The corresponding runtime configuration data.</param>
        public DpapiSymmetricCryptoProviderNode(DpapiSymmetricCryptoProviderData data) : base(data)
        {
            this.data = data;
        }


        /// <summary>
        /// The key protector settings, mode and entropy.
        /// </summary>
        [Required]
        [Editor(typeof(DpapiSettingsEditor), typeof(UITypeEditor))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public DpapiSettings DataProtectionMode
        {
            get { return new DpapiSettings(data.DataProtectionMode); }
            set
            {
                if (this.data.DataProtectionMode == null)
                {
                    this.data.DataProtectionMode = new DpapiSettingsData();
                }
                data.DataProtectionMode.Mode = value.Mode;
                data.DataProtectionMode.Entropy = value.Entropy;
            }
        }
    }
}