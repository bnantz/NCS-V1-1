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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class DatabaseTypeNodeFixture : ConfigurationDesignHostTestBase
    {
        private DatabaseTypeNode databaseTypeNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            ConfigurationNode[] nodes = hierarchy.FindNodesByType(typeof(DatabaseTypeNode));
            foreach (DatabaseTypeNode node in nodes)
            {
                if (node.Name == "SqlServer")
                {
                    databaseTypeNode = node;
                    break;
                }
            }
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            DatabaseTypeData data = databaseSettings.DatabaseTypes["SqlServer"];
            DatabaseTypeNode node = new DatabaseTypeNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.TypeName, node.TypeName);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("SqlServer", databaseTypeNode.Name);
        }

        [Test]
        public void TypeTest()
        {
            Assert.AreEqual(typeof(SqlDatabase).AssemblyQualifiedName, databaseTypeNode.TypeName);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual("SqlServer", databaseTypeNode.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(databaseTypeNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void ValidateNoNameTest()
        {
            databaseTypeNode.Name = string.Empty;
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(databaseTypeNode);
            Assert.AreEqual(1, ValidationErrorsCount);
        }
    }
}

#endif