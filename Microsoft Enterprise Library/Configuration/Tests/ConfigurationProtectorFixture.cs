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
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationProtectorFixture
    {
        private static readonly string typeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static readonly string transformerTypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;
        private static readonly string fileStorage = typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName;
        private static readonly string sectionName = "ApplConfig1";

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\" encrypt=\"true\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + typeName + "\" path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\" " +
                "type=\"" + transformerTypeName + "\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string xmlStringNoStorageProvider =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\" encrypt=\"true\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + typeName + "\" path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\" " +
                "type=\"" + transformerTypeName + "\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string xmlStringWithDpapi =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\" encrypt=\"true\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + typeName + "\" path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\" " +
                "type=\"" + transformerTypeName + "\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "<dpapiSettings mode=\"Machine\" entropy=\"AAECAwQFBgcICQABAgMFBA==\" />" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string xmlStringNoProtection =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\" encrypt=\"false\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + typeName + "\" path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\" " +
                "type=\"" + transformerTypeName + "\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        [Test]
        public void ConfigurationProtectorTestWithoutDpapi()
        {
            string mySecret = "mary had a little lamb";
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            KeyAlgorithmPair pair = new KeyAlgorithmPair(myRijndael.Key, myRijndael.GetType().AssemblyQualifiedName);
            SaveKeyPair(pair, xmlString);
            ConfigurationContext context = CreateContext(xmlString);
            using (ConfigurationProtector protector = new ConfigurationProtector())
            {
                protector.Load(context, sectionName);
                byte[] inBytes = UnicodeEncoding.Unicode.GetBytes(mySecret);
                byte[] encryptedBytes = protector.Encrypt(inBytes);
                Assert.IsFalse(CryptographyUtility.CompareBytes(inBytes, encryptedBytes));
                byte[] decryptedBytes = protector.Decrypt(encryptedBytes);
                Assert.AreEqual(mySecret, UnicodeEncoding.Unicode.GetString(decryptedBytes));
            }
        }

        [Test]
        public void ConfigurationProtectorTestWithDpapi()
        {
            string mySecret = "mary had a little lamb";
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            KeyAlgorithmPair pair = new KeyAlgorithmPair(myRijndael.Key, myRijndael.GetType().AssemblyQualifiedName);
            SaveKeyPair(pair, xmlStringWithDpapi);
            ConfigurationContext context = CreateContext(xmlStringWithDpapi);
            using (ConfigurationProtector protector = new ConfigurationProtector())
            {
                protector.Load(context, sectionName);
                byte[] inBytes = UnicodeEncoding.Unicode.GetBytes(mySecret);
                byte[] encryptedBytes = protector.Encrypt(inBytes);
                byte[] decryptedBytes = protector.Decrypt(encryptedBytes);
                Assert.AreEqual(mySecret, UnicodeEncoding.Unicode.GetString(decryptedBytes));
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ConfigurationProtectorTestEncryptedButNoProvider()
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            KeyAlgorithmPair pair = new KeyAlgorithmPair(myRijndael.Key, myRijndael.GetType().AssemblyQualifiedName);
            SaveKeyPair(pair, xmlStringWithDpapi);
            using (ConfigurationContext context = CreateContext(xmlStringNoStorageProvider))
            {
                using (ConfigurationProtector protector = new ConfigurationProtector())
                {
                    protector.Load(context, sectionName);
                }
            }
        }

        [Test]
        public void ConfigurationProtectorTestNoProtectionDefined()
        {
            string mySecret = "mary had a little lamb";
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            KeyAlgorithmPair pair = new KeyAlgorithmPair(myRijndael.Key, myRijndael.GetType().AssemblyQualifiedName);
            SaveKeyPair(pair, xmlString);
            using (ConfigurationContext context = CreateContext(xmlStringNoProtection))
            {
                using (ConfigurationProtector protector = new ConfigurationProtector())
                {
                    protector.Load(context, sectionName);
                    byte[] inBytes = UnicodeEncoding.Unicode.GetBytes(mySecret);
                    byte[] encryptedBytes = protector.Encrypt(inBytes);
                    Assert.IsTrue(CryptographyUtility.CompareBytes(inBytes, encryptedBytes));
                    byte[] decryptedBytes = protector.Decrypt(encryptedBytes);
                    Assert.AreEqual(mySecret, UnicodeEncoding.Unicode.GetString(decryptedBytes));
                }
            }
        }

        private void SaveKeyPair(KeyAlgorithmPair pair, string xmlToUse)
        {
            KeyAlgorithmStorageProviderFactory factory = new KeyAlgorithmStorageProviderFactory(CreateContext(xmlToUse));
            IKeyAlgorithmPairStorageProvider provider = factory.Create();
            provider.Save(pair);
        }

        private ConfigurationContext CreateContext(string xmlToUse)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlToUse));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            ConfigurationSettings configurationSettings = xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            dictionary.Add(ConfigurationSettings.SectionName, configurationSettings);
            ConfigurationContext context = new ConfigurationContext(dictionary);
            return context;
        }

    }
}

#endif