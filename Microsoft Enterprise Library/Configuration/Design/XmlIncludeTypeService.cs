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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides a service for registering Type objects that will be included for an XmlSerializerTransformer for a section.
    /// </devdoc>
    internal class XmlIncludeTypeService : IXmlIncludeTypeService
    {
        private Hashtable includeTypesBySection;
        
        public XmlIncludeTypeService()
        {
            includeTypesBySection = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        }

        /// <devdoc>
        /// Gets the types for the section.
        /// </devdoc>
        public Type[] GetXmlIncludeTypes(string sectionName)
        {
            if (!includeTypesBySection.Contains(sectionName))
            {
                return null;
            }
            Hashtable list = (Hashtable)includeTypesBySection[sectionName];
            Type[] types = new Type[list.Count];
            list.Values.CopyTo(types, 0);
            return types;
        }

        /// <devdoc>
        /// Add a type for a section.
        /// </devdoc>
        public void AddXmlIncludeType(string sectionName, Type type)
        {
            if (sectionName == null)
            {
                throw new ArgumentNullException("sectionName");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (!includeTypesBySection.Contains(sectionName))
            {
                includeTypesBySection[sectionName] = new Hashtable(5);
            }
            Hashtable table = (Hashtable)includeTypesBySection[sectionName];
            table[type.FullName] = type;
        }
    }
}