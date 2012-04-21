//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if       UNIT_TESTS
using System.Diagnostics;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    /// <summary>
    /// Summary description for PerformanceCounterFixtureBase.
    /// </summary>
    public abstract class PerformanceCounterFixtureBase
    {
        public delegate void PerfCounterEventDelegate();

        protected void FirePerfCounter(string category, string counterName, PerfCounterEventDelegate action)
        {
            PerformanceCounter counter = new PerformanceCounter(category, counterName, "_total_");
            CounterSample sample = counter.NextSample();
            float startVal = sample.RawValue;

            action();
            action();
            action();

            sample = counter.NextSample();
            float endVal = sample.RawValue;

            Assert.AreEqual(startVal + 3, endVal);
        }
    }
}

#endif