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

using System;
using System.Globalization;
using System.Messaging;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an <see cref="EnterpriseLibrary.Logging.Sinks.MsmqSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class MsmqSinkData : SinkData
    {
        private string queuePath;
        private MessagePriority messagePriority;
        private bool recoverable;
        private bool useAuthentication;
        private bool useDeadLetterQueue;
        private bool useEncryption;

        /// <summary>
        /// Create a new instance of a <see cref="MsmqSinkData"/>.
        /// </summary>
        public MsmqSinkData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public MsmqSinkData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="queuePath">
        /// The path to the message queue.
        /// </param>
        public MsmqSinkData(string name, string queuePath) : base(name)
        {
            this.queuePath = queuePath;
            messagePriority = MessagePriority.Normal;
            recoverable = false;
//            timeToReachQueue = new DateTime(new TimeSpan(1, 0, 0, 0, 0).Ticks);
//            timeToBeReceived = new DateTime(new TimeSpan(1, 0, 0 , 0, 0).Ticks);
            useAuthentication = false;
            useDeadLetterQueue = false;
            useEncryption = false;
        }

        /// <summary>
        /// Private or public message queue.
        /// </summary>
        [XmlAttribute("queuePath")]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }

        /// <summary>
        /// Gets the fully qualified assemly name of an <c>MsmqSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(MsmqSink).AssemblyQualifiedName; }
            set {}
        }
      
        /// <summary>
        /// <para>Gets or sets the <see cref="MessagePriority"/> for each message send.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="MessagePriority"/> values.</para>
        /// </value>
        /// <seealso cref="Message.Priority"/>
        [XmlAttribute("priority")]
        public MessagePriority MessagePriority
        {
            get { return messagePriority; }
            set
            {
                if (!Enum.IsDefined(typeof(MessagePriority), value))
                {
                    throw new ArgumentException(SR.ExceptionEnumNotDefined(value.ToString(CultureInfo.InvariantCulture), typeof(MessagePriority).FullName), "MessagePriority");
                }
                messagePriority = value;
            }
        }

        /// <summary>
        /// <para>Gets or sets if the message queue is recoverable.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the queue is recoverable; otherwise <see langword="false"/>.</para>
        /// </value>
        /// <seealso cref="Message.Recoverable"/>
        [XmlAttribute("recoverable")]
        public bool Recoverable
        {
            get { return recoverable; }
            set { recoverable = value; }
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the message was (or must be) authenticated before being sent.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the sending application requested authentication for the message; otherwise, false. The default is <see langword="false"/>.</para>
        /// </value>
        /// <seealso cref="Message.UseAuthentication"/>
        [XmlAttribute("useAuthentication")]
        public bool UseAuthentication
        {
            get { return useAuthentication; }
            set { useAuthentication = value; }
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether a copy of the message that could not be delivered should be sent to a dead-letter queue.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if message-delivery failure should result in a copy of the message being sent to a dead-letter queue; otherwise, <see langword="false"/>. The default is <see langword="false"/></para>
        /// </value>
        /// <seealso cref="Message.UseDeadLetterQueue"/>
        [XmlAttribute("useDeadLetterQueue")]
        public bool UseDeadLetterQueue
        {
            get { return useDeadLetterQueue; }
            set { useDeadLetterQueue = value; }
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether to make the message private.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> to require Message Queuing to encrypt the message; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</para>
        /// </value>
        /// <seealso cref="Message.UseEncryption"/>
        [XmlAttribute("useEncryption")]
        public bool UseEncryption
        {
            get { return useEncryption; }
            set { useEncryption = value; }
        }
    }
}
