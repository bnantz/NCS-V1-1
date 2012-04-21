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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation.Tests
{
    [TestFixture]
    public class InstrumentationEventsFixture : WMIEventsFixtureBase
    {
        private string testMessage = "test";
        private string defaultExpected = "";

        [SetUp]
        public void Setup()
        {
            base.wmiLogged = false;
            base.wmiResult = string.Empty;
            this.defaultExpected = "Message = \"" + testMessage + "\"";
        }

        [Test]
        public void LoggingDistributorEventTest()
        {
            string eventName = "LoggingDistributorEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireLoggingDistributorEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireLoggingDistributorEvent()
        {
            LoggingDistributorEvent.Fire(testMessage, true);
        }

        [Test]
        public void LoggingLogDeliveryFailureEventTest()
        {
            string eventName = "LoggingLogDeliveryFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireLoggingLogDeliveryFailureEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireLoggingLogDeliveryFailureEvent()
        {
            LoggingLogDeliveryFailureEvent.Fire(testMessage);
        }

        [Test]
        public void LoggingLogDistributedEventTest()
        {
            string eventName = "LoggingLogDistributedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireLoggingLogDistributedEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireLoggingLogDistributedEvent()
        {
            LoggingLogDistributedEvent.Fire(testMessage);
        }

        [Test]
        public void LoggingLogWrittenEventTest()
        {
            string eventName = "LoggingLogWrittenEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireLoggingLogWrittenEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireLoggingLogWrittenEvent()
        {
            LoggingLogWrittenEvent.Fire(testMessage);
        }

        [Test]
        public void LoggingServiceFailureEventTest()
        {
            string eventName = "LoggingServiceFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireLoggingServiceFailureEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireLoggingServiceFailureEvent()
        {
            LoggingServiceFailureEvent.Fire(testMessage, new Exception("TEST"));
        }
    }
}

#endif