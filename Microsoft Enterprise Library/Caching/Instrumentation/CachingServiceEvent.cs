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

using System;
using System.Diagnostics;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    public class CachingServiceEvent : BaseEvent
    {
        internal static CounterCreationData[] Counters = new CounterCreationData[]
            {
                new CounterCreationData(
                    SR.PerfCounterTotalCacheEntries,
                    SR.PerfCounterTotalCacheEntriesMsg,
                    PerformanceCounterType.NumberOfItems64),
                new CounterCreationData(
                    SR.PerfCounterCacheHitsSec,
                    SR.PerfCounterCacheHitsSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    SR.PerfCounterCacheMissesSec,
                    SR.PerfCounterCacheMissesSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    SR.PerfCounterCacheHitRatio,
                    SR.PerfCounterCacheHitRatioMsg,
                    PerformanceCounterType.RawFraction),
                new CounterCreationData(
                    SR.PerfCounterCacheAccessAttempts,
                    SR.PerfCounterCacheAccessAttemptsMsg,
                    PerformanceCounterType.RawBase),
                new CounterCreationData(
                    SR.PerfCounterCacheTurnoverRate,
                    SR.PerfCounterCacheTurnoverRateMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32)
            };

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The internal event object responsible for the Performance Counters and
        /// the EventLog.  It also fired the WMI event when fired.
        /// </devdoc>
        [CLSCompliant(false)] protected InstrumentedEvent internalEvent;

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The message for the event.
        /// </devdoc>
        [CLSCompliant(false)] protected string eventMessage;

        /// <summary/>
        /// <exclude/>
        protected CachingServiceEvent()
        {
            eventMessage = string.Empty;
            internalEvent = null;
        }

        /// <summary/>
        /// <exclude/>
        protected CachingServiceEvent(string[] counterNames, EventLogIdentifier[] eventLogIdentifiers) : this()
        {
            internalEvent = new InstrumentedEvent(
                SR.CachingInstrumentationCounterCategory,
                counterNames,
                true,
                SR.CachingInstrumentationEventSource,
                eventLogIdentifiers);
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <exclude/>
        /// <devdoc>
        /// Factory method to create a new InstrumentedEvent based on the counterNames
        /// </devdoc>
        protected CachingServiceEvent(string[] counterNames) : this()
        {
            internalEvent = new InstrumentedEvent(
                SR.CachingInstrumentationCounterCategory,
                counterNames,
                true);
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Exposes the message of the internalEvent. 
        /// </devdoc>
        public string Message
        {
            get { return eventMessage; }
            set { eventMessage = value; }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires the internal Aux Event with the message to include in the event.
        /// </devdoc>
        protected void FireAuxEvent(string message)
        {
            if (internalEvent == null)
            {
                return;
            }

            internalEvent.FireEvent(message);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="eventLogType"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires the internal Aux Event with the message to include in the event and event log entry type
        /// </devdoc>
        protected void FireAuxEvent(string message, EventLogEntryType eventLogType)
        {
            if (internalEvent == null)
            {
                return;
            }

            internalEvent.FireEvent(message, eventLogType);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="counterInstances"/>
        /// <param name="values"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires the internal Aux Event with the message to include in the event and event log entry type
        /// </devdoc>
        protected void FireAuxEvent(string message, PerformanceCounterInstances[] counterInstances, long[] values)
        {
            if (internalEvent == null)
            {
                return;
            }

            internalEvent.FireEvent(message, counterInstances, values);
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Index to the Counters array.
        /// </devdoc>
        internal enum CounterIdentifier
        {
            TotalCacheEntries = 0,
            CacheHitsPerSec = 1,
            CacheMissesPerSec = 2,
            CacheHitRatio = 3,
            TotalCacheReads = 4, // for the base of ratio
            CacheTurnoverPerSec = 5
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Event Log Event IDs
        /// </devdoc>
        internal enum Log
        {
            InternalFailureOccurred = 1, // including generic failures and configuration failures
        }
    }
}