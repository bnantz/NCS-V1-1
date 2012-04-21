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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection
{
    /// <devdoc>
    /// The factory that creates IConfigurationStorageProvider objects.
    /// </devdoc>
    internal class KeyAlgorithmStorageProviderFactory : ProviderFactory
    {
        // since all providers need a name and we only allow one we use this name as a place holder for the name
        // because the framework requires it
        private const string defaultProviderName = "DefaultKeyAlgorithmProviderName";

        public KeyAlgorithmStorageProviderFactory(ConfigurationContext context) : base(SR.KeyAlgorithmPairStorageProvider, context, typeof(IKeyAlgorithmPairStorageProvider))
        {
        }

        public IKeyAlgorithmPairStorageProvider Create()
        {
            return (IKeyAlgorithmPairStorageProvider)base.CreateInstance(defaultProviderName);
        }

        protected override Type GetConfigurationType(string providerName)
        {
            ConfigurationSettings configurationSettings = ConfigurationContext.GetMetaConfiguration();
            if (null == configurationSettings.KeyAlgorithmPairStorageProviderData)
            {
                throw new ConfigurationException(SR.ExceptionNoKeyAlgorithmStorageProvider);
            }
            return GetType(configurationSettings.KeyAlgorithmPairStorageProviderData.TypeName);
        }

        protected override ConfigurationView CreateConfigurationView()
        {
            return new RuntimeConfigurationView(base.ConfigurationContext);
        }
    }
}