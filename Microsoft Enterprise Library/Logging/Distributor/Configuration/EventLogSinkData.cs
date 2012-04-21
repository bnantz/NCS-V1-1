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
    /// Represents the configuration settings for an <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.EventLogSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class EventLogSinkData : SinkData
    {
        private string eventSourceName;
        private string eventLogName;

        /// <summary>
        /// Create a new instance of a <see cref="EventLogSinkData"/>.
        /// </summary>
        public EventLogSinkData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="EventLogSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public EventLogSinkData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="EventLogSinkData"/> class with a name, event source name, and event log name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="eventSourceName">
        /// Event source name.
        /// </param>
        /// <param name="eventLogName">
        /// Event log name.
        /// </param>
        public EventLogSinkData(string name, string eventSourceName, string eventLogName) : base(name)
        {
            this.eventSourceName = eventSourceName;
            this.eventLogName = eventLogName;
        }

        /// <summary>
        /// Event log name.
        /// </summary>
        [XmlAttribute("eventLogName")]
        public string EventLogName
        {
            get { return this.eventLogName; }
            set { this.eventLogName = value; }
        }

        /// <summary>
        /// Event source name.
        /// </summary>
        [XmlAttribute("eventSourceName")]
        public string EventSourceName
        {
            get { return eventSourceName; }
            set { eventSourceName = value; }
        }

        /// <summary>
        /// Gets the fully qualified assembly name of an <c>EventLogSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(EventLogSink).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}