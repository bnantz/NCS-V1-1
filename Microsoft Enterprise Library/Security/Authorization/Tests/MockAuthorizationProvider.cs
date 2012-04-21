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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    public class MockAuthorizationProvider : ConfigurationProvider, IAuthorizationProvider
    {
    	private bool initialized = false;

    	public MockAuthorizationProvider()
        {
        }

		public override void Initialize(ConfigurationView configurationView)
		{
			initialized = true;
		}

        public bool Authorize(object authority, object context)
        {
            return false;
        }

        public bool Authorize(IPrincipal principal, string context)
        {
            return false;
        }

    	public bool Initialized
    	{
    		get { return initialized; }
    	}
    }
}

#endif