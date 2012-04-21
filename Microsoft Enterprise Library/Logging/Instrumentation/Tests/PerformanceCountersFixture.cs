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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // fire events initially to create perf counter instances
            FireLoggingLogDeliveryFailureEvent();
            FireLoggingLogDistributedEvent();
            FireLoggingLogWrittenEvent();
        }

        [Test]
        public void ClientLogWrittenPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                SR.NumLogsWrittenSec,
                new PerfCounterEventDelegate(FireLoggingLogWrittenEvent));
        }

        private void FireLoggingLogWrittenEvent()
        {
            LoggingLogWrittenEvent.Fire("test");
        }

        [Test]
        public void DistributorLogDistributedPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                SR.NumLogsDistributedSec,
                new PerfCounterEventDelegate(FireLoggingLogDistributedEvent));
        }

        private void FireLoggingLogDistributedEvent()
        {
            LoggingLogDistributedEvent.Fire("test");
        }

        [Test]
        public void DistributorLogDistributionFailuresPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                SR.NumLogsDefaultSinkSec,
                new PerfCounterEventDelegate(FireLoggingLogDeliveryFailureEvent));
        }

        private void FireLoggingLogDeliveryFailureEvent()
        {
            LoggingLogDeliveryFailureEvent.Fire("test");
        }
    }
}

#endif