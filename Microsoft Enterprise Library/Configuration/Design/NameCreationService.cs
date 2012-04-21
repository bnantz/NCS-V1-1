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
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides a service that can generate unique names for ConfigurationNode objects.
    /// </devdoc>
    internal class NodeNameCreationService : INodeNameCreationService
    {
        public NodeNameCreationService()
        {
        }

        /// <devdoc>
        /// Creates a new name that is unique to all components in the specified container.
        /// </devdoc>
        public string CreateName(IContainer container, Type dataType)
        {
            int i = 0;
            string name = dataType.Name;
            while (true)
            {
                i++;
                if (container.Components[name + i.ToString(CultureInfo.CurrentUICulture)] == null)
                {
                    break;
                }
            }

            return name + i.ToString(CultureInfo.CurrentUICulture);
        }

        /// <devdoc>
        /// Gets a unique display name for the current node.
        /// </devdoc>
        public string GetUniqueDisplayName(ConfigurationNode node, string newName)
        {
            if (node == null)
            {
                return newName;
            }
            int i = 0;
            string name = newName;
            // make sure it is not there before we start appending names
            if (!node.Nodes.Contains(name))
            {
                return name;
            }
            while (true)
            {
                i++;
                if (!node.Nodes.Contains(name + i.ToString(CultureInfo.CurrentUICulture)))
                {
                    break;
                }
            }

            return name + i.ToString(CultureInfo.CurrentUICulture);
        }

        /// <devdoc>
        /// Gets a value indicating whether the specified name is valid.
        /// </devdoc>
        public bool IsValidName(string name)
        {
            if (name == null || name.Length == 0)
            {
                return false;
            }
            if (!Char.IsLetter(name, 0))
            {
                return false;
            }
            if (name.StartsWith("_"))
            {
                return false;
            }
            for (int i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetterOrDigit(name, i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <devdoc>
        /// Gets a value indicating whether the specified name is valid.
        /// </devdoc>
        public void ValidateName(string name)
        {
            if (!IsValidName(name))
            {
                throw new InvalidOperationException(SR.ExceptionInvalidComponentName(name));
            }
        }
    }
}