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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// An abstract base class for LogSink implementations.
    /// </summary>
    public abstract class LogSink : ConfigurationProvider, ILogSink
    {
		private ILogFormatter formatter;
		
		/// <summary>
        /// Send message to log sink handler.
        /// </summary>
        /// <param name="entry">The log entry to send.</param>
        public void SendMessage(LogEntry entry)
        {
            PrepareMessage(entry);
            SendMessageCore(entry);
        }

        /// <summary>
        /// Gets and sets the <see cref="ILogFormatter"></see> associated with this <see cref="LogSink"></see>
        /// </summary>
		public virtual ILogFormatter Formatter
		{
			set { formatter = value; }
			get
			{
				if (formatter == null)
					formatter = new TextFormatter(new TextFormatterData());

				return formatter;
			}
		}
		
		/// <summary>
		/// Format a log entry using the current formatter.
		/// </summary>
		/// <param name="entry"><see cref="LogEntry"></see> to be formatted</param>
		/// <returns>Formatted message string.</returns>
		protected virtual string FormatEntry(LogEntry entry)
		{
		    string formattedMessage = Formatter.Format(entry);

		    if (entry.ErrorMessages != null)
		    {
		        formattedMessage = entry.ErrorMessages.ToString() + "Message: " +
		            Environment.NewLine + formattedMessage;
		    }

		    return formattedMessage;
		}

        /// <summary>
        /// Prepares a message for sending to the sink.
        /// </summary>
        /// <param name="entry">The <see cref="LogEntry"/> to prepare.</param>
        protected virtual void PrepareMessage(LogEntry entry)
        {
            AddActivityIdToLogEntry(entry);
        }

        /// <summary>
        /// Used by derived classes to implement their specific message sending mechanism.
        /// </summary>
        /// <param name="entry">The <see cref="LogEntry"/> to send.</param>
        protected abstract void SendMessageCore(LogEntry entry);

        /// <summary>
        /// Adds the current <see cref="Tracer.CurrentActivityId"/> to the <see cref="LogEntry.ExtendedProperties"/>
        /// of the given <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="entry">The entry to which the <see cref="Tracer.CurrentActivityId"/> should be added.</param>
        protected void AddActivityIdToLogEntry(LogEntry entry)
        {
            if (Tracer.CurrentActivityId != null)
            {
                if (entry.ExtendedProperties == null)
                {
                    entry.ExtendedProperties = new Hashtable();
                }
                entry.ExtendedProperties[Tracer.ActivityIdPropertyKey] = Tracer.CurrentActivityId;
            }
        }
    }
}
