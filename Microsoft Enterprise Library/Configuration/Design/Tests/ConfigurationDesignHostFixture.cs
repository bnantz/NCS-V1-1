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

#if UNIT_TESTS
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationDesignHostFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void TestAddComponent()
        {
            RootNode node = new RootNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Nodes.Add(new ChildNode());
            Assert.AreEqual(3, Container.Components.Count);
        }

        [Test]
        public void AssertCanGetAllServices()
        {
            IContainer container = Host.GetService(typeof(IContainer)) as IContainer;
            Assert.IsNotNull(container);
            IComponentChangeService changeService = Host.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            Assert.IsNotNull(changeService);
            INodeNameCreationService nodeNameCreationService = Host.GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
            Assert.IsNotNull(nodeNameCreationService);
            INameCreationService nameCreationService = Host.GetService(typeof(INameCreationService)) as INameCreationService;
            Assert.IsNotNull(nameCreationService);
            IUIHierarchyService hierarchyService = Host.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Assert.IsNotNull(hierarchyService);
            IDictionaryService dictionaryService = Host.GetService(typeof(IDictionaryService)) as IDictionaryService;
            Assert.IsNotNull(dictionaryService);
            IConfigurationErrorLogService configurationErrorLogService = Host.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
            Assert.IsNotNull(configurationErrorLogService);
            INodeCreationService nodeCreationService = Host.GetService(typeof(INodeCreationService)) as INodeCreationService;
            Assert.IsNotNull(nodeCreationService);
            IXmlIncludeTypeService xmlIncludeTypeService = Host.GetService(typeof(IXmlIncludeTypeService)) as IXmlIncludeTypeService;
            Assert.IsNotNull(xmlIncludeTypeService);
            ILinkNodeService linkNodeService = Host.GetService(typeof(ILinkNodeService)) as ILinkNodeService;
            Assert.IsNotNull(linkNodeService);
            IMenuContainerService menuContainerService = Host.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Assert.IsNotNull(menuContainerService);
        }

        private class RootNode : ConfigurationNode
        {
            public RootNode() : base()
            {
            }
        }

        private class ChildNode : ConfigurationNode
        {
            public ChildNode() : base()
            {
            }
        }
    }
}

#endif