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
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
	/// <summary>
	/// Represents a message queue logging sink.  
	/// Path is retrieved from the configuration settings.
	/// </summary>
    public class MsmqSink : LogSink
	{
        private LoggingConfigurationView loggingConfigurationView;
        private DefaultLogDestination defaultSink;

		/// <summary>
		/// Default constructor
		/// </summary>
		public MsmqSink()
		{
            this.defaultSink = new DefaultLogDestination();
		}

		/// <summary>
		/// <para>
		/// Initializes the sink with its name and any additional attributes. 
		/// </para>
		/// </summary>        
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="MsmqSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
		{
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
		}

		/// <summary>
		/// Send the log entry to the configured message queue path.
		/// </summary>
		/// <param name="log">Log message to send.</param>
		protected override void SendMessageCore(LogEntry log)
		{
            if (ValidateQueuePath(log))
			{
				try
				{
					SendMessageToQueue(log);
				}
				catch (Exception e)
				{
					log.AddErrorMessage(SR.SinkFailure(e.ToString()));
					throw;
				}
                catch
                {
                    log.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
                }
            }
		}

		private void SendMessageToQueue(LogEntry logEntry)
		{
            MsmqSinkData msmqSinkData = GetMsmqSinkDataFromCursor();

            using (MessageQueue messageQueue = new MessageQueue(msmqSinkData.QueuePath))
            {
			Message queueMessage = new Message();
			queueMessage.Body = FormatEntry(logEntry);
			queueMessage.Label = logEntry.Title;
                queueMessage.Priority = msmqSinkData.MessagePriority;

                MessageQueueTransactionType transactionType = (messageQueue.Transactional)
				? MessageQueueTransactionType.Single
				: MessageQueueTransactionType.None;

                messageQueue.Send(queueMessage, transactionType);
                messageQueue.Close();
            }
        }

        private MsmqSinkData GetMsmqSinkDataFromCursor()
        {
            SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(sinkData, typeof(MsmqSinkData));

            return (MsmqSinkData)sinkData;
		}

        private bool ValidateQueuePath(LogEntry log)
		{
            MsmqSinkData msmqSinkData = GetMsmqSinkDataFromCursor();

            bool valid = true;
            if (msmqSinkData.QueuePath == null || msmqSinkData.QueuePath.Length == 0)
			{
				valid = false;
                log.AddErrorMessage(SR.ExceptionMsmqSinkMissingConfiguration);
				this.defaultSink.SendMessage(log);
			}
			return valid;
		}
	}
}
