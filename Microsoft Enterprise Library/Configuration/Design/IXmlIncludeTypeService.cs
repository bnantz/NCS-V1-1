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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a service for registering <see cref="Type"/> objects that will be included for an <see cref="XmlSerializerTransformer"/> for a section.</para>
    /// </summary>
    public interface IXmlIncludeTypeService
    {
        /// <summary>
        /// <para>When implemented by a class, gets the types for the section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The types for a section.</para>
        /// </param>
        /// <returns>
        /// <para>The types for a section or <see langword="null"/> if no types exist.</para>
        /// </returns>
        Type[] GetXmlIncludeTypes(string sectionName);

        /// <summary>
        /// <para>When implemented by a class, adds a <see cref="Type"/> for a section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The configuration section to register the type.</para>
        /// </param>
        /// <param name="type">
        /// <para>The type to register.</para>
        /// </param>
        void AddXmlIncludeType(string sectionName, Type type);
    }
}