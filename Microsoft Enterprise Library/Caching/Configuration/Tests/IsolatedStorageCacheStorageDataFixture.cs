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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Tests
{
    [TestFixture]
    public class IsolatedStorageCacheStorageDataFixture
    {
        private string configurationSection =
            "<cacheStorage " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:type=\"IsolatedStorageCacheStorageData\" " +
                "name=\"inIsolatedStorage\" " +
                "partitionName=\"Foo\" " +
                "xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\" " +
                "/>";

        [Test]
        public void ConstructStorageDataFromXml()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(IsolatedStorageCacheStorageData));
            IsolatedStorageCacheStorageData objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as IsolatedStorageCacheStorageData;

            Assert.AreEqual("inIsolatedStorage", objectFromConfiguration.Name);
            Assert.AreEqual("Foo", objectFromConfiguration.PartitionName);
            Assert.IsNotNull(Type.GetType(objectFromConfiguration.TypeName));
            object createdInstance = Activator.CreateInstance(Type.GetType(objectFromConfiguration.TypeName), null);
            Assert.IsTrue(createdInstance is IsolatedStorageBackingStore, "Should have created instance of IsolatedStorageBackingStore");
        }
    }
}

#endif