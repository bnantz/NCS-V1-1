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
    public class DatabaseTypeCollectionNodeFixture : ConfigurationDesignHostTestBase
    {
        private DatabaseTypeCollectionNode databaseTypeCollectionNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            databaseTypeCollectionNode = (DatabaseTypeCollectionNode)hierarchy.FindNodeByType(typeof(DatabaseTypeCollectionNode));
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseTypeCollectionNode node = new DatabaseTypeCollectionNode(databaseSettings.DatabaseTypes);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(databaseSettings.DatabaseTypes.Count, node.Nodes.Count);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void CountDatabaseTypeTest()
        {
            Assert.AreEqual(2, databaseTypeCollectionNode.Nodes.Count);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(SR.DefaultDatabaseTypeCollectionNodeName, databaseTypeCollectionNode.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(databaseTypeCollectionNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }
    }
}

#endif