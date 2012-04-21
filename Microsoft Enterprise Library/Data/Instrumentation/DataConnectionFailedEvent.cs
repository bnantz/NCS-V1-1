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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DataConnectionFailedEvent : DataServiceEvent
    {
        private static string[] counterNames = new string[]
            {
                DataServiceEvent.Counters[(int)CounterIndex.ConnectionFailed].CounterName
            };

        private static EventLogIdentifier[] eventLogIds = new EventLogIdentifier[] {new EventLogIdentifier(EventLogEntryType.Warning, (int)LogIndex.ConnectionFailed)};
        private static DataConnectionFailedEvent failedEvent = new DataConnectionFailedEvent(counterNames, eventLogIds);
        private static object lockObj = new object();

        private DataConnectionFailedEvent(string[] counterNames, EventLogIdentifier[] eventLogIDs) : base(counterNames, eventLogIDs)
        {
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public static void Fire(string connectionString)
        {
            failedEvent.FireEvent(connectionString);
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        protected void FireEvent(string connectionString)
        {
            Message = SR.ExceptionMessageDatabaseConnectionFailed(connectionString);
            lock (lockObj)
            {
                InstrumentedEvent.FireWmiEvent(this);
            }
            FireAuxEvent(Message);
        }
    }
}