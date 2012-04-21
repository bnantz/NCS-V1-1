//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation.Tests
{
    [TestFixture]
    public class EventLogFixture
    {
        private int startCount = 0;
        private EventLog log;

        [TestFixtureSetUp]
        public void CreatEventLog()
        {
            log = new EventLog("Application");
        }

        [TestFixtureTearDown]
        public void DisposeEventLog()
        {
            log.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            startCount = log.Entries.Count;
        }

        [Test]
        public void ExceptionHandlingConfigFailureEventTest()
        {
            ExceptionHandlingConfigFailureEvent.Fire("test");

            Assert.AreEqual(1, log.Entries.Count - startCount);

            string expected = "test";
            string actual = log.Entries[log.Entries.Count - 1].Message;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ExceptionHandlingConfigFailureEventTestWithException()
        {
            ExceptionHandlingConfigFailureEvent.Fire("test", new Exception("test exception"));

            Assert.AreEqual(1, log.Entries.Count - startCount);

            string expected = "test Exception: System.Exception: test exception";
            string actual = log.Entries[log.Entries.Count - 1].Message;

            Assert.AreEqual(expected, actual);
        }
    }
}

#endif