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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestFixture]
    public class LoggerFixture : ConfigurationContextFixtureBase
    {
        private LoggingSettings loggingSettings;

        private string message = "testing.... message and category";
        private string category = "foobarbaz";
        private int priority = 420;
        private int eventId = 421;
        private string title = "bar";
        private Severity severity = Severity.Information;
        
        [SetUp]
        public void SetUp()
        {
            loggingSettings = (LoggingSettings)Context.GetConfiguration(LoggingSettings.SectionName);

            SetMockDistributionStrategy();

            MockDistributionStrategy.Entry = null;
            MockDistributionStrategy.MessageXml = string.Empty;
        }

        [TearDown]
        public void TearDown()
        {
            SetInProcDistributionStrategy();
        }

        private void SetMockDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("MockStrategy");
        }

        private void SetInProcDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("InProc");
        }

        [Test]
        public void WriteMessageOnlyOverload()
        {
            Logger.Write(message);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
        }

        [Test]
        public void WriteMessageAndCategoryOnlyOverload()
        {
            Logger.Write(message, category);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
        }

        [Test]
        public void WriteMessageCategoryAndPriorityOverload()
        {
            Logger.Write(message, category, priority);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(priority, MockDistributionStrategy.Entry.Priority, "priority");
        }

        [Test]
        public void WriteMessageCategoryPriorityEventIdOverload()
        {
            Logger.Write(message, category, priority, eventId);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(priority, MockDistributionStrategy.Entry.Priority, "priority");
            Assert.AreEqual(eventId, MockDistributionStrategy.Entry.EventId, "eventid");
        }

        [Test]
        public void WriteMessageCategoryPriorityEventIdSeverityOverload()
        {
            Logger.Write(message, category, priority, eventId, severity);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(priority, MockDistributionStrategy.Entry.Priority, "priority");
            Assert.AreEqual(eventId, MockDistributionStrategy.Entry.EventId, "eventid");
            Assert.AreEqual(severity, MockDistributionStrategy.Entry.Severity, "severity");
        }

        [Test]
        public void WriteMessageCategoryPriorityEventIdSeverityTitleOverload()
        {
            Logger.Write(message, category, priority, eventId, severity, title);

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(priority, MockDistributionStrategy.Entry.Priority, "priority");
            Assert.AreEqual(eventId, MockDistributionStrategy.Entry.EventId, "eventid");
            Assert.AreEqual(severity, MockDistributionStrategy.Entry.Severity, "severity");
            Assert.AreEqual(title, MockDistributionStrategy.Entry.Title, "title");
        }

        [Test]
        public void WriteMessageAndDictionaryOverload()
        {
            Logger.Write(message, CommonUtil.GetPropertiesHashtable());

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(CommonUtil.GetPropertiesHashtable().Count,
                            MockDistributionStrategy.Entry.ExtendedProperties.Count, "hash count");

            Assert.AreEqual(CommonUtil.GetPropertiesHashtable()["key1"],
                            MockDistributionStrategy.Entry.ExtendedProperties["key1"], "hash count");
        }

        [Test]
        public void WriteMessageCategoryAndDictionaryOverload()
        {
            Logger.Write(message, category, CommonUtil.GetPropertiesHashtable());

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(CommonUtil.GetPropertiesHashtable().Count,
                            MockDistributionStrategy.Entry.ExtendedProperties.Count, "hash count");
        }

        [Test]
        public void WriteMessageCategoryPriorityAndDictionaryOverload()
        {
            Logger.Write(message, category, priority, CommonUtil.GetPropertiesHashtable());

            Assert.AreEqual(message, MockDistributionStrategy.Entry.Message, "message");
            Assert.AreEqual(category, MockDistributionStrategy.Entry.Category, "category");
            Assert.AreEqual(priority, MockDistributionStrategy.Entry.Priority, "priority");
            Assert.AreEqual(CommonUtil.GetPropertiesHashtable().Count,
                            MockDistributionStrategy.Entry.ExtendedProperties.Count, "hash count");
        }

        [Test]
        public void WriteDictionary()
        {
            SetInProcDistributionStrategy();
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Category = "DictionaryCategory";
            entry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            Logger.Write(entry);

            string expected = CommonUtil.FormattedMessageWithDictionary;
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteEntryWithModeOff()
        {
            this.loggingSettings.LoggingEnabled = false;
            Logger.Write("Body", "Category1", -1, 1, Severity.Warning, "Header");

            Assert.AreEqual(string.Empty, MockDistributionStrategy.MessageXml, "assert that nothing was written");
        }

        [Test]
        public void SendMessageWithPriorityAboveMinimum()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Category = "MockCategoryOne";
            logEntry.Priority = loggingSettings.MinimumPriority + 1;

            Logger.Write(logEntry);

            LogEntry resultLog = MockDistributionStrategy.Entry;

            Assert.IsNotNull(resultLog, "confirm that the message got logged by the strategy");
            Assert.AreEqual(logEntry.Message, resultLog.Message);
        }

        [Test]
        public void SendMessageWithPriorityBelowMinimum()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Category = "MockCategoryOne";
            logEntry.Priority = loggingSettings.MinimumPriority - 1;

            Logger.Write(logEntry);

            LogEntry resultLog = MockDistributionStrategy.Entry;

            Assert.IsNull(resultLog, "confirm that the message did NOT get logged by the strategy");
        }

        [Test]
        public void WriteEntryWithMessages()
        {
            SetInProcDistributionStrategy();
            LogEntry log = new LogEntry();
            log.Category = "AppTest";
            log.Message = "My Body";
            log.Title = "My Header";
            log.EventId = 25;
            log.Severity = Severity.Warning;
            log.Priority = loggingSettings.MinimumPriority;
            log.AddErrorMessage("hey");
            log.AddErrorMessage("yes");

            Logger.Write(log);

            string actual = CommonUtil.GetLastEventLogEntryCustom();
            string expected = "yes\r\n\r\nhey";
            int pos = actual.IndexOf(expected);
            Assert.IsTrue(pos > -1);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void InvalidDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("INVALID");
            Logger.Write("test");
            CommonUtil.SetDistributionStrategy("InProc");
        }

        [Test]
        public void NullCategoryRevertToDefaultCategory()
        {
            LogEntry log = new LogEntry();
            log.Message = "test";
            log.Category = null;
            Logger.Write(log);

            Assert.AreEqual("test", MockDistributionStrategy.Entry.Message);
            Assert.AreEqual("", MockDistributionStrategy.Entry.Category);
        }

        [Test]
        public void BadCategoryStillLogs()
        {
            LogEntry log = new LogEntry();
            log.EventId = 1;
            log.Message = "test";
            log.Category = "vuyws9to4wefe4ttfhslft4wasfdbngl";
            Logger.Write(log);

            Assert.AreEqual("test", MockDistributionStrategy.Entry.Message);
        }
    }
}

#endif