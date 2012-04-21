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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for logging sinks.  This class is abstract.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomSinkData))]
    [XmlInclude(typeof(EventLogSinkData))]
    [XmlInclude(typeof(FlatFileSinkData))]
    [XmlInclude(typeof(RollingFlatFileSinkData))]
    [XmlInclude(typeof(ConsoleSinkData))]
    [XmlInclude(typeof(EmailSinkData))]
    [XmlInclude(typeof(MsmqSinkData))]
    [XmlInclude(typeof(WMILogSinkData))]
    [XmlInclude(typeof(WSSinkData))]
    public abstract class SinkData : ProviderData
    {
        /// <summary>
        /// Create a new instance of a <see cref="SinkData"/>.
        /// </summary>
        protected SinkData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="SinkData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        protected SinkData(string name) : base(name)
        {
        }
    }
}