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
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// A hash provider for any hash algorithm which derives from <see cref="System.Security.Cryptography.HashAlgorithm"/>.
    /// </summary>
    public class HashAlgorithmProvider : ConfigurationProvider, IHashProvider
	{
		/// <summary>
		/// Returns the salt length used by the provider.
		/// </summary>
        public const int SaltLength = 16;

		/// <summary>
		/// Contains the configuration settings for this instance.
		/// </summary>
        private CryptographyConfigurationView cryptoConfigurationView;

        /// <summary>
        /// Gets and sets the <see cref="CryptographyConfigurationView"></see> in a derived class
        /// </summary>
        protected CryptographyConfigurationView CryptoConfigurationView
        {
            get { return cryptoConfigurationView; }
            set { cryptoConfigurationView = value; }
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

        /// <summary>
        /// Computes a hash using the configured <c>HashAlgorithm</c>.
        /// <seealso cref="IHashProvider.CreateHash"/>
        /// </summary>
        /// <param name="plaintext"><seealso cref="IHashProvider.CreateHash"/></param>
        /// <returns><seealso cref="IHashProvider.CreateHash"/></returns>
        public byte[] CreateHash(byte[] plaintext)
        {
			byte[] hash = CreateHashWithSalt(plaintext, null);
			SecurityCryptoHashCreateEvent.Fire(string.Empty);
			return hash;
        }

        /// <summary>
        /// Compares plain text input with a computed hash using the configured <c>HashAlgorithm</c>.
        /// <seealso cref="IHashProvider.CompareHash"/>
        /// </summary>
        /// <param name="plaintext"><seealso cref="IHashProvider.CompareHash"/></param>
        /// <param name="hashedtext"><seealso cref="IHashProvider.CompareHash"/></param>
        /// <returns><seealso cref="IHashProvider.CompareHash"/></returns>
        public bool CompareHash(byte[] plaintext, byte[] hashedtext)
        {
            ArgumentValidation.CheckForNullReference(plaintext, "plainText");
            ArgumentValidation.CheckForNullReference(hashedtext, "hashedText");
            ArgumentValidation.CheckForZeroBytes(hashedtext, "hashedText");

			bool result = false;
            byte[] hashedPlainText = null;
			byte[] salt = null;
            try
			{
				salt = ExtractSalt(hashedtext);
                hashedPlainText = CreateHashWithSalt(plaintext, salt);
			}
            finally
            {
                CryptographyUtility.ZeroOutBytes(salt);
            }
			result = CryptographyUtility.CompareBytes(hashedPlainText, hashedtext);
            SecurityCryptoHashCheckEvent.Fire(string.Empty);
            if (!result)
            {
                SecurityCryptoHashCheckFailureEvent.Fire(string.Empty);
            }
            return result;
        }
		
        /// <summary>
        /// Creates a hash with a specified salt.
        /// </summary>
        /// <param name="plaintext">The plaintext to hash.</param>
        /// <param name="salt">The hash salt.</param>
        /// <returns>The computed hash.</returns>
		protected virtual byte[] CreateHashWithSalt(byte[] plaintext, byte[] salt)
		{
            AddSaltToPlainText(ref salt, ref plaintext);
            HashCryptographer cryptographer = GetHashCryptographer();
            byte[] hash = cryptographer.ComputeHash(plaintext);
            AddSaltToHash(salt, ref hash);
			return hash;
		}

        /// <summary>
        /// Extracts the salt from the hashedText.
        /// </summary>
        /// <param name="hashedtext">The hash in which to extract the salt.</param>
        /// <returns>The extracted salt.</returns>
        protected byte[] ExtractSalt(byte[] hashedtext)
        {
            HashAlgorithmProviderData data = GetHashAlgorithmProviderDataFromCursor();

            if (!data.SaltEnabled)
            {
                return null;
            }

            byte[] salt = null;
            if (hashedtext.Length > SaltLength)
            {
                salt = new byte[SaltLength];
                Buffer.BlockCopy(hashedtext, 0, salt, 0, SaltLength );
            }
            return salt;
        }

        /// <summary>
        /// Gets the cryptographer used for hashing.
        /// </summary>
        /// <returns>
        /// A <see cref="HashCryptographer"/> object.
        /// </returns>
        protected virtual HashCryptographer GetHashCryptographer() 
        {
            try 
            {
                CheckAlgorithmType();
            } 
            catch (ConfigurationException ex)
            {
                CryptographyUtility.LogCryptographyException(ex);
                throw;
            }
            
            HashAlgorithmProviderData data = GetHashAlgorithmProviderDataFromCursor();

            return new HashCryptographer(data.AlgorithmType);
        }
		
        private HashAlgorithmProviderData GetHashAlgorithmProviderDataFromCursor()
        {
            HashProviderData hashProviderData = cryptoConfigurationView.GetHashProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(hashProviderData, typeof(HashAlgorithmProviderData));
            return (HashAlgorithmProviderData)hashProviderData;
        }

        private void AddSaltToHash(byte[] salt, ref byte[] hash)
        {
            HashAlgorithmProviderData data = GetHashAlgorithmProviderDataFromCursor();

            if (!data.SaltEnabled)
            {
                return;
            }
            hash = CryptographyUtility.CombineBytes(salt, hash);
        }

        private void AddSaltToPlainText(ref byte[] salt, ref byte[] plaintext)
        {
            HashAlgorithmProviderData data = GetHashAlgorithmProviderDataFromCursor();

            if (!data.SaltEnabled)
            {
                return;
            }

            if (salt == null)
            {
                salt = CryptographyUtility.GetRandomBytes(SaltLength);
            }

            plaintext = CryptographyUtility.CombineBytes(salt, plaintext);
        }

        private void CheckAlgorithmType() 
        {
            HashAlgorithmProviderData data = GetHashAlgorithmProviderDataFromCursor();

            Type type = Type.GetType(data.AlgorithmType);

            if (type == null) 
            {
                throw new ConfigurationException(SR.ExceptionCreatingHashAlgorithmInstance);
            } 
            else if (type.IsSubclassOf(typeof(KeyedHashAlgorithm))) 
            {
                throw new ConfigurationException(SR.MustUseKeyedHashAlgorithmProvider);
            }
        }
    }
}
