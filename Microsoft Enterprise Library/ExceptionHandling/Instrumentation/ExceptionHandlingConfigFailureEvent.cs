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

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    /// <devdoc>
    /// Represents a configuration failure in the Exception Handling framework.
    /// </devdoc>
    public class ExceptionHandlingConfigFailureEvent : ExceptionHandlingFailureEvent
    {
        private static ExceptionHandlingConfigFailureEvent exceptionHandlingEvent = new ExceptionHandlingConfigFailureEvent(new EventLogIdentifier[]
            {
                new EventLogIdentifier(EventLogEntryType.Error,
                                       (int)Log.InternalFailureOccurred)
            });

        private static ReaderWriterLock writerLock = new ReaderWriterLock();

        private ExceptionHandlingConfigFailureEvent(EventLogIdentifier[] eventLogIDs) : base(eventLogIDs)
        {
            // Do Nothing
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="ex"/>
        /// <exclude/>
        public static void Fire(string message, Exception ex)
        {
            exceptionHandlingEvent.FireEvent(message, ex);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        public static void Fire(string message)
        {
            exceptionHandlingEvent.FireEvent(message);
        }

        private void FireEvent(string message)
        {
            FireEvent(message, null);
        }
        
        private void FireEvent(string message, Exception ex)
        {
            string errorMsg;

            writerLock.AcquireWriterLock(-1);

            try
            {
                SetMessage(message);
                SetException(ex);
                SetExceptionMessage(string.Empty);
                errorMsg = ExceptionMessage;
                InstrumentedEvent.FireWmiEvent(this); // fire WMI event
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }

            if (ex == null)
            {
                FireAuxEvent(message);
            }
            else
            {
                FireAuxEvent(message + SR.ExceptionString + errorMsg);
            }
        }
    }
}