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
    public class OraclePackageDataFixture
    {
        private static readonly string prefix = "MyStoredProc";
        private static readonly string name = "ENTLIB";

        private static readonly string xmlString =
            "<package prefix=\"" + prefix + "\" name=\"" + name + "\" xmlns=\"" + DatabaseSettings.ConfigurationNamespace + "\" />";

        private OraclePackageData package;

        [TestFixtureSetUp]
        public void Initialize()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OraclePackageData));
            package = xmlSerializer.Deserialize(xmlReader) as OraclePackageData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(package);
        }

        [Test]
        public void Name()
        {
            Assert.AreEqual(name, package.Name);
        }

        [Test]
        public void Prefix()
        {
            Assert.AreEqual(prefix, package.Prefix);
        }
    }
}

#endif