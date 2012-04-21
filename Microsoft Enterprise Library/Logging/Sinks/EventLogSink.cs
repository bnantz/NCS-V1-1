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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents an event log logging sink.  Writes the message to the 
    /// Application Log of the local machine.
    /// </summary>
    public class EventLogSink : LogSink
    {
        private const string DefaultEventLog = "Application";
        private LoggingConfigurationView loggingConfigurationView;
        private DefaultLogDestination defaultSink;

    	/// <summary>
        /// Creates an instance of an EmailLogSink.
        /// </summary>
        public EventLogSink() : base()
        {
            this.defaultSink = new DefaultLogDestination();
        }

        /// <summary>
        /// Initializes the sink with its name and any additional attributes. 
        /// </summary>  
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="EventLogSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
        }

        /// <summary>
        /// Write the log entry to the configured event log.
        /// </summary>
        /// <param name="logEntry"><see cref="LogEntry"></see> to be logged to event log</param>
        protected override void SendMessageCore(LogEntry logEntry)
        {
            EventLogSinkData eventLogSinkData = GetEventLogSinkDataFromCursor();

            short categoryId = defaultSink.GetCategoryId(logEntry);

            if (ValidateParameters(logEntry))
            {
                try
                {
                    using (EventLog eventLog = new EventLog(eventLogSinkData.EventLogName, ".", eventLogSinkData.EventSourceName))
                    {
                        eventLog.WriteEntry(
                            FormatEntry(logEntry),
                            SeverityMap.GetEventLogEntryType(logEntry.Severity),
                            logEntry.EventId,
                            categoryId);
                    }
                }
                catch (Exception e)
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(e.ToString()));
                    throw;
                }
                catch
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
                    throw;
                }
            }
        }

        private bool ValidateParameters(LogEntry log)
        {
            EventLogSinkData eventLogSinkData = GetEventLogSinkDataFromCursor();

            bool valid = true;

            if (eventLogSinkData.EventSourceName == null ||
                eventLogSinkData.EventSourceName.Length == 0)
            {
                valid = false;
                log.AddErrorMessage(SR.EventLogSinkMissingEventSource);
                this.defaultSink.SendMessage(log);
            }

            if (eventLogSinkData.EventLogName == null
                || eventLogSinkData.EventLogName.Length == 0)
            {
                eventLogSinkData.EventLogName = DefaultEventLog;
            }

            return valid;
        }

        private EventLogSinkData GetEventLogSinkDataFromCursor()
        {
            SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(sinkData, typeof (EventLogSinkData));

            return (EventLogSinkData) sinkData;
        }
    }
}