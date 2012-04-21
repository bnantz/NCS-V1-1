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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class EmailSinkDataFixture
    {
        private static readonly string nodeName = "DbSink";
        private static readonly string nodeType = "myType";
        private static readonly string toAddress = "to@email.com";
        private static readonly string fromAddress = "from@email.com";
        private static readonly string smtpServer = "myServer";

        private static readonly string xmlString =
            "<sink " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + DistributorSettings.ConfigurationNamespace + "\" " +
                "xsi:type=\"EmailSinkData\" " +
                "name=\"" + nodeName + "\" " +
                "type=\"" + nodeType + "\" " +
                "toAddress=\"" + toAddress + "\" " +
                "fromAddress=\"" + fromAddress + "\" " +
                "smtpServer=\"" + smtpServer + "\"/>";

        private EmailSinkData data;

        [TestFixtureSetUp]
        public void Init()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xmlString));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SinkData));
            this.data = xmlSerializer.Deserialize(xmlReader) as EmailSinkData;
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(this.data);
        }
    }
}

#endif