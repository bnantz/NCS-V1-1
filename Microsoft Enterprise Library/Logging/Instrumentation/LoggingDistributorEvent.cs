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
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when a log message is distributed.
    /// </summary>
    [ComVisible(false)]
    public class LoggingDistributorEvent : LoggingServiceEvent
    {
        private static LoggingDistributorEvent distributorEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static LoggingDistributorEvent()
        {
            EventLogIdentifier[] eventLogIDs = null;
            distributorEvent = new LoggingDistributorEvent(eventLogIDs);
            writerLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Fire the WMI event.
        /// </summary>
        /// <param name="message">The message to include in the WMI event</param>
        /// <param name="started">True if event is being started</param>
        public static void Fire(string message, bool started)
        {
            distributorEvent.FireEvent(message, started);
        }

        [CLSCompliant(false)] private bool startedFlag;

        /// <summary>
        /// Started firing event.
        /// </summary>
        public bool Started
        {
            get { return startedFlag; }
        }

        /// <summary>
        /// Application base.
        /// </summary>
        public string ApplicationBase
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ApplicationBase; }
        }

        /// <summary>
        /// Application or service name.
        /// </summary>
        public string ApplicationName
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ApplicationName; }
        }

        /// <summary>
        /// Configuration file name.
        /// </summary>
        public string ConfigurationFile
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }

        private LoggingDistributorEvent(EventLogIdentifier[] eventLogIDs)
            : base(null, eventLogIDs)
        {
        }

        private void FireEvent(string message, bool started)
        {
            writerLock.AcquireWriterLock(LockTimeout);
            try
            {
                this.startedFlag = started;
                this.eventMessage = message;
                InstrumentedEvent.FireWmiEvent(this);
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }
        }
    }
}