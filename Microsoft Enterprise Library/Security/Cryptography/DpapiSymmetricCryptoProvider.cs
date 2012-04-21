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
    /// <para>A symmetric provider for the Data Protection API (DPAPI).</para>
    /// </summary>
    public class DpapiSymmetricCryptoProvider : ConfigurationProvider, ISymmetricCryptoProvider
    {
        private CryptographyConfigurationView cryptoConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DpapiSymmetricCryptoProvider"/></para>
        /// </summary>
        public DpapiSymmetricCryptoProvider()
        {
        }


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

        internal DpapiCryptographer DpapiCrypto
        {
            get
            {
                DpapiSymmetricCryptoProviderData data = GetDpapiSymmetricCryptoProviderDataFromCursor();

                return new DpapiCryptographer(data.DataProtectionMode.Mode);
            }
        }

        /// <summary>
        /// <para>Encrypts a secret using DPAPI.</para>
        /// </summary>
        /// <param name="plaintext"><para>The input for which you want to encrypt.</para></param>
        /// <returns><para>The resulting cipher text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Encrypt"/>
        public byte[] Encrypt(byte[] plaintext)
        {
            DpapiSymmetricCryptoProviderData data = GetDpapiSymmetricCryptoProviderDataFromCursor();

            byte[] result = DpapiCrypto.Encrypt(plaintext, data.DataProtectionMode.Entropy);
            SecurityCryptoSymmetricEncryptionEvent.Fire(string.Empty);
            return result;
        }

        /// <summary>
        /// <para>Decrypts cipher text using DPAPI.</para>
        /// </summary>
        /// <param name="cipherText"><para>The cipher text for which you want to decrypt.</para></param>
        /// <returns><para>The resulting plain text.</para></returns>
        /// <seealso cref="ISymmetricCryptoProvider.Decrypt"/>
        public byte[] Decrypt(byte[] cipherText)
        {
            DpapiSymmetricCryptoProviderData data = GetDpapiSymmetricCryptoProviderDataFromCursor();

            byte[] result = DpapiCrypto.Decrypt(cipherText, data.DataProtectionMode.Entropy);
            SecurityCryptoSymmetricDecryptionEvent.Fire(string.Empty);
            return result;
        }

        private DpapiSymmetricCryptoProviderData GetDpapiSymmetricCryptoProviderDataFromCursor()
        {
            SymmetricCryptoProviderData symmetricCryptoProviderData = cryptoConfigurationView.GetSymmetricCryptoProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(symmetricCryptoProviderData, typeof(DpapiSymmetricCryptoProviderData));

            return (DpapiSymmetricCryptoProviderData)symmetricCryptoProviderData;
        }
    }
}