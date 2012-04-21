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
    /// Represents a factory for creating instances of a class which implements <see cref="ISymmetricCryptoProvider"/>.
    /// </summary>
    public class SymmetricCryptoProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class.</para>
        /// </summary>
        public SymmetricCryptoProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SymmetricCryptoProviderFactory"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="context">A <see cref="ConfigurationContext"/> object.</param>
        public SymmetricCryptoProviderFactory(ConfigurationContext context)
            : base(SR.SymmetricCryptoFactory, context, typeof(ISymmetricCryptoProvider))
        {
        }

        /// <summary>
        /// Returns a configured <c>ISymmetricCryptoProvider</c> instance.
        /// </summary>
        /// <param name="cryptoProviderName">Name of crypto provider from configuration</param>
        /// <returns>The configured <c>ISymmetricCryptoProvider</c> object.</returns>
        public ISymmetricCryptoProvider CreateSymmetricCryptoProvider(string cryptoProviderName)
        {
            return (ISymmetricCryptoProvider)base.CreateInstance(cryptoProviderName);
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
        /// <para>Gets the <see cref="Type"/> of the <see cref="ISymmetricCryptoProvider"/> to create based on the name.</para>
        /// </summary>
        /// <param name="cryptoProviderName">
        /// <para>The name of the configuration object to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of <see cref="ISymmetricCryptoProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string cryptoProviderName)
        {
        	CryptographyConfigurationView cryptoConfigurationView = (CryptographyConfigurationView)CreateConfigurationView();
            SymmetricCryptoProviderData symmetricCryptoProviderData = cryptoConfigurationView.GetSymmetricCryptoProviderData(cryptoProviderName);
            return GetType(symmetricCryptoProviderData.TypeName);
        }

    }
}