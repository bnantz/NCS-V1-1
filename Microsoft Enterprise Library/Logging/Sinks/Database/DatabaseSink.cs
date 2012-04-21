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
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database
{
    /// <summary>
    /// Represents a database logging sink.  Executes a stored procedure for each log entry.
    /// Database and stored procedure are retrieved from the configuration settings.
    /// </summary>
    public class DatabaseSink : LogSink
    {
        private DefaultLogDestination defaultSink;
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseSink()
        {
            defaultSink = new DefaultLogDestination();
        }

        /// <summary>
        /// <para>
        /// Initializes the provider with its name and any attributes associated with the provider. 
        /// </para>
        /// </summary>  
        /// <param name="configurationView">A <see cref="LoggingConfigurationView"></see> object.</param>      
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView) configurationView;
        }

        /// <summary>
        /// Execute a stored procedure to record the log entry.
        /// </summary>
        /// <param name="logEntry"><see cref="LogEntry"></see> to send to database</param>
        protected override void SendMessageCore(LogEntry logEntry)
        {
            if (ValidateParameters(logEntry))
            {
                try
                {
                    ExecuteStoredProcedure(logEntry);
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
            bool valid = true;
            DatabaseSinkData databaseSinkData = loggingConfigurationView.GetSinkData(ConfigurationName) as DatabaseSinkData;
            if (databaseSinkData == null)
            {
                valid = false;
            }

            if (databaseSinkData.DatabaseInstanceName == null ||
                databaseSinkData.DatabaseInstanceName.Length == 0)
            {
                valid = false;
            }

            if (databaseSinkData.StoredProcName == null ||
                databaseSinkData.StoredProcName.Length == 0)
            {
                valid = false;
            }

            if (!valid)
            {
                logEntry.AddErrorMessage(SR.DatabaseSinkMissingParameters);
                this.defaultSink.SendMessage(logEntry);
            }

            return valid;
        }

        private void ExecuteStoredProcedure(LogEntry logEntry)
        {
            DatabaseSinkData databaseSinkData = loggingConfigurationView.GetSinkData(ConfigurationName) as DatabaseSinkData;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(loggingConfigurationView.ConfigurationContext);
            Data.Database db = factory.CreateDatabase(databaseSinkData.DatabaseInstanceName);
            DBCommandWrapper cmd = db.GetStoredProcCommandWrapper(databaseSinkData.StoredProcName);

            cmd.AddInParameter("eventID", DbType.Int32, logEntry.EventId);
            cmd.AddInParameter("category", DbType.String, logEntry.Category);
            cmd.AddInParameter("priority", DbType.Int32, logEntry.Priority);
            cmd.AddInParameter("severity", DbType.String, logEntry.Severity.ToString());
            cmd.AddInParameter("title", DbType.String, logEntry.Title);
            cmd.AddInParameter("timestamp", DbType.DateTime, logEntry.TimeStamp);
            cmd.AddInParameter("machineName", DbType.String, logEntry.MachineName);
            cmd.AddInParameter("AppDomainName", DbType.String, logEntry.AppDomainName);
            cmd.AddInParameter("ProcessID", DbType.String, logEntry.ProcessId);
            cmd.AddInParameter("ProcessName", DbType.String, logEntry.ProcessName);
            cmd.AddInParameter("ThreadName", DbType.String, logEntry.ManagedThreadName);
            cmd.AddInParameter("Win32ThreadId", DbType.String, logEntry.Win32ThreadId);
            cmd.AddInParameter("message", DbType.String, logEntry.Message);
            cmd.AddInParameter("formattedmessage", DbType.String, FormatEntry(logEntry));

            db.ExecuteNonQuery(cmd);
        }
    }
}
