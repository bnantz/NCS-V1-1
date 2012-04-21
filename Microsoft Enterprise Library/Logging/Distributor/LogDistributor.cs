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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// Distributes a log message to the corresponding sinks as defined in the configuration file.
    /// </summary>
    public class LogDistributor
    {
        private readonly DefaultLogDestination defaulLogSink;
        internal readonly DistributorEvents events;
        private readonly LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Initializes a new instance of <see cref="LogDistributor"></see>
        /// </summary>
        /// <param name="configurationContext"><see cref="ConfigurationContext"></see> used to initialize this instance</param>
        public LogDistributor(ConfigurationContext configurationContext)
        {
            this.loggingConfigurationView = new LoggingConfigurationView(configurationContext);
            this.events = new DistributorEvents();
            this.defaulLogSink = new DefaultLogDestination();
        }

        /// <summary>
        ///  Gets the <see cref="LoggingConfigurationView"></see>
        /// </summary>
        protected LoggingConfigurationView LoggingConfigurationView
        {
            get { return loggingConfigurationView; }
        }

        /// <summary> 
        /// Distribute a new log message to one or more destination sinks.  
        /// Routing is based on the category.
        /// </summary>
        /// <param name="log">Log message.</param>
        public void ProcessLog(LogEntry log)
        {
            try
            {
                CategoryData category = GetCategory(log);
                if (category == null)
                {
                    return;
                }
                DistributeLogEntry(log, category);
            }
            catch (Exception ex)
            {
                this.events.LogProcessLogException(log, ex);
				throw;
			}
        }

        /// <devdoc>
        /// Send the log message to each destination sink for the selected category.
        /// </devdoc>
        private void DistributeLogEntry(LogEntry log, CategoryData category)
        {
            foreach (DestinationData destination in category.DestinationDataCollection)
            {
                try
                {
                    ILogSink sink = CreateSink(destination.Sink);
					sink.Formatter = CreateFormatter(destination.Format);

                    sink.SendMessage(log);
					
                	LoggingLogWrittenEvent.Fire(sink.Formatter.Format(log));
                }
                catch (Exception ex)
                {
                    this.events.LogSendingMessageException(ex, destination.Sink, log);
                    this.defaulLogSink.SendMessage(log);
                }
            }
        }

        private CategoryData GetCategory(LogEntry log)
        {
            CategoryData data = null;
            if (log.Category.Length == 0)
            {
                data = this.loggingConfigurationView.GetDefaultCategoryData();
                log.Category = data.Name;
            }

            CategoryData categoryData = this.loggingConfigurationView.GetCategoryData(log.Category);
            if (categoryData == null)
            {
                categoryData = this.loggingConfigurationView.GetDefaultCategoryData();
                this.events.LogMissingCategoryInConfigurationWarning(log.Category, categoryData.Name);
            }

            return categoryData;
        }

        /// <devdoc>
        /// Get a cloned instance of the formatter.
        /// </devdoc>
        /// <returns>An instance of the ILogFormatter. 
        /// May return null if no format is specified and the default format does not exist.</returns>
        private ILogFormatter CreateFormatter(string formatterName)
        {
            ILogFormatter formatter = null;
            FormatterData formatterData = null;
            if (formatterName != null && formatterName.Length > 0)
            {
                formatterData = loggingConfigurationView.GetFormatterData(formatterName);
            }
            // if the format isn't defined in the configuration settings
            if (formatterData == null)
            {
                // get the default format
                formatterData = loggingConfigurationView.GetDefaultFormatterData();

                // if the default format doesn't exist, log a warning in the event log
                if (formatterData == null)
                {
                    this.events.LogMissingDefaultFormatterError();
                }
            }
            if (formatterData != null)
            {
                LogFormatterFactory factory = new LogFormatterFactory(loggingConfigurationView.ConfigurationContext);
                formatter = factory.CreateFormatter(formatterData.Name);
            }
            return formatter;
        }

        /// <summary>
        /// Factory method used to create an instance of an object implementing the <see cref="ILogSink"></see> interface. The specific 
        /// kind of <see cref="ILogSink"></see> created depends on the configuration information used by this factory.
        /// </summary>
        /// <param name="sinkName">Configuration name for <see cref="ILogSink"></see> instance to be created</param>
        /// <returns>Instantiated <see cref="ILogSink"></see></returns>
        public ILogSink CreateSink(string sinkName)
        {
            LogSinkFactory factory = new LogSinkFactory(loggingConfigurationView.ConfigurationContext);
            return factory.CreateSink(sinkName);
        }
    }
}
