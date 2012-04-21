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

using System;
using System.Diagnostics;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    /// <devdoc>
    /// The is the base class for all the events instrumented by Enterprise Library.
    /// Types of instrumentation includes: WMI, EventLog, and Performance Counters.
    /// </devdoc>
    [ComVisible(false)]
    public class InstrumentedEvent
    {
        private const uint WbemAccessDenied = 0x80041003;

        private static bool wmifailureReported;
        private static object lockObj = new object();

        private PerformanceCounterInstances[] perfCounters;
        private EventLogger[] eventLoggers;

        /// <summary/>
        /// <param name="counterCategory"/>
        /// <param name="counterNames"/>
        /// <param name="createNewInstance"/>
        /// <param name="eventLogSource"/>
        /// <param name="eventIds"/>
        /// <exclude/>
        /// <devdoc>
        /// This constructor create a object that represents an instrumented event.
        /// When this event is fired, it increments all its performance counter instances
        /// and write to event log for all its event id.
        /// </devdoc>        
        public InstrumentedEvent(string counterCategory, string[] counterNames, bool createNewInstance, string eventLogSource, EventLogIdentifier[] eventIds)
        {
            Initialize(counterCategory, counterNames, createNewInstance, eventLogSource, eventIds);
        }

        /// <summary/>
        /// <param name="counterCategory"/>
        /// <param name="counterNames"/>
        /// <param name="createNewInstance"/>
        /// <exclude/>
        /// <devdoc>
        /// This constructor create an object that only uses the performance counters.
        /// </devdoc>                        
        public InstrumentedEvent(string counterCategory, string[] counterNames, bool createNewInstance)
        {
            Initialize(counterCategory, counterNames, createNewInstance, null, null);
        }

        /// <summary/>
        /// <param name="baseEvent"/>
        /// <exclude/>
        /// <devdoc>
        /// The reason it wraps the BaseEvent.Fire method is to allow Wmi
        /// events to be turned on and off in the Instrumentation assembly.
        /// Otherwise, for all the frameworks, the compiler switch needs to
        /// be turned on/off for all the Instrumentation.xxx projects.
        /// </devdoc>        
        public static void FireWmiEvent(BaseEvent baseEvent)
        {
            FireWmiEventCore(baseEvent);
        }

        /// <summary/>
        /// <param name="exception"/>
        /// <returns/>
        /// <exclude/>
        /// <devdoc>
        /// This method builds the error message from an exception object.
        /// </devdoc> 
        public static string GetErrorMessage(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }
            StringBuilder errorMsg = new StringBuilder(exception.ToString());
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                errorMsg.AppendFormat("{0}{1}{2}{3}", Environment.NewLine, SR.LineSeperator, Environment.NewLine,  innerException.Message);
                innerException = innerException.InnerException;
            }
            return errorMsg.ToString();
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        /// <devdoc>
        /// This method increments the performance counters and write to event log.
        /// </devdoc>
        public void FireEvent(string message)
        {
            int index = 0;
            for (index = 0; index < perfCounters.Length; ++index)
            {
                perfCounters[index].IncrementCore();
            }
            for (index = 0; index < eventLoggers.Length; ++index)
            {
                eventLoggers[index].Log(message);
            }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="eventLogType"/>
        /// <exclude/>
        /// <devdoc>
        /// This method increments the performance counters and write to event log with the event log type passed in.
        /// </devdoc>
        public void FireEvent(string message, EventLogEntryType eventLogType)
        {
            int index = 0;
            for (index = 0; index < perfCounters.Length; ++index)
            {
                perfCounters[index].IncrementCore();
            }
            for (index = 0; index < eventLoggers.Length; ++index)
            {
                eventLoggers[index].Log(message, eventLogType);
            }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="counterInstancesArray"/>
        /// <param name="counterValues"/>
        /// <exclude/>
        /// <devdoc>
        /// This method Increment the performance counters, update the timer counter, and write to event logs.
        /// </devdoc>        
        public void FireEvent(string message, PerformanceCounterInstances[] counterInstancesArray, long[] counterValues)
        {
            int index = 0;
            for (index = 0; index < perfCounters.Length; ++index)
            {
                bool found = false;

                for (int i = 0; i < counterInstancesArray.Length && !found; i++)
                {
                    if (counterInstancesArray[i] == perfCounters[index])
                    {
                        found = true;
                        perfCounters[index].IncrementByCore(counterValues[i]);
                    }
                }
                if (!found)
                {
                    perfCounters[index].Increment();
                }
            }
            for (index = 0; index < eventLoggers.Length; ++index)
            {
                eventLoggers[index].Log(message);
            }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="increment"/>
        /// <exclude/>
        /// <devdoc>
        /// This method Increment the performance counters, update the timer counter, and write to event logs.
        /// </devdoc>
        public void FireEvent(string message, long increment)
        {
            int index = 0;
            for (index = 0; index < perfCounters.Length; ++index)
            {
                perfCounters[index].IncrementByCore(increment);
            }
            for (index = 0; index < eventLoggers.Length; ++index)
            {
                eventLoggers[index].Log(message);
            }
        }

        /// <summary/>
        /// <param name="counterName"/>
        /// <returns/>
        /// <exclude/>
        /// <devdoc>
        /// Searches the internal performance counter instances by the counter name.
        /// </devdoc>
        public PerformanceCounterInstances GetPerformanceCounterInstances(string counterName)
        {
            for (int index = 0; index < perfCounters.Length; ++index)
            {
                if (perfCounters[index].CounterName == counterName)
                {
                    return perfCounters[index];
                }
            }
            return null;
        }

        /// <devdoc>
        /// Double wrap it here so that the switch is totally contained in
        /// this assembly.  The conditional attribute on FireWmiEvent will
        /// still need the compiler switch on all the individual Instrumentation.xxx 
        /// projects.
        /// </devdoc>        
        [Conditional("USEWMI")]
        private static void FireWmiEventCore(BaseEvent baseEvent)
        {
            try
            {
                baseEvent.Fire();
            }
            catch (Exception exp)
            {
                string msg = SR.WmiEventFailure(baseEvent.GetType().Name, GetErrorMessage(exp));
                COMException comException = exp as COMException;
                if (null != comException)
                {
                    if ((uint)comException.ErrorCode == WbemAccessDenied)
                    {
                        msg = string.Concat(SR.ExceptionAccessDeniedFailure, msg);
                    }
                }
                ReportWmiFailure(msg);
            }
        }

        private static void ReportWmiFailure(string message_)
        {
            // log the failure in the event log once per AppDomain.
            lock (lockObj)
            {
                if (!wmifailureReported)
                {
                    wmifailureReported = true;
                    // if the source does not exist, create one here
                    if (EventLog.SourceExists(SR.FailureLogSource) == false)
                    {
                        EventLog.CreateEventSource(SR.FailureLogSource, SR.LogName, SR.MachineName);
                    }
                    EventLog eventlog = new EventLog(SR.LogName, SR.MachineName, SR.FailureLogSource);
                    eventlog.WriteEntry(message_, EventLogger.FailureLogType);
                }
            }
        }

        /// <devdoc>
        /// This method initializes the internal performance counters and event loggers.
        /// </devdoc>
        private void Initialize(string counterCategory, string[] counterNames, bool createNewInstance, string eventLogSource, EventLogIdentifier[] eventIds)
        {
            this.eventLoggers = new EventLogger[0];
            this.perfCounters = new PerformanceCounterInstances[0];

            if (eventLogSource != null && eventIds != null)
            {
                AddEventLogger(eventLogSource, eventIds);
            }

            if (counterCategory != null && counterNames != null)
            {
                AddPerformanceCounter(counterCategory, counterNames, createNewInstance);
            }
        }

        /// <devdoc>
        /// Add the event loggers to the internal array of event loggers.
        /// </devdoc>        
        [Conditional("USEEVENTLOG")]
        private void AddEventLogger(string source, EventLogIdentifier[] eventIds)
        {
            int count = eventIds.Length;
            if (count <= 0)
            {
                return;
            }
            this.eventLoggers = new EventLogger[count];
            for (int i = 0; i < count; i++)
            {
                EventLogger logger = new EventLogger(source, eventIds[i].LogType, eventIds[i].EventId);
                this.eventLoggers[i] = logger;
            }
        }

        /// <devdoc>
        /// This method adds the performance counter instances to the internal array.
        /// </devdoc>        
        [Conditional("USEPERFORMANCECOUNTER")]
        private void AddPerformanceCounter(string category, string[] counterNames, bool createNewInstance)
        {
            int count = counterNames.Length;
            if (count <= 0)
            {
                return;
            }

            this.perfCounters = new PerformanceCounterInstances[count];

            for (int i = 0; i < count; i++)
            {
                PerformanceCounterInstances instances = new PerformanceCounterInstances(category, counterNames[i], createNewInstance);
                this.perfCounters[i] = instances;
            }
        }
    }
}