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

#if     UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class CacheManagerNodeFixture : ConfigurationDesignHostTestBase
    {
        private ApplicationConfigurationNode applicationNode;

        public override void SetUp()
        {
            base.SetUp();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(Host);
        }

        [Test]
        public void NodeTest()
        {
            int expirationPollFrequencyInSeconds = 30;
            int maximumElementsInCacheBeforeScavenging = 5;
            int numberToRemoveWhenScavenging = 8;
            CacheManagerNode node = new CacheManagerNode();

            node.ExpirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            Assert.AreEqual(expirationPollFrequencyInSeconds, node.ExpirationPollFrequencyInSeconds);

            node.MaximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            Assert.AreEqual(maximumElementsInCacheBeforeScavenging, node.MaximumElementsInCacheBeforeScavenging);

            node.NumberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            Assert.AreEqual(numberToRemoveWhenScavenging, node.NumberToRemoveWhenScavenging);

            CacheManagerData data = node.CacheManagerData;
            Assert.AreEqual(SR.DefaultCacheManagerNodeName, data.Name);
            Assert.AreEqual(expirationPollFrequencyInSeconds, data.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(maximumElementsInCacheBeforeScavenging, data.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(numberToRemoveWhenScavenging, data.NumberToRemoveWhenScavenging);

        }

        [Test]
        public void DataTest()
        {
            string name = "testName";
            CacheStorageData cacheStorage = new CustomCacheStorageData();
            cacheStorage.Name = "testCacheStorageName";
            cacheStorage.TypeName = "fakeType";
            int expirationPollFrequencyInSeconds = 30;
            int maximumElementsInCacheBeforeScavenging = 5;
            int numberToRemoveWhenScavenging = 8;

            CacheManagerData data = new CacheManagerData();
            data.Name = name;
            data.CacheStorage = cacheStorage;
            data.ExpirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            data.MaximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            data.NumberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            data.CacheStorage = cacheStorage;

            CacheManagerNode node = new CacheManagerNode(data);
            applicationNode.Nodes.Add(node);
            CacheManagerData nodeData = node.CacheManagerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(expirationPollFrequencyInSeconds, nodeData.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(maximumElementsInCacheBeforeScavenging, nodeData.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(numberToRemoveWhenScavenging, nodeData.NumberToRemoveWhenScavenging);
            Assert.AreEqual(cacheStorage.Name, nodeData.CacheStorage.Name);
            Assert.AreEqual(cacheStorage.TypeName, nodeData.CacheStorage.TypeName);
        }
    }
}

#endif