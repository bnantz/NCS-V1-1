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
    /// Provides designtime configuration for <see cref="SymmetricAlgorithmProviderData"/>.
    /// </summary>
    public class SymmetricAlgorithmProviderNode : SymmetricCryptoProviderNode
    {
        private SymmetricAlgorithmProviderData symmetricAlgorithmProviderData;
        private KeySettings keySettings;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public SymmetricAlgorithmProviderNode() : this(new SymmetricAlgorithmProviderData(SR.SymmetricAlgorithmProviderNodeName))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="symmetricAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public SymmetricAlgorithmProviderNode(SymmetricAlgorithmProviderData symmetricAlgorithmProviderData) : base(symmetricAlgorithmProviderData)
        {
            this.symmetricAlgorithmProviderData = symmetricAlgorithmProviderData;
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public override SymmetricCryptoProviderData SymmetricCryptoProviderData
        {
            get
            {
                if (this.symmetricAlgorithmProviderData.Key == null || this.symmetricAlgorithmProviderData.Key.Length == 0)
                {
                    this.symmetricAlgorithmProviderData.Key = Key.KeyCreator.GenerateKey();
                }
                return base.SymmetricCryptoProviderData;
            }
        }

        /// <summary>
        /// Gets or sets the key for the <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.
        /// </summary>
        [Editor(typeof(KeyManagerEditor), typeof(UITypeEditor))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public KeySettings Key
        {
            get
            {
                this.keySettings = new KeySettings(new SymmetricAlgorithmKeyCreator(AlgorithmType));
                this.keySettings.Key = this.symmetricAlgorithmProviderData.Key;
                return this.keySettings;
            }
            set
            {
                this.keySettings = value;
                this.symmetricAlgorithmProviderData.Key = this.keySettings.Key;
            }
        }

        /// <summary>
        /// Gets or sets the assembly qualified name of the <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.
        /// </summary>
        [ReadOnly(true)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string AlgorithmType
        {
            get { return this.symmetricAlgorithmProviderData.AlgorithmType; }
            set { this.symmetricAlgorithmProviderData.AlgorithmType = value; }
        }
    }
}