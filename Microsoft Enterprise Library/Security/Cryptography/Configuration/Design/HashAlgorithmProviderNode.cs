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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="HashAlgorithmProviderData"/>.
    /// </summary>
    public class HashAlgorithmProviderNode : HashProviderNode
    {
        private HashAlgorithmProviderData hashAlgorithmProviderData;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashAlgorithmProviderNode() : this(new HashAlgorithmProviderData(SR.HashAlgorithmProviderNodeName, string.Empty, true))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="hashAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public HashAlgorithmProviderNode(HashAlgorithmProviderData hashAlgorithmProviderData) : base(hashAlgorithmProviderData)
        {
            this.hashAlgorithmProviderData = hashAlgorithmProviderData;
        }

        [Browsable(false)]
        public override HashProviderData HashProviderData
        {
            get
            {
                this.hashAlgorithmProviderData.SaltEnabled = this.SaltEnabled;
                return base.HashProviderData;
            }
        }

        /// <summary>
        /// Gets or sets the assembly qualified name of the <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        [ReadOnly(true)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string AlgorithmType
        {
            get { return hashAlgorithmProviderData.AlgorithmType; }
            set { hashAlgorithmProviderData.AlgorithmType = value; }
        }

        /// <summary>
        /// Gets or sets the salt enabled flag.
        /// </summary>
        [SRCategory(SR.Keys.CategoryGeneral)]
        public bool SaltEnabled
        {
            get { return hashAlgorithmProviderData.SaltEnabled; }
            set { hashAlgorithmProviderData.SaltEnabled = value; }
        }
    }
}