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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>	
    /// <devdoc>
    /// This class can contain two performance counters, one for the app and another for _total_. 
    /// </devdoc>	
    public class PerformanceCounterInstances
    {
        private static string currentInstanceName;
        private PerformanceCounter currentInstance;
        private PerformanceCounter globalInstance;
        private bool failureReported;
        private string categoryName;
        private string counterName;

        /// <summary/>
        /// <param name="categoryName"/>
        /// <param name="counterName"/>
        /// <param name="createNewInstance"/>
        /// <exclude/>
        /// <devdoc>
        /// This class can contain two performance counters, one for the application and another for _total_. 
        /// </devdoc>
        public PerformanceCounterInstances(string categoryName, string counterName, bool createNewInstance)
        {
            this.categoryName = categoryName;
            this.counterName = counterName;

            try
            {
                if (createNewInstance)
                {
                    this.currentInstance = new PerformanceCounter(this.categoryName, this.counterName, CurrentInstanceName, false);
                }
                this.globalInstance = new PerformanceCounter(categoryName, this.counterName, SR.GlobalInstanceName, false);
            }
            catch (ArgumentNullException ex)
            {
                ReportCounterFailure(SR.ExceptionCreateInstanceFailure(this.counterName, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                ReportCounterFailure(SR.ExceptionCreateInstanceFailure(this.counterName, ex.Message));
            }
            catch (Win32Exception ex)
            {
                ReportCounterFailure(SR.ExceptionCreateInstanceFailure(this.counterName, ex.Message));
            }
        }

        /// <summary/>
        /// <exclude/>
        public static string CurrentInstanceName
        {
            get
            {
                if (currentInstanceName == null)
                {
                    currentInstanceName = ConfigurationSettings.AppSettings[SR.AppSettingApplicationName];
                    if ((currentInstanceName == null) || (currentInstanceName.Length == 0))
                    {
                        currentInstanceName = Process.GetCurrentProcess().ProcessName;
                        if (null == currentInstanceName || 0 == currentInstanceName.Length)
                        {
                            currentInstanceName = SR.DefaultInstanceName;
                        }
                    }
                }
                return currentInstanceName;
            }
        }

        /// <summary/>
        /// <exclude/>
        public string CounterName
        {
            get { return counterName; }
        }

        /// <summary/>
        /// <exclude/>
        public void RawValue(long value)
        {
            RawValueCore(value);
        }

        /// <summary/>
        /// <exclude/>
        public void Decrement()
        {
            DecrementCore();
        }

        /// <summary/>
        /// <exclude/>
        public void Increment()
        {
            IncrementCore();
        }

        /// <summary/>
        /// <param name="value"/>
        /// <exclude/>
        public void IncrementBy(long value)
        {
            IncrementByCore(value);
        }

        /// <devdoc>
        /// Make this method private so that it can have conditional attribute
        /// defined in this project.
        /// </devdoc>        
        [Conditional("USEPERFORMANCECOUNTER")]
        internal void IncrementByCore(long value)
        {
            try
            {
                if (this.currentInstance != null)
                {
                    this.currentInstance.IncrementBy(value);
                }
                if (globalInstance != null)
                {
                    this.globalInstance.IncrementBy(value);
                }
            }
            catch (InvalidOperationException ex)
            {
                ReportCounterFailure(SR.ExceptionIncrementValueFailure(this.counterName, ex.Message));
            }
            catch (Win32Exception ex)
            {
                ReportCounterFailure(SR.ExceptionIncrementValueFailure(this.counterName, ex.Message));
            }
        }

        /// <devdoc>
        /// Make this method private so that it can have conditional attribute /// defined in this project.
        /// </devdoc>
        [Conditional("USEPERFORMANCECOUNTER")]
        internal void IncrementCore()
        {
            try
            {
                if (this.currentInstance != null)
                {
                    this.currentInstance.Increment();
                }
                if (this.globalInstance != null)
                {
                    this.globalInstance.Increment();
                }
            }
            catch (InvalidOperationException ex)
            {
                ReportCounterFailure(SR.ExceptionIncrementValueFailure(this.counterName, ex.Message));
            }
            catch (Win32Exception ex)
            {
                ReportCounterFailure(SR.ExceptionIncrementValueFailure(this.counterName, ex.Message));
            }
        }

        /// <devdoc>
        /// Make this method private so that it can have conditional attribute
        /// defined in this project.
        /// </devdoc>
        [Conditional("USEPERFORMANCECOUNTER")]
        internal void DecrementCore()
        {
            try
            {
                if (this.currentInstance != null)
                {
                    this.currentInstance.Decrement();
                }
                if (this.globalInstance != null)
                {
                    this.globalInstance.Decrement();
                }
            }
            catch (InvalidOperationException ex)
            {
                ReportCounterFailure(SR.ExceptionDecrementValueFailure(this.counterName, ex.Message));
            }
            catch (Win32Exception ex)
            {
                ReportCounterFailure(SR.ExceptionDecrementValueFailure(this.counterName, ex.Message));
            }
        }

        /// <devdoc>
        /// Reports the failure in performance counters once per counter to the event log.
        /// </devdoc>        
        [Conditional("USEPERFORMANCECOUNTER")]
        private void ReportCounterFailure(string message)
        {
            if (this.failureReported)
            {
                return;
            }
            this.failureReported = true;
            // if the source does not exist, create one here
            if (EventLog.SourceExists(SR.FailureLogSource) == false)
            {
                EventLog.CreateEventSource(SR.FailureLogSource, SR.LogName, SR.MachineName);
            }
            EventLog eventlog = new EventLog(SR.LogName, SR.MachineName, SR.FailureLogSource);
            eventlog.WriteEntry(message, EventLogger.FailureLogType);
        }

        /// <devdoc>
        /// Make this method private so that it can have conditional attribute defined in this project.
        /// </devdoc>        
        [Conditional("USEPERFORMANCECOUNTER")]
        private void RawValueCore(long rawValue)
        {
            try
            {
                if (this.currentInstance != null)
                {
                    this.currentInstance.RawValue = rawValue;
                }
                if (this.globalInstance != null)
                {
                    this.globalInstance.RawValue = rawValue;
                }
            }
            catch (InvalidOperationException ex)
            {
                ReportCounterFailure(SR.ExceptionSetRawValueFailure(this.counterName, ex.Message));
            }
            catch (Win32Exception ex)
            {
                ReportCounterFailure(SR.ExceptionSetRawValueFailure(this.counterName, ex.Message));
            }
        }
    }
}