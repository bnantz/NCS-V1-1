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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.Tests
{
    [TestFixture]
    public class KeyAlgorithmStorageProviderFactoryFixture
    {
        private static readonly string xmlFileStorageTypeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static readonly string transformerTypeName = typeof(XmlSerializerTransformerData).AssemblyQualifiedName;
        private static readonly string fileStorage = typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"ApplConfig1\">" +
                "<cache enabled=\"true\" refresh=\"1 * * * *\" />" +
                "<storageProvider xsi:type=\"XmlFileStorageProviderData\" name=\"XmlStorage\" " +
                "type=\"" + xmlFileStorageTypeName + "\" " +
                "path=\"storagefactoryconfig.xml\" " +
                "encrypted=\"false\" " +
                "signed=\"false\" " +
                "refreshOnChange=\"false\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerTypeName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "<dpapiSettings mode=\"Machine\" entropy=\"AAECAwQFBgcICQABAgMFBA==\" />" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        [Test]
        public void CreateTest()
        {
            using (ConfigurationContext context = GetConfigurationContext(xmlString))
            {
                KeyAlgorithmStorageProviderFactory factory = new KeyAlgorithmStorageProviderFactory(context);
                IKeyAlgorithmPairStorageProvider keyAlgorithmPairStorageProvider = factory.Create();
                Assert.IsNotNull(keyAlgorithmPairStorageProvider);
            }
        }

        private ConfigurationContext GetConfigurationContext(string xml)
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