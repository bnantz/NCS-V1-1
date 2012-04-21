//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Text;
using System.Web.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    internal class EmailMessage
    {
        private static object emailLock = new object();

        private EmailSinkData configurationData;
        private LogEntry logEntry;
    	private ILogFormatter formatter;

    	public EmailMessage(EmailSinkData configurationData, LogEntry logEntry, ILogFormatter formatter)
        {
            this.configurationData = configurationData;
            this.logEntry = logEntry;
			this.formatter = formatter;
        }

        public virtual void Send()
        {
            MailMessage message = CreateMailMessage();
            SendMessage(message);
        }

        internal virtual void SendMessage(MailMessage message)
        {
            lock (emailLock)
            {
                SmtpMail.SmtpServer = configurationData.SmtpServer;
                SmtpMail.Send(message);
            }
        }

        private MailMessage CreateMailMessage()
        {
            string header = GenerateSubjectPrefix(configurationData.SubjectLineStarter);
            string footer = GenerateSubjectSuffix(configurationData.SubjectLineEnder);

            string sendToSmtpSubject = header + logEntry.Severity + footer;

            MailMessage message = new MailMessage();
            message.Body = formatter.Format(logEntry);
            message.From = configurationData.FromAddress;
            message.To = configurationData.ToAddress;
            message.Subject = sendToSmtpSubject;
            message.BodyEncoding = Encoding.UTF8;

            return message;
        }

        private string GenerateSubjectPrefix(string subjectLineField)
        {
            return IsEmpty(subjectLineField)
                ? ""
                : subjectLineField + " ";
        }

        private string GenerateSubjectSuffix(string subjectLineField)
        {
            return IsEmpty(subjectLineField)
                ? ""
                : " " + subjectLineField;
        }

        private bool IsEmpty(string subjectLineMarker)
        {
            return subjectLineMarker == null || subjectLineMarker.Length == 0;
        }
    }
}