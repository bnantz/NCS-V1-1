//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestFixture]
    public class DistributionStrategyNodesFixture : ConfigurationDesignHostTestBase
    {
        private ApplicationData data;
        private ApplicationConfigurationNode applicationNode;
        
        public override void SetUp()
        {
            base.SetUp();
            data = ApplicationData.FromCurrentAppDomain();
            applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
        	ClientLoggingConfigurationDesignManager loggingConfigurationDesignManager = new ClientLoggingConfigurationDesignManager();
            loggingConfigurationDesignManager.Register(Host);
        }

        [Test]
        public void StrategiesPropertiesTest()
        {
            DistributionStrategyCollectionNode node = new DistributionStrategyCollectionNode();
            applicationNode.Nodes.Add(node);
            Assert.AreEqual(SR.DistributionStrategies, node.Name);
        }
        

        [Test]
        public void StrategiesDefaultChildNodesTest()
        {
            DistributionStrategyCollectionNode node = new DistributionStrategyCollectionNode();
            applicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(typeof(InProcDistributionStrategyNode), node.Nodes[0].GetType());
        }

        [Test]
        public void StrategiesDataTest()
        {
            string dataName = "TestName";

            DistributionStrategyDataCollection dataCollection = new DistributionStrategyDataCollection();
            DistributionStrategyData data = new CustomDistributionStrategyData();
            data.Name = dataName;
            dataCollection.Add(data);

            DistributionStrategyCollectionNode node = new DistributionStrategyCollectionNode(dataCollection);
            applicationNode.Nodes.Add(node);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(dataName, node.Nodes[0].Name);
        }

        [Test]
        public void InProcNodeTest()
        {
            InProcDistributionStrategyNode node = new InProcDistributionStrategyNode();

            Assert.AreEqual(typeof(InProcLogDistributionStrategy).AssemblyQualifiedName, node.TypeName);
        }

        [Test]
        public void MsmqDataTest()
        {
            string name = SR.MsmqDistributionStrategy;
            string queuePath = "queuePath";
            string typeName = typeof(MsmqLogDistributionStrategy).AssemblyQualifiedName;

            MsmqDistributionStrategyData data = new MsmqDistributionStrategyData();
            data.Name = name;
            data.QueuePath = queuePath;
            data.TypeName = typeName;

            MsmqDistributionStrategyNode node = new MsmqDistributionStrategyNode(data);
            applicationNode.Nodes.Add(node);
            MsmqDistributionStrategyData nodeData = (MsmqDistributionStrategyData) node.DistributionStrategyData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(queuePath, nodeData.QueuePath);
            Assert.AreEqual(typeName, nodeData.TypeName);
        }

        [Test]
        public void MsmqNodeTest()
        {
            string name = SR.MsmqDistributionStrategy;
            string queuePath = "queuePath";
            string typeName = typeof(MsmqLogDistributionStrategy).AssemblyQualifiedName;

            MsmqDistributionStrategyNode node = new MsmqDistributionStrategyNode();
            applicationNode.Nodes.Add(node);
            node.QueuePath = queuePath;

            Assert.AreEqual(queuePath, node.QueuePath);
            Assert.AreEqual(typeName, node.TypeName);
            Assert.AreEqual(name, node.Name);
        }

        [Test]
        public void CustomStrategyDataTest()
        {
            NameValueItem item = new NameValueItem("TeST", "VALUE");
            string name = "testName";
            string type = "fakeType";

            CustomDistributionStrategyData data = new CustomDistributionStrategyData();
            data.Name = name;
            data.TypeName = type;
            data.Attributes.Add(item);

            CustomDistributionStrategyNode node = new CustomDistributionStrategyNode(data);
            applicationNode.Nodes.Add(node);
            CustomDistributionStrategyData nodeData = (CustomDistributionStrategyData) node.DistributionStrategyData;

            Assert.AreEqual(item, nodeData.Attributes[0]);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.TypeName);
        }

        [Test]
        public void CustomStrategyNodeTest()
        {
            NameValueItem item = new NameValueItem("TeST", "VALUE");
            string name = "testName";
            string type = "fakeType";

            CustomDistributionStrategyNode node = new CustomDistributionStrategyNode();
            applicationNode.Nodes.Add(node);
            node.Name = name;
            node.TypeName = type;
            node.Attributes.Add(item);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.TypeName);
            Assert.AreEqual(item, node.Attributes[0]);
        }
    }
}

#endif