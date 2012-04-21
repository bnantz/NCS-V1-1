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

using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies
{
    /// <summary>
    /// Node representing a custom distribution strategy.
    /// </summary>
    public class CustomDistributionStrategyNode : DistributionStrategyNode
    {
        private CustomDistributionStrategyData data;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public CustomDistributionStrategyNode() : this(new CustomDistributionStrategyData(SR.CustomDistributionStrategy))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        public CustomDistributionStrategyNode(CustomDistributionStrategyData data) : base(data)
        {
            this.data = data;
        }

        /// <summary>
        /// Custom configuration attributes.
        /// </summary>
        [SRDescription(SR.Keys.CustomDistributionStrategyAttributesDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Attributes
        {
            get { return data.Attributes; }
        }
    }
}