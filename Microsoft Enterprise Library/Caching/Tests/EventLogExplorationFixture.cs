//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.Diagnostics;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class EventLogExplorationFixture
    {
        private static readonly string eventSource = "EnterpriseLibrary.Caching.Tests.EventLogExplorationFixture";

        [SetUp]
        public void ResetEventSources()
        {
            if (EventLog.Exists(eventSource))
            {
                EventLog.DeleteEventSource(eventSource);
            }
            EventLog.CreateEventSource(eventSource, "Application");
        }

        [TearDown]
        public void RemoveEventSources()
        {
            EventLog.DeleteEventSource(eventSource);
        }

        [Test]
        public void CanGetCountFromEventLog()
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                Assert.IsTrue(eventLog.Entries.Count > 0);
            }
        }

        [Test]
        public void CanWriteEvent()
        {
            using (EventLog log = new EventLog("Application"))
            {
                log.Source = eventSource;

                log.WriteEntry("This is a test");

                EventLogEntry logEntry = log.Entries[log.Entries.Count - 1];
                Assert.AreEqual("This is a test", logEntry.Message);
            }
        }
    }
}

#endif