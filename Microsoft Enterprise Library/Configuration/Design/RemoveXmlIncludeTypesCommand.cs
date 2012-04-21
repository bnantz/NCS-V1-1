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
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	
    /// <devdoc>
    /// Remove an XmlIncludeType node from the IUIHierarchy
    /// </devdoc>
	internal class RemoveXmlIncludeTypesCommand : ConfigurationNodeCommand
	{
        public RemoveXmlIncludeTypesCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
		{
		}

        public RemoveXmlIncludeTypesCommand(IServiceProvider serviceProvider, bool clearErrorService) : base(serviceProvider, clearErrorService)
        {
        }
        
        [ReflectionPermission(SecurityAction.Demand,  Flags=ReflectionPermissionFlag.MemberAccess)]
        protected override void ExecuteCore(ConfigurationNode node)
	    {
            ConfigurationSectionCollectionNode configurationSectionCollectionNode = CurrentHierarchy.FindNodeByType(typeof(ConfigurationSectionCollectionNode)) as ConfigurationSectionCollectionNode;
            if (configurationSectionCollectionNode == null)
            {
                return;
            }

            IXmlIncludeTypeService service = XmlIncludeTypeService;
            Type[] types = null;
            XmlSerializerTransformerNode transformerNode = null;
            foreach (ConfigurationNode configurationNode in configurationSectionCollectionNode.Nodes)
            {
                transformerNode = CurrentHierarchy.FindNodeByType(configurationNode, typeof(XmlSerializerTransformerNode)) as XmlSerializerTransformerNode;
                if (transformerNode == null)
                {
                    continue;
                }
                types = service.GetXmlIncludeTypes(configurationNode.Name);
                if (types == null)
                {
                    continue;
                }
                ArrayList nodesToRemove = new ArrayList();
                foreach (Type t in types)
                {
                    if (!FindNodTypeToDataTypeMatchRecursive(node, t)) continue;
                    foreach (XmlIncludeTypeNode xmlIncludeTypeNode in transformerNode.Nodes)
                    {
                        Type includeType = Type.GetType(xmlIncludeTypeNode.TypeName, false, true);
                        if (includeType.Equals(t))
                        {
                            nodesToRemove.Add(xmlIncludeTypeNode);
                        }
                    }
                }
                foreach (ConfigurationNode nodeToRemove in nodesToRemove)
                {
                    nodeToRemove.Remove();
                }
            }
	    }

	    private bool FindNodTypeToDataTypeMatchRecursive(ConfigurationNode node, Type t)
	    {
	        if (NodeTypeMatchesDataType(node, t) && !OtherNodeTypesExists(node)) return true;
	        foreach (ConfigurationNode configurationNode in node.Nodes)
	        {
                if (FindNodTypeToDataTypeMatchRecursive(configurationNode, t)) return true;	                
	        }
            return false;
	    }

	    private bool NodeTypeMatchesDataType(ConfigurationNode node, Type t)
	    {
	        INodeCreationService nodeCreationService = NodeCreationService;
	        return (nodeCreationService.DoesNodeTypeMatchDataType(node.GetType(), t));
	    }

	    private bool OtherNodeTypesExists(ConfigurationNode node)
	    {
	        ConfigurationNode[] nodes = CurrentHierarchy.FindNodesByType(node.GetType());
            return (nodes != null && nodes.Length > 0);
	    }
	}
}
