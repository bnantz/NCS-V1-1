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
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestFixture]
    public class HashAlgorithmProviderFixture : HashProviderBaseFixture
    {
        private readonly byte[] plainText = new byte[] {0, 1, 2, 3, 4, 5, 6};
        private const string hashInstance = "hashAlgorithm1";

        [Test]
        public void HashSha1()
        {
            IHashProvider hashProvider = DefaultHashProvider;
            SHA1 sha1 = SHA1Managed.Create();
            byte[] origHash = sha1.ComputeHash(plainText);
            byte[] providerHash = hashProvider.CreateHash(plainText);

            Assert.IsTrue(CryptographyUtility.CompareBytes(origHash, providerHash));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void HashHMACSHA1FailsUsingHashAlgorithmProvider()
        {
            HashAlgorithmProviderData keyData = new HashAlgorithmProviderData();
            keyData.AlgorithmType = typeof(HMACSHA1).AssemblyQualifiedName;
            keyData.SaltEnabled = false;
            keyData.Name = "BadHMACSHA1";
            IHashProvider hashProvider = new HashAlgorithmProvider();
            hashProvider.Initialize(new TestCryptographyConfigurationView(keyData));
            hashProvider.CreateHash(plainText);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void HashWithBadType()
        {
            HashAlgorithmProviderData data = new HashAlgorithmProviderData();
            data.AlgorithmType = "bad type";
            data.Name = "bad";
            IHashProvider hashProvider = new HashAlgorithmProvider();
            hashProvider.Initialize(new TestCryptographyConfigurationView(data));
            hashProvider.CreateHash(plainText);
        }

        public override IHashProvider DefaultHashProvider
        {
            get
            {
                HashProviderFactory factory = new HashProviderFactory(Context);
                return factory.CreateHashProvider(hashInstance);
            }
        }

        public override IHashProvider GetProvider()
        {
            return new HashAlgorithmProvider();
        }

        public override HashProviderData GetNewData()
        {
            return new HashAlgorithmProviderData();
        }

        public override IHashProvider SaltedHashProvider
        {
            get
            {
                HashAlgorithmProviderData data = new HashAlgorithmProviderData();
                data.Name = "hashAl1";
                data.AlgorithmType = typeof(SHA1Managed).AssemblyQualifiedName;
                data.SaltEnabled = true;
                HashAlgorithmProvider hashProvider = new HashAlgorithmProvider();
                hashProvider.Initialize(new TestCryptographyConfigurationView(data));
                return hashProvider;
            }
        }

        private class TestCryptographyConfigurationView : CryptographyConfigurationView
        {
            private readonly HashProviderData data;

            public TestCryptographyConfigurationView(HashProviderData data) : base(new ConfigurationContext(new ConfigurationDictionary()))
            {
                this.data = data;
            }

            public override HashProviderData GetHashProviderData(string name)
            {
                return data;
            }
        }
    }
}

#endif