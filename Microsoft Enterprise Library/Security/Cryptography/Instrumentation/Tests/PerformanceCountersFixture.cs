//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation.Tests
{
    [TestFixture]
    public class PerformanceCountersFixture : PerformanceCounterFixtureBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // fire events initially to create perf counter instances
            FireSecurityCryptoHashCreateEvent();
        }

        [Test]
        public void CryptoCreateHashPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Hash Creates/Sec",
                new PerfCounterEventDelegate(FireSecurityCryptoHashCreateEvent));
        }

        private void FireSecurityCryptoHashCreateEvent()
        {
            SecurityCryptoHashCreateEvent.Fire("test");
        }

        [Test]
        public void CryptoCheckHashPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Hash Checks/Sec",
                new PerfCounterEventDelegate(FireSecurityCryptoHashCheckEvent));
        }

        private void FireSecurityCryptoHashCheckEvent()
        {
            SecurityCryptoHashCheckEvent.Fire("test");
        }

        [Test]
        public void CryptoFailureHashPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Hash Check Failures/Sec",
                new PerfCounterEventDelegate(FireSecurityCryptoHashFailureEvent));
        }

        private void FireSecurityCryptoHashFailureEvent()
        {
            SecurityCryptoHashCheckFailureEvent.Fire("test");
        }

        [Test]
        public void CryptoSymmetricEncryptionPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Symmetric Encryptions/Sec",
                new PerfCounterEventDelegate(FireSecurityCryptoSymmetricEncryptionEvent));
        }

        private void FireSecurityCryptoSymmetricEncryptionEvent()
        {
            SecurityCryptoSymmetricEncryptionEvent.Fire("test");
        }

        [Test]
        public void CryptoSymmetricDecryptionPerfCounter()
        {
            base.FirePerfCounter(
                SR.InstrumentationCounterCategory,
                "# of Symmetric Decryptions/Sec",
                new PerfCounterEventDelegate(FireSecurityCryptoSymmetricDecryptionEvent));
        }

        private void FireSecurityCryptoSymmetricDecryptionEvent()
        {
            SecurityCryptoSymmetricDecryptionEvent.Fire("test");
        }
    }
}

#endif