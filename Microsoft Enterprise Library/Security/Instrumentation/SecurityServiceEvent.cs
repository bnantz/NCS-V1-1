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

using System;
using System.Diagnostics;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    public class SecurityServiceEvent : BaseEvent
    {
        internal static CounterCreationData[] Counters;

        /// <summary/>
        /// <exclude/>
        [CLSCompliant(false)] protected InstrumentedEvent internalEvent;

        /// <summary/>
        /// <exclude/>
        [CLSCompliant(false)] protected string eventMessage;

        /// <devdoc>
        /// Index to the Counters array.
        /// </devdoc>
        internal enum CounterID : int
        {
            AuthenticationCheck = 0,
            AuthenticationFailure = 1,
            AuthorizationCheck = 2,
            AuthorizationFailure = 3,
            ProfileLoad = 4,
            ProfileSave = 5,
            RoleLoad = 6,
            CacheHits = 7,
            CacheMisses = 8,
            CacheHitRatio = 9,
            TotalCacheReads = 10,
        }

        /// <devdoc>
        /// Event Log Event IDs
        /// </devdoc>
        internal enum Log : int
        {
            // including generic failures and configuration failures
            InternalFailureOccurred = 1,
        }

        static SecurityServiceEvent()
        {
            Counters = new CounterCreationData[]
                {
                    new CounterCreationData(
                        "# of Authentication Checks/Sec",
                        SR.PerfCounterNumAuthenticationChecksSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Authentication Failures/Sec",
                        SR.PerfCounterNumAuthenticationFailuresSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Authorization Checks/Sec",
                        SR.PerfCounterNumAuthorizationChecksSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Authorization Failures/Sec",
                        SR.PerfCounterNumAuthorizationFailuresSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Profile Loads/Sec",
                        SR.PerfCounterNumProfileLoadsSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Profile Saves/Sec",
                        SR.PerfCounterNumProfileSavesSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Role Loads/Sec",
                        SR.PerfCounterNumRoleLoadsSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Cache Hits Per Sec",
                        SR.PerfCounterNumCacheHitsSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "# of Cache Misses Per Sec",
                        SR.PerfCounterNumCacheMissesSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        "Cache Hit Ratio",
                        SR.PerfCounterRatioCacheHitsMsg,
                        PerformanceCounterType.RawFraction),
                    new CounterCreationData(
                        "Total # of Cache Access Attempts",
                        SR.PerfCounterCacheAccessAttemptsMsg,
                        PerformanceCounterType.RawBase),
                };
        }

        /// <summary/>
        /// <exclude/>
        protected SecurityServiceEvent()
        {
            this.eventMessage = string.Empty;
            this.internalEvent = null;
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <param name="eventLogIDs"/>
        /// <exclude/>
        protected SecurityServiceEvent(string[] counterNames, EventLogIdentifier[] eventLogIDs)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true,
                SR.InstrumentationEventSource,
                eventLogIDs);
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <exclude/>
        protected SecurityServiceEvent(string[] counterNames)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void FireAuxEvent(string message)
        {
            if (this.internalEvent != null)
            {
                this.internalEvent.FireEvent(message);
            }
        }

        /// <summary/>
        /// <exclude/>
        public string Message
        {
            get { return this.eventMessage; }
            set { this.eventMessage = value; }
        }
    }
}