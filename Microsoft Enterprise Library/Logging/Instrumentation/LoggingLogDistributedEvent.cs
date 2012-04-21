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
    /// Represents the WMI event fired when a log message is sent by the client to a distribution strategy.
    /// </summary>
    [ComVisible(false)]
    public class LoggingLogDistributedEvent : LoggingServiceEvent
    {
        private static LoggingLogDistributedEvent loggingEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static LoggingLogDistributedEvent()
        {
            string[] counterNames = new string[] {LoggingServiceEvent.counters[(int)CounterID.LogDistributed].CounterName};
            loggingEvent = new LoggingLogDistributedEvent(counterNames);
            writerLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Fire the WMI event
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        public static void Fire(string message)
        {
            loggingEvent.FireEvent(message);
        }

        private LoggingLogDistributedEvent(string[] counterNames)
            : base(counterNames, null)
        {
        }

        /// <summary>
        /// Fire the WMI event
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        protected void FireEvent(string message)
        {
            writerLock.AcquireWriterLock(LockTimeout);
            try
            {
                eventMessage = message;
                InstrumentedEvent.FireWmiEvent(this); // fire WMI event
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }

            FireAuxEvent(string.Empty);
        }
    }
}