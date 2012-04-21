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
    public class DatabaseTypeDataFixture
    {
        private static readonly string name = "SqlServer";
        private static readonly string type = "Microsoft.Practices.EnterpriseLibrary.Data.SqlDatabase, Microsoft.Practices.EnterpriseLibrary.Data";

        private static readonly string xmlString = "<databaseType name=\"" + name + "\" type=\"" + type + "\" xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\" />";

        private DatabaseTypeData databaseType;

        [TestFixtureSetUp]
        public void Init()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DatabaseTypeData));
            databaseType = xmlSerializer.Deserialize(xmlReader) as DatabaseTypeData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(databaseType);
        }

        [Test]
        public void Name()
        {
            Assert.AreEqual(name, databaseType.Name);
        }

        [Test]
        public void Type()
        {
            Assert.AreEqual(type, databaseType.TypeName);
        }
    }
}

#endif