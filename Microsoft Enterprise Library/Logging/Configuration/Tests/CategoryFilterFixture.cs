//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestFixture]
    public class CategoryFilterFixture
    {
        private static readonly string name = "MyCategoryFilter";

        private static readonly string xmlString =
            "<categoryFilter name=\"" + name + "\" xmlns=\"" + LoggingSettings.ConfigurationNamespace + "\"/>";

        private CategoryFilterData filter;

        [TestFixtureSetUp]
        public void Init()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoryFilterData));
            filter = xmlSerializer.Deserialize(xmlReader) as CategoryFilterData;

        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(filter);
        }

        [Test]
        public void CategoryFilterName()
        {
            Assert.AreEqual(name, filter.Name);
        }
    }
}

#endif