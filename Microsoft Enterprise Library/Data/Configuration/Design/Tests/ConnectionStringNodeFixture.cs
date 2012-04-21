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
    public class ConnectionStringNodeFixture : ConfigurationDesignHostTestBase
    {
        private ConnectionStringNode connectionStringNode;
        private ConnectionStringData connectionStringData;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            connectionStringData = databaseSettings.ConnectionStrings["Northwind"];
            Assert.IsNotNull(connectionStringData);
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(databaseSettings);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            ConfigurationNode[] nodes = hierarchy.FindNodesByType(typeof(ConnectionStringNode));
            foreach (ConnectionStringNode node in nodes)
            {
                if (node.Name == "Northwind")
                {
                    connectionStringNode = node;
                    break;
                }

            }
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            ConnectionStringData data = databaseSettings.ConnectionStrings["Northwind"];
            ConnectionStringNode node = new ConnectionStringNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(3, node.Nodes.Count);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("Northwind", connectionStringNode.Name);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual("Northwind", connectionStringNode.Name);
        }

        [Test]
        public void ParameterNodesTest()
        {
            Assert.AreEqual(3, connectionStringNode.Nodes.Count);
        }

        [Test]
        public void RuntimeMatch()
        {
            Assert.AreSame(this.connectionStringData, this.connectionStringNode.ConnectionStringData);
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