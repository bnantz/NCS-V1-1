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

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// InProcLogDistributionStrategy will process the distribution of log messages synchronously
    /// by sending the log entry directly to the corresponding log sinks.
    /// </summary>
    public class InProcLogDistributionStrategy : ConfigurationProvider, ILogDistributionStrategy
    {
        private LogDistributor logDistributor;

        /// <summary>
        /// <para>
        /// Default constructor
        /// </para>
        /// </summary>        
        public InProcLogDistributionStrategy()
        {
        }

        /// <summary>
        /// Initializes the LogDistributor with its current configuration
        /// </summary>
        /// <param name="configurationView">View of configuration used by the LogDistributor</param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");

            this.logDistributor = new LogDistributor(configurationView.ConfigurationContext);
        }

        /// <summary>
        /// Distribute a log entry to the sinks using a <see cref="LogDistributor"/>.
        /// </summary>
        /// <param name="logEntry">Message body</param>
        public void SendLog(LogEntry logEntry)
        {
            this.logDistributor.ProcessLog(logEntry);

        	TextFormatter formatter = new TextFormatter(new TextFormatterData());
        	string message = formatter.Format(logEntry);
        	LoggingLogDistributedEvent.Fire(message);
        }
    }
}