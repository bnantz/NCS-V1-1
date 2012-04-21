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

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
    /// <summary>
    /// Represents a client-side log filter based on message priority.  Configuration file defines
    /// the minimum priority a message needs to be processed, all other messages are dropped.
    /// </summary>
    internal class PriorityFilter : ILogFilter
    {
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Create a new instance of a <see cref="PriorityFilter"/>.  Reads the minimum priority
        /// from the configuration settings.
        /// </summary>
        public PriorityFilter(LoggingConfigurationView loggingConfigurationView)
        {
            this.loggingConfigurationView = loggingConfigurationView;
        }

        /// <summary>
        /// Test a log entry to see is greater than or equal to the minimum priority.
        /// </summary>
        /// <param name="log">Log entry to test.</param>
        /// <returns>Returns true if the log entry passes through the category filter.</returns>
        public bool Filter(LogEntry log)
        {
            LoggingSettings settings = loggingConfigurationView.GetLoggingSettings();
            int minPriority = settings.MinimumPriority;
            if (log.Priority < 0)
            {
                log.Priority = minPriority;
            }
            return (log.Priority >= minPriority);
        }
    }
}