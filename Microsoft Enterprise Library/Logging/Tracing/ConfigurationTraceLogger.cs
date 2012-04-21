//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tracing
{
    /// <summary>
    /// A TraceLogger that is configuration context aware.
    /// </summary>
    internal class ConfigurationTraceLogger : ITraceLogger
    {
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationTraceLogger"/> class.
        /// </summary>
        /// <param name="loggingConfigurationView">The <see cref="LoggingConfigurationView"/> to query for the configuration data.</param>
        public ConfigurationTraceLogger(LoggingConfigurationView loggingConfigurationView)
        {
            ArgumentValidation.CheckForNullReference(loggingConfigurationView, "loggingConfigurationView");
            this.loggingConfigurationView = loggingConfigurationView;
        }

        /// <summary>
        /// Writes the given log entry to the log.
        /// </summary>
        /// <param name="entry">The log entry to write.</param>
        public void Write(LogEntry entry)
        {
            if (IsTracingEnabled())
            {
                LogWriter writer = new LogWriter(loggingConfigurationView.ConfigurationContext);
                writer.Write(entry);
            }
        }

        private bool IsTracingEnabled()
        {
            LoggingSettings settings = loggingConfigurationView.GetLoggingSettings();
            return settings.TracingEnabled;
        }
    }
}