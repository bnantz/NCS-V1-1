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
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class StorageProviderFactoryFixture
    {
        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider xsi:type=\"XmlFileStorageProviderData\" name=\"XmlStorage\" " +
                "path=\"storagefactoryconfig.xml\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string notImplXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<cache enabled=\"true\" refresh=\"1 * * * *\" />" +
                "<storageProvider xsi:type=\"NotRealStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"storagefactoryconfig.xml\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string badTypeXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<storageProvider name=\"XmlStorage\" " +
                "xsi:type=\"XmlFileStorageProviderData2\" " +
                "path=\"storagefactoryconfig.xml\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string privateConstructorTypeXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<cache enabled=\"true\" refresh=\"1 * * * *\" />" +
                "<storageProvider xsi:type=\"PrivateConstructorStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"storagefactoryconfig.xml\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string exceptionConstructorTypeXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<cache enabled=\"true\" refresh=\"1 * * * *\" />" +
                "<storageProvider xsi:type=\"ExceptionConstructorStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "path=\"StorageFactoryConfig.xml\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        [Test]
        public void CreateTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(xmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create("ApplConfig1");
                Assert.IsNotNull(storage);
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void PrivateConstructorCreateTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(privateConstructorTypeXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create("ApplConfig1");
                Assert.Fail("Should never reach here because we should get a ConfigurationException.");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ExceptionConstructorCreateTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(exceptionConstructorTypeXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create("ApplConfig1");
                Assert.Fail("Should never reach here because we should get a ConfigurationException.");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void TypeDoesNotImplementTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(notImplXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create("ApplConfig1");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadSectionTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(notImplXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create("Foo");
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BadTypeTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(badTypeXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create("ApplConfig1");
                Assert.Fail("Should not get here");
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