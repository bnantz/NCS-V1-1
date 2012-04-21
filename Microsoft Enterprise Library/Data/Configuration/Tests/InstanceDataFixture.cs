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
    public class InstanceDataFixture
    {
        private static readonly string name = "Northwind";
        private static readonly string type = "SqlServer";
        private static readonly string connectionString = "Foo";

        private static readonly string xmlString =
            "<instance name=\"" + name + "\" type=\"" + type + "\" connectionString=\"" + connectionString + "\" xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\" />";

        private InstanceData instance;

        [TestFixtureSetUp]
        public void Init()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(InstanceData));
            instance = xmlSerializer.Deserialize(xmlReader) as InstanceData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(instance);
        }

        [Test]
        public void DatabaseName()
        {
            Assert.AreEqual(name, instance.Name);
        }

        [Test]
        public void Type()
        {
            Assert.AreEqual(type, instance.DatabaseTypeName);
        }

        [Test]
        public void ConnectionString()
        {
            Assert.AreEqual(connectionString, instance.ConnectionString);
        }
    }
}

#endif