//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a custom data transformer for configuration data in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>dataTransformer</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("dataTransformer", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class CustomTransformerData : TransformerData
    {
        private NameValueItemCollection attributes;
        private string typeName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CustomTransformerData"/> class..</para>
        /// </summary>  
        public CustomTransformerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="CustomTransformerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CustomTransformerData"/>.
        /// </param>
        public CustomTransformerData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="CustomTransformerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the custom handler.
        /// </param>
        /// <param name="typeName">
        /// The type of the custom handler.
        /// </param>
        public CustomTransformerData(string name, string typeName) : this(name, typeName, new NameValueItemCollection())
        {
        }

        private CustomTransformerData(string name, string typeName, NameValueItemCollection attributes) : base(name)
        {
            this.typeName = typeName;
            this.attributes = attributes;
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <remarks>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        /// </remarks>
        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>
        [XmlElement("Attributes")]
        public NameValueItemCollection Attributes
        {
            get { return this.attributes; }
        }

        /// <summary>
        /// <para>Creates a deep copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A deep copy of this instance.</para>
        /// </returns>
        public override object Clone()
        {
            return new CustomTransformerData(this.Name, typeName, new NameValueItemCollection(attributes));
        }
    }
}
