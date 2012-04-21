//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.Security.Principal;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests
{
    [TestFixture]
    public class CachedProfilesFixture
    {
        private object profile;
        private const string defaultInstance = "Security Cache Provider1";

        public CachedProfilesFixture()
        {
        }

        [SetUp]
        public void SetUp()
        {
            profile = "testprofile";

        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetValidSecurityCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);

            Assert.IsNotNull(securityCache);
        }

        [Test]
        public void SaveProfileWithDefaultExpiration()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);
        }

        [Test]
        public void SaveProfileWithTokenFromPreviouslyCachedItem()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IIdentity identity = new GenericIdentity("zman", "testauthtype");

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);

            securityCache.SaveProfile(profile, token);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            object tmpProfile = securityCache.GetProfile(token);
            Assert.IsNotNull(tmpProfile);
            Assert.AreEqual(tmpProfile.ToString(), "testprofile");
        }

        [Test]
        public void ExplicitlyExpireProfile()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.ExpireProfile(token);

            object tmpProfile = securityCache.GetProfile(token);
            Assert.IsNull(tmpProfile);
        }

        [Test]
        public void RetreiveCachedProfile()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveProfile(profile);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            object cachedProfile = securityCache.GetProfile(token);
            Assert.IsNotNull(cachedProfile);
            Assert.AreEqual(cachedProfile.ToString(), "testprofile");
        }

        [Test]
        public void RetreiveProfileNotInCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = new GuidToken() as IToken;
            Assert.IsNotNull(token);

            object cachedProfile = securityCache.GetProfile(token);
            Assert.IsNull(cachedProfile);
        }
    }
}

#endif