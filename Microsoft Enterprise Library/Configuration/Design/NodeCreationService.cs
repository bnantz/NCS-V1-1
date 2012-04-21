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
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides the a way to create nodes based on data types.
    /// </devdoc>
    internal class NodeCreationService : INodeCreationService
    {
        private Hashtable nodeTypesByDataType;
        private Hashtable namesByNodeType;
        private Hashtable nodeTypeByDisplayName;
        private Hashtable nodeEntryByDisplayName;
        
        public NodeCreationService()
        {
            nodeTypesByDataType = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            namesByNodeType = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            nodeTypeByDisplayName = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            nodeEntryByDisplayName = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
        }

        /// <devdoc>
        /// Gets the names for the base node type that previous nodes were registered.
        /// </devdoc>
        public StringCollection GetDisplayNames(Type nodeType)
        {
            if (namesByNodeType.Contains(nodeType.FullName))
            {
                return (StringCollection)namesByNodeType[nodeType.FullName];
            }
            return new StringCollection();
        }

        /// <devdoc>
        /// Gets the NodeCreationEntry for the displayName.
        /// </devdoc>
        public NodeCreationEntry GetNodeCreationEntry(string displayName)
        {
            return nodeEntryByDisplayName[displayName] as NodeCreationEntry;
        }

        /// <devdoc>
        /// Create a node based on the data type.
        /// </devdoc>
        public ConfigurationNode CreateNode(Type dataType)
        {
            return CreateNode(dataType, null);
        }

        /// <devdoc>
        /// Create a node based on the data type and some constructor arguments.
        /// </devdoc>
        public ConfigurationNode CreateNode(Type dataType, object[] constructorArguments)
        {
            ArgumentValidation.CheckForNullReference(dataType, "dataType");
            
            Type nodeType = nodeTypesByDataType[dataType.FullName] as Type;
            if (nodeType == null)
            {
                return null;
            }
            return (ConfigurationNode)Activator.CreateInstance(nodeType,
                                                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.CreateInstance,
                                                               null,
                                                               constructorArguments,
                                                               null);
        }

        /// <devdoc>
        /// Creates a map between the node and the data that it represents.
        /// </devdoc>
        public void AddNodeCreationEntry(NodeCreationEntry nodeCreationEntry)
        {
            ArgumentValidation.CheckForNullReference(nodeCreationEntry, "nodeCreationEntry");

            nodeEntryByDisplayName[nodeCreationEntry.DisplayName] = nodeCreationEntry;
            string displayName = nodeCreationEntry.DisplayName;
            nodeTypesByDataType[nodeCreationEntry.DataType.FullName] = nodeCreationEntry.NodeType;

            Type baseType = nodeCreationEntry.NodeType.BaseType;    
            Type nodeTypeToStore = nodeCreationEntry.NodeType;
            while(baseType != typeof(ConfigurationNode))
            {
                nodeTypeToStore = baseType;
                baseType = baseType.BaseType;
            }
            nodeTypeByDisplayName[displayName] = nodeTypeToStore;
            nodeCreationEntry.BaseTypeToCompare = nodeTypeToStore;
            AddNamesForBaseType(nodeTypeToStore.FullName, displayName);
        }

        /// <devdoc>
        /// Determines if a node type matches it's registered data type.
        /// </devdoc>
        public bool DoesNodeTypeMatchDataType(Type nodeType, Type dataType)
        {
            ArgumentValidation.CheckForNullReference(nodeType, "nodeType");
            ArgumentValidation.CheckForNullReference(dataType, "dataType");

            Type type = nodeTypesByDataType[dataType.FullName] as Type;
            if (type == null) return false;

            return (type.Equals(nodeType));

        }

        /// <devdoc>
        /// Gets the node type for the given data type.
        /// </devdoc>
        public Type GetNodeType(Type dataType)
        {
            ArgumentValidation.CheckForNullReference(dataType, "dataType");

            return nodeTypesByDataType[dataType.FullName] as Type;
        }

        private void AddNamesForBaseType(string baseTypeName, string displayName)
        {
            if (!namesByNodeType.Contains(baseTypeName))
            {
                namesByNodeType[baseTypeName] = new StringCollection();
            }
            StringCollection names = namesByNodeType[baseTypeName] as StringCollection;
            if (!names.Contains(displayName))
            {
                names.Add(displayName);
            }
        }
    }
}