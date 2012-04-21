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

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography
{
    /// <summary>
    /// Implementation of Symmetric Storage Encryption used by the Caching block
    /// </summary>
    public class SymmetricStorageEncryptionProvider : ConfigurationProvider, IStorageEncryptionProvider
    {
        private CachingConfigurationView cachingConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SymmetricAlgorithmProvider"/> class.</para>
        /// </summary>
        public SymmetricStorageEncryptionProvider()
        {
        }

        /// <summary>
        /// Initializes this provider with the correct Symmetric Cryptographic Provider Factory
        /// to create the correct cryptographic provider to encrypt or decrypt the data stored
        /// in the cache.
        /// </summary>
        /// <param name="configurationView">A <see cref="CachingConfigurationView"></see> object</param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (CachingConfigurationView));

            cachingConfigurationView = (CachingConfigurationView) configurationView;
        }

        /// <summary>
        /// Encrypts the data passed to this method according to the correct symmetric cryptographic
        /// algorithm as defined in configuration
        /// </summary>
        /// <param name="plaintext">Data to be encrypted</param>
        /// <returns>Encrypted data</returns>
        public byte[] Encrypt(byte[] plaintext)
        {
            ISymmetricCryptoProvider provider = CreateProvider();
            return provider.Encrypt(plaintext);
        }

        /// <summary>
        /// Decrypts the data passed to this method according to the correct symmetric cryptographic
        /// algoritm as defined in configuration
        /// </summary>
        /// <param name="ciphertext">Encrypted data to be decrypted</param>
        /// <returns>Decrypted data</returns>
        public byte[] Decrypt(byte[] ciphertext)
        {
            ISymmetricCryptoProvider provider = CreateProvider();
            return provider.Decrypt(ciphertext);
        }

        private ISymmetricCryptoProvider CreateProvider()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(cachingConfigurationView.ConfigurationContext);
            SymmetricStorageEncryptionProviderData data = (SymmetricStorageEncryptionProviderData) cachingConfigurationView.GetStorageEncryptionProviderData(ConfigurationName);
            return factory.CreateSymmetricCryptoProvider(data.SymmetricInstance);
        }
    }
}