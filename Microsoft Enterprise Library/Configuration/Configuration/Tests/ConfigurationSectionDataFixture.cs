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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationSectionDataFixture
    {
        private static readonly string storageTypeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<configurationSection xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" name=\"ApplConfig1\" encrypt=\"false\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>";

        [Test]
        public void DeserializeTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSectionData));
            ConfigurationSectionData configurationSection = xmlSerializer.Deserialize(xmlReader) as ConfigurationSectionData;
            Assert.IsNotNull(configurationSection);
            Assert.IsNotNull(configurationSection.StorageProvider);
            Assert.IsNotNull(configurationSection.Transformer);
            Assert.AreEqual("ApplConfig1", configurationSection.Name);
        }

        [Test]
        public void SerializeTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSectionData));
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("ApplConfig1", false, GetStorageProvider(), GetTransformer());
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            xmlSerializer.Serialize(writer, configurationSection);
            writer.Close();
            Assert.AreEqual(xmlString, configXml.ToString());
        }

        private StorageProviderData GetStorageProvider()
        {
            XmlFileStorageProviderData storage = new XmlFileStorageProviderData("XmlStorage");
            storage.TypeName = storageTypeName;
            return storage;
        }

        private TransformerData GetTransformer()
        {
            return new XmlSerializerTransformerData("XmlTransformer");
        }
    }
}

#endif