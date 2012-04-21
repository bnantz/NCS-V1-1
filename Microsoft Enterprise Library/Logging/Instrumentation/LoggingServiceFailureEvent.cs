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
    /// Represents the WMI event fired when an error occurs.
    /// </summary>
    [ComVisible(false)]
    public class LoggingServiceFailureEvent : LoggingServiceEvent
    {
        private static LoggingServiceFailureEvent loggingEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static LoggingServiceFailureEvent()
        {
            loggingEvent = new LoggingServiceFailureEvent(null);
            writerLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Fire the WMI event.
        /// </summary>
        /// <param name="message">Message to include in the WMI event</param>
        /// <param name="ex"><see cref="Exception"></see> to be included with WMI event</param>
        public static void Fire(string message, Exception ex)
        {
            loggingEvent.FireEvent(message, ex);
        }

        /// <summary>
        /// Failure exception.
        /// </summary>
        [CLSCompliant(false)] protected Exception failureException;

        /// <summary>
        /// Failure messge.
        /// </summary>
        [CLSCompliant(false)] protected string failureExceptionMessage;

        /// <summary>
        /// Create a new instance of the event.
        /// </summary>
        /// <param name="eventLogIDs">Array of <see cref="EventLogIdentifier"></see>s used to initialize this base event</param>
        protected LoggingServiceFailureEvent(EventLogIdentifier[] eventLogIDs)
            : base(null, eventLogIDs)
        {
            failureException = null;
            failureExceptionMessage = string.Empty;
        }

        private void FireEvent(string message, Exception ex)
        {
            writerLock.AcquireWriterLock(LockTimeout);

            try
            {
                eventMessage = message;
                failureException = ex;
                failureExceptionMessage = string.Empty;

                InstrumentedEvent.FireWmiEvent(this);
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Stack trace for the exception.
        /// </summary>
        public string ExceptionStackTrace
        {
            get { return failureException == null ? string.Empty : failureException.StackTrace; }
        }

        /// <summary>
        /// Exception message.
        /// </summary>
        public string ExceptionMessage
        {
            get
            {
                if (failureExceptionMessage.Length == 0)
                {
                    failureExceptionMessage = InstrumentedEvent.GetErrorMessage(failureException);
                }
                return failureExceptionMessage;
            }
        }
    }
}