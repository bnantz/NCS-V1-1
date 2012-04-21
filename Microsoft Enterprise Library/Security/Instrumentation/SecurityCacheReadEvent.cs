//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    public class SecurityCacheReadEvent
    {
        private static SecurityCacheReadEvent securityEvent;
        private PerformanceCounterInstances[] perfCounters;
        private InstrumentedEvent internalEvent;

        static SecurityCacheReadEvent()
        {
            string[] counterNames = new string[]
                {
                    SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheHits].CounterName,
                    SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheMisses].CounterName,
                    SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.TotalCacheReads].CounterName,
                    SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheHitRatio].CounterName
                };
            securityEvent = new SecurityCacheReadEvent(counterNames);
        }

        private SecurityCacheReadEvent(string[] counterNames)
        {
            internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory, counterNames, true);
            perfCounters = new PerformanceCounterInstances[]
                {
                    internalEvent.GetPerformanceCounterInstances(SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheHits].CounterName),
                    internalEvent.GetPerformanceCounterInstances(SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheMisses].CounterName),
                    internalEvent.GetPerformanceCounterInstances(SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.TotalCacheReads].CounterName),
                    internalEvent.GetPerformanceCounterInstances(SecurityServiceEvent.Counters[(int)SecurityServiceEvent.CounterID.CacheHitRatio].CounterName)
                };
        }

        /// <summary/>
        /// <param name="hitOrMiss"/>
        /// <exclude/>
        public static void Fire(bool hitOrMiss)
        {
            securityEvent.FireEvent(hitOrMiss);
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