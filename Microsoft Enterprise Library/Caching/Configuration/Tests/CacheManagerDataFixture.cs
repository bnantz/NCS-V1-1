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
    public class CacheManagerDataFixture
    {
        private string configurationSection =
            "<cacheManager " +
                "name=\"InDatabasePersistence\" " +
                "expirationPollFrequencyInSeconds=\"60\" " +
                "maximumElementsInCacheBeforeScavenging=\"100\" " +
                "numberToRemoveWhenScavenging=\"10\" " +
                "xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\" " +
                "/>";

        [Test]
        public void CanReadCacheManagerData()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheManagerData));
            CacheManagerData objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as CacheManagerData;

            Assert.AreEqual("InDatabasePersistence", objectFromConfiguration.Name);
            Assert.AreEqual(60, objectFromConfiguration.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(100, objectFromConfiguration.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(10, objectFromConfiguration.NumberToRemoveWhenScavenging);
        }

        [Test]
        public void CopyToTest()
        {
            CacheManagerDataCollection collection = new CacheManagerDataCollection();

            CacheManagerData data = new CacheManagerData();
            data.Name = "Cache1";
            CacheManagerData data1 = new CacheManagerData();
            data1.Name = "Cache2";

            collection.Add(data);
            collection.Add(data1);

            CacheManagerData[] array = new CacheManagerData[collection.Count];

            collection.CopyTo(array, 0);

            Assert.AreSame(array[0], data);
            Assert.AreSame(array[1], data1);
        }
    }
}

#endif