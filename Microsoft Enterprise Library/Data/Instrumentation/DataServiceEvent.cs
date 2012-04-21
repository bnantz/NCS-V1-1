//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DataServiceEvent : BaseEvent
    {
        internal static CounterCreationData[] Counters = new CounterCreationData[]
            {
                new CounterCreationData(SR.NumConnPerSec, SR.NumConnPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumConnFailPerSec, SR.NumConnFailPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumCmdsPerSec, SR.NumCmdsPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumCmdsFailPerSec, SR.NumCmdsFailPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumTransOpenPerSec, SR.NumTransOpenPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumTransFailPerSec, SR.NumTransFailPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumTransCommitPerSec, SR.NumTransCommitPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.NumTransAbortPerSec, SR.NumTransAbortPerSecMsg, PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(SR.AvgCmdTime, SR.AvgCmdTimeMsg, PerformanceCounterType.AverageTimer32),
                new CounterCreationData(SR.TotalCmd, SR.TotalCmdMsg, PerformanceCounterType.AverageBase)
            };

        private InstrumentedEvent instrumentedEvent;
        private string message;

        /// <summary/>
        /// <exclude/>
        protected DataServiceEvent()
        {
            Message = string.Empty;
            InternalEvent = null;
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <exclude/>
        protected DataServiceEvent(string[] counterNames)
        {
            Message = string.Empty;
            InternalEvent = new InstrumentedEvent(SR.CounterCategory, counterNames, true);
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <param name="eventLogIds"/>
        /// <exclude/>
        protected DataServiceEvent(string[] counterNames, EventLogIdentifier[] eventLogIds)
        {
            Message = string.Empty;
            InternalEvent = new InstrumentedEvent(SR.CounterCategory, counterNames, true, SR.EventSource, eventLogIds);
        }

        /// <summary/>
        /// <exclude/>
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The internal event object responsible for the Performance Counters and
        /// the EventLog.  And, it constructed with a BaseEvent object, it also
        /// ties the WMI event when fired.
        /// </devdoc>
        protected InstrumentedEvent InternalEvent
        {
            get { return this.instrumentedEvent; }
            set { this.instrumentedEvent = value; }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void FireAuxEvent(string message)
        {
            if (InternalEvent == null)
            {
                return;
            }

            InternalEvent.FireEvent(message);
        }

        /// <devdoc>
        /// Index to the Counters array.
        /// </devdoc>
        internal enum CounterIndex : int
        {
            ConnectionOpenned = 0,
            ConnectionFailed = 1,
            CommandExecuted = 2,
            CommandFailed = 3,
            TransactionOpenned = 4,
            TransactionFailed = 5,
            TransactionCommitted = 6,
            TransactionRolledBack = 7,
            AvgCommandExecuteTime = 8,
            TotalCommandExecuted = 9
        }

        /// <devdoc>
        /// Index to the Event Log array.
        /// </devdoc>   
        internal enum LogIndex : int
        {
            FailureOccurred = 1,
            ConnectionFailed = 10,
        }
    }
}