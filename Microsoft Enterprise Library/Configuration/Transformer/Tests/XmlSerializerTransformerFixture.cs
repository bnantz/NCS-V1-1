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
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer.Tests
{
    [TestFixture]
    public class XmlSerializerTransformerFixture : ConfigurationContextFixtureBase
    {
        private static readonly string transformerName = "MockConfig";
        private static readonly string transformerType = typeof(MockTransformer).AssemblyQualifiedName;
        private static readonly string includeName = "string";
        private static readonly string includeType = "System.String, mscorlib";

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection " +
                "type=\"" + typeof(XmlSerializerTransformerData).AssemblyQualifiedName + "\">" +
                "<dataTransformer " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "name=\"" + transformerName + "\" " +
                "xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\">" +
                "<includeTypes>" +
                "<includeType name=\"" + includeName + "\" type=\"" + includeType + "\" />" +
                "</includeTypes>" +
                "</dataTransformer>" +
                "</xmlSerializerSection>";

        private static readonly string xmlNoSection =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<badTest/>";

        private static readonly string xmlNoTypeInSection =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection/>";

        private static readonly string xmlBadTypeInSection =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection type=\"NotRealType, Microsoft.Practices.EnterpriseLibrary.Configuration\"/>";

        private static readonly string xmlBadTypeFileInSection =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection type=\"NotRealType, NotRealType\"/>";

        private static readonly string xmlNonCreatableBuilder =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection " +
                "type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.Tests.NonCreatableTransformer, Microsoft.Practices.EnterpriseLibrary.Configuration\">" +
                "</xmlSerializerSection>";

        private static readonly string xmlBadConstructionBuilder =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection " +
                "type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.Tests.BadConstructionTransformer, Microsoft.Practices.EnterpriseLibrary.Configuration\">" +
                "</xmlSerializerSection>";

        private static readonly string xmlBadTypeString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlSerializerSection " +
                "type=\"Microsoft.Practices.EnterpriseLibrary.Configuration.XmlSerializerTransformerData, Microsoft.Practices.EnterpriseLibrary.Configuration\">" +
                "<dataTransformer " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:type=\"MyBadType\" " +
                "name=\"" + transformerName + "\" " +
                "type=\"" + transformerType + "\">" +
                "<includeTypes>" +
                "<includeType name=\"" + includeName + "\" type=\"" + includeType + "\" />" +
                "</includeTypes>" +
                "</dataTransformer>" +
                "</xmlSerializerSection>";

        private ITransformer transformer;

        [SetUp]
        public void CreateTransformer()
        {
            XmlSerializerTransformer xmlTransformer = new XmlSerializerTransformer();
            // just a fake out to test the transformer
        	RuntimeConfigurationView configurationView = new RuntimeConfigurationView(Context);
            xmlTransformer.CurrentSectionName = "ApplConfig1";
            xmlTransformer.Initialize(configurationView);
            transformer = xmlTransformer;
        }

        [Test]
        public void SectionHandlerTest()
        {
            XmlSerializerTransformerData settings = transformer.Deserialize(GetSection(xmlString)) as XmlSerializerTransformerData;
            Assert.IsNotNull(settings);
            Assert.AreEqual(transformerName, settings.Name);
            Assert.AreEqual(typeof(XmlSerializerTransformer).AssemblyQualifiedName, settings.TypeName);
            Assert.AreEqual(includeType, settings.XmlIncludeTypes[includeName].TypeName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSectionTest()
        {
            transformer.Deserialize(null);
            Assert.Fail("Should never reach here because we should get a ArgumentNullException");
        }

        [Test]
        public void SectionHandlerWriterTest()
        {
            XmlSerializerTransformerData settings = new XmlSerializerTransformerData(transformerName);
            Assert.IsNotNull(settings);
            settings.TypeName = transformerType;
            settings.XmlIncludeTypes.Add(new XmlIncludeTypeData(includeName, includeType));
            XmlNode node = (XmlNode)transformer.Serialize(settings);
            XmlNode expectedNode = GetSection(xmlString);
            Assert.AreEqual(expectedNode.OuterXml, node.OuterXml);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadSectionTest()
        {
            transformer.Deserialize(GetSection(xmlNoSection));
            Assert.Fail("Should never reach here because we should get a ConfigurationException");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoTypeInSectionTest()
        {
            transformer.Deserialize(GetSection(xmlNoTypeInSection));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadTypeInSectionTest()
        {
            transformer.Deserialize(GetSection(xmlBadTypeInSection));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadTypeFileInSectionTest()
        {
            transformer.Deserialize(GetSection(xmlBadTypeFileInSection));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NonCreatableTransformerTest()
        {
            transformer.Deserialize(GetSection(xmlNonCreatableBuilder));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadTypeTest()
        {
            transformer.Deserialize(GetSection(xmlNonCreatableBuilder));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadConstructionTrasnformerTest()
        {
            transformer.Deserialize(GetSection(xmlBadConstructionBuilder));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadXsiTypeTest()
        {
            transformer.Deserialize(GetSection(xmlBadTypeString));
        }

        private XmlNode GetSection(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}

#endif