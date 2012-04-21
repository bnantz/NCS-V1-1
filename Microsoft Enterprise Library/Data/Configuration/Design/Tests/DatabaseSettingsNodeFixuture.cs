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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class DatabaseSettingsNodeFixture : ConfigurationDesignHostTestBase
    {
        private DatabaseSettingsNode databaseSettingsNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            GeneratedApplicationNode.Nodes.Add(databaseSettingsNode);
            databaseSettingsNode.ResolveNodeReferences();
        }

        [Test]
        public void InstanceNodeTest()
        {
            Assert.IsNotNull(databaseSettingsNode.DefaultInstanceNode);
            Assert.AreEqual("Service_Dflt", databaseSettingsNode.DefaultInstanceNode.Name);
        }

        [Test]
        public void NodesCountTest()
        {
            Assert.AreEqual(3, databaseSettingsNode.Nodes.Count);
        }

        [Test]
        public void ConnectionStringCollectionNodeTest()
        {
            ConnectionStringCollectionNode node = HierarchyService.SelectedHierarchy.FindNodeByType(databaseSettingsNode, typeof(ConnectionStringCollectionNode)) as ConnectionStringCollectionNode;
            Assert.IsNotNull(node);
        }

        [Test]
        public void DatabaseTypesNodeTest()
        {
            DatabaseTypeCollectionNode node = HierarchyService.SelectedHierarchy.FindNodeByType(databaseSettingsNode, typeof(DatabaseTypeCollectionNode)) as DatabaseTypeCollectionNode;
            Assert.IsNotNull(node);
        }

        [Test]
        public void InstancesNodeTest()
        {
            InstanceCollectionNode node = HierarchyService.SelectedHierarchy.FindNodeByType(databaseSettingsNode, typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            Assert.IsNotNull(node);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(SR.DefaultDatabaseSettingsName, databaseSettingsNode.Name);
        }
    }
}

#endif