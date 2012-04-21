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

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>A symmetric provider for any symmetric algorithm which derives from <see cref="System.Security.Cryptography.SymmetricAlgorithm"/>.</para>
    /// </summary>
    public class SymmetricAlgorithmProvider : ConfigurationProvider, ISymmetricCryptoProvider
    {
        private CryptographyConfigurationView cryptoConfigurationView;

        /// <summary>
        /// <para>Initializes the provider with a <see cref="CryptographyConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="CryptographyConfigurationView"/> object.</para>
        /// </param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(CryptographyConfigurationView));

            this.cryptoConfigurationView = (CryptographyConfigurationView)configurationView;
        }

        /// <summary>
        /// <para>Encrypts a secret using the configured <c>SymmetricAlgorithm</c>.</para>
        /// </summary>
        /// <param name="plaintext"><para>The input for which you want to encrypt.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Encrypt"/>
        public byte[] Encrypt(byte[] plaintext)
        {
            ArgumentValidation.CheckForNullReference(plaintext, "plaintext");
            ArgumentValidation.CheckForZeroBytes(plaintext, "plaintext");

            byte[] output = null;

            SymmetricAlgorithmProviderData data = GetSymmetricAlgorithmProviderDataFromCursor();

            SymmetricCryptographer crypto = new SymmetricCryptographer(data.AlgorithmType, data.Key);
            output = crypto.Encrypt(plaintext);
            SecurityCryptoSymmetricEncryptionEvent.Fire(string.Empty);
            return output;
        }

        /// <summary>
        /// Decrypts a secret using the configured <c>SymmetricAlgorithm</c>.
        /// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
        /// </summary>
        /// <param name="ciphertext"><para>The cipher text for which you want to decrypt.</para></param>
        /// <returns><para>The resulting plain text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
        public byte[] Decrypt(byte[] ciphertext)
        {
            ArgumentValidation.CheckForNullReference(ciphertext, "encryptedText");
            ArgumentValidation.CheckForZeroBytes(ciphertext, "encryptedText");

            byte[] output = null;

            SymmetricAlgorithmProviderData data = GetSymmetricAlgorithmProviderDataFromCursor();

            SymmetricCryptographer crypto = new SymmetricCryptographer(data.AlgorithmType, data.Key);
            output = crypto.Decrypt(ciphertext);
            SecurityCryptoSymmetricDecryptionEvent.Fire(string.Empty);
            return output;
        }

        private SymmetricAlgorithmProviderData GetSymmetricAlgorithmProviderDataFromCursor()
        {
            SymmetricCryptoProviderData symmetricCryptoProviderData = cryptoConfigurationView.GetSymmetricCryptoProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(symmetricCryptoProviderData, typeof(SymmetricAlgorithmProviderData));
            return (SymmetricAlgorithmProviderData)symmetricCryptoProviderData;
        }

    }
}