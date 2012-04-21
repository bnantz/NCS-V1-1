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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents a flat file logging sink.  Writes the message in a text file format 
    /// on the local machine.  Path and text file name are retrieved from the configuration settings.
    /// </summary>
    public class WSSink : LogSink
    {
        private LoggingConfigurationView loggingConfigurationView;
        private DefaultLogDestination defaultSink;
        private WSSinkData wsSinkData;

        /// <summary>
        /// Create an instance of a FlatFileSink.
        /// </summary>        
        public WSSink()
        {
            this.defaultSink = new DefaultLogDestination();
        }

        /// <summary>
        /// Initializes the sink with its name and any additional attributes. 
        /// </summary>  
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="FlatFileSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
        }

        /// <summary>
        /// Append the log entry to the configured text file.
        /// </summary>
        /// <param name="logEntry"><see cref="LogEntry"></see> to be appended to logging file</param>
        protected override void SendMessageCore(LogEntry logEntry)
        {
            if (ValidateParameters(logEntry))
            {
                try
                {
                   webservice.Logging ws = new webservice.Logging();
                   ws.Url = this.wsSinkData.Url;
                   
                   webservice.WSLogEntry wsLogEntry = new webservice.WSLogEntry();
                   wsLogEntry.Message = logEntry.Message;
                   wsLogEntry.Priority = logEntry.Priority;
                   wsLogEntry.Category = logEntry.Category;
                   wsLogEntry.EventId = logEntry.EventId;
                   wsLogEntry.Severity = (webservice.Severity)Enum.Parse(typeof(webservice.Severity), logEntry.Severity.ToString());
                   wsLogEntry.Title = logEntry.Title;
                   wsLogEntry.ExtendedProperties = WSLogEntry.ToJaggedArray((Hashtable)logEntry.ExtendedProperties);

                   ws.Log(wsLogEntry);
                   
                }
                catch (Exception e)
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(e.ToString()));
                    throw;
                }
                catch
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
                }
            }
        }

        private bool ValidateParameters(LogEntry logEntry)
        {
            wsSinkData = GetWSSinkDataFromCursor();

            bool valid = true;


            if (wsSinkData.Url.Length > 0)
            {
                valid = false;
                logEntry.AddErrorMessage(SR.WSSinkMissingConfiguration);
                this.defaultSink.SendMessage(logEntry);
            }

           try
           {
              Uri testUrl = new Uri(wsSinkData.Url);
           }
           catch
           {
              valid = false;
              logEntry.AddErrorMessage(SR.WSSinkMissingConfiguration);
              this.defaultSink.SendMessage(logEntry);
           }
           
           return valid;
        }

        private WSSinkData GetWSSinkDataFromCursor()
        {
            SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(sinkData, typeof (WSSinkData));

            return (WSSinkData) sinkData;
        }
    }
}