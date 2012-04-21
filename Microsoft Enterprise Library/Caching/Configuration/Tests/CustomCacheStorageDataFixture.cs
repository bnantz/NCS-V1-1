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
    public class CustomCacheStorageDataFixture
    {
        private string configurationSection =
            "<cacheStorage name=\"inIsolatedStorage\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"CustomCacheStorageData\" " +
                "type=\"TestType\" " +
                "xmlns=\"" + CacheManagerSettings.ConfigurationNamespace + "\">" +
                "<extensions>" +
                "<extension name=\"testName\" value=\"testValue\"/>" +
                "</extensions>" +
                "</cacheStorage>";

        [Test]
        public void CanReadCacheStorageObject()
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(configurationSection));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomCacheStorageData));
            CustomCacheStorageData objectFromConfiguration = xmlSerializer.Deserialize(xmlReader) as CustomCacheStorageData;

            Assert.AreEqual("inIsolatedStorage", objectFromConfiguration.Name);
            Assert.AreEqual("TestType", objectFromConfiguration.TypeName);
            Assert.AreEqual("testValue", objectFromConfiguration.Extensions["testName"]);
            Assert.AreEqual(1, objectFromConfiguration.Extensions.Count);
        }
    }
}

#endif