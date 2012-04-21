//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


using System;
using System.Xml.Serialization;
#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a data transformer for configuration data in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>dataTransformer</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("dataTransformer", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(XmlSerializerTransformerData))]
    [XmlInclude(typeof(CustomTransformerData))]
#if UNIT_TESTS
    [XmlInclude(typeof(NotRealTransformerData))]
#endif
        public abstract class TransformerData : ProviderData, ICloneable
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TransformerData"/> class..</para>
        /// </summary>
        protected TransformerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TransformerData"/> class with a name and fully qualified type name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the transformer.</para>
        /// </param>
        protected TransformerData(string name) : base(name)
        {
        }

        /// <summary>
        /// <para>Creates a new object that is a copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new object that is a copy of this instance.</para>
        /// </returns>
        public abstract object Clone();

    }
}