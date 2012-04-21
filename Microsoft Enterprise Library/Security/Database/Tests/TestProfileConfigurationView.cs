//===============================================================================
// Microsoft patterns & practices Enterprise Library
// XXXXX Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    public class TestProfileConfigurationView : SecurityConfigurationView
    {
        public DbProfileProviderData data;

        public TestProfileConfigurationView(DbProfileProviderData data, ConfigurationContext context) : base(context)
        {
            this.data = data;
        }

        public override ProfileProviderData GetProfileProviderData(string profileProviderName)
        {
            return data;
        }

    }
}

#endif