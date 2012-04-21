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
using System;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class InstanceNodeFixture : ConfigurationDesignHostTestBase
    {
        private static readonly string name = "Service_Dflt";
        private static readonly string instanceType = "SqlServer";
        private static readonly string connectionStringName = "Northwind";

        private InstanceNode instanceNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            GeneratedApplicationNode.Nodes.Add(databaseSettingsNode);
            databaseSettingsNode.ResolveNodeReferences();
            InstanceCollectionNode collectionNode = GeneratedHierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            instanceNode = GeneratedHierarchy.FindNodeByName(collectionNode, name) as InstanceNode;
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            databaseSettingsNode.ResolveNodeReferences();
            InstanceData data = databaseSettings.Instances[name];
            InstanceCollectionNode collectionNode = hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            InstanceNode node = hierarchy.FindNodeByName(collectionNode, name) as InstanceNode;
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.DatabaseTypeName, node.DatabaseTypeName);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void DatabaseTypeNodeTest()
        {
            Assert.IsNotNull(instanceNode.DatabaseTypeNode);
            Assert.AreEqual(instanceType, instanceNode.DatabaseTypeNode.Name);
        }

        [Test]
        public void ConnectionStringNodeTest()
        {
            Assert.IsNotNull(instanceNode.ConnectionStringNode);
            Assert.AreEqual(connectionStringName, instanceNode.ConnectionStringNode.Name);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(name, instanceNode.Name);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(name, instanceNode.Name);
        }

        [Test]
        public void TypeTest()
        {
            Assert.AreEqual(instanceType, instanceNode.DatabaseTypeName);
        }

        [Test]
        public void ConnectionStringTest()
        {
            Assert.AreEqual(connectionStringName, instanceNode.ConnectionString);
        }

        [Test]
        public void NewDatabaseTypeNodeTest()
        {
            IUIHierarchy hierarchy = HierarchyService.SelectedHierarchy;
            InstanceCollectionNode collectionNode = hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            InstanceNode localInstanceNode = hierarchy.FindNodeByName(collectionNode, name) as InstanceNode;
            DatabaseTypeCollectionNode databaseTypeCollectionNode = hierarchy.FindNodeByType(typeof(DatabaseTypeCollectionNode)) as DatabaseTypeCollectionNode;
            DatabaseTypeNode databaseTypeNode = hierarchy.FindNodeByName(databaseTypeCollectionNode, "SqlServer") as DatabaseTypeNode;
            localInstanceNode.DatabaseTypeNode = databaseTypeNode;
            Assert.AreEqual(databaseTypeNode.Name, localInstanceNode.DatabaseTypeName);
            databaseTypeNode.Name = "newName";
            Assert.AreEqual(databaseTypeNode.Name, localInstanceNode.DatabaseTypeName);
            databaseTypeNode.Remove();
            Assert.IsNull(localInstanceNode.DatabaseTypeNode);
        }

        [Test]
        public void NewConnectionStringNodeTest()
        {
            IUIHierarchy hierarchy = HierarchyService.SelectedHierarchy;
            InstanceCollectionNode collectionNode = hierarchy.FindNodeByType(typeof(InstanceCollectionNode)) as InstanceCollectionNode;
            InstanceNode node = hierarchy.FindNodeByName(collectionNode, name) as InstanceNode;
            ConnectionStringCollectionNode connectionStringCollectionNode = hierarchy.FindNodeByType(typeof(ConnectionStringCollectionNode)) as ConnectionStringCollectionNode;
            ConnectionStringNode connectionStringNode = hierarchy.FindNodeByName(connectionStringCollectionNode, "Northwind") as ConnectionStringNode;
            Assert.IsNotNull(connectionStringNode);
            node.ConnectionStringNode = connectionStringNode;
            Assert.AreEqual(connectionStringNode.Name, node.ConnectionString);
            connectionStringNode.Name = "newName";
            Assert.AreEqual(connectionStringNode.Name, node.ConnectionString);
            connectionStringNode.Remove();
            Assert.IsNull(node.ConnectionStringNode);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(instanceNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void ValidateNoConnectionStringNodeTest()
        {
            instanceNode.ConnectionStringNode = null;
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(instanceNode);
            Assert.AreEqual(1, ValidationErrorsCount);
        }
    }
}

#endif