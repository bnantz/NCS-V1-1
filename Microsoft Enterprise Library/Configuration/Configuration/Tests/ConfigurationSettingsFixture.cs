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
using System;
using System.Globalization;
using System.IO;
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
    public class ConfigurationSettingsFixture
    {
        private static readonly string typeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static readonly string transformerTypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;
        private static readonly string fileStorage = typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +

                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "applicationName=\"MyApplication\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\">" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\" encrypt=\"false\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "<configurationSection name=\"ApplConfig2\" encrypt=\"false\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" path=\"foo.data\">" +
                "<dpapiSettings mode=\"Machine\" entropy=\"AAECAwQFBgcICQABAgMFBg==\" />" +
                "</keyAlgorithmStorageProvider>" +
                "<includeTypes>" +
                "<includeType name=\"My Custom Storage Provider\" type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomStorageProvider, Microsoft.Practices.EnterpriseLibrary.Configuration\" />" +
                "<includeType name=\"My Custom Transformer\" type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomTransformer, Microsoft.Practices.EnterpriseLibrary.Configuration\" />" +
                "<includeType name=\"My Custom Key Algorithm Pair Storage Provider Data\" type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomKeyAlgorithmPairStorageProviderData, Microsoft.Practices.EnterpriseLibrary.Configuration\" />" +
                "</includeTypes>" +
                "</enterpriselibrary.configurationSettings>";

        [Test]
        public void DeserializeTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            ConfigurationSettings configurationSettings = xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
            Assert.IsNotNull(configurationSettings);
        }

        [Test]
        public void SerializeTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings), new Type[] {typeof(XmlFileStorageProviderData)});
            ConfigurationSettings configurationSettings = new ConfigurationSettings();
            configurationSettings.XmlIncludeTypes.Add(new XmlIncludeTypeData("My Custom Storage Provider", "Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomStorageProvider, Microsoft.Practices.EnterpriseLibrary.Configuration"));
            configurationSettings.XmlIncludeTypes.Add(new XmlIncludeTypeData("My Custom Transformer", "Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomTransformer, Microsoft.Practices.EnterpriseLibrary.Configuration"));
            configurationSettings.XmlIncludeTypes.Add(new XmlIncludeTypeData("My Custom Key Algorithm Pair Storage Provider Data", "Microsoft.Practices.EnterpriseLibrary.Configuration.MyCustomKeyAlgorithmPairStorageProviderData, Microsoft.Practices.EnterpriseLibrary.Configuration"));
            configurationSettings.ApplicationName = "MyApplication";
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("ApplConfig1", false, GetStorageProvider(), GetTransformer());
            configurationSettings.ConfigurationSections.Add(configurationSection);
            configurationSection = new ConfigurationSectionData("ApplConfig2", false, GetStorageProvider(), GetTransformer());
            configurationSettings.ConfigurationSections.Add(configurationSection);
            FileKeyAlgorithmPairStorageProviderData fileKeyAlgorithmPairStorageProviderData = new FileKeyAlgorithmPairStorageProviderData("FileStore", "foo.data");
            DpapiSettingsData dpapiData = new DpapiSettingsData(new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 5, 6}, DpapiStorageMode.Machine);
            fileKeyAlgorithmPairStorageProviderData.DpapiSettings = dpapiData;
            configurationSettings.KeyAlgorithmPairStorageProviderData = fileKeyAlgorithmPairStorageProviderData;
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            xmlSerializer.Serialize(writer, configurationSettings);
            writer.Close();
            Assert.AreEqual(xmlString, configXml.ToString());
        }

        [Test]
        public void ItemTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            ConfigurationSettings configurationSettings = xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
            Assert.IsNotNull(configurationSettings);
            ConfigurationSectionData section = configurationSettings["ApplConfig1"];
            Assert.IsNotNull(section);
            Assert.AreEqual("ApplConfig1", section.Name);
            Assert.IsNotNull(configurationSettings.KeyAlgorithmPairStorageProviderData);
        }

        private StorageProviderData GetStorageProvider()
        {
            XmlFileStorageProviderData provider = new XmlFileStorageProviderData("XmlStorage");
            provider.TypeName = typeName;
            return provider;
        }

        private TransformerData GetTransformer()
        {
            return new XmlSerializerTransformerData("XmlTransformer");
        }
    }
}

#endif