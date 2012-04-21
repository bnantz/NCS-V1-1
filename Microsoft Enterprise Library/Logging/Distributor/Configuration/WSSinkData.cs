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
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.FlatFileSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class WSSinkData : SinkData
    {
        private string url = String.Empty;

        /// <summary>
        /// Create a new instance of a <see cref="WSSinkData"/>.
        /// </summary>
        public WSSinkData()
        {
        }

         /// <summary>
         /// Initialize a new instance of the <see cref="WSSinkData"/> class with a name.
         /// </summary>
         /// <param name="name">
         /// The name of the sink.
         /// </param>
         public WSSinkData(string name) : base(name)
         {
         }

        /// <summary>
        /// Initialize a new instance of the <see cref="WSSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// required url.
        /// </param>
        /// <param name="url">
        /// Optional footer to write after each log message.
        /// </param>
        public WSSinkData(string name, string url) : base(name)
        {
            this.url = url;
        }

        /// <summary>
        /// File name to log.  File will be created if it does not exist.
        /// </summary>
        [XmlAttribute("url")]
        public string Url
        {
            get { return this.url; }
            set { url = value; }
        }

        /// <summary>
        /// Gets the fully qualified assembly name of a <c>WSSinkDataSinkData</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(WSSink).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}