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
using System.ComponentModel;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a category destination.
    /// </summary>
    [XmlRoot("destination", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class DestinationData
    {
        private string name;
        private string sink;
        private string format;

        /// <summary>
        /// Create a new instance of <see cref="DestinationData"/> class.
        /// </summary>
        public DestinationData() : this(string.Empty)
        {
        }

		/// <summary>
		/// Create a new instance of <see cref="DestinationData"/> class using the given values.
		/// </summary>
		/// <param name="name">The name for the destination.</param>
		/// <param name="sink">The sink to use with this destination.</param>
		public DestinationData(string name, string sink) : this(name, sink, String.Empty)
    	{
    	}

    	/// <summary>
        /// Create a new instance of <see cref="DestinationData"/> class using the given name.
        /// </summary>
        /// <param name="name">The name for the destination.</param>
        public DestinationData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="DestinationData"/> class using the given values.
        /// </summary>
        /// <param name="name">The name for the destination.</param>
        /// <param name="sink">The sink to use with this destination.</param>
        /// <param name="format">The format for the destination.</param>
        public DestinationData(string name, string sink, string format)
        {
            this.name = name;
            this.sink = sink;
            this.format = format;
        }

        /// <summary>
        /// Name of the destination node in the designer.
        /// </summary>
        [XmlAttribute("name")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Name of the sink.
        /// </summary>
        [XmlAttribute("sink")]
        public string Sink
        {
            get { return this.sink; }
            set { this.sink = value; }
        }

        /// <summary>
        /// Formatter for the message.  Optional.
        /// </summary>
        [XmlAttribute("format")]
        public string Format
        {
            get { return this.format; }
            set { this.format = value; }
        }
    }
}