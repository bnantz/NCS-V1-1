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
    /// <para>Represents the common configuration data for all providers.</para>
    /// </summary>
    [XmlType(IncludeInSchema=false)]
    public abstract class ProviderData
    {
        private string name;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ProviderData"/> class.</para>
        /// </summary>
        protected ProviderData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Intializes a new instance of the <see cref="ProviderData"/> class with a name and a <see cref="System.Type"/> name.</para>
        /// </summary>
        /// <param name="name"><para>The name of the provider.</para></param>
        protected ProviderData(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// <para>Gets or sets the name of the provider.</para>
        /// </summary>
        /// <value><para>The name of the provider.</para></value>
        [XmlAttribute("name")]
        public virtual string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// <para>When implemented by a class, Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para><b>Not to implementers:</b> You do not have to implement the set operation.  If you have only one type for your data object you can ignore the set.</para>
        /// </remarks>
        [XmlIgnore]
        public abstract string TypeName { get; set; }
    }
}