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
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    public sealed class ConfigurationSettingsBuilder
    {
        private static readonly string typeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static readonly string transformerTypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;
        private static readonly string fileStorage = typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\">" +
                "<configurationSections>" +
                "<configurationSection name=\"Test\" encrypt=\"false\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + typeName + "\" " +
                "path=\"c:\\MyFile.config\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"XmlTransformer\" " +
                "type=\"" + transformerTypeName + "\">" +
                "<includeTypes />" +
                "</dataTransformer>" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "<dpapiSettings mode=\"Machine\" entropy=\"AAECAwQFBgcICQABAgMFBg==\" />" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        private ConfigurationSettingsBuilder()
        {
        }

        public static ConfigurationSettings Create()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            return xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
        }
    }
}

#endif