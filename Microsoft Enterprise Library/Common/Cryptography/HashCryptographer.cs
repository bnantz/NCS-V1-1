//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography
{
    /// <summary>
    /// <para>Represents a basic cryptography services for a <see cref="HashAlgorithm"/>.</para>
    /// </summary>
    public class HashCryptographer
    {
        private string algorithmType;
        private byte[] key;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashCryptographer"/> with an algorithm type.</para>
        /// </summary>
        /// <param name="algorithmType">A fully qualifed type name derived from <see cref="HashAlgorithm"/>.</param>
        public HashCryptographer(string algorithmType)
        {
            this.algorithmType = algorithmType;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashCryptographer"/> with an algorithm type and key.</para>
        /// </summary>
        /// <param name="algorithmType">A fully qualifed type name derived from <see cref="HashAlgorithm"/>.</param>
        /// <param name="key"><para>The key for a <see cref="KeyedHashAlgorithm"/>.</para></param>
        /// <remarks>
        /// While this overload will work with a specified <see cref="HashAlgorithm"/>, the key 
        /// is only relevant when initializing with a specified <see cref="KeyedHashAlgorithm"/>.
        /// </remarks>
        public HashCryptographer(string algorithmType, byte[] key) : this(algorithmType)
        {
            this.key = key;
        }

        /// <summary>
        /// <para>Computes the hash value of the plaintext.</para>
        /// </summary>
        /// <param name="plaintext"><para>The plaintext in which you wish to hash.</para></param>
        /// <returns><para>The resulting hash.</para></returns>
        public byte[] ComputeHash(byte[] plaintext)
        {
            byte[] hash = null;

            using (HashAlgorithm algorithm = GetHashAlgorithm())
            {
                hash = algorithm.ComputeHash(plaintext);
            }

            return hash;
        }

        private HashAlgorithm GetHashAlgorithm()
        {
            HashAlgorithm algorithm = null;

            try
            {
                Type type = Type.GetType(algorithmType);
                algorithm = Activator.CreateInstance(type, true) as HashAlgorithm;
                KeyedHashAlgorithm keyedHashAlgorithm = algorithm as KeyedHashAlgorithm;
                if ((null != keyedHashAlgorithm) && (key != null))
                {
                    keyedHashAlgorithm.Key = key;
                }
            }
            catch (Exception ex)
            {
                CryptographyUtility.LogCryptographyException(ex);
                throw new CryptographicException(SR.ExceptionCreatingHashAlgorithmInstance);
            }

            if (algorithm == null)
            {
                throw new CryptographicException(SR.ExceptionCastingHashAlgorithmInstance);
            }

            return algorithm;
        }
    }
}