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

using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Enumeration of log entry severities.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Default severity.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Informational severity.
        /// </summary>
        Information,

        /// <summary>
        /// Non-critical error severity.
        /// </summary>
        Warning,

        /// <summary>
        /// Critical error severity.
        /// </summary>
        Error
    }

    /// <devdoc>
    /// Translate Enterprise Library severities to EventLogEntryType severities.
    /// </devdoc>
    internal class SeverityMap
    {
        private SeverityMap()
        {
        }

        public static EventLogEntryType GetEventLogEntryType(Severity severity)
        {
            switch (severity)
            {
                case (Severity.Error):
                    return EventLogEntryType.Error;
                case (Severity.Warning):
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}