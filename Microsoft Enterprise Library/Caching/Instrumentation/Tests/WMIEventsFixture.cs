//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestFixture]
    public class WMIEventsFixture : WMIEventsFixtureBase
    {
        [SetUp]
        public void Setup()
        {
            base.wmiLogged = false;
            base.wmiResult = string.Empty;
        }

        [Test]
        public void CachingServiceCacheFlushedEventTest()
        {
            string eventName = "CachingServiceCacheFlushedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireCachingServiceCacheFlushedEvent);

            string expected = "Message = \"\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireCachingServiceCacheFlushedEvent()
        {
            CachingServiceCacheFlushedEvent.FireEvent();
        }

        [Test]
        public void CachingServiceCacheScavengedEventTest()
        {
            string eventName = "CachingServiceCacheScavengedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireCachingServiceCacheScavengedEvent);

            string expected = "ConfiguredSizeLimit = \"1\";\n\tMessage = \"\";\n\tNumberOfItemRemoved = \"1\";\n\tScavengingRange = 1;\n\t";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireCachingServiceCacheScavengedEvent()
        {
            CachingServiceCacheScavengedEvent.Fire(1, 1, 1);
        }

        [Test]
        public void CachingServiceInternalFailureEventTest()
        {
            string eventName = "CachingServiceInternalFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireCachingServiceInternalFailureEvent);

            string expected = "Message = \"test\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireCachingServiceInternalFailureEvent()
        {
            CachingServiceInternalFailureEvent.Fire("test");
        }

        [Test]
        public void CachingServiceInternalFailureEventTestWithException()
        {
            string eventName = "CachingServiceInternalFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireCachingServiceInternalFailureEventWithException);

            string expected = "ExceptionMessage = \"System.Exception: test exception\";\n\tExceptionStackTrace = NULL;\n\tMessage = \"test\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireCachingServiceInternalFailureEventWithException()
        {
            CachingServiceInternalFailureEvent.Fire("test", new Exception("test exception"));
        }
    }
}

#endif