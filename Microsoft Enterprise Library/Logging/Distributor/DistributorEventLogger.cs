//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// Event logger for distributor windows service.
    /// This class writes event log entries 
    /// </summary>
    public class DistributorEventLogger
    {
        private static readonly string DefaultApplicationName = SR.DistributorEventLoggerDefaultApplicationName;
        private const string DefaultLogName = "Application";
        private string logName = null;
        private string applicationName = null;
        private NameValueCollection additionalInfo = new NameValueCollection();

        /// <summary>
        /// The Event logger used to back up configured logging sinks in the event of problems.
        /// Used to write diagnostic messages to the event log.
        /// </summary>
        public DistributorEventLogger()
        {
            this.ApplicationName = DefaultApplicationName;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DistributorEventLogger"/>.
        /// </summary>
        /// <param name="logName">The name of the Event Log</param>
        public DistributorEventLogger(string logName)
        {
            this.EventLogName = logName;
            this.ApplicationName = applicationName;
        }

        /// <summary>
        /// Name of the windows service.
        /// </summary>
        public string ApplicationName
        {
            get { return ((null == applicationName) ? DefaultApplicationName : applicationName); }
            set { applicationName = value; }
        }

        /// <summary>
        /// The Event Log name (i.e. the name of the event log to write to; e.g. "Application").
        /// </summary>
        public string EventLogName
        {
            get { return ((null == logName) ? DefaultLogName : logName); }
            set { logName = value; }
        }

        /// <summary>
        /// Add a message to the additional information name value collection.
        /// </summary>
        /// <param name="message">The message key</param>
        /// <param name="value">The actual message (this value will be shown in the event log)</param>
        public void AddMessage(string message, string value)
        {
            additionalInfo.Add(message, value);
        }

        /// <summary>
        /// Method used to write an exception and additional information to the event log.
        /// </summary>
        /// <param name="value">The message value itself. This will be added to the existing additionalinfo</param>
        /// <param name="exception">The exception (this is shown in the additional exception information in the event log)</param>
        /// <param name="severity">The type of event log entry (warning, information, error,...)</param>
        public void WriteToLog(string value, Exception exception, Severity severity)
        {
            additionalInfo.Add(SR.DistEventLoggerMessagePrefix, value);
            WriteToLog(exception, severity);
        }

        /// <summary>
        /// Method used to write exception and additional information to the event log, and clears the
        /// messages collection.
        /// </summary>
        /// <param name="exception">The exception object whose information should be written to log file.</param>
        /// <param name="severity">The type of event log entry (warning, information, error,...)</param>
        public void WriteToLog(Exception exception, Severity severity)
        {
            // Verify that the specified event log exists and is valid to write to.
            CheckEventLog();

            string finalMessage = String.Empty;

            finalMessage = GetMessage(exception);
            additionalInfo.Clear();

            if (severity == Severity.Error || severity == Severity.Warning)
            {
                // this only fires WMI events
                LoggingServiceFailureEvent.Fire(finalMessage, exception);
            }
            EventLog.WriteEntry(this.ApplicationName, finalMessage, SeverityMap.GetEventLogEntryType(severity));

        }

        /// <summary>
        /// Gets the message for the exception.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to get the message for.</param>
        /// <returns>The message for the exception.</returns>
        public string GetMessage(Exception exception)
        {
            ExceptionFormatter exFormatter = new ExceptionFormatter(additionalInfo, this.ApplicationName);
            return exFormatter.GetMessage(exception);
        }

        /// <summary>
        /// Check if the specified event log has been created and is available to write to.
        /// </summary>
        private void CheckEventLog()
        {
            try
            {
                if (!EventLog.SourceExists(this.EventLogName))
                {
                    EventLog.CreateEventSource(SR.ApplicationLogName , this.EventLogName);
                }
            }
            catch (SecurityException e)
            {
                throw new SecurityException(SR.EventLogAccessDenied(this.EventLogName), e.InnerException);
            }
        }
    }
}