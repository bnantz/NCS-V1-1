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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Represents a command for adding a XmlIncludeTypeNode to a ConfigurationSectionNode.
    /// </devdoc>
	internal class AddXmlIncludeTypesCommand : ConfigurationNodeCommand
	{
	    private Type nodeType;

        public AddXmlIncludeTypesCommand(IServiceProvider serviceProvider, Type nodeType) : this(serviceProvider, true, nodeType)
        {
        }

	    public AddXmlIncludeTypesCommand(IServiceProvider serviceProvider, bool clearErrorService, Type nodeType) : base(serviceProvider, clearErrorService)
		{
            if (nodeType == null) throw new ArgumentNullException("nodeType");
            this.nodeType = nodeType;
		}

	    protected override void ExecuteCore(ConfigurationNode node)
	    {
            ConfigurationSectionCollectionNode configurationSectionCollectionNode = node.Hierarchy.FindNodeByType(typeof(ConfigurationSectionCollectionNode)) as ConfigurationSectionCollectionNode;
            if (configurationSectionCollectionNode == null) return;

            IXmlIncludeTypeService service = GetService(typeof(IXmlIncludeTypeService)) as IXmlIncludeTypeService;
            Type[] types = null;
            XmlSerializerTransformerNode transformerNode = null;
            foreach (ConfigurationNode configurationNode in configurationSectionCollectionNode.Nodes)
            {
                transformerNode = node.Hierarchy.FindNodeByType(configurationNode, typeof(XmlSerializerTransformerNode)) as XmlSerializerTransformerNode;
                if (transformerNode == null) continue;
                types = service.GetXmlIncludeTypes(configurationNode.Name);
                if (types == null) continue;
                foreach (Type t in types)
                {
                    INodeCreationService nodeCreationService = (INodeCreationService)GetService(typeof(INodeCreationService));
                    if (!nodeCreationService.DoesNodeTypeMatchDataType(nodeType, t)) continue;
                    if (node.Hierarchy.FindNodeByName(transformerNode, t.Name) == null)
                    {
                        transformerNode.Nodes.Add(new XmlIncludeTypeNode(new XmlIncludeTypeData(t.Name, t.AssemblyQualifiedName)));
                    }
                }
            }
	    }
	}
}
