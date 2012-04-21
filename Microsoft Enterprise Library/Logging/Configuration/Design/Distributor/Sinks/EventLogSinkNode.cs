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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing an Eventlog Sink.
    /// </summary>
    public class EventLogSinkNode : SinkNode
    {
        private EventLogSinkData eventLogSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public EventLogSinkNode() : this(new EventLogSinkData(SR.EventLogSink, DefaultValues.EventLogSinkEventSource, DefaultValues.EventLogSinkLogName))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="eventLogSinkData">Configuration data.</param>
        public EventLogSinkNode(EventLogSinkData eventLogSinkData) : base(eventLogSinkData)
        {
            this.eventLogSinkData = eventLogSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.EventLogSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return eventLogSinkData.TypeName; }
        }

        /// <summary>
        /// Event log name.
        /// </summary>
        [SRDescription(SR.Keys.EventLogSinkEventLogNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string EventLogName
        {
            get { return eventLogSinkData.EventLogName; }
            set { eventLogSinkData.EventLogName = value; }
        }

        /// <summary>
        /// Event source name.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.EventLogSinkEventSourceNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string EventSourceName
        {
            get { return eventLogSinkData.EventSourceName; }
            set { eventLogSinkData.EventSourceName = value; }
        }
    }
}