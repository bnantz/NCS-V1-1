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
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing an MSMQ Sink.
    /// </summary>
    public class MsmqSinkNode : SinkNode
    {
        private MsmqSinkData msmqSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public MsmqSinkNode() : this(new MsmqSinkData(SR.MsmqSink, DefaultValues.DistributorMsmqPath))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="msmqSinkData">Configuration data.</param>
        public MsmqSinkNode(MsmqSinkData msmqSinkData) : base(msmqSinkData)
        {
            this.msmqSinkData = msmqSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.MsmqSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return msmqSinkData.TypeName; }
        }

        /// <summary>
        /// Path for MSMQ.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.MsmqSinkQueuePathDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string QueuePath
        {
            get { return msmqSinkData.QueuePath; }
            set { msmqSinkData.QueuePath = value; }
        }

        /// <summary>
        /// <para>Gets or sets the message priority.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="MessagePriority"/> values.</para>
        /// </value>
        [DefaultValue(MessagePriority.Normal)]
        [SRDescription(SR.Keys.MsmqSinkMessagePriorityDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public MessagePriority MessagePriority
        {
            get { return msmqSinkData.MessagePriority; }
            set { msmqSinkData.MessagePriority = value; }
        }
    }
}