//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration
{
    /// <summary>
    /// Configuration data for Symmetric Storage Encryption
    /// </summary>
    [XmlRoot("storageEncryption", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class SymmetricStorageEncryptionProviderData : StorageEncryptionProviderData
    {
        private string symmetricInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SymmetricStorageEncryptionProviderData()
        {
        }

        /// <summary>
        /// Create provider with a specified name.
        /// </summary>
        /// <param name="name">The configured name of the provider.</param>
        public SymmetricStorageEncryptionProviderData(string name) : base(name)
        {
        }

        /// <summary>
        /// Create provider with a specified name and symmetric instance.
        /// </summary>
        /// <param name="name">The configured name of the provider</param>
        /// <param name="symmetricInstance">The full name of a <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</param>
        public SymmetricStorageEncryptionProviderData(string name, string symmetricInstance) : base(name)
        {
            this.symmetricInstance = symmetricInstance;
        }

        /// <summary>
        /// Name of symmetric instance
        /// </summary>
        [XmlAttribute("symmetricInstance")]
        public string SymmetricInstance
        {
            get { return symmetricInstance; }
            set { symmetricInstance = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name of this provider.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(SymmetricStorageEncryptionProvider).AssemblyQualifiedName; }

            set
            {
            }
        }
    }
}