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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents an email logging sink.  Sends logs messages via smtp.
    /// Parameters, including To and From address, are retrieved from the configuration settings.
    /// </summary>
    public class EmailSink : LogSink
    {
        private DefaultLogDestination defaultSink;
        private LoggingConfigurationView loggingConfigurationView;

        /// <summary>
        /// Creates an instance of the sink.
        /// </summary>        
        public EmailSink() : base()
        {
            this.defaultSink = new DefaultLogDestination();
        }

        /// <summary>
        /// Initializes the sink with its name and any additional attributes. 
        /// </summary> 
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="EmailSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
        }

        /// <summary>
        /// Send a log entry message via email.
        /// </summary>
        /// <param name="log"><see cref="LogEntry"></see> included in email message</param>
        protected override void SendMessageCore(LogEntry log)
        {
            if (ValidateParameters(log))
            {
                try
                {
                    EmailLogEntry(log, Formatter);
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

        private bool ValidateParameters(LogEntry log)
        {
            EmailSinkData emailSinkData = GetEmailSinkDataFromView();

            bool valid = true;

            if (emailSinkData.ToAddress == null ||
                emailSinkData.ToAddress.Length == 0)
            {
                valid = false;
            }

            if (emailSinkData.FromAddress == null ||
                emailSinkData.FromAddress.Length == 0)
            {
                valid = false;
            }

            if (emailSinkData.SmtpServer == null ||
                emailSinkData.SmtpServer.Length == 0)
            {
                valid = false;
            }

            if (!valid)
            {
                string error = SR.EmailSinkMissingParameters;
                log.AddErrorMessage(error);
                this.defaultSink.SendMessage(log);
            }

            return valid;
        }

        private EmailSinkData GetEmailSinkDataFromView()
        {
            SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(sinkData, typeof (EmailSinkData));

            return (EmailSinkData) sinkData;
        }

        private void EmailLogEntry(LogEntry logEntry, ILogFormatter formatter)
        {
            EmailSinkData emailSinkData = GetEmailSinkDataFromView();
            EmailMessage emailMessage = CreateEmailMessage(emailSinkData, logEntry, formatter);
            emailMessage.Send();
        }

        internal virtual EmailMessage CreateEmailMessage(EmailSinkData sinkParameters, LogEntry logEntry, ILogFormatter formatter)
        {
            return new EmailMessage(sinkParameters, logEntry, formatter);
        }
    }
}