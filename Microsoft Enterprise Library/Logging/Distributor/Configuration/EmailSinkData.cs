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

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.EmailSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class EmailSinkData : SinkData
    {
        private string toAddress = string.Empty;
        private string fromAddress = string.Empty;
        private string subjectLineStarter = string.Empty;
        private string subjectLineEnder = string.Empty;
        private string smtpServer = string.Empty;

        /// <summary>
        /// Create a new instance of a <see cref="EmailSinkData"/>.
        /// </summary>
        public EmailSinkData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="EmailSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// Tha name of the sink
        /// </param>
        public EmailSinkData(string name) : this(name, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="EmailSinkData"/> class.
        /// </summary>
        /// <param name="name">
        /// Tha name of the sink
        /// </param>
        /// <param name="toAddress">
        /// One or more email semicolon separated addresses.
        /// </param>
        /// <param name="fromAddress">
        /// Email address that messages will be sent from.
        /// </param>
        /// <param name="subjectLineStarter">
        /// Subject prefix.
        /// </param>
        /// <param name="subjectLineEnder">
        /// Subject suffix.
        /// </param>
        /// <param name="smtpServer">
        /// SMTP server to send messages through.
        /// </param>
        public EmailSinkData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer) : base(name)
        {
            this.toAddress = toAddress;
            this.fromAddress = fromAddress;
            this.subjectLineStarter = subjectLineStarter;
            this.subjectLineEnder = subjectLineEnder;
            this.smtpServer = smtpServer;
        }

        /// <summary>
        /// One or more email semicolon separated addresses.
        /// </summary>
        [XmlAttribute("toAddress")]
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }

        /// <summary>
        /// Email address that messages will be sent from.
        /// </summary>
        [XmlAttribute("fromAddress")]
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }

        /// <summary>
        /// Subject prefix.
        /// </summary>
        [XmlAttribute("subjectLineStarter")]
        public string SubjectLineStarter
        {
            get { return subjectLineStarter; }
            set { subjectLineStarter = value; }
        }

        /// <summary>
        /// Subject suffix.
        /// </summary>
        [XmlAttribute("subjectLineEnder")]
        public string SubjectLineEnder
        {
            get { return subjectLineEnder; }
            set { subjectLineEnder = value; }
        }

        /// <summary>
        /// SMTP server to send messages through.
        /// </summary>
        [XmlAttribute("smtpServer")]
        public string SmtpServer
        {
            get { return smtpServer; }
            set { smtpServer = value; }
        }

        /// <summary>
        /// Gets the fully qualified assembly name of the <c>EmailSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(EmailSink).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}