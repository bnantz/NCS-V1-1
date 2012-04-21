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
    public class ConnectionStringCollectionNodeFixture : ConfigurationDesignHostTestBase
    {
        private ConnectionStringCollectionNode connectionStringCollectionNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            connectionStringCollectionNode = (ConnectionStringCollectionNode)hierarchy.FindNodeByType(typeof(ConnectionStringCollectionNode));
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            ConnectionStringCollectionNode node = (ConnectionStringCollectionNode)hierarchy.FindNodeByType(typeof(ConnectionStringCollectionNode));
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void CountConnectionStringTest()
        {
            Assert.AreEqual(2, connectionStringCollectionNode.Nodes.Count);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(SR.DefaultConnectionStringCollectionNodeName, connectionStringCollectionNode.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(connectionStringCollectionNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }
    }
}

#endif