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
    public class ParameterNodeFixture : ConfigurationDesignHostTestBase
    {
        private ParameterNode parameterNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(DatabaseSettingsBuilder.Create(Host));
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            ConfigurationNode[] nodes = hierarchy.FindNodesByType(typeof(ParameterNode));
            foreach (ParameterNode node in nodes)
            {
                if (node.Name == "server")
                {
                    parameterNode = node;
                    break;
                }
            }
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            ParameterData data = databaseSettings.ConnectionStrings["Northwind"].Parameters["server"];
            ParameterNode node = new ParameterNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.Value, node.Value);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("server", parameterNode.Name);
        }

        [Test]
        public void ValueTest()
        {
            Assert.AreEqual("localhost", parameterNode.Value);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual("server", parameterNode.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(this.parameterNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void ValidateNoNameTest()
        {
            parameterNode.Name = string.Empty;
            Assert.IsTrue(parameterNode.Name.Length == 0);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(this.parameterNode);
            Assert.AreEqual(1, ValidationErrorsCount);
        }
    }
}

#endif