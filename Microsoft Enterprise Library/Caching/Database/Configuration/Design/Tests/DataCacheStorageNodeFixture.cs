//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Tests
{
    [TestFixture]
    public class DataCacheStorageNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void NodeTest()
        {
            string databaseInstanceName = "dabaseInstanceName";
            string databasePartitionName = "databasePartitionName";
            
            InstanceNode databaseNode = new InstanceNode(new InstanceData(databaseInstanceName));
            
            DataCacheStorageNode node = new DataCacheStorageNode();
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            hierarchy.AddNode(databaseNode);
            node.DatabaseInstance = databaseNode;
            node.PartitionName = databasePartitionName;
            Assert.AreEqual(databaseInstanceName, node.DatabaseInstance.Name);

            DataCacheStorageData nodeData = (DataCacheStorageData) node.CacheStorageData;
            Assert.AreEqual(databaseInstanceName, nodeData.DatabaseInstanceName);
        }

		[Test]
		public void DataTest()
		{
			string name = "testName";
			string type = typeof(DataBackingStore).AssemblyQualifiedName;

			DataCacheStorageData data = new DataCacheStorageData();
			data.Name = name;

			DataCacheStorageNode node = new DataCacheStorageNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
			Assert.AreEqual(name, node.Name);
			Assert.AreEqual(type, node.Type);
		}

        [Test]
        public void RegisterXmlIncludeTypeTest()
        {
            DataCacheStorageNode node = new DataCacheStorageNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            GeneratedHierarchy.Open();
            Type[] types = XmlIncludeTypeService.GetXmlIncludeTypes(CacheManagerSettings.SectionName);
            Assert.AreEqual(1, types.Length);
            Assert.AreEqual(typeof(DataCacheStorageData), types[0]);
        }
    }
}

#endif