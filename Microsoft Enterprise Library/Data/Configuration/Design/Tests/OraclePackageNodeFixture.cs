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
    public class OraclePackageNodeFixture : ConfigurationDesignHostTestBase
    {
        private OraclePackageNode packageNode;

        public override void SetUp()
        {
            base.SetUp();
            DatabaseSettingsNode databaseSettingsNode = new DatabaseSettingsNode(DatabaseSettingsBuilder.Create(Host));
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(databaseSettingsNode, CreateDefaultConfiguration());
            packageNode = (OraclePackageNode)hierarchy.FindNodeByType(typeof(OraclePackageNode));
        }

        [Test]
        public void CreateFromRuntimeTest()
        {
            DatabaseSettings databaseSettings = DatabaseSettingsBuilder.Create(Host);
            OraclePackageData data = ((OracleConnectionStringData)databaseSettings.ConnectionStrings["OracleConnection"]).OraclePackages["ENTLIB"];
            OraclePackageNode node = new OraclePackageNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.Prefix, node.Prefix);
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual("ENTLIB", packageNode.Name);
        }

        [Test]
        public void PrefixTest()
        {
            Assert.AreEqual("MyStoredProc", packageNode.Prefix);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual("ENTLIB", ((ConfigurationNode)packageNode).Name);
        }

        [Test]
        public void DefaultDisplayNameTest()
        {
            OraclePackageNode node = new OraclePackageNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(SR.DefaultOraclePackageNodeName, node.Name);
        }

        [Test]
        public void ValidateTest()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(packageNode);
        }

        [Test]
        public void ValidateNoPrefixTest()
        {
            packageNode.Prefix = string.Empty;
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(packageNode);
        }
    }
}

#endif