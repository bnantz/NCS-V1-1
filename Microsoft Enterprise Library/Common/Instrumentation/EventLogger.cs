//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    public class EventLogger
    {
        internal const EventLogEntryType FailureLogType = EventLogEntryType.Error;

        private EventLogEntryType logType;
        private EventLog eventLog;
        private int eventId;
        private object lockObj = new object();

        /// <summary/>
        /// <param name="source"/>
        /// <param name="logType"/>
        /// <param name="eventId"/>
        /// <exclude/>
        public EventLogger(string source, EventLogEntryType logType, int eventId)
        {
            this.eventLog = new EventLog(SR.LogName, SR.MachineName, source);
            this.logType = logType;
            this.eventId = eventId;
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        public virtual void Log(string message)
        {
            lock (lockObj)
            {
                this.eventLog.WriteEntry(message, this.logType, this.eventId);
            }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="eventLogType"/>
        /// <exclude/>
        public virtual void Log(string message, EventLogEntryType eventLogType)
        {
            lock (lockObj)
            {
                this.eventLog.WriteEntry(message, eventLogType, this.eventId);
            }
        }
    }
}