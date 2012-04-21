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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestFixture]
    public class DistributionStrategyDataFixture
    {
        private static readonly string name = "MyDistributionStrategy";
        private static readonly string type = typeof(InProcLogDistributionStrategy).AssemblyQualifiedName;

        private static readonly string xmlString =
            "<distributionStrategy " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + LoggingSettings.ConfigurationNamespace + "\" " +
                "xsi:type=\"MsmqDistributionStrategyData\" " +
                "name=\"" + name + "\" " +
                "type=\"" + type + "\" " +
                "queuePath=\".\\Private$\\entlib\"/>";

        private DistributionStrategyData strategy;

        [TestFixtureSetUp]
        public void Init()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DistributionStrategyData));
            strategy = xmlSerializer.Deserialize(xmlReader) as DistributionStrategyData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(strategy);
        }

        [Test]
        public void PropertiesTest()
        {
            Assert.AreEqual(name, strategy.Name);
        }

        [Test]
        public void CustomPropertiesTest()
        {
            NameValueItem testItem = new NameValueItem("TEST", "VALUE");

            CustomDistributionStrategyData data = new CustomDistributionStrategyData();
            data.Attributes.Add(testItem);

            Assert.AreEqual(testItem, data.Attributes[0]);
        }
    }
}

#endif