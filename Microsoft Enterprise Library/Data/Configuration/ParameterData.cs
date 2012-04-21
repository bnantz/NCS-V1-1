//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// <para>Represents a parameter for a <see cref="ConnectionString"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>This class maps to the <c>parameter</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("parameter", Namespace=DatabaseSettings.ConfigurationNamespace)]
    public class ParameterData
    {
        private string name;
        private string value;
        private bool isSensitive;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ParameterData"/> class.</para>
        /// </summary>
        public ParameterData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ParameterData"/> class with a name and value.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the parameter.</para>
        /// </param>
        /// <param name="value">
        /// <para>The value for the parameter.</para>
        /// </param>
        public ParameterData(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// <para>Gets or sets the name of the parameter.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the parameter. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>name</c> attribute in configuration.</para>
        /// </remarks>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// <para>Gets or sets the value of the parameter.</para>
        /// </summary>
        /// <value>
        /// <para>The value of the parameter. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>value</c> attribute in configuration.</para>
        /// </remarks>
        [XmlAttribute("value")]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// <para>Gets or sets the value indicating whether or not this data is sensitive.</para>
        /// </summary>
        /// <remarks>
        /// <para>This property is to help tools know how to handle the data (i.e. show value in the plain or masked).</para>
        /// </remarks>
        [XmlAttribute("isSensitive")]
        public bool IsSensitive
        {
            get { return isSensitive; }
            set { isSensitive = value; }
        }
    }
}