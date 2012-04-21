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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Tests
{
    [TestFixture]
    public class CachingDatabaseConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void RegisterTest()
        {
            CachingDatabaseConfigurationDesignManager manager = new CachingDatabaseConfigurationDesignManager();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            manager.Register(Host);
            
            DataCacheStorageNode testNode = NodeCreationService.CreateNode(typeof(DataCacheStorageData), new object[] {new DataCacheStorageData()}) as DataCacheStorageNode;

            Assert.IsNotNull(testNode);
        }
    }
}

#endif