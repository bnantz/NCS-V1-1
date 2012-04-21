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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class XmlSerializerTransformerDataFixture
    {
        private static readonly string transformerName = "MockConfig";
        private static readonly string transformerType = typeof(XmlSerializerTransformer).AssemblyQualifiedName;
        private static readonly string includeName = "myName";
        private static readonly string includeType = "myType";

        private static readonly string xmlBaseString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<dataTransformer " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"" + transformerName + "\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\">" +
                "<includeTypes>" +
                "<includeType name=\"" + includeName + "\" type=\"" + includeType + "\" />" +
                "</includeTypes>" +
                "</dataTransformer>";

        [Test]
        public void DeserializeTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlBaseString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TransformerData));
            TransformerData transformer = xmlSerializer.Deserialize(xmlReader) as TransformerData;
            Assert.IsNotNull(transformer);
            Assert.AreEqual(transformerName, transformer.Name);
            Assert.AreEqual(transformerType, transformer.TypeName);
        }

        [Test]
        public void SerializeTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TransformerData));
            XmlSerializerTransformerData settings = new XmlSerializerTransformerData(transformerName);
            Assert.IsNotNull(settings);
            settings.TypeName = transformerType;
            settings.XmlIncludeTypes.Add(new XmlIncludeTypeData(includeName, includeType));
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            xmlSerializer.Serialize(writer, settings);
            writer.Close();
            Assert.AreEqual(xmlBaseString, configXml.ToString());
        }
    }
}

#endif