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

using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a log formatter.
    /// </summary>
    [XmlRoot("formatter", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class TextFormatterData : FormatterData
    {
        private XmlCDataSection templateData;

        /// <summary>
        /// Create a new instance of <see cref="TextFormatterData"/>.
        /// </summary>
        public TextFormatterData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public TextFormatterData(string name) : base(name)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        public TextFormatterData(string name, string templateData) : base(name)
        {
            Template.Value = templateData;
        }

        /// <summary>
        /// Template containing tokens to replace.
        /// </summary>
        [XmlElement("template")]
        public XmlCDataSection Template
        {
            get
            {
                if (templateData == null)
                {
                    XmlDocument doc = new XmlDocument();
                    this.templateData = doc.CreateCDataSection(String.Empty);
                }
                return this.templateData;
            }
            set { this.templateData = value; }
        }

        /// <summary>
        /// Gets the fully qualified type name of a <c>FormatterData</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(TextFormatter).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}