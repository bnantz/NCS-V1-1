//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // execute one event initially to be sure that the counter instances
            // are created
            FireDataCommandExecutedEvent();
        }

        [Test]
        public void DataCommandExecutedPerfCounter()
        {
            base.FirePerfCounter(
                SR.CounterCategory,
                SR.NumCmdsPerSec,
                new PerfCounterEventDelegate(FireDataCommandExecutedEvent));
        }

        private void FireDataCommandExecutedEvent()
        {
            DataCommandExecutedEvent.Fire(DateTime.Now);
        }

        [Test]
        public void DataCommandFailedEventPerfCounter()
        {
            FirePerfCounter(SR.CounterCategory,
                            SR.NumCmdsFailPerSec,
                            new PerfCounterEventDelegate(FireDataCommandFailedEvent));
        }

        private void FireDataCommandFailedEvent()
        {
            DataCommandFailedEvent.Fire("test1", "test2");
        }

        [Test]
        public void DataConnectionOpenedEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumConnPerSec,
                new PerfCounterEventDelegate(FireDataConnectionOpenedEvent));
        }

        private void FireDataConnectionOpenedEvent()
        {
            DataConnectionOpenedEvent.Fire("test1");
        }

        [Test]
        public void DataConnectionFailedEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumConnFailPerSec,
                new PerfCounterEventDelegate(FireDataConnectionFailedEvent));
        }

        private void FireDataConnectionFailedEvent()
        {
            DataConnectionFailedEvent.Fire("test1");
        }

        [Test]
        public void DataTransactionOpenedEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumTransOpenPerSec,
                new PerfCounterEventDelegate(FireDataTransactionOpenedEvent));
        }

        private void FireDataTransactionOpenedEvent()
        {
            DataTransactionOpenedEvent.Fire();
        }

        [Test]
        public void DataTransactionCommittedEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumTransCommitPerSec,
                new PerfCounterEventDelegate(FireDataTransactionCommittedEvent));
        }

        private void FireDataTransactionCommittedEvent()
        {
            DataTransactionCommittedEvent.Fire();
        }

        [Test]
        public void DataTransactionFailedEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumTransFailPerSec,
                new PerfCounterEventDelegate(FireDataTransactionFailedEvent));
        }

        private void FireDataTransactionFailedEvent()
        {
            DataTransactionFailedEvent.Fire("test1");
        }

        [Test]
        public void DataTransactionRolledBackEventPerfCounter()
        {
            FirePerfCounter(
                SR.CounterCategory,
                SR.NumTransAbortPerSec,
                new PerfCounterEventDelegate(FireDataTransactionRolledBackEvent));
        }

        private void FireDataTransactionRolledBackEvent()
        {
            DataTransactionRolledBackEvent.Fire("test1");
        }
    }
}

#endif