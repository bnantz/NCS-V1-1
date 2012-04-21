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
    /// <para>Represents a type to include in the <see cref="XmlSerializerTransformerData"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>configurationSection</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("xmlIncludeType", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class XmlIncludeTypeData
    {
        private string name;
        private string typeName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlIncludeTypeData"/> class.</para>
        /// </summary>
        public XmlIncludeTypeData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlIncludeTypeData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the transformer.</para>
        /// </param>
        public XmlIncludeTypeData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlIncludeTypeData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the transformer.</para>
        /// </param>
        /// <param name="typeName">
        /// <para>The fully qualified type name of the transformer.</para>
        /// </param>      
        public XmlIncludeTypeData(string name, string typeName)
        {
            this.name = name;
            this.typeName = typeName;
        }

        /// <summary>
        /// <para>Gets or sets the name of the include types.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the include type. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>name</c> attribute in configuration for the type.</para>
        /// </remarks>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// <para>Gets or sets the name of the transformer.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the transformer. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>type</c> attribute in configuration for the type.</para>
        /// </remarks>
        [XmlAttribute("type")]
        public string TypeName
        {
            get { return this.typeName; }
            set { this.typeName = value; }
        }
    }
}