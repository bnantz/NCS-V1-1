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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a MSMQ distribution strategy.
    /// </summary>
    [XmlRoot("distributionStrategy", Namespace=LoggingSettings.ConfigurationNamespace)]
    public class MsmqDistributionStrategyData : DistributionStrategyData
    {
        private string queuePath;

        /// <summary>
        /// Create a new instance of a <see cref="MsmqDistributionStrategyData"/>.
        /// </summary>
        public MsmqDistributionStrategyData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqDistributionStrategyData"/> class with a name and queue path.
        /// </summary>
        /// <param name="name">
        /// The name of of the strategy.
        /// </param>
        /// <param name="queuePath">
        /// The path to the queue.
        /// </param>
        public MsmqDistributionStrategyData(string name, string queuePath) : base(name)
        {
            this.queuePath = queuePath;
        }

        /// <summary>
        /// Private or public non-transactional message queue path.
        /// </summary>
        [XmlAttribute("queuePath")]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }

        /// <summary>
        /// Returns the fully qualified assembly name of a <c>MsmqDistributionStrategyData</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(MsmqLogDistributionStrategy).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}