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
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents a backup destination that is used if the configured logging destination fails.
    /// Log messages are written to the event log.
    /// </summary>
    public class DefaultLogDestination
    {
        private static readonly string eventLogSourceName = SR.DefaultLogDestinationSinkName;

        /// <summary>
        /// Initialize a new instance of a <see cref="DefaultLogDestination"/> class.
        /// </summary>
        public DefaultLogDestination()
        {
        }

        /// <summary>
        /// Write the log entry to the configured event log.
        /// </summary>
        /// <param name="logEntry"><see cref="LogEntry"></see> to be written to the event log</param>
        public void SendMessage(LogEntry logEntry)
        {
            try
            {
                short categoryID = GetCategoryId(logEntry);

                logEntry.AddErrorMessage(SR.DefaultLogDestinationMessage);

                TextFormatter formatter = formatter = new TextFormatter(new TextFormatterData());
                string message = FormatEntry(formatter, logEntry);


                EventLog.WriteEntry(
                    eventLogSourceName,
                    message,
                    SeverityMap.GetEventLogEntryType(logEntry.Severity),
                    logEntry.EventId,
                    categoryID);

                LoggingLogDeliveryFailureEvent.Fire(message);
            }
            catch (Exception e)
            {
                throw new LoggingException(SR.DefaultLogDestinationFailed, e);
            }
            catch
            {
                throw new LoggingException(SR.DefaultLogDestinationFailed);
            }
        }

        /// <summary>
        /// Formats the given <see cref="LogEntry"></see> through the given <see cref="ILogFormatter"></see>
        /// </summary>
        /// <param name="formatter">Instance of class implementing the <see cref="ILogFormatter"></see> interface</param>
        /// <param name="logEntry">The <see cref="LogEntry"></see> to format</param>
        /// <returns>The formatted <see cref="LogEntry"></see></returns>
        protected virtual string FormatEntry(ILogFormatter formatter, LogEntry logEntry)
        {
            string formattedMessage = formatter.Format(logEntry);

            if (logEntry.ErrorMessages != null)
            {
                formattedMessage = logEntry.ErrorMessages.ToString() + "Message: " +
                    Environment.NewLine + formattedMessage;
            }

            return formattedMessage;
        }

        /// <summary>
        /// Reads the category ID from the extended properties dictionary.
        /// </summary>
        /// <param name="log">Log Entry.</param>
        /// <returns>Category ID.</returns>
        public short GetCategoryId(LogEntry log)
        {
            short categoryId = 0;
            try
            {
                if (log.ExtendedProperties == null)
                {
                    return categoryId;
                }

                object categoryIdObject = null;
                foreach (string key in log.ExtendedProperties.Keys)
                {
                    if(string.Compare(key, "categoryid", true, CultureInfo.InvariantCulture) == 0)
                    {
                        categoryIdObject = log.ExtendedProperties[key];
                        break;
                    }
                }

                if (categoryIdObject != null)
                {
                    if (IsNumeric(categoryIdObject.ToString()))
                    {
                        categoryId = Convert.ToInt16(categoryIdObject, CultureInfo.InvariantCulture);
                    }
                }
            }
            catch
            { /* ignore exceptions */
            }
            return categoryId;
        }

        private static bool IsNumeric(string test)
        {
            double output;
            return Double.TryParse(test, NumberStyles.Integer, CultureInfo.InvariantCulture, out output);
        }

    }
}