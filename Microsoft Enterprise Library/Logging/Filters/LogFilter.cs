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

using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
    /// <summary>
    /// Provides client-side log filtering based on category and priority.  Each filter
    /// implements the ILogFilter interface and is registered in this class.
    /// Category filtering is done using a <see cref="CategoryFilter"/> and priority filtering
    /// is done using a <see cref="PriorityFilter"/>.
    /// </summary>
    internal class LogFilter
    {
        private Hashtable filters = new Hashtable();

        /// <summary>
        /// Initialize a new instance of a <see cref="LogFilter"/> class.  Registers each ILogFilter.
        /// </summary>
        public LogFilter(LoggingConfigurationView loggingConfigurationView)
        {
            RegisterFilters(loggingConfigurationView);
        }

        /// <summary>
        /// Tests the log message against the registered filters.
        /// </summary>
        /// <param name="log">Log entry message.</param>
        /// <returns>Return true if the message passes through all of the filters.</returns>
        public bool CheckFilters(LogEntry log)
        {
            bool passFilters = true;

            foreach (ILogFilter filter in filters.Values)
            {
                passFilters = filter.Filter(log) && passFilters;
            }

            return passFilters;
        }

        private void RegisterFilters(LoggingConfigurationView loggingConfigurationView)
        {
            filters.Add("categoryFilter", new CategoryFilter(loggingConfigurationView));
            filters.Add("priorityFilter", new PriorityFilter(loggingConfigurationView));
        }
    }
}