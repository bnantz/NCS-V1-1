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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // execute one of the events initially to be sure that the counter instances
            // are created
            FireExceptionHandledEvent();
        }

        [Test]
        public void HandledExceptionsPerfCounter()
        {
            base.FirePerfCounter(
                ExceptionHandledEvent.InstrumentationCounterCategory,
                ExceptionHandlingEvent.PerfCounterExceptionsHandled,
                new PerfCounterEventDelegate(FireExceptionHandledEvent));
        }

        [Test]
        public void UnhandledExceptionsPerfCounter()
        {
            base.FirePerfCounter(
                ExceptionHandledEvent.InstrumentationCounterCategory,
                ExceptionHandlingEvent.PerfCounterExceptionsUnhandled,
                new PerfCounterEventDelegate(FireExceptionNotHandledEvent));
        }

        private void FireExceptionNotHandledEvent()
        {
            ExceptionNotHandledEvent.Fire();
        }

        private void FireExceptionHandledEvent()
        {
            ExceptionHandledEvent.Fire();
        }
    }
}

#endif