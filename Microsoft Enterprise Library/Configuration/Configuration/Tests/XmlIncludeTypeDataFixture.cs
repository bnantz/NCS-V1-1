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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class XmlIncludeTypeDataFixture
    {
        private static readonly string name = "string";
        private static readonly string type = "System.String, mscorlib";

        private static readonly string xmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<xmlIncludeType xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "name=\"" + name + "\" " +
                "type=\"" + type + "\" " +
                "xmlns=\"" + ConfigurationSettings.ConfigurationNamespace + "\" />";

        private XmlIncludeTypeData typeData;

        [TestFixtureSetUp]
        public void Initialize()
        {
            typeData = new XmlIncludeTypeData(name, type);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(name, typeData.Name);
        }

        [Test]
        public void TypeTest()
        {
            Assert.AreEqual(type, typeData.TypeName);
        }

        [Test]
        public void CreateType()
        {
            Type t = Type.GetType(typeData.TypeName, true);
            Assert.AreSame(t, typeof(string));
        }

        [Test]
        public void DeserializeTest()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlIncludeTypeData));
            XmlIncludeTypeData typeData = xmlSerializer.Deserialize(xmlReader) as XmlIncludeTypeData;
            Assert.IsNotNull(typeData);
            Assert.AreEqual(name, typeData.Name);
            Assert.AreEqual(type, typeData.TypeName);
        }

        [Test]
        public void SerializeTest()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlIncludeTypeData));
            StringBuilder configXml = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(configXml, CultureInfo.CurrentCulture));
            writer.Formatting = Formatting.None;
            XmlIncludeTypeData typeData = new XmlIncludeTypeData(name, type);
            xmlSerializer.Serialize(writer, typeData);
            writer.Close();
            Assert.AreEqual(xmlString, configXml.ToString());
        }
    }
}

#endif