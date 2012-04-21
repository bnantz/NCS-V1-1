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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for distribution strategies.  This class is abstract.
    /// </summary>
    [XmlRoot("distributionStrategy", Namespace=LoggingSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomDistributionStrategyData))]
    [XmlInclude(typeof(InProcDistributionStrategyData))]
    [XmlInclude(typeof(MsmqDistributionStrategyData))]
    public abstract class DistributionStrategyData : ProviderData
    {
        /// <summary>
        /// Create a new instance of <see cref="DistributionStrategyData"/>.
        /// </summary>
        protected DistributionStrategyData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DistributionStrategyData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the strategry.
        /// </param>
        protected DistributionStrategyData(string name) : base(name)
        {
        }
    }
}