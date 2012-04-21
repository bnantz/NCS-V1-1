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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    /// <devdoc>
    /// Instrumentation event called when item is read from cache
    /// </devdoc>
    public class CachingServiceItemReadEvent
    {
        private static CachingServiceItemReadEvent cachingEvent =
            new CachingServiceItemReadEvent(
                new string[]
                    {
                        SR.PerfCounterCacheHitsSec,
                        SR.PerfCounterCacheMissesSec,
                        SR.PerfCounterCacheAccessAttempts,
                        SR.PerfCounterCacheHitRatio
                    }
                );

        private PerformanceCounterInstances[] perfCounters;
        private InstrumentedEvent internalEvent;

        private CachingServiceItemReadEvent(string[] counterNames)
        {
            internalEvent = new InstrumentedEvent(
                SR.CachingInstrumentationCounterCategory, counterNames, true);
            perfCounters = new PerformanceCounterInstances[]
                {
                    internalEvent.GetPerformanceCounterInstances(SR.PerfCounterCacheHitsSec),
                    internalEvent.GetPerformanceCounterInstances(SR.PerfCounterCacheMissesSec),
                    internalEvent.GetPerformanceCounterInstances(SR.PerfCounterCacheAccessAttempts),
                    internalEvent.GetPerformanceCounterInstances(SR.PerfCounterCacheHitRatio)
                };
        }

        /// <summary/>
        /// <param name="hitOrMiss"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires event to instrumentation with notification of a hit or miss in the cache.
        /// </devdoc>
        public static void Fire(bool hitOrMiss)
        {
            cachingEvent.FireEvent(hitOrMiss);
        }

        private void FireEvent(bool hitOrMiss)
        {
            long[] counterValues = null;
            if (hitOrMiss == true)
            {
                counterValues = new long[] {1, 0, 1, 1};
            }
            else
            {
                counterValues = new long[] {0, 1, 1, 0};
            }
            internalEvent.FireEvent(string.Empty, perfCounters, counterValues);
        }
    }
}