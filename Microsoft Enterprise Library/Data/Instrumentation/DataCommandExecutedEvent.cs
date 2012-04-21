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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    /// <devdoc>
    /// For performance reason, this class does not inherit from DataServiceEvent because
    /// the event does not need to fire a WMI event.
    /// </devdoc>    
    public sealed class DataCommandExecutedEvent
    {
        private static string[] counterNames = new string[]
            {
                DataServiceEvent.Counters[(int)DataServiceEvent.CounterIndex.CommandExecuted].CounterName,
                DataServiceEvent.Counters[(int)DataServiceEvent.CounterIndex.AvgCommandExecuteTime].CounterName,
                DataServiceEvent.Counters[(int)DataServiceEvent.CounterIndex.TotalCommandExecuted].CounterName
            };

        private static InstrumentedEvent instrumentedEvent = new InstrumentedEvent(SR.CounterCategory, counterNames, true);
        private static PerformanceCounterInstances[] timerCounters = new PerformanceCounterInstances[] {instrumentedEvent.GetPerformanceCounterInstances(DataServiceEvent.Counters[(int)DataServiceEvent.CounterIndex.AvgCommandExecuteTime].CounterName)};

        private DataCommandExecutedEvent()
        {
        }

        /// <summary/>
        /// <param name="startTime"/>
        /// <exclude/>
        public static void Fire(DateTime startTime)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            long[] counterIncrements = new long[] {duration.Ticks};
            instrumentedEvent.FireEvent(string.Empty, timerCounters, counterIncrements);
        }
    }
}