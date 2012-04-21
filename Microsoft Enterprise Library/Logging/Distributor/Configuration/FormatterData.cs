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
    /// Represents the configuration settings for a log formatter.  This class is abstract.
    /// </summary>
    [XmlRoot("formatter", Namespace=DistributorSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomFormatterData))]
    [XmlInclude(typeof(TextFormatterData))]
    public abstract class FormatterData : ProviderData
    {
        /// <summary>
        /// Create a new instance of a <see cref="FormatterData"/>.
        /// </summary>
        protected FormatterData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="FormatterData"/> with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        protected FormatterData(string name) : base(name)
        {
        }
    }
}