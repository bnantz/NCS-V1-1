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

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    public abstract class SymmetricProviderBaseFixture
    {
        public abstract ISymmetricCryptoProvider DefaultSymmProvider { get; }

        [Test]
        public void EncryptAndDecrypt()
        {
            byte[] plainText = new byte[50];
            EncryptAndDecryptTest(plainText);
        }

        [Test]
        public void EncryptAndDecryptOneByte()
        {
            byte[] plainText = new byte[1];
            EncryptAndDecryptTest(plainText);
        }

        [Test]
        public void EncryptAndDecryptOneKilobyte()
        {
            byte[] plainText = new byte[1024];
            EncryptAndDecryptTest(plainText);
        }

        [Test]
        public void EncryptAndDecryptOneMegabyte()
        {
            byte[] plainText = new byte[1024 * 1024];
            EncryptAndDecryptTest(plainText);
        }

        public void EncryptAndDecryptTest(byte[] plainText)
        {
            plainText = CryptographyUtility.GetRandomBytes(plainText.Length);

            byte[] cipherText = DefaultSymmProvider.Encrypt(plainText);
            Assert.IsFalse(CryptographyUtility.CompareBytes(cipherText, plainText), "encrypted");

            byte[] decryptedText = DefaultSymmProvider.Decrypt(cipherText);
            Assert.IsTrue(CryptographyUtility.CompareBytes(plainText, decryptedText), "decrypted");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNull()
        {
            DefaultSymmProvider.Encrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptZeroLength()
        {
            DefaultSymmProvider.Encrypt(new byte[] {});
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNull()
        {
            DefaultSymmProvider.Decrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptZeroLength()
        {
            DefaultSymmProvider.Decrypt(new byte[] {});
        }
    }
}

#endif