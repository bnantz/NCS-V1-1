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
    /// Represents the configuration settings for a custom distribution strategy or for
    /// a distribution strategy that does not have any additional settings.
    /// </summary>
    [XmlRoot("distributionStrategy", Namespace=LoggingSettings.ConfigurationNamespace)]
    public class CustomDistributionStrategyData : DistributionStrategyData
    {
        private NameValueItemCollection attributes;
        private string typeName;

        /// <summary>
        /// Create a new instance of a <see cref="CustomDistributionStrategyData"/>.
        /// </summary>
        public CustomDistributionStrategyData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomDistributionStrategyData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the strategy data.
        /// </param>
        public CustomDistributionStrategyData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomDistributionStrategyData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the strategy data.
        /// </param>
        /// <param name="typeName">
        /// The type name of the provider.
        /// </param>
        public CustomDistributionStrategyData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
            attributes = new NameValueItemCollection();
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Retrieves custom configuration attributes.
        /// </summary>
        [XmlElement("attributes")]
        public NameValueItemCollection Attributes
        {
            get { return attributes; }
        }
    }
}