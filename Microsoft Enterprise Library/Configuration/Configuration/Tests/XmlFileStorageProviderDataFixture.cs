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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class XmlFileStorageProviderDataFixture
    {
        private static string xmlFileStorageTypeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<storageProvider xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"c:\\mypath.config\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\" />";

        private static readonly string xmlNoTypeString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<storageProvider xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "name=\"XmlStorage\" " +
                "path=\"\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\" />";

        [Test]
        public void DeserializeAsStorageProviderTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(StorageProviderData));
            XmlFileStorageProviderData settings = xmlSerializer.Deserialize(xmlReader) as XmlFileStorageProviderData;
            Assert.IsNotNull(settings);
            Assert.AreEqual("XmlStorage", settings.Name);
            Type storageProviderType = Type.GetType(settings.TypeName, true);
            Assert.AreEqual(typeof(XmlFileStorageProvider), storageProviderType);
        }

        [Test]
        public void SerializeAsStorageProviderTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(StorageProviderData));
            XmlFileStorageProviderData provider = new XmlFileStorageProviderData("XmlStorage", "c:\\mypath.config");
            Assert.IsNotNull(provider);
            provider.TypeName = xmlFileStorageTypeName;
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            xmlSerializer.Serialize(writer, provider);
            writer.Close();
            Assert.AreEqual(xmlString, configXml.ToString());
        }

        [Test]
        public void DeserializeAsXmlFileStorageProviderTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlFileStorageProviderData));
            XmlFileStorageProviderData settings = xmlSerializer.Deserialize(xmlReader) as XmlFileStorageProviderData;
            Assert.IsNotNull(settings);
            Assert.AreEqual("XmlStorage", settings.Name);
            Type storageProviderType = Type.GetType(settings.TypeName, true);
            Assert.AreEqual(typeof(XmlFileStorageProvider), storageProviderType);
            Assert.AreEqual("c:\\mypath.config", settings.Path);
        }

        [Test]
        public void SerializeAsXmlFileStorageProviderTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlFileStorageProviderData));
            XmlFileStorageProviderData provider = new XmlFileStorageProviderData("XmlStorage");
            Assert.IsNotNull(provider);
            provider.TypeName = xmlFileStorageTypeName;
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            xmlSerializer.Serialize(writer, provider);
            writer.Close();
            Assert.AreEqual(xmlNoTypeString, configXml.ToString());
        }

        [Test]
        public void DefaultValuesTest()
        {
            XmlFileStorageProviderData data = new XmlFileStorageProviderData(string.Empty);
            Assert.AreEqual(string.Empty, data.Name);
            Assert.AreEqual(typeof(XmlFileStorageProvider).AssemblyQualifiedName, data.TypeName);
            Assert.AreEqual(string.Empty, data.Path);
        }
    }
}

#endif