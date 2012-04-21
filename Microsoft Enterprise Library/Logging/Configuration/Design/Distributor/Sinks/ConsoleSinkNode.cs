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
    /// Node representing a flat file sink.
    /// </summary>
    public class ConsoleSinkNode : SinkNode
    {
        private ConsoleSinkData consoleSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public ConsoleSinkNode() : this(new ConsoleSinkData(SR.ConsoleSink, DefaultValues.ConsoleSinkHeader, DefaultValues.ConsoleSinkFooter))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="consoleSinkData">Configuration data.</param>
        public ConsoleSinkNode(ConsoleSinkData consoleSinkData) : base(consoleSinkData)
        {
            this.consoleSinkData = consoleSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.ConsoleSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return consoleSinkData.TypeName; }
        }

        /// <summary>
        /// Optional header to write before each log message.
        /// </summary>
        [SRDescription(SR.Keys.ConsoleSinkHeader)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Header
        {
            get { return consoleSinkData.Header; }
            set { consoleSinkData.Header = value; }
        }

        /// <summary>
        /// Optional footer to write after each log message.
        /// </summary>
        [SRDescription(SR.Keys.ConsoleSinkFooter)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Footer
        {
            get { return consoleSinkData.Footer; }
            set { consoleSinkData.Footer = value; }
        }
    }
}