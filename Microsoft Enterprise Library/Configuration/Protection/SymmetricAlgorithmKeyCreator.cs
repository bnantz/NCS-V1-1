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
using System.Security.Cryptography;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection
{
    /// <summary>
    /// <para>Creates keys for a <see cref="SymmetricAlgorithm"/>.</para>
    /// </summary>
    public class SymmetricAlgorithmKeyCreator : IKeyCreator
    {
        private SymmetricAlgorithm algorithm;

        /// <summary>
        /// <para>Initializes an instance of the <see cref="SymmetricAlgorithmKeyCreator"/> class for a specific <see cref="SymmetricAlgorithm"/>.</para>
        /// </summary>
        /// <param name="symmetricAlgorithmName">The assembly qualified name of a class which derives from a <see cref="SymmetricAlgorithm"/>.</param>
        [ReflectionPermission(SecurityAction.Demand,  Flags=ReflectionPermissionFlag.MemberAccess)]
        public SymmetricAlgorithmKeyCreator(string symmetricAlgorithmName)
        {
            Type algorithmType = Type.GetType(symmetricAlgorithmName, true);
            this.algorithm = (SymmetricAlgorithm)Activator.CreateInstance(algorithmType);
        }

        /// <summary>
        /// <para>Gets the length of the key.</para>
        /// </summary>
        /// <value>The length of the key.</value>
        public int KeyLength
        {
            get { return this.algorithm.Key.Length; }
        }

        /// <summary>
        /// <para>Generates a random key.</para>
        /// </summary>
        /// <returns><para>A random key.</para></returns>
        public byte[] GenerateKey()
        {
            this.algorithm.GenerateKey();
            return this.algorithm.Key;
        }

        /// <summary>
        /// <para>Determines if the <paramref name="key"/> is valid.</para>
        /// </summary>
        /// <param name="key">The key to test.</param>
        /// <returns><para><see langword="true"/> if the key is valid; otherwise <see langword="false"/>.</para></returns>
        public bool KeyIsValid(byte[] key)
        {
            bool valid = false;
            int keyLength = key.Length * 8;

            foreach (KeySizes keySizes in this.algorithm.LegalKeySizes)
            {
                if (keyLength == keySizes.MinSize || keyLength == keySizes.MaxSize)
                {
                    valid = true;
                    break;
                }

                if (keyLength > keySizes.MinSize && keyLength < keySizes.MaxSize)
                {
                    if (keyLength % keySizes.SkipSize == 0)
                    {
                        valid = true;
                        break;
                    }
                }
            }

            return valid;
        }
    }
}
