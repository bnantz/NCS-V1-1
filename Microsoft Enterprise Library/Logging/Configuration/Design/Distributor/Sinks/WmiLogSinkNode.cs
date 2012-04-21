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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing an WMI log sink.
    /// </summary>
    public class WmiLogSinkNode : SinkNode
    {
        private WMILogSinkData wmiLogSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public WmiLogSinkNode() : this(new WMILogSinkData(SR.WmiLogSink))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="wmiLogSinkData">Configuration data.</param>
        public WmiLogSinkNode(WMILogSinkData wmiLogSinkData) : base(wmiLogSinkData)
        {
            this.wmiLogSinkData = wmiLogSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.WMILogSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return wmiLogSinkData.TypeName; }
        }
    }
}