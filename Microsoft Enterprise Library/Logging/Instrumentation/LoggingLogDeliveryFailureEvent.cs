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

using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when a log message cannot be distributed.
    /// </summary>
    [ComVisible(false)]
    public class LoggingLogDeliveryFailureEvent : LoggingServiceEvent
    {
        private static LoggingLogDeliveryFailureEvent failureEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static LoggingLogDeliveryFailureEvent()
        {
            int performanceCounterId =
                (int)CounterID.LogDistributedToDefaultSink;
            string[] counterNames = new string[]
                {
                    LoggingServiceEvent.counters[performanceCounterId].CounterName
                };
            failureEvent = new LoggingLogDeliveryFailureEvent(counterNames);
            writerLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Fire the WMI event
        /// </summary>
        /// <param name="message">Message included with WMI event</param>
        public static void Fire(string message)
        {
            failureEvent.FireEvent(message);
        }

        private LoggingLogDeliveryFailureEvent(string[] counterNames) : base(counterNames, null)
        {
        }

        /// <summary>
        /// Fire the WMI event
        /// </summary>
        /// <param name="message">Message included with WMI event</param>
        protected void FireEvent(string message)
        {
            writerLock.AcquireWriterLock(LockTimeout);
            try
            {
                eventMessage = message;
                InstrumentedEvent.FireWmiEvent(this);
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }

            FireAuxEvent(string.Empty);
        }
    }
}