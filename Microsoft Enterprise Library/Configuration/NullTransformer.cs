 // Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <devdoc>
    /// Represents a null transformer object. Introduces the null object pattern since transformers are not required.
    /// </devdoc>
    internal class NullTransformer : TransformerProvider
    {
        public NullTransformer()
        {
        }

        /// <devdoc>
        /// This method does nothing by default.
        /// </devdoc>
        public override void Initialize(ConfigurationView configurationView)
        {
        }

        /// <devdoc>
        /// This method does nothing by default.
        /// </devdoc>
        public override object Deserialize(object section)
        {
            return section;
        }

        /// <devdoc>
        /// This method does nothing by default.
        /// </devdoc>
        public override object Serialize(object value)
        {
            return value;
        }
    }
}