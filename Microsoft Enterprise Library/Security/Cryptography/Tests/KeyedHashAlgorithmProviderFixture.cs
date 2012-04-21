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
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestFixture]
    public class KeyedHashAlgorithmProviderFixture : HashProviderBaseFixture
    {
        private KeyedHashAlgorithmProviderData data;
        private KeyedHashAlgorithmProvider defaultHashProvider;

        private byte[] plainText;

        [SetUp]
        public void SetUp()
        {
            // create a new random plain text secret
            this.plainText = new byte[12];
            RNGCryptoServiceProvider.Create().GetNonZeroBytes(this.plainText);

            this.data = new KeyedHashAlgorithmProviderData();
            this.data.Name = "name";
            this.data.AlgorithmType = typeof(HMACSHA1).AssemblyQualifiedName;
            this.data.SaltEnabled = false;
            this.data.Key = new byte[] {1, 2, 3, 4};

            this.defaultHashProvider = (KeyedHashAlgorithmProvider)CreateProvider(this.data);
        }

        [Test]
        public void Initialize()
        {
            byte[] originalHash = defaultHashProvider.CreateHash(this.plainText);

            KeyedHashAlgorithmProvider hash2 = (KeyedHashAlgorithmProvider)CreateProvider(this.data);

            Assert.IsTrue(hash2.CompareHash(this.plainText, originalHash));
        }

        [Test]
        public void CreateHashWithSalt()
        {
            byte[] saltedHash = SaltedHashProvider.CreateHash(this.plainText);

            int saltLength = 16;
            int hashMinusSaltLength = 20;

            byte[] salt = new byte[saltLength];
            byte[] hashMinusSalt = new byte[hashMinusSaltLength];

            Array.Copy(saltedHash, 0, salt, 0, saltLength);
            Array.Copy(saltedHash, saltLength, hashMinusSalt, 0, hashMinusSaltLength);

            byte[] saltedPlainText = new byte[this.plainText.Length + salt.Length];

            salt.CopyTo(saltedPlainText, 0);
            this.plainText.CopyTo(saltedPlainText, saltLength);

            byte[] expectedResult = HashWithHMACSHA1(saltedPlainText);

            Assert.IsTrue(CryptographyUtility.CompareBytes(expectedResult, hashMinusSalt));
        }

        private byte[] HashWithHMACSHA1(byte[] saltedPlainText)
        {
            HMACSHA1 bigmac = new HMACSHA1(data.Key);

            CryptoStream cs = new CryptoStream(Stream.Null, bigmac, CryptoStreamMode.Write);
            cs.Write(saltedPlainText, 0, saltedPlainText.Length);
            cs.Close();

            return bigmac.Hash;
        }

        public override IHashProvider DefaultHashProvider
        {
            get { return defaultHashProvider; }
        }

        public override IHashProvider SaltedHashProvider
        {
            get
            {
                KeyedHashAlgorithmProviderData saltedData = new KeyedHashAlgorithmProviderData();
                saltedData.Name = "name";
                saltedData.AlgorithmType = typeof(HMACSHA1).AssemblyQualifiedName;
                saltedData.SaltEnabled = true;
                saltedData.Key = new byte[] {1, 2, 3, 4};

                return (KeyedHashAlgorithmProvider)CreateProvider(saltedData);
            }
        }

        public override IHashProvider GetProvider()
        {
            return new KeyedHashAlgorithmProvider();
        }

        public override HashProviderData GetNewData()
        {
            return new KeyedHashAlgorithmProviderData();
        }
    }
}

#endif