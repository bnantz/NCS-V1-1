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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// Summary description for DistributorEvents.
    /// </summary>
    internal class DistributorEvents
    {
        private const string Header = "HEADER";

        private DistributorEventLogger logger;

        public DistributorEvents()
        {
            logger = new DistributorEventLogger();
        }

        public void LogProcessLogException(LogEntry logEntry, Exception ex)
        {
            // distribution failed so write the event to the event log as a backup
            logger.AddMessage(Header, SR.ProcessMessageFailed);
			
        	TextFormatter formatter = new TextFormatter(new TextFormatterData());
			string formattedMessage = formatter.Format(logEntry);
			logger.AddMessage(SR.ProcessMessageFailed2, SR.ProcessMessageFailed3(formattedMessage));

            logger.WriteToLog(ex, Severity.Error);
        }

        public void LogSendingMessageException(Exception ex, string sink, LogEntry logEntry)
        {
            logger.AddMessage(Header, SR.InvalidSink(sink));
			
			TextFormatter formatter = new TextFormatter(new TextFormatterData());
			string formattedMessage = formatter.Format(logEntry);
			logger.AddMessage(logEntry.Category, SR.InvalidSinkMessage(formattedMessage));
            
			logger.WriteToLog(ex, Severity.Warning);
        }

        /// <devdoc>
        /// Write a entry to the event log indicating that the configuration file 
        /// does not contain the desired category
        /// </devdoc>
        public void LogMissingCategoryInConfigurationWarning(string categoryName, string defaultCategory)
        {
            logger.AddMessage(Header, SR.CategoryNotFound(categoryName, defaultCategory));
            logger.WriteToLog(null, Severity.Warning);
        }

        /// <devdoc>
        /// </devdoc>
        public void LogMissingDefaultFormatterError()
        {
            logger.AddMessage(Header, SR.MissingDefaultFormatter);
            logger.WriteToLog(null, Severity.Warning);
        }
    }
}