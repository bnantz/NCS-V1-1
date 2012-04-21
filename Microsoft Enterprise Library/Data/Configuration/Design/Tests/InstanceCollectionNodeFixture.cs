//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class InstanceCollectionNodeFixture : ConfigurationDesignHostTestBase
    {
        private InstanceCollectionNode instancesNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            databaseSettingsNode.ResolveNodeReferences();
            instancesNode = (InstanceCollectionNode)hierarchy.FindNodeByType(typeof(InstanceCollectionNode));
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            databaseSettingsNode.ResolveNodeReferences();
            InstanceCollectionNode node = (InstanceCollectionNode)hierarchy.FindNodeByType(typeof(InstanceCollectionNode));
            Assert.AreEqual(databaseSettings.Instances.Count, node.Nodes.Count);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void GetInstancesTest()
        {
            InstanceDataCollection instances = instancesNode.InstanceDataCollection;
            Assert.IsNotNull(instances);
            Assert.AreEqual(3, instances.Count);
        }

        [Test]
        public void CountInstancesTest()
        {
            Assert.AreEqual(3, instancesNode.Nodes.Count);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(SR.DefaultInstaceCollectionNodeName, instancesNode.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(instancesNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }
    }
}

#endif