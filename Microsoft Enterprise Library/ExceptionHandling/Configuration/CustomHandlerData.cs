//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the custom configuration information provided for an <see cref="ExceptionHandler"></see>.
    /// </summary>
    [XmlRoot("exceptionHandler", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class CustomHandlerData : ExceptionHandlerData
    {
        private NameValueItemCollection attributes;
        private string typeName;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomHandlerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="CustomHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CustomHandlerData"/>.
        /// </param>
        public CustomHandlerData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="CustomHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the custom handler.
        /// </param>
        /// <param name="typeName">
        /// The type of the custom handler.
        /// </param>
        public CustomHandlerData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
            attributes = new NameValueItemCollection();
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
    }
}