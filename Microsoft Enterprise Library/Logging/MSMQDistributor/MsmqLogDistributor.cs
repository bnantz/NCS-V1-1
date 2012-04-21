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
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// Receive new log messages from MSMQ and distribute each log entry.
    /// </summary>
    internal class MsmqLogDistributor : LogDistributor
    {
        private const string Header = "HEADER";

        private bool isCompleted = true;
        private bool stopReceiving = false;

        private DistributorEventLogger eventLogger;

        /// <summary>
        /// Setup the queue and the formatter of the messages.
        /// </summary>
        public MsmqLogDistributor(ConfigurationContext configurationContext) : this(configurationContext, new DistributorEventLogger())
        {
        }

        /// <summary>
        /// Set up the queue with the specified logger
        /// </summary>
        /// <param name="configurationContext"><see cref="ConfigurationContext"></see> containing current configuration information</param>
        /// <param name="eventLogger"><see cref="DistributorEventLogger"></see> to use for distributing events from this object</param>
        public MsmqLogDistributor(ConfigurationContext configurationContext, DistributorEventLogger eventLogger) : base(configurationContext)
        {
            this.eventLogger = eventLogger;
        }

        /// <summary>
        /// Read-only property to check if the synchronous receive is completed.
        /// </summary>
        public virtual bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        /// <summary>
        /// Instructs the listener to stop receiving messages.
        /// </summary>
        public virtual bool StopReceiving
        {
            get { return this.stopReceiving; }
            set { this.stopReceiving = value; }
        }

        /// <summary>
        /// Start receiving the message(s) from the queue.
        /// The messages will be taken from the queue until the queue is empty.
        /// This method is triggered every x seconds. (x is defined in application configuration file)
        /// </summary>
        public virtual void CheckForMessages()
        {
            try
            {
                ReceiveQueuedMessages();
            }
            catch (MessageQueueException qex)
            {
                string errorMsg = LogMessageQueueException(qex.MessageQueueErrorCode, qex);
                throw new LoggingException(errorMsg, qex);
            }
            catch (LoggingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msmqPath = GetMsmqPath();
                string errorMsg = SR.MsmqReceiveGeneralError(msmqPath);
                this.eventLogger.AddMessage(Header, errorMsg);
                this.eventLogger.WriteToLog(ex, Severity.Error);

                throw new LoggingException(errorMsg, ex);
            }
            catch
            {
                string msmqPath = GetMsmqPath();
                string errorMsg = SR.MsmqReceiveGeneralError(msmqPath);
                this.eventLogger.AddMessage(Header, errorMsg);

                throw new LoggingException(errorMsg);
            }
            finally
            {
                this.isCompleted = true;
            }
        }

        protected string LogMessageQueueException(MessageQueueErrorCode code, Exception e)
        {
            Severity logType = Severity.Error;
            string errorMsg = string.Empty;
            string msmqPath = GetMsmqPath();

            if (code == MessageQueueErrorCode.TransactionUsage)
            {
                errorMsg = SR.MsmqInvalidTransactionUsage(msmqPath);

                this.eventLogger.AddMessage(Header, SR.MsmqInvalidTransactionUsage(msmqPath));
            }
            else if (code == MessageQueueErrorCode.IOTimeout)
            {
                errorMsg = SR.MsmqReceiveTimeout(msmqPath);
                this.eventLogger.AddMessage(Header, SR.MsmqReceiveTimeout(msmqPath));
                logType = Severity.Warning;
            }
            else if (code == MessageQueueErrorCode.AccessDenied)
            {
                errorMsg = SR.MsmqAccessDenied(msmqPath, WindowsIdentity.GetCurrent().Name);
                this.eventLogger.AddMessage(Header, errorMsg);
            }
            else
            {
                errorMsg = SR.MsmqReceiveError(msmqPath);
                this.eventLogger.AddMessage(Header, errorMsg);
            }

            this.eventLogger.WriteToLog(e, logType);

            return errorMsg;
        }

        private MessageQueue CreateMessageQueue()
        {
            string msmqPath = GetMsmqPath();
            MessageQueue msmq = new MessageQueue(msmqPath);
            ((XmlMessageFormatter) msmq.Formatter).TargetTypeNames = new string[] {"System.String"};
            return msmq;
        }

        private bool IsQueueEmpty()
        {
            bool empty = false;
            try
            {
                using (MessageQueue msmq = CreateMessageQueue())
                {
                    msmq.Peek(new TimeSpan(0));
                }
            }
            catch (MessageQueueException e)
            {
                if (e.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    empty = true;
                }
            }

            return empty;
        }

        private void ReceiveQueuedMessages()
        {
            this.isCompleted = false;
            while (!IsQueueEmpty())
            {
                using (MessageQueue msmq = CreateMessageQueue())
                {
                    Message message = msmq.Peek();
                    string serializedEntry = message.Body.ToString();

                    LoggingLogDistributedEvent.Fire(SR.MsmqLogReceived(serializedEntry));

                    if (serializedEntry.IndexOf("<SOAP-ENV:Envelope") > -1)
                    {
                        SoapFormatter formatter = new SoapFormatter();
                        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(serializedEntry)))
                        {
                            LogEntry logEntry = formatter.Deserialize(ms) as LogEntry;
                            if (logEntry != null)
                            {
                                this.ProcessLog(logEntry);
                            }
                        }

                    }

                    message = msmq.Receive();

                    if (this.StopReceiving)
                    {
                        this.isCompleted = true;
                        return;
                    }
                }
            }
        }

        private string GetMsmqPath()
        {
            return LoggingConfigurationView.GetDistributorSettings().DistributorService.MsmqPath;
        }
    }
}