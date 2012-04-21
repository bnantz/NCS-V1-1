//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // fire events initially to create perf counter instances
            FireSecurityAuthenticationCheckEvent();
        }

        [Test]
        public void SecurityAuthenticationCheckPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Authentication Checks/Sec",
                new PerfCounterEventDelegate(FireSecurityAuthenticationCheckEvent));
        }

        private void FireSecurityAuthenticationCheckEvent()
        {
            SecurityAuthenticationCheckEvent.Fire("test");
        }

        [Test]
        public void SecurityAuthenticationFailurePerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Authentication Failures/Sec",
                new PerfCounterEventDelegate(FireSecurityAuthenticationFailureEvent));
        }

        private void FireSecurityAuthenticationFailureEvent()
        {
            SecurityAuthenticationFailedEvent.Fire("test");
        }

        [Test]
        public void SecurityAuthorizationFailurePerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Authorization Failures/Sec",
                new PerfCounterEventDelegate(FireSecurityAuthorizationFailureEvent));
        }

        private void FireSecurityAuthorizationFailureEvent()
        {
            SecurityAuthorizationFailedEvent.Fire("test", "action");
        }

        [Test]
        public void SecurityAuthorizationCheckPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Authorization Checks/Sec",
                new PerfCounterEventDelegate(FireSecurityAuthorizationCheckEvent));
        }

        private void FireSecurityAuthorizationCheckEvent()
        {
            SecurityAuthorizationCheckEvent.Fire("test", "action");
        }

        [Test]
        public void SecurityProfileLoadPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Profile Loads/Sec",
                new PerfCounterEventDelegate(FireSecurityProfileLoadEvent));
        }

        private void FireSecurityProfileLoadEvent()
        {
            SecurityProfileLoadEvent.Fire("test");
        }

        [Test]
        public void SecurityProfileSavePerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Profile Saves/Sec",
                new PerfCounterEventDelegate(FireecurityProfileSaveEvent));
        }

        private void FireecurityProfileSaveEvent()
        {
            SecurityProfileSaveEvent.Fire("test");
        }

        [Test]
        public void SecurityRoleLoadPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Role Loads/Sec",
                new PerfCounterEventDelegate(FireSecurityRoleLoadEvent));
        }

        private void FireSecurityRoleLoadEvent()
        {
            SecurityRoleLoadEvent.Fire("test");
        }

        [Test]
        public void CacheHitsCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Cache Hits Per Sec",
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        private void FireCacheHitCounter()
        {
            SecurityCacheReadEvent.Fire(true);
        }

        private void FireCacheMissCounter()
        {
            SecurityCacheReadEvent.Fire(false);
        }

        [Test]
        public void CacheMissesCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Cache Misses Per Sec",
                new PerfCounterEventDelegate(FireCacheMissCounter));
        }

        [Test]
        public void CacheHitRatioCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "Cache Hit Ratio",
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        [Test]
        public void CacheAccessAttemptsCounterHit()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "Total # of Cache Access Attempts",
                new PerfCounterEventDelegate(FireCacheHitCounter));
        }

        [Test]
        public void CacheAccessAttemptsCounterMiss()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "Total # of Cache Access Attempts",
                new PerfCounterEventDelegate(FireCacheMissCounter));
        }
    }
}

#endif