//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Represents a factory for creating instances of a class which implements <see cref="IHashProvider"/>.
    /// </summary>
    public class HashProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashProviderFactory"/> class.</para>
        /// </summary>
        public HashProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes the factory with a specified configuration context.
        /// </summary>
        /// <param name="context">The configuration context.</param>
        public HashProviderFactory(ConfigurationContext context) : base(SR.HashFactory, context, typeof(IHashProvider))
        {
        }

        /// <summary>
        /// Returns a configured <c>IHashProvider</c> instance.
        /// </summary>
        /// <param name="hashProviderName">Name of hash provider from configuration</param>
        /// <returns>The configured <c>IHashProvider</c> object.</returns>
        public IHashProvider CreateHashProvider(string hashProviderName)
        {
            return (IHashProvider)base.CreateInstance(hashProviderName);
        }

        /// <summary>
        /// <para>Creates the <see cref="CryptographyConfigurationView"/> for the factory.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CryptographyConfigurationView"/> for the factory.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new CryptographyConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of the <see cref="IHashProvider"/> to create based on the name.</para>
        /// </summary>
        /// <param name="hashProviderName">
        /// <para>The name of the configuration object to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of <see cref="IHashProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string hashProviderName)
        {
        	CryptographyConfigurationView cryptoConfigurationView = (CryptographyConfigurationView)CreateConfigurationView();
            HashProviderData hashProviderData = cryptoConfigurationView.GetHashProviderData(hashProviderName);
            return GetType(hashProviderData.TypeName);
        }
    }
}