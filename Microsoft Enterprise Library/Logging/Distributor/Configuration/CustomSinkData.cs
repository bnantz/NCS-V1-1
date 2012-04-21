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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Contains configuration data for a custom Sink.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class CustomSinkData : SinkData
    {
        private NameValueItemCollection attributes = new NameValueItemCollection();
        private string typeName;

        /// <summary>
        /// Create a new instance of a <see cref="CustomSinkData"/> class.
        /// </summary>
        public CustomSinkData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public CustomSinkData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomSinkData"/> class with a name and type name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="typeName">
        /// The type name of the provider.
        /// </param>
        public CustomSinkData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
        }

        /// <summary>
        /// Retrieves custom configuration items
        /// </summary>
        [XmlElement("extensions")]
        public NameValueItemCollection Attributes
        {
            get { return this.attributes; }
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

    }
}