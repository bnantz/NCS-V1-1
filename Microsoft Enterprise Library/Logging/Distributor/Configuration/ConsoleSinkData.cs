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
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.ConsoleSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class ConsoleSinkData : SinkData
    {
        private string header = string.Empty;
        private string footer = string.Empty;

        /// <summary>
        /// Create a new instance of a <see cref="ConsoleSinkData"/>.
        /// </summary>
        public ConsoleSinkData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ConsoleSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public ConsoleSinkData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ConsoleSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="header">
        /// Optional header to write before each log message.
        /// </param>
        /// <param name="footer">
        /// Optional footer to write after each log message.
        /// </param>
        public ConsoleSinkData(string name, string header, string footer) : base(name)
        {
            this.header = header;
            this.footer = footer;
        }

        /// <summary>
        /// Optional header to write before each log message.
        /// </summary>
        [XmlAttribute("header")]
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// Optional footer to write after each log message.
        /// </summary>
        [XmlAttribute("footer")]
        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }

        /// <summary>
        /// Gets the fully qualified assembly name of a <c>ConsoleSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(ConsoleSink).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}