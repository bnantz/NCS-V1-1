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

#if  UNIT_TESTS
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Tests
{
    [TestFixture]
    public class CacheManagerSettingsFixture
    {
        private string configurationSection =
            "<enterpriseLibrary.cacheSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\" defaultCacheManager=\"InMemoryPersistence\"> " +
                "<cacheManagers> " +
                "<cacheManager  " +
                "name=\"InMemoryPersistence\"  " +
                "cacheStorageName=\"inMemory\" " +
                "expirationPollFrequencyInSeconds=\"60\" " +
                "maximumElementsInCacheBeforeScavenging=\"100\" " +
                "numberToRemoveWhenScavenging=\"10\" " +
                "> " +
                "<cacheStorage xsi:type=\"CustomCacheStorageData\" name=\"NullBackingStore\"/>" +
                "</cacheManager>" +
                "</cacheManagers> " +
                "</enterpriseLibrary.cacheSettings>";

        private CacheManagerSettings objectFromConfiguration;

        [TestFixtureSetUp]
        public void ParseSml()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheManagerSettings));
            objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as CacheManagerSettings;
        }

        [Test]
        public void CanGetDefaultCacheManagerName()
        {
            Assert.AreEqual("InMemoryPersistence", objectFromConfiguration.DefaultCacheManager);
        }

        [Test]
        public void GetCacheManagerDataFromConfiguration()
        {
            Assert.AreEqual(1, objectFromConfiguration.CacheManagers.Count);
            Assert.AreEqual("InMemoryPersistence", objectFromConfiguration.CacheManagers["InMemoryPersistence"].Name);
            Assert.AreEqual("NullBackingStore", objectFromConfiguration.CacheManagers["InMemoryPersistence"].CacheStorage.Name);
        }
    }
}

#endif