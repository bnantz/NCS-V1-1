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
    /// Represents config data for a <see cref="EnterpriseLibrary.Logging.Distributor.InProcLogDistributionStrategy"/>.
    /// </summary>
    [XmlRoot("distributionStrategy", Namespace=LoggingSettings.ConfigurationNamespace)]
    public class InProcDistributionStrategyData : DistributionStrategyData
    {
        /// <summary>
        /// Create a new instance of the <see cref="InProcDistributionStrategyData"/>.
        /// </summary>
        public InProcDistributionStrategyData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InProcDistributionStrategyData"/> with the given name.
        /// </summary>
        /// <param name="name">The name to use for this instance.</param>
        public InProcDistributionStrategyData(string name) : base(name)
        {
        }

        /// <summary>
        /// Returns the fully qualified assmebly name for an <c>InProcDistributionStrategyData</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(InProcLogDistributionStrategy).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}