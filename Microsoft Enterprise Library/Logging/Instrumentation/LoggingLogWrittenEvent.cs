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
    /// Represents the WMI event fired when a log message is written to a sink..
    /// </summary>
    [ComVisible(false)]
    public class LoggingLogWrittenEvent : LoggingServiceEvent
    {
        private static LoggingLogWrittenEvent logEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static LoggingLogWrittenEvent()
        {
            string[] counterNames = new string[] {LoggingServiceEvent.counters[(int)CounterID.LogWritten].CounterName};
            logEvent = new LoggingLogWrittenEvent(counterNames);
            writerLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Fire the WMI event.
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        public static void Fire(string message)
        {
            logEvent.FireEvent(message);
        }

        private LoggingLogWrittenEvent(string[] counterNames) : base(counterNames, null)
        {
        }

        /// <summary>
        /// Fire the WMI event.
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
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