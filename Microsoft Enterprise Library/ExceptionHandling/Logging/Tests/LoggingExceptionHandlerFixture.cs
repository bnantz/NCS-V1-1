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

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests
{
    [TestFixture]
    public class LoggingExceptionHandlerFixture
    {
        [TearDown]
        public void TearDown()
        {
            MockLogSink.Clear();
        }

        [Test]
        public void HandleTest()
        {
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));

            Assert.AreEqual("TestCat", MockLogSink.GetLastEntry().Category);
            Assert.AreEqual(5, MockLogSink.GetLastEntry().EventId);
            Assert.AreEqual(Severity.Error, MockLogSink.GetLastEntry().Severity);
            Assert.AreEqual("TestTitle", MockLogSink.GetLastEntry().Title);
            //Console.Write(MockLogSink.MessageBody);
        }

        [Test]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleBadFormatterTest()
        {
            ExceptionPolicy.HandleException(new Exception(), "Invalid Formatter Policy");
        }

        [Test]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleBadConstructorFormatterTest()
        {
            ExceptionPolicy.HandleException(new Exception(), "Logging Bad Formatter Constructor Policy");
        }
    }
}

#endif