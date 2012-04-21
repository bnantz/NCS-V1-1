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
    public class CacheStorageDataFixture
    {
        private static readonly string typeName = typeof(NullBackingStore).AssemblyQualifiedName;

        private string configurationSection =
            "<cacheStorage xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  xsi:type=\"CustomCacheStorageData\" name=\"inIsolatedStorage\" " +
                "type=\"" + typeName + "\" " +
                "xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\" " +
                "/>";

        [Test]
        public void CanReadCacheStorageObject()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheStorageData));
            CacheStorageData objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as CacheStorageData;

            Assert.AreEqual("inIsolatedStorage", objectFromConfiguration.Name);
            Assert.IsNotNull(objectFromConfiguration.TypeName);
            Assert.IsNotNull(Type.GetType(objectFromConfiguration.TypeName));
            Assert.IsTrue(typeof(IBackingStore).IsAssignableFrom(Type.GetType(objectFromConfiguration.TypeName)));
        }
    }
}

#endif