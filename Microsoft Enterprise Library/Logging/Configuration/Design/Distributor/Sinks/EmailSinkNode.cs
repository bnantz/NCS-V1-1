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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing an Email Sink.
    /// </summary>
    public class EmailSinkNode : SinkNode
    {
        private EmailSinkData emailSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public EmailSinkNode() : this(new EmailSinkData(SR.EmailSink))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="emailSinkData">Configuration data.</param>
        public EmailSinkNode(EmailSinkData emailSinkData) : base(emailSinkData)
        {
            this.emailSinkData = emailSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.EmailSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return emailSinkData.TypeName; }
        }

        /// <summary>
        /// The email address to send to.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.EmailSinkToAddressDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string ToAddress
        {
            get { return emailSinkData.ToAddress; }
            set { emailSinkData.ToAddress = value; }
        }

        /// <summary>
        /// Email address that messages will be sent from.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.EmailSinkFromAddressDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string FromAddress
        {
            get { return emailSinkData.FromAddress; }
            set { emailSinkData.FromAddress = value; }
        }

        /// <summary>
        /// Subject prefix.
        /// </summary>
        [SRDescription(SR.Keys.EmailSinkSubjectLineStarterDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string SubjectLineStarter
        {
            get { return emailSinkData.SubjectLineStarter; }
            set { emailSinkData.SubjectLineStarter = value; }
        }

        /// <summary>
        /// Subject suffix.
        /// </summary>
        [SRDescription(SR.Keys.EmailSinkSubjectLineEnderDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string SubjectLineEnder
        {
            get { return emailSinkData.SubjectLineEnder; }
            set { emailSinkData.SubjectLineEnder = value; }
        }

        /// <summary>
        /// SMTP server to send messages through.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.EmailSinkSmtpServerDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string SmtpServer
        {
            get { return emailSinkData.SmtpServer; }
            set { emailSinkData.SmtpServer = value; }
        }
    }
}