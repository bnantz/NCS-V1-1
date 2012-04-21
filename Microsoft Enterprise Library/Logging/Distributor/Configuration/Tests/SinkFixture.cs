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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class SinkFixture
    {
        [Test]
        public void BasePropertiesTest()
        {
            SinkData data = new CustomSinkData();
            string typeName = "Test Type";
            string name = "Test Name";

            data.TypeName = typeName;
            data.Name = name;

            Assert.AreEqual(typeName, data.TypeName);
            Assert.AreEqual(name, data.Name);
        }

        [Test]
        public void CustomSinkDataPropertiesTest()
        {
            CustomSinkData data = new CustomSinkData();
            NameValueItemCollection attributes = new NameValueItemCollection();
            attributes.Add(new NameValueItem("TEST", "test"));

            data.Attributes.Add(attributes[0]);

            Assert.AreEqual(attributes["TEST"], data.Attributes["TEST"]);
        }

        [Test]
        public void EventLogSinkDataPropertiesTest()
        {
            EventLogSinkData data = new EventLogSinkData();
            string eventSourceName = "Application";

            data.EventSourceName = eventSourceName;

            Assert.AreEqual(eventSourceName, data.EventSourceName);
        }

        [Test]
        public void FlatFileSinkDataPropertiesTest()
        {
            FlatFileSinkData data = new FlatFileSinkData();
            string fileName = "Filename";
            string footer = "Footer";
            string header = "Header";

            data.FileName = fileName;
            data.Footer = footer;
            data.Header = header;

            Assert.AreEqual(fileName, data.FileName);
            Assert.AreEqual(footer, data.Footer);
            Assert.AreEqual(header, data.Header);
        }

        [Test]
        public void EmailSinkDataPropertiesTest()
        {
            EmailSinkData data = new EmailSinkData();
            string fromAddress = "From Address";
            string smtpServer = "Smtp Server";
            string subjectLineEnder = "Subject Line ENd";
            string subjectLineStarter = "Subject Line Start";
            string toAddress = "To Address";

            data.FromAddress = fromAddress;
            data.SmtpServer = smtpServer;
            data.SubjectLineEnder = subjectLineEnder;
            data.SubjectLineStarter = subjectLineStarter;
            data.ToAddress = toAddress;

            Assert.AreEqual(fromAddress, data.FromAddress);
            Assert.AreEqual(smtpServer, data.SmtpServer);
            Assert.AreEqual(subjectLineEnder, data.SubjectLineEnder);
            Assert.AreEqual(subjectLineStarter, data.SubjectLineStarter);
            Assert.AreEqual(toAddress, data.ToAddress);
        }

        [Test]
        public void MsmqSinkDataPropertiesTest()
        {
            MsmqSinkData data = new MsmqSinkData();
            string queuePath = "Path";

            data.QueuePath = queuePath;

            Assert.AreEqual(queuePath, data.QueuePath);
        }
    }
}

#endif