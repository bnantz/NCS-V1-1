//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Provides functionality to encrypt and decrypt configuraiton data with symetric algorithms defined in the <see cref="KeyAlgorithmPair"/>.</para>
    /// </summary>
    public class ConfigurationProtector : IDisposable
    {
        private KeyAlgorithmPair keyAlgorithmPair;
        private bool encrypted = false;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationProtector"/> class with a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        public ConfigurationProtector()
        {
        }

        /// <summary>
        /// <para>Allows an the <see cref="ConfigurationProtector"/> to attempt to free resources and perform other cleanup operations before the <see cref="ConfigurationProtector"/> is reclaimed by garbage collection. </para>
        /// </summary>
        ~ConfigurationProtector()
        {
            Dispose(false);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (keyAlgorithmPair != null)
                {
                    keyAlgorithmPair.Dispose();
                }

            }
        }

        /// <summary>
        /// <para>Load the configured protection mechanism for the section. This is the <see cref="KeyAlgorithmPairStorageProviderData"/> to load the <see cref="KeyAlgorithmPair"/>.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to encrypt.</para>
        /// </param>
        /// <param name="context">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="context"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured in configuration.</para>
        /// </exception>
        public void Load(ConfigurationContext context, string sectionName)
        {
            ArgumentValidation.CheckForNullReference(context, "context");
            ArgumentValidation.CheckForNullReference(sectionName, "sectionName");
            ArgumentValidation.CheckForEmptyString(sectionName, "sectionName");

            ConfigurationSettings settings = context.GetMetaConfiguration();
            ConfigurationSectionData data = settings.ConfigurationSections[sectionName];
            if (null == data)
            {
                InvalidSectionExceptionBuilder builder = new InvalidSectionExceptionBuilder(sectionName, context.ConfigurationFile);
                throw builder.ThrowException();
            }

            if ((null == settings.KeyAlgorithmPairStorageProviderData) && (data.Encrypt))
            {
                throw new ConfigurationException(SR.ExceptionNoKeyAlgorithmStorageProvider);
            }

            if (data.Encrypt)
            {
                SetDataToBeEncrypted(context);
            }
        }

        private void SetDataToBeEncrypted(ConfigurationContext context)
        {
            KeyAlgorithmStorageProviderFactory factory = new KeyAlgorithmStorageProviderFactory(context);
            IKeyAlgorithmPairStorageProvider provider = factory.Create();
            keyAlgorithmPair = provider.Load();
            if (null == keyAlgorithmPair)
            {
                throw new InvalidOperationException(SR.ExceptionKeyAlgorithmPairLoad);
            }

            encrypted = true;
        }

        /// <summary>
        /// <para>Determines if the data is to be encrypted or not.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the data is to be encrypted; otherwise <see langword="false"/>.</para>
        /// </value>
        public bool Encrypted
        {
            get { return encrypted; }
        }

        /// <summary>
        /// <para>Encrypts bytes with the initialized algorithm and key.</para>
        /// </summary>
        /// <param name="plaintext"><para>The plaintext in which you wish to encrypt.</para></param>
        /// <returns><para>The resulting ciphertext.</para></returns>
        /// <remarks>
        /// <para>If no encryption is defined, the bytes passed in are returned.</para>
        /// </remarks>
        public byte[] Encrypt(byte[] plaintext)
        {
            if (encrypted)
            {
                SymmetricCryptographer crypt = new SymmetricCryptographer(keyAlgorithmPair.AlgorithmTypeName, keyAlgorithmPair.Key);
                return crypt.Encrypt(plaintext);
            }
            return plaintext;
        }

        /// <summary>
        /// <para>Decrypts bytes with the initialized algorithm and key.</para>
        /// </summary>
        /// <param name="ciphertext"><para>The ciphertext in which you wish to decrypt.</para></param>
        /// <returns><para>The resulting plaintext.</para></returns>
        /// <remarks>
        /// <para>If no encryption is defined, the bytes passed in are returned.</para>
        /// </remarks>
        public byte[] Decrypt(byte[] ciphertext)
        {
            if (encrypted)
            {
                SymmetricCryptographer crypt = new SymmetricCryptographer(keyAlgorithmPair.AlgorithmTypeName, keyAlgorithmPair.Key);
                return crypt.Decrypt(ciphertext);
            }
            return ciphertext;
        }

    }
}