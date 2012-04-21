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

#if    UNIT_TESTS
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class TransformerFactoryFixture
    {
        private static readonly string storageTypeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static readonly string transformerTypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;
        private static readonly string xmlSerializerTransformerTypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerTypeName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string notImplXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"false\" />" +
                "<dataTransformer " +
                "xsi:type=\"NotRealTransformerData\" " +
                "name=\"NotReal\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string badTypeXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"false\" />" +
                "<dataTransformer " +
                "xsi:type=\"NoSuchData\" " +
                "name=\"NoSuch\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string noTransformerXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"false\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string typedXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"TypedConfig\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlSerializerTransformer\" " +
                "type=\"" + xmlSerializerTransformerTypeName + "\" >" +
                "<includeTypes>" +
                "<includeType name=\"string\" type=\"System.String, mscorlib\" />" +
                "</includeTypes>" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        [Test]
        public void CreateTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(xmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                ITransformer transformer = factory.Create("ApplConfig1");
                Assert.IsNotNull(transformer);
            }
        }

        [Test]
        public void CreateTypedTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(typedXmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                ITransformer transformer = factory.Create("TypedConfig");
                Assert.IsNotNull(transformer);
                XmlSerializerTransformer xmlTransformer = transformer as XmlSerializerTransformer;
                Assert.IsNotNull(xmlTransformer);
                Assert.AreEqual(1, xmlTransformer.GetTypes().Length);
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void TypeDoesNotImplementTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(notImplXmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                factory.Create("ApplConfig1");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadSectionTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(notImplXmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                factory.Create("Foo");
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BadTypeTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(badTypeXmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                factory.Create("ApplConfig1");
            }
        }

        [Test]
        public void NoTransformerDefinedTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(noTransformerXmlString))
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                ITransformer transformer = factory.Create("ApplConfig1");
                Assert.AreEqual(typeof(NullTransformer), transformer.GetType());
            }
        }

        private ConfigurationContext CreateConfigurationContext(string xml)
        {
            ConfigurationSettings settings = GetConfigurationSettings(xml);
            ConfigurationBuilder builder = new ConfigurationBuilder(settings);
            return new ConfigurationContext(new DisposingWrapper(builder)); 
        }

        private ConfigurationSettings GetConfigurationSettings(string xml)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            return xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
        }
    }
}

#endif