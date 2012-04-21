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

#if UNIT_TESTS
using System.Security.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography.Tests
{
    [TestFixture]
    public class HashCryptographerFixture
    {
        [Test]
        public void HashMD5()
        {
            byte[] plaintext = new byte[] {0, 1, 2, 3};
            HashCryptographer cryptographer = new HashCryptographer(typeof(MD5CryptoServiceProvider).AssemblyQualifiedName);
            byte[] hash1 = cryptographer.ComputeHash(plaintext);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plaintext, hash1));

            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] hash2 = md5.ComputeHash(plaintext);

            Assert.IsTrue(CryptographyUtility.CompareBytes(hash1, hash2));
        }

        [Test]
        public void HashHMACSHA1()
        {
            byte[] plaintext = new byte[] {0, 1, 2, 3};
            byte[] key = new byte[] {0, 4, 5, 2, 3, 4};
            HashCryptographer cryptographer = new HashCryptographer(typeof(HMACSHA1).AssemblyQualifiedName, key);
            byte[] hash1 = cryptographer.ComputeHash(plaintext);

            Assert.IsFalse(CryptographyUtility.CompareBytes(plaintext, hash1));

            KeyedHashAlgorithm hmacsha1 = HMACSHA1.Create();
            hmacsha1.Key = key;
            byte[] hash2 = hmacsha1.ComputeHash(plaintext);

            Assert.IsTrue(CryptographyUtility.CompareBytes(hash1, hash2));
        }
    }
}

#endif