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
    /// <para>Configuration settings for the <c>KeyedHashAlgorithm</c> hash provider.</para>
    /// </summary>
    [XmlRoot("hashProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class KeyedHashAlgorithmProviderData : HashAlgorithmProviderData
    {
        private byte[] key;

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class.</para>
        /// </summary>
        public KeyedHashAlgorithmProviderData()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name"><para>The name for the provider.</para></param>
        public KeyedHashAlgorithmProviderData(string name) : base(name)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class with a name, algorithm type, and if salt should be added.</para>
        /// </summary>
        /// <param name="name"><para>The name for the provider.</para></param>
        /// <param name="algorithmType"><para>The type name of the hash algorithm.</para></param>
        /// <param name="saltEnabled"><para>Determines if salt should be used during encryption.</para></param>
        public KeyedHashAlgorithmProviderData(string name, string algorithmType, bool saltEnabled)
            : base(name, algorithmType, saltEnabled)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class with a name, algorithm type, if salt should be added, and the key.</para>
        /// </summary>
        /// <param name="name"><para>The name for the provider.</para></param>
        /// <param name="algorithmType"><para>The type name of the hash algorithm.</para></param>
        /// <param name="saltEnabled"><para>Determines if salt should be used during encryption.</para></param>
        /// <param name="key"><para>The key to use for the hash provider.</para></param>
        public KeyedHashAlgorithmProviderData(string name, string algorithmType, bool saltEnabled, byte[] key)
            : base(name, algorithmType, saltEnabled)
        {
            this.key = key;
        }

        /// <summary>
        /// <para>Gets or sets the key for the hash algorithm.</para>
        /// </summary>
        /// <value>
        /// <para>The key for the hash algorithm.</para>
        /// </value>
        [XmlAttribute("key")]
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// <para>Gets the assembly qualified name for a <see cref="KeyedHashAlgorithmProvider"/>.</para>
        /// </summary>
        /// <value>
        /// <para>Alwasy returns the <see cref="Type.AssemblyQualifiedName"/> of <see cref="KeyedHashAlgorithmProvider"/>.</para>
        /// </value>
        public override string TypeName
        {
            get { return typeof(KeyedHashAlgorithmProvider).AssemblyQualifiedName; }
        }

    }
}