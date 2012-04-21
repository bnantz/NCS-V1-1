//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="IExceptionHandler"/>.
    /// </summary>
    [XmlRoot("exceptionHandler", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomHandlerData))]
    [XmlInclude(typeof(WrapHandlerData))]
    [XmlInclude(typeof(ReplaceHandlerData))]
    public abstract class ExceptionHandlerData : ProviderData
    {
        /// <summary>
        /// Creates a new instance of ExceptionHandlerData.
        /// </summary>
        protected ExceptionHandlerData()
        {
        }

        /// <summary>
        /// <para>Intializes a new instance of the <see cref="ExceptionHandlerData"/> class with a name and a <see cref="System.Type"/> name.</para>
        /// </summary>
        /// <param name="name"><para>The name of the provider.</para></param>
        protected ExceptionHandlerData(string name) : base(name)
        {
        }
    }
}