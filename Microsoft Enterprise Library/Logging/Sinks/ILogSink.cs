using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents the interface for all logging sinks.  Defines a single method, SendMessage.
    /// </summary>
    public interface ILogSink
    {
        /// <summary>
        /// Send message to log sink handler.
        /// </summary>
        /// <param name="log">xmlMessage as string</param>
        void SendMessage(LogEntry log);

		/// <summary>
		/// Sets the formatter to be used by this sink.
		/// </summary>
    	ILogFormatter Formatter { get; set; }

    }
}