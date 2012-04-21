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
    public class OracleConnectionStringNodeFixture : ConfigurationDesignHostTestBase
    {
        private OracleConnectionStringNode connectionStringNode;
        private OracleConnectionStringData connectionStringData;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            connectionStringData = databaseSettings.ConnectionStrings["OracleConnection"] as OracleConnectionStringData;
            Assert.IsNotNull(connectionStringData);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            ConfigurationNode[] nodes = hierarchy.FindNodesByType(typeof(OracleConnectionStringNode));
            foreach (OracleConnectionStringNode node in nodes)
            {
                connectionStringNode = node;
            }
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            OracleConnectionStringData data = databaseSettings.ConnectionStrings["OracleConnection"] as OracleConnectionStringData;
            OracleConnectionStringNode node = new OracleConnectionStringNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(4, node.Nodes.Count);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("OracleConnection", connectionStringNode.Name);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual("OracleConnection", ((ConfigurationNode)connectionStringNode).Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(connectionStringNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void ValidateNoNameTest()
        {
            connectionStringNode.Name = string.Empty;
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(connectionStringNode);
            Assert.AreEqual(1, ValidationErrorsCount);
        }
    }
}

#endif