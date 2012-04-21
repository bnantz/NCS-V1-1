//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    public class ExceptionHandlingEvent : BaseEvent
    {
        internal const string EventSourceName = "Enterprise Library Exception Handling";
        internal const string PerfCounterExceptionsHandled = "# of Exceptions Handled/Sec";
        internal const string PerfCounterExceptionsUnhandled = "# of Exceptions Unhandled/Sec";

        internal static CounterCreationData[] counters = new CounterCreationData[]
            {
                new CounterCreationData(PerfCounterExceptionsHandled,
                                        SR.PerfCounterExceptionsHandledMsg,
                                        PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(PerfCounterExceptionsUnhandled,
                                        SR.PerfCounterExceptionsUnhandledMsg,
                                        PerformanceCounterType.RateOfCountsPerSecond32)
            };

        private InstrumentedEvent internalEvent;

        
        /// <devdoc>
        /// Create's a new ExceptionHandlingEvent object with a null Internal Event.
        /// </devdoc>
        /// <summary/>
        /// <exclude/>
        protected ExceptionHandlingEvent()
        {
            InternalEvent = null;
        }

        /// <devdoc>
        /// Creates a new ExceptionHandlingEvent object and instantiates an internal event
        /// with the specified counter names and event log ID's.
        /// </devdoc>
        /// <summary/>
        /// <param name="counterNames"/>
        /// <param name="eventLogIDs"/>
        /// <exclude/>
        protected ExceptionHandlingEvent(string[] counterNames, EventLogIdentifier[] eventLogIDs)
        {
            InternalEvent = new InstrumentedEvent(
                ExceptionHandledEvent.InstrumentationCounterCategory,
                counterNames,
                true,
                EventSourceName,
                eventLogIDs);
        }

        /// <devdoc>
        /// Creates a new ExceptionHandlingEvent object and instantiates an internal event
        /// with the specified counter names.
        /// </devdoc>
        /// <summary/>
        /// <param name="counterNames"/>
        /// <exclude/>
        protected ExceptionHandlingEvent(string[] counterNames)
        {
            InternalEvent = new InstrumentedEvent(
                ExceptionHandledEvent.InstrumentationCounterCategory,
                counterNames,
                true);
        }

        /// <devdoc>
        /// Index to the Counters array.
        /// </devdoc>
        internal enum CounterID : int
        {
            HandledPerSec = 0,
            UnhandledPerSec = 1
        }

        /// <devdoc>
        /// Event Log Event IDs
        /// </devdoc>
        internal enum Log : int
        {
            InternalFailureOccurred = 1 // including generic failures and configuration failures
        }

        /// <devdoc>
        /// The internal event object responsible for the Performance Counters and
        /// the EventLog.  And, it constructed with a BaseEvent object, it also
        /// ties the WMI event when fired.
        /// </devdoc>
        /// <summary/>
        /// <exclude/>
        protected InstrumentedEvent InternalEvent
        {
            get { return internalEvent; }
            set { internalEvent = value; }
        }

        /// <devdoc>
        /// Fires the internal event if specified.
        /// </devdoc>
        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void FireAuxEvent(string message)
        {
            if (InternalEvent != null)
            {
                InternalEvent.FireEvent(message);
            }
        }
    }
}