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
    /// Represents a client-side log filter based on message category.  
    /// Either specific categories can be explicitly allowed, or specific categories can be denied.
    /// </summary>
    internal class CategoryFilter : ILogFilter
    {
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Initializes a new instance with configuration data.
        /// </summary>
        public CategoryFilter(LoggingConfigurationView loggingConfigurationView)
        {
            this.loggingConfigurationView = loggingConfigurationView;
        }

        /// <summary>
        /// Test a log entry against the category filters.
        /// </summary>
        /// <param name="log">Log entry to test.</param>
        /// <returns>Returns true if the log entry passes through the category filter.</returns>
        public bool Filter(LogEntry log)
        {
            LoggingSettings settings = loggingConfigurationView.GetLoggingSettings();
            bool passFilter = true;
            CategoryFilterData categoryFilterData = settings.CategoryFilters[log.Category];
            string category = null;
            if (categoryFilterData != null)
            {
                category = categoryFilterData.Name;
            }
            CategoryFilterMode mode = settings.CategoryFilterMode;
            // if the category is not in the hashtable and the mode is allow - filter out the message
            if ((category == null) && (mode == CategoryFilterMode.DenyAllExceptAllowed))
            {
                passFilter = false;
            }
                // if the category is in the hashtable and the mode is deny - filter out the message
            else if ((category != null) && (mode == CategoryFilterMode.AllowAllExceptDenied))
            {
                passFilter = false;
            }
            return passFilter;
        }
    }
}