//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Tests
{
    [TestFixture]
    public class SymmetricStorageEncryptionProviderFixture : ConfigurationContextFixtureBase
    {
        [Test]
        public void GetProvider()
        {
            SymmetricStorageEncryptionProviderData data = new SymmetricStorageEncryptionProviderData();
            data.Name = "symm1";
            data.SymmetricInstance = "dpapi1";

            SymmetricStorageEncryptionProvider provider = new SymmetricStorageEncryptionProvider();
            provider.ConfigurationName = "InMemoryPersistenceWithSymmetricEncryption";
            provider.Initialize(new CachingConfigurationView(Context));

            byte[] plainText = new byte[] {0, 1, 2, 3, 4};
            byte[] encrypted = provider.Encrypt(plainText);
            Assert.IsFalse(CompareBytes(plainText, encrypted), "enc");

            byte[] decrypted = provider.Decrypt(encrypted);

            Assert.IsTrue(CompareBytes(plainText, decrypted), "dec");
        }

        [Test]
        public void GetCacheManagerWithDpapiEncryption()
        {
            CacheManagerFactory factory = new CacheManagerFactory(Context);
            CacheManager mgr = factory.GetCacheManager("InMemoryPersistenceWithSymmetricEncryption");
            CacheManagerTest(mgr);
        }

        [Test]
        public void GetCacheManagerWithNullEncryption()
        {
            CacheManagerFactory factory = new CacheManagerFactory(Context);
            CacheManager mgr = factory.GetCacheManager("InMemoryPersistenceWithNullEncryption");
            CacheManagerTest(mgr);
        }

        private void CacheManagerTest(CacheManager mgr)
        {
            string key = "key1";
            string val = "value123";

            Assert.IsNull(mgr.GetData(key), "null");

            mgr.Add(key, val);

            string result = (string)mgr.GetData(key);
            Assert.AreEqual(val, result, "result");
        }

        /// <summary>
        /// Test if two byte arrays are equal.
        /// </summary>
        /// <returns>True if they are the same.</returns>
        public static bool CompareBytes(byte[] byte1, byte[] byte2)
        {
            if (byte1 == null || byte2 == null)
            {
                return false;
            }
            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}

#endif