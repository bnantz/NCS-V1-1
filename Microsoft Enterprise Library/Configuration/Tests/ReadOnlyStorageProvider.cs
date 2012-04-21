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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    public class ReadOnlyStorageProvider : StorageProvider
    {
        public ReadOnlyStorageProvider()
        {
        }

        public event ConfigurationChangedEventHandler ConfigurationChanged;

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        public override object Read()
        {
            return null;
        }
        
        public override IConfigurationChangeWatcher CreateConfigurationChangeWatcher()
        {
            return null;
        }
        
        protected virtual void OnConfigurationChanged(ConfigurationChangedEventArgs e)
        {
            if (null != ConfigurationChanged)
            {
                ConfigurationChanged(this, e);
            }
        }
    }
}

#endif