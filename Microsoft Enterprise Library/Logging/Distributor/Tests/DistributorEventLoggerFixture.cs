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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
    [TestFixture]
    public class DistributorEventLoggerFixture
    {
        private DistributorEventLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new DistributorEventLogger("Application");
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();
        }

        [Test]
        public void Constructor()
        {
            logger = new DistributorEventLogger();
            Assert.IsNotNull(logger);
        }

        [Test]
        public void ConstructorWithParams()
        {
            logger = new DistributorEventLogger("myLog");
            Assert.IsNotNull(logger);
            Assert.AreEqual("myLog", logger.EventLogName);
        }

        [Test]
        public void WriteExceptionToLog()
        {
            using (EventLog log = new EventLog("Application"))
            {
                int entryCount = log.Entries.Count;

                this.logger.AddMessage("HEADER", "Simulated Exception");

                ThrowAndLogException();

                Assert.AreEqual(1, log.Entries.Count - entryCount);
                string entry = log.Entries[log.Entries.Count - 1].Message;

                // assert that the non machine/time specific parts of the message have been logged
                string expected = "EnterpriseLibrary.Logging.Distributor.Tests.DistributorEventLoggerFixture.ThrowAndLogException()";
                Assert.IsTrue(entry.IndexOf(expected) > -1, "stack");

                expected = "Exception Type: System.ApplicationException\nMessage: an error has occurred\nTargetSite: Void ThrowAndLogException()";
                Assert.IsTrue(entry.IndexOf(expected) > -1, "exception type");

                expected = "MachineName: " + Environment.MachineName;
                Assert.IsTrue(entry.IndexOf(expected) > -1, "machine");

                expected = "distributoreventloggerfixture.cs:line";
                Assert.IsTrue(entry.ToLower().IndexOf(expected) > -1, "file and line");
            }
        }

        [Test]
        public void WriteExceptionToLogWithMessage()
        {
            this.logger.AddMessage("HEADER", "Simulated Exception with messages");

            logger.AddMessage("msg1", "my custom message val1");
            logger.AddMessage("msg2", "my custom message val2");

            using (EventLog log = new EventLog("Application"))
            {
                int entryCount = log.Entries.Count;

                ThrowAndLogException();

                Assert.AreEqual(1, log.Entries.Count - entryCount);

                string entry = log.Entries[log.Entries.Count - 1].Message;

                string expected = "my custom message val1";
                Assert.IsTrue(entry.IndexOf(expected) > -1);

                expected = "my custom message val2";
                Assert.IsTrue(entry.IndexOf(expected) > -1);
            }
        }

        private void ThrowAndLogException()
        {
            try
            {
                throw new ApplicationException("an error has occurred");
            }
            catch (Exception ex)
            {
                this.logger.WriteToLog(ex, Severity.Error);
            }
        }
    }
}

#endif