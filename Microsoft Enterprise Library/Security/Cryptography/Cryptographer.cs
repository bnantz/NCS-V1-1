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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Facade which exposes common cryptography uses.
    /// </summary>
    public sealed class Cryptographer
    {
        private Cryptographer()
        {
        }

        /// <summary>
        /// Computes the hash value of plain text.
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public static byte[] CreateHash(string hashInstance, byte[] plaintext)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return CreateHash(hashInstance, plaintext, context);
		}

		internal static byte[] CreateHash(string hashInstance, byte[] plaintext, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(hashInstance, "hashInstance");
			ArgumentValidation.CheckForEmptyString(hashInstance, "hashInstance");

			HashProviderFactory factory = new HashProviderFactory(context);
			IHashProvider hashProvider = factory.CreateHashProvider(hashInstance);
			return hashProvider.CreateHash(plaintext);
		}

        /// <summary>
        /// Computes the hash value of plain text.
        /// </summary>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which to compute the hash represented as a base64 encoded string.</param>
        /// <returns>The computed hash code as a base64 encoded string.</returns>
        public static string CreateHash(string hashInstance, string plaintext)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return CreateHash(hashInstance, plaintext, context);
        }

		internal static string CreateHash(string hashInstance, string plaintext, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(hashInstance, "hashInstance");
			ArgumentValidation.CheckForEmptyString(hashInstance, "hashInstance");

			byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
			byte[] resultBytes = CreateHash(hashInstance, plainTextBytes, context);
			CryptographyUtility.GetRandomBytes(plainTextBytes);

			return Convert.ToBase64String(resultBytes);
		}
		
		/// <summary>
        /// Compares plain text input with a computed hash.
        /// </summary>
        /// <remarks>
        /// Use this method to compare hash values. Since hashes may contain a random "salt" value, two seperately generated
        /// hashes of the same plain text may result in different values. 
        /// </remarks>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input for which you want to compare the hash to.</param>
        /// <param name="hashedText">The hash value for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        public static bool CompareHash(string hashInstance, byte[] plaintext, byte[] hashedText)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return CompareHash(hashInstance, plaintext, hashedText, context);
        }

		internal static bool CompareHash(string hashInstance, byte[] plaintext, byte[] hashedText, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(hashInstance, "hashInstance");
			ArgumentValidation.CheckForEmptyString(hashInstance, "hashInstance");

			HashProviderFactory factory = new HashProviderFactory(context);
			IHashProvider hashProvider = factory.CreateHashProvider(hashInstance);
			return hashProvider.CompareHash(plaintext, hashedText);
		}

		/// <summary>
        /// Compares plain text input with a computed hash.
        /// </summary>
        /// <remarks>
        /// Use this method to compare hash values. Since hashes contain a random "salt" value, two seperately generated
        /// hashes of the same plain text will result in different values. 
        /// </remarks>
        /// <param name="hashInstance">A hash instance from configuration.</param>
        /// <param name="plaintext">The input as a string for which you want to compare the hash to.</param>
        /// <param name="hashedText">The hash as a string for which you want to compare the input to.</param>
        /// <returns><c>true</c> if plainText hashed is equal to the hashedText. Otherwise, <c>false</c>.</returns>
        public static bool CompareHash(string hashInstance, string plaintext, string hashedText)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return CompareHash(hashInstance, plaintext, hashedText, context);
        }

		internal static bool CompareHash(string hashInstance, string plaintext, string hashedText, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(hashInstance, "hashInstance");
			ArgumentValidation.CheckForEmptyString(hashInstance, "hashInstance");

			byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
			byte[] hashedTextBytes = Convert.FromBase64String(hashedText);

			bool result = CompareHash(hashInstance, plainTextBytes, hashedTextBytes, context);
			CryptographyUtility.GetRandomBytes(plainTextBytes);

			return result;
		}
		
		/// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input for which you want to encrypt.</param>
        /// <returns>The resulting cipher text.</returns>
        public static byte[] EncryptSymmetric(string symmetricInstance, byte[] plaintext)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return EncryptSymmetric(symmetricInstance, plaintext, context);
        }

		internal static byte[] EncryptSymmetric(string symmetricInstance, byte[] plaintext, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(symmetricInstance, "symmetricInstance");
			ArgumentValidation.CheckForEmptyString(symmetricInstance, "symmetricInstance");

			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
			ISymmetricCryptoProvider symmetricProvider = factory.CreateSymmetricCryptoProvider(symmetricInstance);
			return symmetricProvider.Encrypt(plaintext);
		}
		
		/// <summary>
        /// Encrypts a secret using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="plaintext">The input as a base64 encoded string for which you want to encrypt.</param>
        /// <returns>The resulting cipher text as a base64 encoded string.</returns>
        public static string EncryptSymmetric(string symmetricInstance, string plaintext)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return EncryptSymmetric(symmetricInstance,  plaintext, context);
		}

		internal static string EncryptSymmetric(string symmetricInstance, string plaintext, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(symmetricInstance, "symmetricInstance");
			ArgumentValidation.CheckForEmptyString(symmetricInstance, "symmetricInstance");

			byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plaintext);
			byte[] cipherTextBytes = EncryptSymmetric(symmetricInstance, plainTextBytes, context);
			CryptographyUtility.GetRandomBytes(plainTextBytes);
			return Convert.ToBase64String(cipherTextBytes);
		}
		
		/// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertext">The cipher text for which you want to decrypt.</param>
        /// <returns>The resulting plain text.</returns>
        public static byte[] DecryptSymmetric(string symmetricInstance, byte[] ciphertext)
        {
        	ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return DecryptSymmetric(symmetricInstance, ciphertext, context);
        }

		internal static byte[] DecryptSymmetric(string symmetricInstance, byte[] ciphertext, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(symmetricInstance, "symmetricInstance");
			ArgumentValidation.CheckForEmptyString(symmetricInstance, "symmetricInstance");

			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
			ISymmetricCryptoProvider symmetricProvider = factory.CreateSymmetricCryptoProvider(symmetricInstance);
			return symmetricProvider.Decrypt(ciphertext);
		}
		
		/// <summary>
        /// Decrypts a cipher text using a specified symmetric cryptography provider.
        /// </summary>
        /// <param name="symmetricInstance">A symmetric instance from configuration.</param>
        /// <param name="ciphertextBase64">The cipher text as a base64 encoded string for which you want to decrypt.</param>
        /// <returns>The resulting plain text as a string.</returns>
        public static string DecryptSymmetric(string symmetricInstance, string ciphertextBase64)
        {
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return DecryptSymmetric(symmetricInstance,  ciphertextBase64, context);
		}
	
		internal static string DecryptSymmetric(string symmetricInstance, string ciphertextBase64, ConfigurationContext context)
		{
			ArgumentValidation.CheckForNullReference(symmetricInstance, "symmetricInstance");
			ArgumentValidation.CheckForEmptyString(symmetricInstance, "symmetricInstance");

			byte[] cipherTextBytes = Convert.FromBase64String(ciphertextBase64);
			byte[] decryptedBytes = DecryptSymmetric(symmetricInstance, cipherTextBytes, context);
			string decryptedString = UnicodeEncoding.Unicode.GetString(decryptedBytes);
			CryptographyUtility.GetRandomBytes(decryptedBytes);

			return decryptedString;
		}
	}
}