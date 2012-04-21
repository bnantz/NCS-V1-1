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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies
{
    /// <summary>
    /// Contains a collection of <see cref="DistributionStrategyNode"/>s.
    /// </summary>
    [Image(typeof(DistributionStrategyCollectionNode))]
    public class DistributionStrategyCollectionNode : ConfigurationNode
    {
        private DistributionStrategyDataCollection distributionStrategyDataCollection;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public DistributionStrategyCollectionNode() : this(new DistributionStrategyDataCollection())
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="distributionStrategyDataCollection">Configuration data.</param>
        public DistributionStrategyCollectionNode(DistributionStrategyDataCollection distributionStrategyDataCollection) : base()
        {
            if (distributionStrategyDataCollection == null)
            {
                throw new ArgumentNullException("distributionStrategyDataCollection");
            }
            this.distributionStrategyDataCollection = distributionStrategyDataCollection;
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
        /// Adds a <see cref="InProcDistributionStrategyNode"/> as the default node.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            InProcDistributionStrategyNode node = new InProcDistributionStrategyNode();
            Nodes.AddWithDefaultChildren(node);
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual DistributionStrategyDataCollection DistributionStrategyDataCollection
        {
            get
            {
                distributionStrategyDataCollection.Clear();
                foreach (DistributionStrategyNode node in Nodes)
                {
                    distributionStrategyDataCollection.Add(node.DistributionStrategyData);
                }

                return distributionStrategyDataCollection;
            }
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(DistributionStrategyNode));
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DistributionStrategies;
            CreateDynamicNodes(distributionStrategyDataCollection);
        }

    }
}