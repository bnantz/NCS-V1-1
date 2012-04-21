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
    public class CachedPrincipalsFixture
    {
        private IPrincipal principal;
        private const string defaultInstance = "Security Cache Provider1";

        public CachedPrincipalsFixture()
        {
        }

        [SetUp]
        public void SetUp()
        {
            IIdentity identity = new GenericIdentity("zman", "testAuthType");
            string[] roles = new string[] {"admin", "managers"};
            principal = new GenericPrincipal(identity, roles);
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
        public void SavePrincipalWithDefaultExpiration()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SavePrincipal(principal);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);
        }

        [Test]
        public void SavePrincipalWithTokenFromPreviouslyCachedItem()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IIdentity identity = new GenericIdentity("zman", "testauthtype");

            IToken token = securityCache.SaveIdentity(identity);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.SavePrincipal(principal, token);
        }

        [Test]
        public void ExplicitlyExpirePrincipal()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SavePrincipal(principal);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            securityCache.ExpirePrincipal(token);

            IPrincipal cachedPrincipal = securityCache.GetPrincipal(token);
            Assert.IsNull(cachedPrincipal);
        }

        [Test]
        public void RetreiveCachedPrincipal()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider(defaultInstance);
            Assert.IsNotNull(securityCache);

            IToken token = securityCache.SavePrincipal(principal);
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Value);

            IPrincipal cachedPrincipal = securityCache.GetPrincipal(token);
            Assert.IsNotNull(cachedPrincipal);
            Assert.AreEqual(cachedPrincipal.Identity.Name, "zman");
        }

        [Test]
        public void RetreivePrincipalNotInCache()
        {
            ISecurityCacheProvider securityCache = SecurityCacheFactory.GetSecurityCacheProvider();
            Assert.IsNotNull(securityCache);

            IToken token = new GuidToken() as IToken;
            Assert.IsNotNull(token);

            IPrincipal cachedPrincipal = securityCache.GetPrincipal(token);
            Assert.IsNull(cachedPrincipal);
        }
    }
}

#endif