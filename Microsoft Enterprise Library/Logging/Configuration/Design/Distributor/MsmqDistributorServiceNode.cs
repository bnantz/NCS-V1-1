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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor
{
    /// <summary>
    /// Summary description for MsmqDistributorServiceNode.
    /// </summary>
	[Image(typeof(MsmqDistributorServiceNode))]
	public class MsmqDistributorServiceNode : ConfigurationNode
    {
        private MsmqDistributorServiceData msmqDistributorServiceData;

        /// <summary>
        /// Creates a node with default data.
        /// </summary>
        public MsmqDistributorServiceNode() : this(new MsmqDistributorServiceData(DefaultValues.DistributorServiceName, DefaultValues.DistributorMsmqPath, DefaultValues.DistributorQueueTimerInterval))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        public MsmqDistributorServiceNode(MsmqDistributorServiceData msmqDistributorServiceData) : base()
        {
            if (msmqDistributorServiceData == null)
            {
                throw new ArgumentNullException("msmqDistributorServiceData");
            }
            this.msmqDistributorServiceData = msmqDistributorServiceData;            
        }

        /// <summary>
        /// Name of the MSMQ distributor windows service.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DistributorServiceNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string ServiceName
        {
            get { return this.msmqDistributorServiceData.ServiceName; }
            set { this.msmqDistributorServiceData.ServiceName = value; }
        }

        /// <summary>
        /// The configured name.
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Timer interval to poll the message queue in the MSMQ distributor windows service.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.QueueTimerIntervalDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int QueueTimerInterval
        {
            get { return this.msmqDistributorServiceData.QueueTimerInterval; }
            set { this.msmqDistributorServiceData.QueueTimerInterval = value; }
        }

        /// <summary>
        /// Message queue path used by the MSMQ distributor windows service.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.MsmqPathDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string MsmqPath
        {
            get { return this.msmqDistributorServiceData.MsmqPath; }
            set { this.msmqDistributorServiceData.MsmqPath = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual MsmqDistributorServiceData MsmqDistributorServiceData
        {
            get { return msmqDistributorServiceData; }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.DistributorService;
        }

    }
}