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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Configuration data for the Msmq Distributor Service
    /// </summary>
    [XmlRoot("distributorService", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class MsmqDistributorServiceData
    {
        private string serviceName;
        private string msmqPath;
        private int queueTimerInterval;

        /// <summary>
        /// Creates a new instance of the <see cref="MsmqDistributorServiceData"/> class using default values.
        /// </summary>
        public MsmqDistributorServiceData()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MsmqDistributorServiceData"/> class using 
        /// the given service name, MSMQ Path and queue interval.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="msmqPath">The MSMQ Path to use.</param>
        /// <param name="queueTimerInterval">The queue timer interval.</param>
        public MsmqDistributorServiceData(string serviceName, string msmqPath, int queueTimerInterval)
        {
            this.serviceName = serviceName;
            this.msmqPath = msmqPath;
            this.queueTimerInterval = queueTimerInterval;
        }

        /// <summary>
        /// Name of the MSMQ distributor windows service.
        /// </summary>
        [XmlAttribute("serviceName")]
        public string ServiceName
        {
            get { return this.serviceName; }
            set { this.serviceName = value; }
        }

        /// <summary>
        /// Message queue path used by the MSMQ distributor windows service.
        /// </summary>
        [XmlAttribute("msmqPath")]
        public string MsmqPath
        {
            get { return this.msmqPath; }
            set { this.msmqPath = value; }
        }

        /// <summary>
        /// Timer interval to poll the message queue in the MSMQ distributor windows service.
        /// </summary>
        [XmlAttribute("queueTimerInterval")]
        public int QueueTimerInterval
        {
            get { return this.queueTimerInterval; }
            set { this.queueTimerInterval = value; }
        }
    }
}