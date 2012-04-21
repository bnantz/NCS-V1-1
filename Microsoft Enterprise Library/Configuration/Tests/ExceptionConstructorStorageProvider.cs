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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    public class ExceptionConstructorStorageProvider : StorageProvider
    {
        public ExceptionConstructorStorageProvider()
        {
            throw new InvalidOperationException();
        }

        public event ConfigurationChangedEventHandler ConfigurationChanged;

        public bool IsInitialized
        {
            get { return false; }
        }

        public string SectionName
        {
            get { return string.Empty; }
            set
            {
            }
        }

        public override object Read()
        {
            return null;
        }

        public void Dispose()
        {
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

        public override void Initialize(ConfigurationView configurationView)
        {
        }

    }
}

#endif