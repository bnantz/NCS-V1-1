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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Node for a custom formatter.
    /// </summary>
    public class CustomFormatterNode : FormatterNode
    {
        private CustomFormatterData customFormatterData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public CustomFormatterNode() : this(new CustomFormatterData(SR.CustomFormatter))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        public CustomFormatterNode(CustomFormatterData data) : base(data)
        {
            this.customFormatterData = data;
        }

        /// <summary>
        /// Custom configuration attributes.
        /// </summary>
        [SRDescription(SR.Keys.CustomFormatterAttributesDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Attributes
        {
            get { return customFormatterData.Attributes; }
        }
    }
}