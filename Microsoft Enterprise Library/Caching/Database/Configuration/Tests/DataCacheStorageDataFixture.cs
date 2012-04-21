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
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Tests
{
    [TestFixture]
    public class DataCacheStorageDataFixture
    {
        private string configurationSection =
            "<cacheStorage " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\" " +
                "xsi:type=\"DataCacheStorageData\" " +
                "name=\"inIsolatedStorage\" " +
                "databaseInstanceName=\"Foo\" " +
                "partitionName=\"Partition1\"" +
                "/>";

        [Test]
        public void CanReadCacheStorageObject()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataCacheStorageData));
            DataCacheStorageData objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as DataCacheStorageData;

            Assert.AreEqual("inIsolatedStorage", objectFromConfiguration.Name);
            Assert.IsNotNull(objectFromConfiguration.TypeName);
            Assert.IsNotNull(Type.GetType(objectFromConfiguration.TypeName));
            Assert.AreEqual("Foo", objectFromConfiguration.DatabaseInstanceName);
            Assert.AreEqual("Partition1", objectFromConfiguration.PartitionName);
        }
    }
}

#endif