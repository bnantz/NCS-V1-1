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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class RemoveNodeCommandFixture : ConfigurationDesignHostTestBase
    {
        private ApplicationConfigurationNode appNode;

        public override void SetUp()
        {
            base.SetUp();
            appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            CreateHierarchyAndAddToHierarchyService(appNode, CreateDefaultConfiguration());
            appNode.Nodes.Add(new ConfigurationSectionNode());
        }

        [Test]
        public void RemoveNodeTest()
        {
            ConfigurationSectionNode node = appNode.Nodes[0] as ConfigurationSectionNode;
            Assert.IsNotNull(node);
            node.Remove();
            Assert.AreEqual(0, appNode.Nodes.Count);
        }
    }
}

#endif