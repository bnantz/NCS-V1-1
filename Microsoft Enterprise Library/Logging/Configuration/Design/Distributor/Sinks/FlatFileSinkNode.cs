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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing a flat file sink.
    /// </summary>
    public class FlatFileSinkNode : SinkNode
    {
        private FlatFileSinkData flatFileSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public FlatFileSinkNode() : this(new FlatFileSinkData(SR.FlatFileSink, DefaultValues.FlatFileSinkFileName, DefaultValues.FlatFileSinkHeader, DefaultValues.FlatFileSinkFooter))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="flatFileSinkData">Configuration data.</param>
        public FlatFileSinkNode(FlatFileSinkData flatFileSinkData) : base(flatFileSinkData)
        {
            this.flatFileSinkData = flatFileSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.FlatFileSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return flatFileSinkData.TypeName; }
        }

        /// <summary>
        /// Fully qualified file name to save the log output.
        /// </summary>
        [Required]
        [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
        [FilteredFileNameEditor("All files (*.*)|*.*")]
        [SRDescription(SR.Keys.FlatFileSinkFlatFileName)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Filename
        {
            get { return flatFileSinkData.FileName; }
            set { flatFileSinkData.FileName = value; }
        }

        /// <summary>
        /// Optional header to write before each log message.
        /// </summary>
        [SRDescription(SR.Keys.FlatFileSinkHeader)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Header
        {
            get { return flatFileSinkData.Header; }
            set { flatFileSinkData.Header = value; }
        }

        /// <summary>
        /// Optional footer to write after each log message.
        /// </summary>
        [SRDescription(SR.Keys.FlatFileSinkFooter)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Footer
        {
            get { return flatFileSinkData.Footer; }
            set { flatFileSinkData.Footer = value; }
        }
    }
}