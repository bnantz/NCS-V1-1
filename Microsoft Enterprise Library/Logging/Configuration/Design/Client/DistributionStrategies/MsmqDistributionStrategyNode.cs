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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies
{
    /// <summary>
    /// Node representing a <see cref="EnterpriseLibrary.Logging.Distributor.MsmqLogDistributionStrategy"/>.
    /// </summary>
    public class MsmqDistributionStrategyNode : DistributionStrategyNode
    {
        private MsmqDistributionStrategyData msmqDistributionStrategyData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public MsmqDistributionStrategyNode() : this(new MsmqDistributionStrategyData(SR.MsmqDistributionStrategy, DefaultValues.DistributorMsmqPath))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="msmqDistributionStrategyData">Configuration data.</param>
        public MsmqDistributionStrategyNode(MsmqDistributionStrategyData msmqDistributionStrategyData) : base(msmqDistributionStrategyData)
        {
            this.msmqDistributionStrategyData = msmqDistributionStrategyData;
        }

        /// <summary>
        /// Path for MSMQ.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.MsmqPathDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string QueuePath
        {
            get { return msmqDistributionStrategyData.QueuePath; }
            set { msmqDistributionStrategyData.QueuePath = value; }
        }

        /// <summary>
        /// The configured name.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Read only.  Returns the type name of a <see cref="EnterpriseLibrary.Logging.Distributor.MsmqLogDistributionStrategy"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return typeof(MsmqLogDistributionStrategy).AssemblyQualifiedName; }
        }

        /// <summary>
        /// Returns the <c>TypeName</c> for visibility in the UI.
        /// </summary>
        [ReadOnly(true)]
        [SRDescription(SR.Keys.DistributionStrategyTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Type
        {
            get { return TypeName; }
        }
    }
}