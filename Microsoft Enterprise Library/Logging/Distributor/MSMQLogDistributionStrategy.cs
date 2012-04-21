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
using System.IO;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// MsmqLogDistributionStrategy will distribute log messages asynchronously, meaning
    /// log messages will be sent to the MSMQ instead of log sinks.  The DistributorService 
    /// will pick the messages from MSMQ and send them to corresponding log
    /// sinks.
    /// This strategy can also be used to send all log messages to the remote 
    /// queue to centralize the log message processing.
    /// </summary>
    public class MsmqLogDistributionStrategy : ConfigurationProvider, ILogDistributionStrategy
    {
        private static object syncObject = new object();
        private MsmqDistributionStrategyData msmqDistributionStrategyData;

        /// <devdoc>
        /// We don't create a default MsmqDistributionStrategyData since we really need a queue path.
        /// </devdoc>
        public MsmqLogDistributionStrategy()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MsmqLogDistributionStrategy"></see>
        /// </summary>
        /// <param name="msmqDistributionStrategyData">Configuration data for <see cref="MsmqLogDistributionStrategy"></see></param>
        public MsmqLogDistributionStrategy(MsmqDistributionStrategyData msmqDistributionStrategyData)
        {
            ArgumentValidation.CheckForNullReference(msmqDistributionStrategyData, "msmqDistributionStrategyData");

            this.msmqDistributionStrategyData = msmqDistributionStrategyData;
        }

        /// <summary>
        /// <para>
        /// Initializes the provider with its name and any attributes associated with the provider. 
        /// </para>
        /// </summary>   
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="MsmqLogDistributionStrategy"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (LoggingConfigurationView));
            LoggingConfigurationView loggingConfigurationView = (LoggingConfigurationView) configurationView;

            DistributionStrategyData distributionStrategyData = loggingConfigurationView.GetDistributionStrategyData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(distributionStrategyData, typeof (MsmqDistributionStrategyData));

            msmqDistributionStrategyData = (MsmqDistributionStrategyData) distributionStrategyData;
        }

        /// <summary>
        /// Send message to MSMQ for asynchronous message distribution.
        /// </summary>
        /// <param name="entry">Message to log.</param>
        public void SendLog(LogEntry entry)
        {
            // CODE_REVIEW : Highly suspect we can probably optimize away the need of the SoapFormatter
            SoapFormatter formatter = new SoapFormatter();

            string serializedEntry = String.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, entry);
                ms.Position = 0;
                byte[] buffer = new byte[ms.Length];
                ms.Read(buffer, 0, (int) ms.Length);
                serializedEntry = Encoding.UTF8.GetString(buffer);
            }

            SendMsmqMesage(serializedEntry);

            LoggingLogDistributedEvent.Fire(serializedEntry);
        }

        private void SendMsmqMesage(string serializedEntry)
        {
            lock (syncObject)
            {
                using (MessageQueue messageQueue = new MessageQueue(msmqDistributionStrategyData.QueuePath))
                {
                    MessageQueueTransactionType transactionType = (messageQueue.Transactional)
                        ? MessageQueueTransactionType.Single
                        : MessageQueueTransactionType.None;

                    messageQueue.Formatter = new XmlMessageFormatter();
                    messageQueue.Send(serializedEntry, transactionType);
                    messageQueue.Close();
                }
            }
        }
    }
}
