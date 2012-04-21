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
    public class EventLogIdentifier
    {
        private EventLogEntryType logType;
        private int eventId;

        /// <summary/>
        /// <param name="logType"/>
        /// <param name="eventId"/>
        /// <exclude/>
        public EventLogIdentifier(EventLogEntryType logType, int eventId)
        {
            this.logType = logType;
            this.eventId = eventId;
        }

        /// <summary/>
        /// <exclude/>
        public EventLogEntryType LogType
        {
            get { return this.logType; }
        }

        /// <summary/>
        /// <exclude/>
        public int EventId
        {
            get { return this.eventId; }
        }
    }
}