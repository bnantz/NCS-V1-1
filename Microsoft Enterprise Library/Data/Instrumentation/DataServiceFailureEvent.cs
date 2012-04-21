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

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DataServiceFailureEvent : DataServiceEvent
    {
        private static EventLogIdentifier[] eventLogIds = new EventLogIdentifier[] {new EventLogIdentifier(EventLogEntryType.Error, (int)LogIndex.FailureOccurred)};
        private static DataServiceFailureEvent failureEvent = new DataServiceFailureEvent(eventLogIds);
        private static object lockObj = new object();

        private Exception exception;
        private string exceptionMessage;

        /// <summary/>
        /// <param name="eventLogIDs"/>
        /// <exclude/>
        protected DataServiceFailureEvent(EventLogIdentifier[] eventLogIDs) : base(null, eventLogIDs)
        {
            this.exceptionMessage = string.Empty;
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="ex"/>
        /// <exclude/>
        public static void Fire(string message, Exception ex)
        {
            failureEvent.FireEvent(message, ex);
        }

        /// <summary/>
        /// <exclude/>
        protected Exception Exception
        {
            get { return this.exception; }
            set { this.exception = value; }
        }

        /// <summary/>
        /// <exclude/>
        public string ExceptionMessage
        {
            get
            {
                if (this.exceptionMessage.Length == 0)
                {
                    this.exceptionMessage = InstrumentedEvent.GetErrorMessage(exception);
                }
                return this.exceptionMessage;
            }
            set { this.exceptionMessage = value; }
        }

        /// <summary/>
        /// <exclude/>
        public string ExceptionStackTrace
        {
            get { return this.exception == null ? string.Empty : this.exception.StackTrace; }
        }

        private void FireEvent(string message, Exception ex)
        {
            lock (lockObj)
            {
                Message = message;
                Exception = ex;
                ExceptionMessage = string.Empty;
                InstrumentedEvent.FireWmiEvent(this);
            }

            FireAuxEvent(string.Concat(message, SR.ExceptionMsg, ExceptionMessage));
        }
    }
}