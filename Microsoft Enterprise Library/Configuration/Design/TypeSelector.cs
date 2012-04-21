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
using System.Reflection;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Hanldes the building of the tree view based on the assembly types and filter based on a type.
    /// </devdoc>
    internal class TypeSelector
    {
        private TreeNode rootNode;
        private Type currentType;
        private TreeNode currentTypeTreeNode;
        private Type baseType;
        private TreeView treeView;
        private TypeSelectorIncludeFlags flags;
        private bool includeBaseType;
        private bool includeAbstractTypes;
        private bool includeNonPublicTypes;
        private bool includeAllInterfaces;
        private Hashtable loadedAssemblies;

        public TypeSelector(Type currentType, Type baseType, TreeView treeView) : this(currentType, baseType, TypeSelectorIncludeFlags.Default, treeView)
        {
        }

        public TypeSelector(Type currentType, Type baseType, TypeSelectorIncludeFlags flags, TreeView treeView)
        {
            if (treeView == null)
            {
                throw new ArgumentNullException("treeView");
            }
            if (baseType == null)
            {
                throw new ArgumentNullException("typeToVerify");
            }
            loadedAssemblies = new Hashtable();
            this.treeView = treeView;
            this.currentType = currentType;
            this.baseType = baseType;
            this.flags = flags;
            this.includeAbstractTypes = IsSet(TypeSelectorIncludeFlags.AbstractTypes);
            this.includeAllInterfaces = IsSet(TypeSelectorIncludeFlags.Interfaces);
            this.includeNonPublicTypes = IsSet(TypeSelectorIncludeFlags.NonpublicTypes);
            this.includeBaseType = IsSet(TypeSelectorIncludeFlags.BaseType);

            LoadTypes(baseType);
        }

        public Type TypeToVerify
        {
            get { return this.baseType; }
        }

        public bool LoadTreeView(Assembly assembly)
        {
            if (loadedAssemblies.Contains(assembly.FullName))
            {
                return true;
            }

            TreeNodeTable nodeTable = new TreeNodeTable(assembly);
            Type[] types = LoadTypesFromAssembly(assembly);
            if (types == null)
            {
                return false;
            }

            LoadValidTypes(types, nodeTable);

            return AddTypesToTreeView(nodeTable, assembly);
        }

        private bool AddTypesToTreeView(TreeNodeTable nodeTable, Assembly assembly)
        {
            if (nodeTable.AssemblyNode.Nodes.Count > 0)
            {
                this.rootNode.Nodes.Add(nodeTable.AssemblyNode);
                this.rootNode.ExpandAll();
                loadedAssemblies[assembly.FullName] = 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Type[] LoadTypesFromAssembly(Assembly assembly)
        {
            Type[] types = null;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
            }
            return types;
        }

        private void LoadValidTypes(Type[] types, TreeNodeTable nodeTable)
        {
            if (types == null)
            {
                return;
            }

            foreach (Type t in types)
            {
                if (IsTypeValid(t))
                {
                    TreeNode newNode = nodeTable.AddType(t);
                    if (t == this.currentType)
                    {
                        this.currentTypeTreeNode = newNode;
                    }
                }
                //LoadValidTypes(t.GetNestedTypes(), nodeTable);
            }
        }

        public bool IsTypeValid(Type selectedType)
        {
            bool valid = false;

            if (includeAllInterfaces && selectedType.IsInterface)
            {
                valid = true;
            }
            else if (selectedType == baseType)
            {
                valid = includeBaseType;
            }
            else
            {
                valid = baseType.IsAssignableFrom(selectedType);

                if (valid)
                {
                    if ((!includeAbstractTypes) && (selectedType.IsAbstract || selectedType.IsInterface))
                    {
                        valid = false;
                    }
                }

                if (valid)
                {
                    if (!(selectedType.IsPublic) && !(selectedType.IsNestedPublic) && (!includeNonPublicTypes))
                    {
                        valid = false;
                    }
                }
            }
            return valid;
        }

        private bool IsSet(TypeSelectorIncludeFlags compareFlag)
        {
            return ((this.flags & compareFlag) == compareFlag);
        }

        private void LoadTypes(Type baseType)
        {
            TreeNode treeNode = new TreeNode(String.Empty, -1, -1);
            if (baseType.IsInterface)
            {
                treeNode.Text = SR.TypeSelectorInterfaceRootNodeText(baseType.FullName);
            }
            else if (baseType.IsClass)
            {
                treeNode.Text = SR.TypeSelectorClassRootNodeText(baseType.FullName);
            }
            this.treeView.Nodes.Add(treeNode);
            this.rootNode = new TreeNode(SR.AssembliesLabelText, 0, 1);
            treeNode.Nodes.Add(rootNode);
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                this.LoadTreeView(assembly);
            }
            treeNode.ExpandAll();
            this.treeView.SelectedNode = this.currentTypeTreeNode;
        }

        /// <devdoc>
        /// Represents the table of tree nodes by assembly type.
        /// </devdoc>
        private class TreeNodeTable
        {
            private Hashtable namespaceTable;
            private TreeNode assemblyNode;

            public TreeNodeTable(Assembly assembly)
            {
                this.namespaceTable = new Hashtable();
                this.assemblyNode = new TreeNode(assembly.GetName().Name, (int)TypeImages.Assembly, (int)TypeImages.Assembly);
            }

            public TreeNode AssemblyNode
            {
                get { return this.assemblyNode; }
            }

            public TreeNode AddType(Type t)
            {
                TreeNode namespaceNode = GetNamespaceNode(t);
                TreeNode typeNode = new TreeNode(t.Name, (int)TypeImages.Class, (int)TypeImages.Class);
                typeNode.Tag = t;
                namespaceNode.Nodes.Add(typeNode);
                return typeNode;
            }

            private TreeNode GetNamespaceNode(Type t)
            {
                TreeNode namespaceNode = null;
                if (t.Namespace == null)
                {
                    namespaceNode = this.assemblyNode;
                }
                else if (this.namespaceTable.ContainsKey(t.Namespace))
                {
                    namespaceNode = (TreeNode)this.namespaceTable[t.Namespace];
                }
                else
                {
                    namespaceNode = new TreeNode(t.Namespace, (int)TypeImages.Namespace, (int)TypeImages.Namespace);
                    this.assemblyNode.Nodes.Add(namespaceNode);
                    this.namespaceTable.Add(t.Namespace, namespaceNode);
                }
                return namespaceNode;
            }
        }
    }
}