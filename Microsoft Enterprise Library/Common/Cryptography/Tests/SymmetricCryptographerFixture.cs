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
    public class SymmetricCryptographerFixture
    {
        [Test]
        public void EncryptAndDecryptWithAlgorithm()
        {
            SymmetricAlgorithm alg = RijndaelManaged.Create();
            byte[] key = new byte[16];
            CryptographyUtility.GetRandomBytes(key);

            SymmetricCryptographer symm = new SymmetricCryptographer(alg, key);

            byte[] plainText = new byte[12];
            CryptographyUtility.GetRandomBytes(plainText);

            byte[] cipherText = symm.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText));

            byte[] decryptedText = symm.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }

        [Test]
        public void EncryptAndDecryptWithType()
        {
            string alg = typeof(RijndaelManaged).AssemblyQualifiedName;
            byte[] key = new byte[16];
            CryptographyUtility.GetRandomBytes(key);

            SymmetricCryptographer symm = new SymmetricCryptographer(alg, key);

            byte[] plainText = new byte[12];
            CryptographyUtility.GetRandomBytes(plainText);

            byte[] cipherText = symm.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText));

            byte[] decryptedText = symm.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText));
        }
    }
}

#endif