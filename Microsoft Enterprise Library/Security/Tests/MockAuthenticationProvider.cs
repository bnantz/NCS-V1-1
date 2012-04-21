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
    public class MockAuthenticationProvider : ConfigurationProvider, IAuthenticationProvider
    {
        public MockAuthenticationProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        public bool Authenticate(object credentials, out IIdentity identity)
        {
            identity = new GenericIdentity(String.Empty);
            return false;
        }
    }
}

#endif