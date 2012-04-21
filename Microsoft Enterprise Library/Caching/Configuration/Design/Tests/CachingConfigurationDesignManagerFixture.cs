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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class CachingConfiguraitonDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(Host);
        }

        [Test]
        public void RegisterTest()
        {
            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(Host);
        }

        [Test]
        public void OpenAndSaveTest()
        {
            GeneratedHierarchy.Load();
            Assert.AreEqual(0, ConfigurationErrorLogService.ConfigurationErrors.Count);
            GeneratedHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorLogService.ConfigurationErrors.Count);

            CacheManagerSettingsNode rootNode = (CacheManagerSettingsNode)GeneratedHierarchy.FindNodeByType(typeof(CacheManagerSettingsNode));
            Assert.IsNotNull(rootNode);
            Assert.AreEqual("InMemoryPersistence", rootNode.DefaultCacheManager.Name);
            rootNode.DefaultCacheManager.Name = "BadData";

            GeneratedHierarchy.Save();
            Assert.AreEqual(0, ConfigurationErrorLogService.ConfigurationErrors.Count);

            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode applicationNode = new ApplicationConfigurationNode(data);
            IUIHierarchy newHierarchy = CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = newHierarchy;
            newHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorLogService.ConfigurationErrors.Count);

            rootNode = (CacheManagerSettingsNode)newHierarchy.FindNodeByType(typeof(CacheManagerSettingsNode));

            Assert.AreEqual("BadData", rootNode.DefaultCacheManager.Name);

            rootNode.DefaultCacheManager.Name = "InMemoryPersistence";

            newHierarchy.Save();
        }

        [Test]
        public void BuildContextTest()
        {
            CachingConfigurationDesignManager b = new CachingConfigurationDesignManager();
            b.Register(Host);
            b.Open(Host);
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            b.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains("cachingConfiguration"));
        }
    }
}

#endif