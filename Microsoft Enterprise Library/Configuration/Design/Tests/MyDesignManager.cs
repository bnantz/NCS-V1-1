//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    public class MyDesignManager : IConfigurationDesignManager
    {
        void IConfigurationDesignManager.BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary dictionary)
        {
            dictionary.Add("mySection", "My Configuration Data");
        }

        void IConfigurationDesignManager.Save(IServiceProvider serviceProvider)
        {
        }

        void IConfigurationDesignManager.Open(IServiceProvider serviceProvider)
        {
        }

        void IConfigurationDesignManager.Register(IServiceProvider serviceProvider)
        {
        }
    }
}

#endif