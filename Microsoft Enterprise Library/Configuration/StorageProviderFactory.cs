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

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <devdoc>
    /// The factory that creates IStorageProviderReader / IStorageProviderWriter objects.
    /// </devdoc>
    internal class StorageProviderFactory : ProviderFactory
    {
        public StorageProviderFactory(ConfigurationContext context) : base(SR.StorageProvider, context, typeof(IStorageProviderReader))
        {
        }

        public IStorageProviderReader Create(string sectionName)
        {
            return (IStorageProviderReader)base.CreateInstance(sectionName);
        }
        
        protected override Type GetConfigurationType(string sectionName)
        {
            StorageProviderData data = GetStorageProviderData(sectionName);
            return GetType(data.TypeName);
        }

        protected override ConfigurationView CreateConfigurationView()
        {
            return new RuntimeConfigurationView(base.ConfigurationContext);
        }

        protected override void InitializeConfigurationProvider(string sectionName, IConfigurationProvider provider)
        {
            StorageProviderData storageProviderData = GetStorageProviderData(sectionName);
            provider.ConfigurationName = storageProviderData.Name;
            ((IStorageProviderReader)provider).CurrentSectionName = sectionName;
            base.InitializeConfigurationProvider (sectionName, provider);
        }


        private StorageProviderData GetStorageProviderData(string sectionName)
        {
        	RuntimeConfigurationView runtimeConfigurationView = (RuntimeConfigurationView)CreateConfigurationView();
            return runtimeConfigurationView.GetStorageProviderData(sectionName);
        }
    }
}