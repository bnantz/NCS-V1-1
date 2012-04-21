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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // execute one of the events initially to be sure that the counter instances
            // are created
            FireCacheHitCounter();
        }

        [Test]
        public void TotalCacheEntriesPerfCounter()
        {
            CachingServiceItemTurnoverEvent.SetItemsTotal(0);

            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterTotalCacheEntries,
                new PerfCounterEventDelegate(FireCachingServiceItemTurnoverEventSetTotal));
        }

        private void FireCachingServiceItemTurnoverEventSetTotal()
        {
            CachingServiceItemTurnoverEvent.SetItemsTotal(3);
        }

        [Test]
        public void ItemTurnoverAddCounter()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheTurnoverRate,
                new PerfCounterEventDelegate(FireCachingServiceItemTurnoverEventAddItems));
        }

        private void FireCachingServiceItemTurnoverEventAddItems()
        {
            CachingServiceItemTurnoverEvent.FireAddItems(1);
        }

        [Test]
        public void ItemTurnoverRemoveCounter()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheTurnoverRate,
                new PerfCounterEventDelegate(FireCachingServiceItemTurnoverEventRemoveItems));
        }

        private void FireCachingServiceItemTurnoverEventRemoveItems()
        {
            CachingServiceItemTurnoverEvent.FireRemoveItems(1);
        }

        [Test]
        public void CacheHitsCounter()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheHitsSec,
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        private void FireCacheHitCounter()
        {
            CachingServiceItemReadEvent.Fire(true);
        }

        private void FireCacheMissCounter()
        {
            CachingServiceItemReadEvent.Fire(false);
        }

        [Test]
        public void CacheMissesCounter()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheMissesSec,
                new PerfCounterEventDelegate(FireCacheMissCounter));
        }

        [Test]
        public void CacheHitRatioCounter()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheHitRatio,
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        [Test]
        public void CacheAccessAttemptsCounterHit()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheAccessAttempts,
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        [Test]
        public void CacheAccessAttemptsCounterMiss()
        {
            base.FirePerfCounter(
                SR.CachingInstrumentationCounterCategory,
                SR.PerfCounterCacheAccessAttempts,
                new PerfCounterEventDelegate(FireCacheMissCounter));
        }
    }
}

#endif