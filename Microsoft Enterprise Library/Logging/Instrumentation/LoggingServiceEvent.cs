//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// The base class for all the instrumented events fired
    /// </summary>
    [ComVisible(false)]
    public class LoggingServiceEvent : BaseEvent
    {
        internal static CounterCreationData[] counters;

        /// <summary>
        /// The internal event object responsible for the Performance Counters and
        /// the EventLog.  And, it constructed with a BaseEvent object, it also
        /// ties the WMI event when fired.
        /// </summary>
        [CLSCompliant(false)] protected InstrumentedEvent internalEvent;

        /// <summary>
        /// The message for the event.
        /// </summary>
        [CLSCompliant(false)] protected string eventMessage;

        /// <summary>
        /// Index to the Counters array.
        /// </summary>
        internal enum CounterID : int
        {
            LogWritten = 0,
            LogDistributed = 1,
            LogDistributedToDefaultSink = 2
        }

        /// <summary>
        /// Event Log Event IDs
        /// </summary>
        internal enum Log : int
        {
            // including generic failures and configuration failures
            InternalFailureOccurred = 1,
        }

        static LoggingServiceEvent()
        {
            counters = new CounterCreationData[]
                {
                    new CounterCreationData(
                        SR.NumLogsWrittenSec,
                        SR.NumLogsWrittenSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        SR.NumLogsDistributedSec,
                        SR.NumLogsDistributedSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32),
                    new CounterCreationData(
                        SR.NumLogsDefaultSinkSec,
                        SR.NumLogsDefaultSinkSecMsg,
                        PerformanceCounterType.RateOfCountsPerSecond32)
                };
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected LoggingServiceEvent()
        {
            this.eventMessage = string.Empty;
            this.internalEvent = null;
        }

        /// <summary>
        /// Constructor with performance counter names and event log Ids.
        /// </summary>
        /// <param name="counterNames">Array of names of performance counters defined for Logging</param>
        /// <param name="eventLogIDs">Array of <see cref="EventLogIdentifier"></see>s</param>
        protected LoggingServiceEvent(string[] counterNames, EventLogIdentifier[] eventLogIDs)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true,
                SR.InstrumentationEventSource,
                eventLogIDs);
        }

        /// <summary>
        /// Constructor with performance counter names only.
        /// </summary>
        /// <param name="counterNames">Array of names of performance counters defined for Logging</param>
        protected LoggingServiceEvent(string[] counterNames)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true);
        }

        /// <summary>
        /// Fire auxillary event.
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        protected void FireAuxEvent(string message)
        {
            if (this.internalEvent != null)
            {
                this.internalEvent.FireEvent(message);
            }
        }

        /// <summary>
        /// Fire auxillary event.
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        /// <param name="eventLogEntryType"><see cref="EventLogEntryType"></see> for event</param>
        protected void FireAuxEvent(string message, EventLogEntryType eventLogEntryType)
        {
            if (this.internalEvent != null)
            {
                this.internalEvent.FireEvent(message, eventLogEntryType);
            }
        }

        /// <summary>
        /// Exposes the message of the internalEvent. 
        /// </summary>
        public string Message
        {
            get { return this.eventMessage; }
            set { this.eventMessage = value; }
        }
    }
}