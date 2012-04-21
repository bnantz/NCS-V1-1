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

using System;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration data for the <c>SymmetricAlgorithm</c> provider.</para>
    /// </summary>
    [XmlRoot("symmetricCryptoProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class SymmetricAlgorithmProviderData : SymmetricCryptoProviderData
    {
        private string algorithmType;
        private byte[] key;

        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricAlgorithmProviderData"/> class.</para>
        /// </summary>
        public SymmetricAlgorithmProviderData()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricAlgorithmProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name"><para>The name to use for the provider.</para></param>
        public SymmetricAlgorithmProviderData(string name) : this(name, string.Empty, null)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricAlgorithmProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name"><para>The name to use for the provider.</para></param>
        /// <param name="algorithmType"><para>The hash algorithm to use.</para></param>
        /// <param name="key"><para>The key to use with the given encryption provider.</para></param>
        public SymmetricAlgorithmProviderData(string name, string algorithmType, byte[] key) : base(name)
        {
            this.algorithmType = algorithmType;
            this.key = key;
        }

        /// <summary>
        /// <para>Gets or sets the type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The type of <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
        /// </value>
        [XmlAttribute("algorithmType")]
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        /// <summary>
        /// <para>Gets the assembly qualified name of this provider.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type.AssemblyQualifiedName"/> for the <see cref="SymmetricAlgorithmProvider"/>.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(SymmetricAlgorithmProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// <para>Gets or sets the key for this provider.</para>
        /// </summary>
        /// <value>
        /// <para>The key for this provider.</para>
        /// </value>
        [XmlAttribute("key")]
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}