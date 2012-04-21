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
using System;
using System.Web.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    [TestFixture]
    public class EmailSinkFixture
    {
        private EmailSink sink;
        private EmailSinkData sinkParams;
        private static MailMessage lastMailMessageSent;

        [SetUp]
        public void Setup()
        {
            sinkParams = new EmailSinkData();
            sinkParams.ToAddress = "obviously.bad.email.address@127.0.0.1";
            sinkParams.FromAddress = "logging@entlib.com";
            sinkParams.SubjectLineStarter = "EntLib-Logging:";
            sinkParams.SubjectLineEnder = "has occurred";
            sinkParams.SmtpServer = "smtphost";
            this.sink = new EmailSink();
            this.sink.Initialize(new TestLogSinkConfigurationView(sinkParams));
            CommonUtil.ResetEventLogCounter();
            lastMailMessageSent = null;
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();
        }

        [Test]
        public void LogMessageToEmail()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            this.sink.SendMessage(logEntry);

            // confirm no messages written to event log
            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "send email using smtp server");

            // check your inbox
        }

        [Test]
        public void MissingRequiredParameters()
        {
            // create a flatfile sink without the required parameters
            EmailSink badSink = new EmailSink();
            badSink.Initialize(new TestLogSinkConfigurationView(new EmailSinkData()));
            badSink.SendMessage(CommonUtil.GetDefaultLogEntry());

            Assert.AreEqual(1, CommonUtil.EventLogEntryCount());

            string entry = CommonUtil.GetLastEventLogEntry();
            string expected = SR.DefaultLogDestinationMessage + Environment.NewLine + Environment.NewLine +
                "E-Mail Sink is missing one of these keys in the Distributor's XML file: ToAddress, FromAddress, and/or SmtpServer";
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");

            expected = CommonUtil.FormattedMessage;

            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm message is inside");
        }

        [Test]
        public void EmailContentsAreCorrect()
        {
            MockEmailSink mockSink = new MockEmailSink();
            mockSink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            LogEntry entry = new LogEntry("Test Message", "Test Category", 42, 999, Severity.Information, "Test Title", null);
            DateTime messageTimestamp = DateTime.Now;
            mockSink.SendMessage(entry);

            Assert.AreEqual("EntLib-Logging: Information has occurred", lastMailMessageSent.Subject);
            Assert.AreEqual("obviously.bad.email.address@127.0.0.1", lastMailMessageSent.To);
            Assert.AreEqual("logging@entlib.com", lastMailMessageSent.From);
            AssertContainsSubstring(lastMailMessageSent.Body, messageTimestamp.ToString());
        }

        [Test]
        public void SubjectIsCorrectWithNullSubjectStarterAndEnder()
        {
            sinkParams.SubjectLineEnder = null;
            sinkParams.SubjectLineStarter = null;

            MockEmailSink mockSink = new MockEmailSink();
            mockSink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            mockSink.SendMessage(entry);

            Assert.AreEqual("Unspecified", lastMailMessageSent.Subject);
        }

        [Test]
        public void SubjectIsCorrectWithEmptySubjectStarterAndEnder()
        {
            sinkParams.SubjectLineEnder = "";
            sinkParams.SubjectLineStarter = "";

            MockEmailSink mockSink = new MockEmailSink();
            mockSink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            mockSink.SendMessage(entry);

            Assert.AreEqual("Unspecified", lastMailMessageSent.Subject);
        }

        private void AssertContainsSubstring(string completeString, string subString)
        {
            Assert.IsTrue(completeString.IndexOf(subString) != -1);
        }

        private class MockEmailSink : EmailSink
        {
            public MockEmailSink() : base()
            {
            }

            internal override EmailMessage CreateEmailMessage(EmailSinkData sinkParameters, LogEntry logEntry, ILogFormatter formatter)
            {
                return new MockEmailMessage(sinkParameters, logEntry, formatter);
            }
        }

        private class MockEmailMessage : EmailMessage
        {
            public MockEmailMessage(EmailSinkData sinkParameters, LogEntry logEntry, ILogFormatter formatter) : base(sinkParameters, logEntry, formatter)
            {
            }

            internal override void SendMessage(MailMessage message)
            {
                EmailSinkFixture.lastMailMessageSent = message;
            }
        }
    }
}

#endif