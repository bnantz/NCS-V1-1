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
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography.Tests
{
    [TestFixture]
    public class DpapiCryptographerFixture
    {
        private byte[] plainText;
        private byte[] entropy;

        [SetUp]
        public void Setup()
        {
            entropy = new byte[12];
            RandomNumberGenerator rnd = RNGCryptoServiceProvider.Create();
            rnd.GetBytes(entropy);

            plainText = new byte[12];
            rnd = RNGCryptoServiceProvider.Create();
            rnd.GetBytes(plainText);
        }

        [Test]
        public void DefaultStorageMode()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer();
            Assert.AreEqual(DpapiStorageMode.Machine, dpapi.StorageMode);
        }

        [Test]
        public void EncryptAndDecryptMachineMode()
        {
            DpapiStorageMode mode = DpapiStorageMode.Machine;
            DpapiCryptographer dpapi = new DpapiCryptographer(mode);

            byte[] cipherText = dpapi.Encrypt(this.plainText, this.entropy);

            Assert.IsFalse(CryptographyUtility.CompareBytes(this.plainText, cipherText));

            byte[] decryptedText = dpapi.Decrypt(cipherText, this.entropy);

            Assert.IsTrue(CryptographyUtility.CompareBytes(this.plainText, decryptedText));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EncryptAndDecryptMachineModeWithMissingEntropy()
        {
            DpapiStorageMode mode = DpapiStorageMode.Machine;
            DpapiCryptographer dpapi = new DpapiCryptographer(mode);

            dpapi.Encrypt(this.plainText);
        }

        [Test]
        public void EncryptAndDecryptUserMode()
        {
            DpapiStorageMode mode = DpapiStorageMode.User;
            DpapiCryptographer dpapi = new DpapiCryptographer(mode);

            byte[] cipherText = dpapi.Encrypt(this.plainText);

            Assert.IsFalse(CryptographyUtility.CompareBytes(this.plainText, cipherText));

            byte[] decryptedText = dpapi.Decrypt(cipherText);

            Assert.IsTrue(CryptographyUtility.CompareBytes(this.plainText, decryptedText));
        }

        [Test]
        [ExpectedException(typeof(Win32Exception))]
        public void DecryptBadData()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer();
            byte[] cipherText = new byte[] {0, 1};
            dpapi.Decrypt(cipherText, this.entropy);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EncryptMachineWithOutEntropy()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.Machine);
            dpapi.Encrypt(new byte[] {0, 1, 2, 3, 4});
        }

        [Test]
        [ExpectedException(typeof(Win32Exception))]
        public void DecryptWithDifferentEntropyThanEncrypt()
        {
            byte[] plainBytes = new byte[] {0, 1, 2, 3, 4};
            byte[] entropy1 = new byte[16];
            byte[] entropy2 = new byte[16];
            CryptographyUtility.GetRandomBytes(entropy1);
            CryptographyUtility.GetRandomBytes(entropy2);

            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.Machine);
            byte[] encrypted = dpapi.Encrypt(plainBytes, entropy1);
            dpapi.Decrypt(encrypted, entropy2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptWithNullCipherText()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.User);
            dpapi.Decrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptWithNullPlainText()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.User);
            dpapi.Encrypt(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptWithNullEntropy()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.Machine);
            dpapi.Encrypt(new byte[] {0, 1, 2}, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EncryptWithZeroLengthEntropy()
        {
            DpapiCryptographer dpapi = new DpapiCryptographer(DpapiStorageMode.Machine);
            dpapi.Encrypt(new byte[] {0, 1, 2}, new byte[] {});
        }
    }
}

#endif