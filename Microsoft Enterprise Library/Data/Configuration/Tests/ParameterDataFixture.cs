//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class ParameterDataFixture
    {
        private static readonly string name = "MyName";
        private static readonly string val = "MyVal";

        private static readonly string xmlString =
            "<parameter name=\"" + name + "\" value=\"" + val + "\" xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\" />";

        private static readonly string xmlStringSensitive =
            "<parameter name=\"" + name + "\" value=\"" + val + "\" isSensitive=\"true\" xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\" />";

        private ParameterData parameter;

        [TestFixtureSetUp]
        public void Initialize()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParameterData));
            parameter = xmlSerializer.Deserialize(xmlReader) as ParameterData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(parameter);
        }

        [Test]
        public void DatabaseName()
        {
            Assert.AreEqual(name, parameter.Name);
        }

        [Test]
        public void Type()
        {
            Assert.AreEqual(val, parameter.Value);
        }

        [Test]
        public void Sensitive()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlStringSensitive));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParameterData));
            ParameterData sensitiveParameter = xmlSerializer.Deserialize(xmlReader) as ParameterData;
            Assert.AreEqual(true, sensitiveParameter.IsSensitive);
        }
    }
}

#endif