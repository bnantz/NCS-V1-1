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
using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    public class MockSecurityCacheProvider : ConfigurationProvider, ISecurityCacheProvider
    {
        public MockSecurityCacheProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        public IToken SaveIdentity(IIdentity identity)
        {
            return null;
        }

        void ISecurityCacheProvider.SaveIdentity(IIdentity identity, IToken token)
        {
        }

        public IToken SavePrincipal(IPrincipal principal)
        {
            return null;
        }

        void ISecurityCacheProvider.SavePrincipal(IPrincipal principal, IToken token)
        {
        }

        public IToken SaveProfile(object profile)
        {
            return null;
        }

        void ISecurityCacheProvider.SaveProfile(object profile, IToken token)
        {
        }

        public void ExpireIdentity(IToken token)
        {
        }

        public void ExpirePrincipal(IToken token)
        {
        }

        public void ExpireProfile(IToken token)
        {
        }

        public IIdentity GetIdentity(IToken token)
        {
            return null;
        }

        public IPrincipal GetPrincipal(IToken token)
        {
            return null;
        }

        public Object GetProfile(IToken token)
        {
            return null;
        }
    }
}

#endif