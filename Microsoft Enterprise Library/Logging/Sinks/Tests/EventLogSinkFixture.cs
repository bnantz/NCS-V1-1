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
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    [TestFixture]
    public class EventLogSinkFixture
    {
        [SetUp]
        public void Setup()
        {
            CommonUtil.ResetEventLogCounter();
            CommonUtil.ResetEventLogCounterCustom();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();
            CommonUtil.ResetEventLogCounterCustom();
        }

        [Test]
        public void DefaultConstructor()
        {
            EventLogSink sink = new EventLogSink();
            Assert.IsNotNull(sink);
        }

        [Test]
        public void SinkDataConstructor()
        {
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = CommonUtil.EventLogSourceName;

            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));
            Assert.IsNotNull(sink);
        }

        [Test]
        public void LogMessageToEventLog()
        {
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = CommonUtil.EventLogSourceName;
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));
            CommonUtil.SendTestMessage(sink);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom());

                Assert.AreEqual(CommonUtil.FormattedMessage, customLog.Entries[customLog.Entries.Count - 1].Message);
            }
        }

        [Test]
        public void LogMessageToEventLogWithCategoryId()
        {
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = CommonUtil.EventLogSourceName;
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.EventId = 1234;
            logEntry.ExtendedProperties["CategoryID"] = "4567";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual("Enterprise Library Unit Tests", entry.Source);
                Assert.AreEqual(1234, entry.EventID);
                Assert.AreEqual(4567, entry.CategoryNumber);
            }
        }

        [Test]
        public void LogMessageToEventLogWithCategoryIdSpellings()
        {
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = CommonUtil.EventLogSourceName;
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.ExtendedProperties["CategoryID"] = "4567";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual(4567, entry.CategoryNumber);
            }

            logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.ExtendedProperties["CategoryId"] = "890";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual(890, entry.CategoryNumber);
            }

            logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.ExtendedProperties["categoryId"] = "321";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual(321, entry.CategoryNumber);
            }

            logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.ExtendedProperties["categoryid"] = "654";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual(654, entry.CategoryNumber);
            }

            logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.ExtendedProperties = new Hashtable();
            logEntry.ExtendedProperties["cATEGoryid"] = "0";
            sink.SendMessage(logEntry);

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                EventLogEntry entry = customLog.Entries[customLog.Entries.Count - 1];
                Assert.AreEqual(0, entry.CategoryNumber);
            }
        }

        [Test]
        public void SendTextFormattedMessage()
        {
			TextFormatter formatter = CommonUtil.CreateTextFormatter("Timestamp: {timestamp}\n\nTitle: {title}\n\nBody: {message}");
			
			EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = CommonUtil.EventLogSourceName;

			EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));
			sink.Formatter = formatter;

        	LogEntry entry = CommonUtil.GetDefaultLogEntry();
            sink.SendMessage(entry);

			string expected = "Timestamp: " + entry.TimeStampString + "\n\nTitle: " + entry.Title + "\n\nBody: " + entry.Message;
            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(expected, customLog.Entries[customLog.Entries.Count - 1].Message);
            }
        }

    	[Test]
        public void SendWithEmptyEventSource()
        {
            // create an invalid set of params
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventSourceName = string.Empty;
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(sink);

            Assert.AreEqual(1, CommonUtil.EventLogEntryCount(), "confirm there is one new entry in the Application event log");

//            string expected = SR.DefaultLogDestinationMessage + Environment.NewLine + Environment.NewLine +
//                "Event Log Sink is missing the key -EventSourceName- in the Distributor's configuration file.\r\n\r\nMessage: \r\nTimestamp: 12/31/9999 11:59:59 PM\r\nMessage: My message body\r\nCategory: foo\r\nPriority: 100\r\nEventId: 1\r\nSeverity: Unspecified\r\nTitle:=== Header ===\r\nMachine: machine" +
//                "\r\nApplication: domain-Microsoft.Practices.EnterpriseLibrary.Logging.dll\r\nAssembly: " + Assembly.GetExecutingAssembly().FullName +
//                "\r\nThread: " + AppDomain.GetCurrentThreadId().ToString() + "\r\nUser: " + Environment.UserDomainName + @"\" + Environment.UserName + "\r\nExtended Properties: ";

            string expected = SR.DefaultLogDestinationMessage + Environment.NewLine + Environment.NewLine
                + "Event Log Sink is missing the key -EventSourceName- in the Distributor's configuration file.\r\n\r\nMessage: \r\n"
                + CommonUtil.FormattedMessage;

            string actual = CommonUtil.GetLastEventLogEntry();
            Assert.AreEqual(expected, actual, "confirm the one entry in the log is the error");
        }

        [Test]
        public void SendWithCustomEventSource()
        {
            string expected = "MyCustomSource";
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = CommonUtil.EventLogNameCustom;
            sinkParams.EventSourceName = expected;
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(sink);

            Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom(), "confirm there is only one entry in the event log");

            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                string actual = customLog.Entries[customLog.Entries.Count - 1].Source;
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void SendToCustomEventLog()
        {
            string eventLogName = "EntLib Tests";
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = eventLogName;
            sinkParams.EventSourceName = "EntLibSource";
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            int count = 0;
            EventLog entlibLog = null;
            try
            {
                entlibLog = new EventLog(eventLogName);
                count = entlibLog.Entries.Count;
            }
            catch (InvalidOperationException)
            { /* event log doesn't exist yet */
            }

            CommonUtil.SendTestMessage(sink);

            entlibLog = new EventLog(eventLogName);

            Assert.AreEqual(1, entlibLog.Entries.Count - count, "confirm there is only one entry in the event log");

            string actual = entlibLog.Entries[entlibLog.Entries.Count - 1].Message;

            Assert.AreEqual(CommonUtil.FormattedMessage, actual);
        }

        [Test]
        public void SendWithEmptyEventLog()
        {
            EventLogSinkData sinkParams = new EventLogSinkData();
            sinkParams.EventLogName = null;
            sinkParams.EventSourceName = "EntLib Unit Tests";
            EventLogSink sink = new EventLogSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(sink);

            Assert.AreEqual(1, CommonUtil.EventLogEntryCount(), "confirm there is one entry in the Application event log");

            Assert.AreEqual(CommonUtil.FormattedMessage, CommonUtil.GetLastEventLogEntry());
        }
    }
}

#endif