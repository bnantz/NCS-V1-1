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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="KeyedHashAlgorithmProviderData"/>.
    /// </summary>
    public class KeyedHashAlgorithmProviderNode : HashAlgorithmProviderNode
    {
        private KeyedHashAlgorithmProviderData hashAlgorithmProviderData;
        private KeySettings keySettings;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public KeyedHashAlgorithmProviderNode() : this(new KeyedHashAlgorithmProviderData(SR.HashAlgorithmProviderNodeName, string.Empty, true))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="hashAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public KeyedHashAlgorithmProviderNode(KeyedHashAlgorithmProviderData hashAlgorithmProviderData) : base(hashAlgorithmProviderData)
        {
            this.hashAlgorithmProviderData = hashAlgorithmProviderData;
        }

        /// <summary>
        /// Gets or sets the hash key.
        /// </summary>
        [Editor(typeof(KeyManagerEditor), typeof(UITypeEditor))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public KeySettings Key
        {
            get
            {
                this.keySettings = new KeySettings(new KeyedHashAlgorithmKeyCreator(AlgorithmType));
                this.keySettings.Key = this.hashAlgorithmProviderData.Key;
                return this.keySettings;
            }
            set
            {
                this.keySettings = value;
                this.hashAlgorithmProviderData.Key = this.keySettings.Key;
            }
        }
    }
}