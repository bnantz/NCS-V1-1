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
    /// Represents config for a custom formatter.
    /// </summary>
    [XmlRoot("formatter", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class CustomFormatterData : FormatterData
    {
        private NameValueItemCollection attributes;
        private string typeName;

        /// <summary>
        /// Create a new instance of a <see cref="CustomFormatterData"/> class.
        /// </summary>
        public CustomFormatterData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomFormatterData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        public CustomFormatterData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomFormatterData"/> class with a name and type.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        /// <param name="typeName">
        /// The type name to use as the custom formatter.
        /// </param>
        public CustomFormatterData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
            attributes = new NameValueItemCollection();
        }

        /// <summary>
        /// The type name to use as the custom formatter.
        /// </summary>
        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Retrieves configuration extensions as a NameValueCollection
        /// </summary>
        [XmlElement("attributes")]
        public NameValueItemCollection Attributes
        {
            get { return this.attributes; }
        }
    }
}