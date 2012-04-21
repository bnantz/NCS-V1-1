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
    /// Represents the configuration settings for an <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.WMILogSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class WMILogSinkData : SinkData
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="WMILogSinkData"/> class.
        /// </summary>
        public WMILogSinkData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="WMILogSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public WMILogSinkData(string name) : base(name)
        {
        }

        /// <summary>
        /// Gets the fully qualified assembly name of a <c>WMILogSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(WMILogSink).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}