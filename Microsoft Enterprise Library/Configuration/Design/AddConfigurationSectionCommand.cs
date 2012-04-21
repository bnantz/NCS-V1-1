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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command for adding a <see cref="ConfigurationSectionNode"/> to an <see cref="ConfigurationSectionCollectionNode"/>.</para>
    /// </summary>
    public class AddConfigurationSectionCommand : AddChildNodeCommand
    {
        private ConfigurationNode newSectionNode;
        private string sectionName;
     
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/> and the child <see cref="Type"/> to add.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        /// <param name="sectionName">
        /// <para>The name for the configuration section.</para>
        /// </param>
        public AddConfigurationSectionCommand(IServiceProvider serviceProvider, Type childType, string sectionName) : base(serviceProvider, childType)
        {
            this.sectionName = sectionName;
        }
        
        /// <summary>
        /// <para>Creates a <see cref="ConfigurationSectionNode"/> for the parent node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="ConfigurationSectionNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ConfigurationSectionCollectionNode sectionsNode = SelectConfigurationSectionsNode();
            ConfigurationSectionNode sectionNode = UIHierarchyService.SelectedHierarchy.FindNodeByName(sectionsNode, sectionName) as ConfigurationSectionNode;
            newSectionNode = sectionNode;
            if (sectionNode == null)
            {
                newSectionNode = ConfigurationSectionNode.CreateReadOnlyDefault(sectionName, sectionsNode);
            }
            base.ExecuteCore(node);
        }

        private ConfigurationSectionCollectionNode SelectConfigurationSectionsNode()
        {
            IUIHierarchy hierarchy = UIHierarchyService.SelectedHierarchy;
            ApplicationConfigurationNode applicationNode = hierarchy.RootNode as ApplicationConfigurationNode;
            Debug.Assert(applicationNode != null, "Expected an application node to be root for this node.");
            ConfigurationSectionCollectionNode sectionsNode = (ConfigurationSectionCollectionNode)hierarchy.FindNodeByType(hierarchy.RootNode, typeof(ConfigurationSectionCollectionNode));
            if (sectionsNode == null)
            {
                sectionsNode = new ConfigurationSectionCollectionNode();
                applicationNode.Nodes.AddWithDefaultChildren(sectionsNode);
            }
            return sectionsNode;
        }
    }
}