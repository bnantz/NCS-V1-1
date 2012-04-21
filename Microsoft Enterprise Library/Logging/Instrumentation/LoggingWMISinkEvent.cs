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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// Represents a Windows Management Instrument event for that is fired by the WMILogSink.
    /// </summary>
    public class LoggingWMISinkEvent : LoggingServiceEvent
    {
        /// <summary>
        /// Fire a <see cref="LoggingWMISinkEvent"/> event.
        /// </summary>
        /// <param name="log">Log message to record.</param>
        public static void Fire(LogEntry log)
        {
            System.Management.Instrumentation.Instrumentation.Fire(log);
        }
    }
}