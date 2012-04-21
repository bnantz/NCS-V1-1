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

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NUnit.Framework;
using EntLibDb = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestFixture]
    public class DatabaseCacheExceptionBehavior : ConfigurationContextFixtureBase
    {
        private CacheManager cache;

        public override void FixtureSetup()
        {
            base.FixtureSetup ();
            CacheManagerFactory factory = new CacheManagerFactory(Context);
            cache = factory.GetCacheManager("InDatabasePersistence");
        }

        [TearDown]
        public void TearDown()
        {
            cache.Flush();
        }

        [TestFixtureTearDown]
        public void OneTimeTearDown()
        {
            cache.Dispose();
        }

        [Test]
        public void ItemRemovedFromCacheCompletelyIfAddFails()
        {
            cache.Add("foo", new SerializableClass());

            try
            {
                cache.Add("foo", new NonSerializableClass());
                Assert.Fail("should have thrown exception in Cache.Add");
            }
            catch (Exception)
            {
                Assert.IsFalse(cache.Contains("foo"));

                string isItInDatabaseQuery = "select count(*) from CacheData";
                Data.Database db = DatabaseFactory.CreateDatabase("CachingDatabase");
                DBCommandWrapper wrapper = db.GetSqlStringCommandWrapper(isItInDatabaseQuery);
                int count = (int)db.ExecuteScalar(wrapper);

                Assert.AreEqual(0, count);
            }
        }

        private class NonSerializableClass
        {
        }

        [Serializable]
        private class SerializableClass
        {
        }
    }
}

#endif