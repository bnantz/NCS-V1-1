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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies
{
    /// <summary>
    /// Node representing an In Process Distribution Strategy.
    /// </summary>
    public class InProcDistributionStrategyNode : DistributionStrategyNode
    {
        private InProcDistributionStrategyData inProcDistributionStrategyData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public InProcDistributionStrategyNode() : this(new InProcDistributionStrategyData(SR.InProcDistributionStrategy))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="inProcDistributionStrategyData">Configuration data.</param>
        public InProcDistributionStrategyNode(InProcDistributionStrategyData inProcDistributionStrategyData) : base(inProcDistributionStrategyData)
        {
            this.inProcDistributionStrategyData = inProcDistributionStrategyData;
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
        /// Read only.  Returns the type name of a <see cref="EnterpriseLibrary.Logging.Distributor.InProcLogDistributionStrategy"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return inProcDistributionStrategyData.TypeName; }
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