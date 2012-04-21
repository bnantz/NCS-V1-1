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
    public class DataTransactionRolledBackEvent : DataServiceEvent
    {
        private static string[] counterNames = new string[] {DataServiceEvent.Counters[(int)CounterIndex.TransactionRolledBack].CounterName};
        private static DataTransactionRolledBackEvent transactionRolledBackEvent = new DataTransactionRolledBackEvent(counterNames);
        private static object lockObj = new object();

        private DataTransactionRolledBackEvent(string[] counterNames) : base(counterNames)
        {
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public static void Fire(string connectionString)
        {
            transactionRolledBackEvent.FireEvent(connectionString);
        }

        private void FireEvent(string connectionString)
        {
            lock (lockObj)
            {
                Message = SR.MessageConnection(connectionString);
                InstrumentedEvent.FireWmiEvent(this);
            }

            FireAuxEvent(string.Empty);
        }
    }
}