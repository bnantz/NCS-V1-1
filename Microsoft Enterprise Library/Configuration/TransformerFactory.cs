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
    /// The factory class creates ITransformer objects.
    /// </devdoc>
    internal class TransformerFactory : ProviderFactory
    {
        public TransformerFactory(ConfigurationContext context) : base(SR.Transformer, context, typeof(ITransformer))
        {
        }

        public ITransformer Create(string sectionName)
        {
            return (ITransformer)base.CreateInstance(sectionName);
        }

        protected override Type GetConfigurationType(string sectionName)
        {
            TransformerData transformerData = GetTransformerData(sectionName);
            if (null != transformerData)
            {
                string typeName = transformerData.TypeName;
                return GetType(typeName);
            }
            return typeof(NullTransformer);
        }

        protected override ConfigurationView CreateConfigurationView()
        {
            return new RuntimeConfigurationView(base.ConfigurationContext);
        }

        protected override void InitializeConfigurationProvider(string sectionName, IConfigurationProvider provider)
        {
            TransformerData transformerData = GetTransformerData(sectionName);
            // since we could be using a NullTransformer
            if (transformerData != null)
            {
                provider.ConfigurationName = transformerData.Name;
            }
            ((ITransformer)provider).CurrentSectionName = sectionName;
            base.InitializeConfigurationProvider(sectionName, provider);
        }

        private TransformerData GetTransformerData(string sectionName)
        {
        	RuntimeConfigurationView configurationView = (RuntimeConfigurationView)CreateConfigurationView();
            return configurationView.GetTransformerProviderData(sectionName);
        }

    }
}