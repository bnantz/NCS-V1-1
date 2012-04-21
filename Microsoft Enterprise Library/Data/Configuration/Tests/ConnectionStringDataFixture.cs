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

#if UNIT_TESTS
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class ConnectionStringDataFixture
    {
        private static readonly string name = "localhost";
        private static readonly string sspiName = "Integrated Security";
        private static readonly string sspiValue = "true";
        private static readonly string paramName = "Name";
        private static readonly string paramValue = "Value";

        private static readonly string xmlString =
            "<connectionString name=\"" + name + "\" " +
                "xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\">" +
                "<parameters>" +
                "<parameter name=\"" + sspiName + "\" value=\"" + sspiValue + "\" />" +
                "<parameter name=\"" + paramName + "\" value=\"" + paramValue + "\" />" +
                "</parameters>" +
                "</connectionString>";

        private ConnectionStringData connectionString;

        [TestFixtureSetUp]
        public void Initialize()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConnectionStringData));
            connectionString = xmlSerializer.Deserialize(xmlReader) as ConnectionStringData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(connectionString);
        }

        [Test]
        public void Name()
        {
            Assert.AreEqual(name, connectionString.Name);
        }

        [Test]
        public void ParamSettingsCount()
        {
            Assert.AreEqual(2, connectionString.Parameters.Count);
        }

        [Test]
        public void SspiValue()
        {
            Assert.AreEqual(sspiValue, connectionString.Parameters[sspiName].Value);
        }

        [Test]
        public void ParamValue()
        {
            Assert.AreEqual(paramValue, connectionString.Parameters[paramName].Value);
        }
    }
}

#endif