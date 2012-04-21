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

#if UNIT_TESTS
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.Tests
{
    [TestFixture]
    public class FileKeyAlgorithmPairStorageProviderFixture
    {
        private const string keyFileName = @"ENTLIBTEST_FakeKeyAlgorithmPair.dat";
        private string filePath;

        [SetUp]
        public void SetUp()
        {
            filePath = Path.Combine(Path.GetTempPath(), keyFileName);
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Test]
        public void SaveAndLoadPlainText()
        {
            SaveAndLoad(new TestKeyAlgorithmPairStorageProviderConfigurationView(new ConfigurationContext(new ConfigurationDictionary()), DefaultProviderData));
        }

        [Test]
        public void SaveAndLoadProtected()
        {
            SaveAndLoad(new TestKeyAlgorithmPairStorageProviderConfigurationView(new ConfigurationContext(new ConfigurationDictionary()), ProtectedProviderData));
        }

        private void SaveAndLoad(RuntimeConfigurationView configurationView)
        {
            FileKeyAlgorithmPairStorageProvider provider = new FileKeyAlgorithmPairStorageProvider();
            provider.Initialize(configurationView);

            provider.Save(DefaultKey);
            KeyAlgorithmPair key = provider.Load();

            Assert.IsNotNull(key);
            Assert.AreEqual(DefaultKey.AlgorithmTypeName, key.AlgorithmTypeName);
            Assert.IsTrue(CryptographyUtility.CompareBytes(DefaultKey.Key, key.Key));
            FileKeyAlgorithmPairStorageProviderData data = (FileKeyAlgorithmPairStorageProviderData)configurationView.GetKeyAlgorithmPairStorageProviderData();
            // If protected, ensure it's protected properly
            if (null != data.DpapiSettings)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    bool deserialized = true;
                    try
                    {
                        KeyAlgorithmPair deserializedKey = (KeyAlgorithmPair)formatter.Deserialize(fs);
                        Assert.IsNotNull(deserializedKey);
                    }
                    catch
                    {
                        deserialized = false;
                    }

                    // Shouldn't deserialize if it's DPAPI protected
                    Assert.IsFalse(deserialized);
                }
            }
        }

        private KeyAlgorithmPair DefaultKey
        {
            get
            {
                KeyAlgorithmPair key = new KeyAlgorithmPair();
                key.AlgorithmTypeName = typeof(RijndaelManaged).AssemblyQualifiedName;
                key.Key = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5};
                return key;
            }
        }

        private FileKeyAlgorithmPairStorageProviderData ProtectedProviderData
        {
            get
            {
                FileKeyAlgorithmPairStorageProviderData providerData = new FileKeyAlgorithmPairStorageProviderData();
                providerData.Name = "protectedProviderData";
                providerData.Path = filePath;
                providerData.DpapiSettings = new DpapiSettingsData(new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 5, 4}, DpapiStorageMode.Machine);
                return providerData;
            }
        }

        private FileKeyAlgorithmPairStorageProviderData DefaultProviderData
        {
            get
            {
                FileKeyAlgorithmPairStorageProviderData providerData = new FileKeyAlgorithmPairStorageProviderData();
                providerData.Name = "defaultProviderData";
                providerData.Path = keyFileName;
                return providerData;
            }
        }

        private class TestKeyAlgorithmPairStorageProviderConfigurationView : RuntimeConfigurationView
        {
            public KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData;

            public TestKeyAlgorithmPairStorageProviderConfigurationView(ConfigurationContext context, KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData) : base(context)
            {
                this.keyAlgorithmPairStorageProviderData = keyAlgorithmPairStorageProviderData;
            }

            public override KeyAlgorithmPairStorageProviderData GetKeyAlgorithmPairStorageProviderData()
            {
                return keyAlgorithmPairStorageProviderData;
            }

        }
    }
}

#endif