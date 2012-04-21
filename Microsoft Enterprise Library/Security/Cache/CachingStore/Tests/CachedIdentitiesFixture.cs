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
    public class CachedIdentitiesFixture
    {
        private IIdentity identity;

        public CachedIdentitiesFixture()
        {
        }

        [SetUp]
        public void SetUp()
        {
            identity = new GenericIdentity("zman", "testAuthType");

        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetValidSecurityCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();

            Assert.IsNotNull(securityCache);
        }

        [Test]
        public void SaveIdentityWithDefaultExpiration()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);
        }

        [Test]
        public void SaveIdentityWithTokenFromPreviousCachedItem()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            string[] roles = new string[] {"admin", "manager"};
            IPrincipal principal = new GenericPrincipal(identity, roles);

            IToken token = securityCache.SavePrincipal(principal);
            Assert.IsNotNull(token);

            securityCache.SaveIdentity(identity, token);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNotNull(cachedIdentity);
            Assert.AreEqual(cachedIdentity.Name, "zman");
        }

        [Test]
        public void ExplicitlyExpireIdentity()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.ExpireIdentity(token);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNull(cachedIdentity);
        }

        [Test]
        public void RetreiveCachedIdentity()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNotNull(cachedIdentity);
            Assert.AreEqual(cachedIdentity.Name, "zman");
        }

        [Test]
        public void RetreiveIdentityNotInCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = new GuidToken() as IToken;
            Assert.IsNotNull(token);

            IIdentity cachedIdentity = securityCache.GetIdentity(token);
            Assert.IsNull(cachedIdentity);
        }
    }
}

#endif