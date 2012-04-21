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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a data transformer that takes data and serializes / deserializes the configuration data info Xml.</para>
    /// </summary>      
    [XmlRoot("dataTransformer", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class XmlSerializerTransformerData : TransformerData
    {
        private XmlIncludeTypeDataCollection xmlIncludeTypes;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlSerializerTransformerData"/> class.</para>
        /// </summary>
        public XmlSerializerTransformerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlSerializerTransformerData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the transformer.</para>
        /// </param>       
        public XmlSerializerTransformerData(string name) : base(name)
        {
            xmlIncludeTypes = new XmlIncludeTypeDataCollection();
        }

        /// <summary>
        /// <para>Gets the <see cref="XmlIncludeTypeDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The include types available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>includeTypes</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="includeTypes", Namespace=ConfigurationSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="includeType", Type=typeof(XmlIncludeTypeData), Namespace=ConfigurationSettings.ConfigurationNamespace)]
        public XmlIncludeTypeDataCollection XmlIncludeTypes
        {
            get { return xmlIncludeTypes; }
        }

        /// <summary>
        /// <para>Gets the fully qualified assembly name for a <see cref="XmlSerializerTransformer"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name for a <see cref="XmlSerializerTransformer"/>.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(XmlSerializerTransformer).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// <para>Creates a new object that is a copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new object that is a copy of this instance.</para>
        /// </returns>
        /// <remarks>
        /// <para>This clone does a deep copy.</para>
        /// </remarks>
        public override object Clone()
        {
            XmlSerializerTransformerData transformerData = new XmlSerializerTransformerData(Name);
            foreach (XmlIncludeTypeData data in XmlIncludeTypes)
            {
                transformerData.XmlIncludeTypes.Add(new XmlIncludeTypeData(data.Name, data.TypeName));
            }
            return transformerData;
        }
    }
}