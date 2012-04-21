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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Design.Tests
{
    [TestFixture]
    public class DatabaseSinkNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void NodeTest()
        {
            string storedProcName = "psTest";
            InstanceNode instanceNode = new InstanceNode();
            GeneratedApplicationNode.Nodes.Add(instanceNode);
            instanceNode.Name = "TestNode";

            DatabaseSinkNode node = new DatabaseSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.StoredProcName = storedProcName;
            Assert.AreEqual(storedProcName, node.StoredProcName);
            
            node.DatabaseInstance = instanceNode;
            Assert.AreEqual(instanceNode.Name, node.DatabaseInstance.Name);
        }

        [Test]
        public void DataTest()
        {
            string storedProcName = "psTest";

            DatabaseSinkData data = new DatabaseSinkData();
            data.StoredProcName = storedProcName;

            DatabaseSinkNode node = new DatabaseSinkNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            DatabaseSinkData nodeData = (DatabaseSinkData)node.SinkData;

            Assert.AreEqual(storedProcName, nodeData.StoredProcName);
        }
    }
}

#endif