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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Instance based class to write log messages to a specific configuration context.
    /// Messages are routed based on category.  Logging can be configured  
    /// synchronously or asynchronously using the Msmq Log Distribution Strategy.
    /// </summary>
    /// <remarks>
    /// To write log messages to the current configuration context, use 
    /// the <see cref="Logger"/> facade.  Only create an instance of a LogWriter
    /// if you need to write log messages using a custom configuration context.
    /// </remarks>
    public class LogWriter
    {
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="configurationContext"><see cref="ConfigurationContext"></see> containing all information needed to write a <see cref="LogEntry"></see></param>
        public LogWriter(ConfigurationContext configurationContext)
        {
            this.loggingConfigurationView = new LoggingConfigurationView(configurationContext);
        }

        /// <summary>
        /// Add a key/value pair to the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> dictionary.  
        /// Context items will be recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key</param>
        /// <param name="value">Value.  Objects will be serialized.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        public void SetContextItem(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            ContextItems items = new ContextItems();
            items.SetContextItem(key, value);
        }

        /// <summary>
        /// Empty the context items dictionary.
        /// </summary>
        public void FlushContextItems()
        {
            ContextItems items = new ContextItems();
            items.FlushContextItems();
        }

        /// <summary>
        /// Write a new log entry as defined in the <see cref="LogEntry"/> parameter.
        /// </summary>
        /// <param name="log">Log entry object to write.</param>
        public void Write(LogEntry log)
        {
            LoggingSettings settings = loggingConfigurationView.GetLoggingSettings();
            if (!settings.LoggingEnabled)
            {
                return;
            }

            LogFilter filters = new LogFilter(loggingConfigurationView);
            if (filters.CheckFilters(log))
            {
                SendMessage(log);
            }
        }

        private void SendMessage(LogEntry logEntry)
        {
            ContextItems items = new ContextItems();
            items.ProcessContextItems(logEntry);
            LoggingSettings settings = loggingConfigurationView.GetLoggingSettings();
            LogDistributionStrategyFactory factory = new LogDistributionStrategyFactory(this.loggingConfigurationView.ConfigurationContext);
            ILogDistributionStrategy strategy = factory.CreateDistributionStrategy(settings.DistributionStrategy);
            strategy.SendLog(logEntry);
        }
    }
}