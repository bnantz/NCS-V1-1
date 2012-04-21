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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DataConnectionOpenedEvent : DataServiceEvent
    {
        private static string[] counterNames = new string[] {DataServiceEvent.Counters[(int)CounterIndex.ConnectionOpenned].CounterName};
        private static DataConnectionOpenedEvent openedEvent = new DataConnectionOpenedEvent(counterNames);
        private static object lockObj = new object();

        private DataConnectionOpenedEvent(string[] counterNames) : base(counterNames, null)
        {
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public static void Fire(string connectionString)
        {
            openedEvent.FireEvent(connectionString);
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        protected void FireEvent(string connectionString)
        {
            lock (lockObj)
            {
                Message = SR.MessageDataConnectionOpened(connectionString);
                InstrumentedEvent.FireWmiEvent(this);
            }
            FireAuxEvent(Message);
        }
    }
}