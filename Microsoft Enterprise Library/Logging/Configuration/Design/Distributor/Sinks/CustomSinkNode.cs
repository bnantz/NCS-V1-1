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

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Node representing a custom Sink.
    /// </summary>
    public class CustomSinkNode : SinkNode
    {
        private CustomSinkData customSinkData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public CustomSinkNode() : this(new CustomSinkData(SR.CustomSink))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        public CustomSinkNode(CustomSinkData data) : base(data)
        {
            this.customSinkData = data;
        }

        /// <summary>
        /// Custom configuration attributes.
        /// </summary>
        [SRDescription(SR.Keys.CustomSinkAttributesDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Attributes
        {
            get { return customSinkData.Attributes; }
        }
    }
}