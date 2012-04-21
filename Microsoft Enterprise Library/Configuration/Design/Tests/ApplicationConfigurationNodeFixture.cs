//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;
[assembly : ConfigurationDesignManager(typeof(MyDesignManager))]

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ApplicationConfigurationNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void ConfigurationFileTest()
        {
            ApplicationData applicationData = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(applicationData);
            CreateHierarchyAndAddToHierarchyService(node, new ConfigurationContext(node.ConfigurationFile));
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, node.ConfigurationFile);
        }

        [Test]
        public void FindTypeNodeInHierarchy()
        {
            ApplicationData applicationData = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode applicationNode = new ApplicationConfigurationNode(applicationData);
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(applicationNode, new ConfigurationContext(applicationNode.ConfigurationFile));
            MyConfigNode configNode = new MyConfigNode("MyBlock");
            applicationNode.Nodes.Add(configNode);
            ConfigurationNode node = hierarchy.FindNodeByType(typeof(MyConfigNode));
            Assert.IsNotNull(node);
            Assert.AreSame(configNode, node);
        }

        [Test]
        public void NameTest()
        {
            string name = "MyBlock";
            ApplicationData applicationData = ApplicationData.FromCurrentAppDomain();
            applicationData.Name = name;
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(applicationData);
            CreateHierarchyAndAddToHierarchyService(node, new ConfigurationContext(node.ConfigurationFile));
            Assert.AreEqual(name, node.Name);
        }

        private class MyConfigNode : ConfigurationNode
        {
            private string name;

            public MyConfigNode(string name) : base()
            {
                this.name = name;
            }

            protected override void OnSited()
            {
                base.OnSited();
                Site.Name = name;
            }

        }
    }
}

#endif