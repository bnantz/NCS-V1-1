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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing a flat file sink.
    /// </summary>
    public class WSSinkNode : SinkNode
    {
        private WSSinkData wsSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public WSSinkNode() : this(new WSSinkData(SR.WSSink, DefaultValues.WSSinkUrl))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="wsSinkData" cref="WSSinkData">Configuration data.</param>
        public WSSinkNode(WSSinkData wsSinkData) : base(wsSinkData)
        {
            this.wsSinkData = wsSinkData;
        }

        /// <summary>
        /// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.FlatFileSink"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return wsSinkData.TypeName; }
        }

        /// <summary>
        /// Fully qualified file name to save the log output.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.WSSinkUrl)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Url
        {
            get {return wsSinkData.Url;}
            set { wsSinkData.Url = value; }
        }
    }
}