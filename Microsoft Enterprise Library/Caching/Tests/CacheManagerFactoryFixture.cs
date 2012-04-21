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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class CacheManagerFactoryFixture : ConfigurationContextFixtureBase
    {
        private CacheManagerFactory factory;

        [SetUp]
        public void CreateFactory()
        {
            factory = new CacheManagerFactory(Context);
        }

        [Test]
        public void CreateNamedCacheInstance()
        {
            CacheManager cache = factory.GetCacheManager("InMemoryPersistence");
            Assert.IsNotNull(cache, "Should have created caching instance through factory");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void WillThrowExceptionIfCannotFindCacheInstance()
        {
            factory.GetCacheManager("ThisIsABadName");
            Assert.Fail("Should have thrown ConfigurationException");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WillThrowExceptionIfInstanceNameIsNull()
        {
            factory.GetCacheManager(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfInstanceNameIsEmptyString()
        {
            factory.GetCacheManager("");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void WillThrowExceptionIfNullCacheStorage()
        {
            factory.GetCacheManager("CacheManagerWithBadCacheStorageInstance");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void WillThrowExceptionIfCannotCreateNamedStorageType()
        {
            factory.GetCacheManager("CacheManagerWithBadStoreType");
        }

        [Test]
        public void CallingSameFactoryTwiceReturnsSameInstance()
        {
            CacheManager cacheOne = factory.GetCacheManager("InMemoryPersistence");
            CacheManager cacheTwo = factory.GetCacheManager("InMemoryPersistence");
            Assert.AreSame(cacheOne, cacheTwo, "CacheManagerFactory should always return the same instance when using the same instance name");
        }

        [Test]
        public void CallingDifferentFactoryTwiceReturnsDifferentInstances()
        {
            CacheManager cacheOne = factory.GetCacheManager("InMemoryPersistence");

            CacheManagerFactory secondFactory = new CacheManagerFactory(Context);
            CacheManager cacheTwo = secondFactory.GetCacheManager("InMemoryPersistence");

            Assert.IsFalse(object.ReferenceEquals(cacheOne, cacheTwo), "Different factories should always return different instances for same instance name");
        }

        [Test]
        public void CanCreateDefaultCacheManager()
        {
            CacheManager cacheManager = factory.GetCacheManager();
            Assert.IsNotNull(cacheManager);
        }

        [Test]
        public void DefaultCacheManagerAndNamedDefaultInstanceAreSameObject()
        {
            CacheManager defaultInstance = factory.GetCacheManager();
            CacheManager namedInstance = factory.GetCacheManager("ShortInMemoryPersistence");

            Assert.AreSame(defaultInstance, namedInstance);
        }
    }
}

#endif