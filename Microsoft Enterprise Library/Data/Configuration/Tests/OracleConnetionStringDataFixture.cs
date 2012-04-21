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
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class OracleConnetionStringDataFixture
    {
        private static readonly string name = "ENTLIBCON";
        private static readonly string sspiName = "Integrated Security";
        private static readonly string sspiValue = "true";
        private static readonly string paramName = "Name";
        private static readonly string paramValue = "Value";
        private static readonly string prefix = "MyStoredProc";
        private static readonly string packageName = "ENTLIB";

        private static readonly string oracleXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<connectionString xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:type=\"OracleConnectionStringData\" " +
                "name=\"" + name + "\" " +
                "xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\">" +
                "<parameters>" +
                "<parameter name=\"" + sspiName + "\" value=\"" + sspiValue + "\" isSensitive=\"false\" />" +
                "<parameter name=\"" + paramName + "\" value=\"" + paramValue + "\" isSensitive=\"false\" />" +
                "</parameters>" +
                "<packages>" +
                "<package prefix=\"MyStoredProc\" name=\"ENTLIB\" />" +
                "</packages>" +
                "</connectionString>";

        private OracleConnectionStringData oracleConnectionString;

        [TestFixtureSetUp]
        public void Initialize()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(oracleXmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConnectionStringData));
            oracleConnectionString = xmlSerializer.Deserialize(xmlReader) as OracleConnectionStringData;
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(name, oracleConnectionString.Name);
        }

        [Test]
        public void PackagesTest()
        {
            Assert.AreEqual(1, oracleConnectionString.OraclePackages.Count);
        }

        [Test]
        public void ParamSettingsCountTest()
        {
            Assert.AreEqual(2, oracleConnectionString.Parameters.Count);
        }

        [Test]
        public void SspiValueTest()
        {
            Assert.AreEqual(sspiValue, oracleConnectionString.Parameters[sspiName].Value);
        }

        [Test]
        public void ParamValueTest()
        {
            Assert.AreEqual(paramValue, oracleConnectionString.Parameters[paramName].Value);
        }

        [Test]
        public void SerializeTest()
        {
            OracleConnectionStringData connectionString = new OracleConnectionStringData(name);
            connectionString.OraclePackages.Add(new OraclePackageData(packageName, prefix));
            connectionString.Parameters.Add(new ParameterData(sspiName, sspiValue));
            connectionString.Parameters.Add(new ParameterData(paramName, paramValue));
            XmlSerializer serializer = new XmlSerializer(typeof(ConnectionStringData));
            StringWriter stringWriter = new StringWriter(CultureInfo.CurrentUICulture);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            try
            {
                serializer.Serialize(xmlTextWriter, connectionString);
                xmlTextWriter.Flush();
                string xml = stringWriter.ToString();
                Assert.AreEqual(oracleXmlString, xml);
            }
            finally
            {
                xmlTextWriter.Close();
                stringWriter.Close();
            }
        }
    }
}

#endif