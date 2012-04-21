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
    public sealed class DataTransactionCommittedEvent
    {
        private static string[] counterNames = new string[] {DataServiceEvent.Counters[(int)DataServiceEvent.CounterIndex.TransactionCommitted].CounterName};
        private static InstrumentedEvent instrumentedEvent = new InstrumentedEvent(SR.CounterCategory, counterNames, true);

        private DataTransactionCommittedEvent()
        {
        }

        /// <summary/>
        /// <exclude/>
        public static void Fire()
        {
            instrumentedEvent.FireEvent(string.Empty);
        }
    }
}