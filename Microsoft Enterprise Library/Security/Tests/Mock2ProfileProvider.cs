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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    public class Mock2ProfileProvider : ConfigurationProvider, IProfileProvider
    {
        private bool initialized;

        public Mock2ProfileProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
            this.initialized = true;
        }

        public object GetProfile(IIdentity identity)
        {
            return null;
        }

        public void SetProfile(IIdentity identity, object profile)
        {
        }

        public bool Initialized
        {
            get { return initialized; }
        }
    }
}

#endif