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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestFixture]
    public class CustomLogEntryFixture
    {
        [TearDown]
        public void TearDown()
        {
            SetInProcDistributionStrategy();
            MockLogSink.Clear();
        }

        [Test]
        public void SendToMockDistributionStrategy()
        {
            SetMockDistributionStrategy();
            CustomLogEntry customLog = GetCustomLogEntry();

            Logger.Write(customLog);

            CustomLogEntry mockEntry = (CustomLogEntry)MockDistributionStrategy.Entry;
            Assert.AreEqual(customLog.AcmeCoField1, mockEntry.AcmeCoField1);
            Assert.AreEqual(customLog.AcmeCoField2, mockEntry.AcmeCoField2);
            Assert.AreEqual(customLog.AcmeCoField3, mockEntry.AcmeCoField3);
            Assert.AreEqual(customLog.Category, mockEntry.Category);
            Assert.AreEqual(customLog.Message, mockEntry.Message);
        }

        [Test]
        public void SendToMockSink()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customLog = GetCustomLogEntry();
            Logger.Write(customLog);

            CustomLogEntry deserializedLog = (CustomLogEntry)MockLogSink.GetLastEntry();

            Assert.AreEqual(customLog.AcmeCoField1, deserializedLog.AcmeCoField1);
            Assert.AreEqual(customLog.AcmeCoField2, deserializedLog.AcmeCoField2);
            Assert.AreEqual(customLog.AcmeCoField3, deserializedLog.AcmeCoField3);
            Assert.AreEqual(customLog.Category, deserializedLog.Category);
            Assert.AreEqual(customLog.Message, deserializedLog.Message);
        }

        [Test]
        public void SendToEventLog()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customLog = GetCustomLogEntry();
            customLog.Category = "AppTest";
            customLog.TimeStamp = DateTime.MaxValue;
            customLog.MachineName = "machine";
            Logger.Write(customLog);

            string expected = "<EntLibLog>\r\n\t<message>My message body</message>\r\n\t<timestamp>12/31/9999 11:59:59 PM</timestamp>\r\n\t<title>My Custom Log Entry Title</title>\r\n</EntLibLog>";
            string entry = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, entry);
        }

        [Test]
        public void SendToFormattedCategory()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customLog = GetCustomLogEntry();
            customLog.Category = "FormattedCategory";
            Logger.Write(customLog);

            string expected = customLog.TimeStamp +
                ": " + customLog.Title +
                "\r\n\r\n" + customLog.Message;
            string entry = CommonUtil.GetLastEventLogEntryCustom();
            Assert.IsTrue(entry.IndexOf(expected) > -1);
        }

        private CustomLogEntry GetCustomLogEntry()
        {
            CustomLogEntry customLog = new CustomLogEntry();
            customLog.Category = "MockCategoryOne";
            customLog.EventId = CommonUtil.MsgEventID;
            customLog.Message = CommonUtil.MsgBody;
            customLog.Title = "My Custom Log Entry Title";
            customLog.AcmeCoField1 = "Red";
            customLog.AcmeCoField2 = "Blue";
            customLog.AcmeCoField3 = "Green";
            customLog.TimeStamp = DateTime.MaxValue;

            return customLog;
        }

        [Test]
        public void SendToCustomLogEntrySink()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customLog = GetCustomLogEntry();
            customLog.Category = "CustomMessageCategory";

            Logger.Write(customLog);

            Assert.AreEqual(customLog.AcmeCoField1, CustomLogEntrySink.Field1);
            Assert.AreEqual(customLog.AcmeCoField2, CustomLogEntrySink.Field2);
            Assert.AreEqual(customLog.AcmeCoField3, CustomLogEntrySink.Field3);
        }

        [Test]
        public void SendRegularLogEntryToCustomLogEntrySink()
        {
            SetInProcDistributionStrategy();

            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Category = "CustomMessageCategory";

            Logger.Write(logEntry);

            Assert.AreEqual(CustomLogEntrySink.Category, "CustomMessageCategory");
        }

        [Test]
        public void SendToCustomLogEntrySinkAndMockSink()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customEntry = GetCustomLogEntry();
            customEntry.Category = "MixedCategory";

            Logger.Write(customEntry);

            Assert.AreEqual(CustomLogEntrySink.fullMessage, MockLogSink.FormatLogEntry(MockLogSink.GetLastEntry()));
            Assert.AreEqual(CustomLogEntrySink.Body, MockLogSink.GetLastEntry().Message);
            Assert.AreEqual(CustomLogEntrySink.EventID, MockLogSink.GetLastEntry().EventId);
            Assert.AreEqual(CustomLogEntrySink.Category, MockLogSink.GetLastEntry().Category);
        }

        [Test]
        public void CustomTextFormatter()
        {
            SetInProcDistributionStrategy();

            CustomLogEntry customEntry = GetCustomLogEntry();
            customEntry.Category = "CustomFormattedCategory";

            Logger.Write(customEntry);

            string expected = "Timestamp: " + customEntry.TimeStampString +
                "\r\nTitle: My Custom Log Entry Title\r\n\r\nAcme Field1: Red\r\nAcme Field2: Blue\r\nAcme Field3: Green\r\n\r\nMessage: My message body";

            string entry = CommonUtil.GetLastEventLogEntryCustom();
            Assert.IsTrue(entry.IndexOf(expected) > -1);
        }

        private void SetMockDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("MockStrategy");
        }

        private void SetInProcDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("InProc");
        }
    }
}

#endif