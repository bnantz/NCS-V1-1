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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies
{
    /// <summary>
    /// Base class for nodes which represent distribution strategies.
    /// </summary>
    [Image(typeof(DistributionStrategyNode))]
    public abstract class DistributionStrategyNode : ConfigurationNode
    {
        private DistributionStrategyData distributionStrategyData;

        /// <summary>
        /// Creates node with specified runtime configuration data.
        /// </summary>
        /// <param name="distributionStrategyData">Runtime configuration data in which to initialize this node.</param>
        protected DistributionStrategyNode(DistributionStrategyData distributionStrategyData) : base()
        {
            if (distributionStrategyData == null)
            {
                throw new ArgumentNullException("data");
            }
            this.distributionStrategyData = distributionStrategyData;
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual DistributionStrategyData DistributionStrategyData
        {
            get { return distributionStrategyData; }
        }

        /// <summary>
        /// The fully qualified assembly name of the <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.ILogDistributionStrategy"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ILogDistributionStrategy))]
        [SRDescription(SR.Keys.DistributionStrategyTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return distributionStrategyData.TypeName; }
            set { distributionStrategyData.TypeName = value; }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = distributionStrategyData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            distributionStrategyData.Name = e.Node.Name;
        }


    }
}