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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// <para>Represents a view to navigate the <see cref="CryptographySettings"/> configuration data.</para>
    /// </summary>
    public class CryptographyConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CryptographyConfigurationView"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public CryptographyConfigurationView(ConfigurationContext configurationContext) : base(configurationContext)
        {
        }

        /// <summary>
        /// <para>Gets the named <see cref="HashProviderData"/> from the <see cref="CryptographySettings"/>.</para>
        /// </summary>
        /// <param name="hashProviderName">
        /// <para>The name of the <see cref="HashProviderData"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="HashProviderData"/> from the <see cref="CryptographySettings"/>.</para>
        /// </returns>
        public virtual HashProviderData GetHashProviderData(string hashProviderName)
        {
            ArgumentValidation.CheckForNullReference(hashProviderName, "instanceName");
            ArgumentValidation.CheckForEmptyString(hashProviderName, "instanceName");

            CryptographySettings settings = GetCryptographySettings();
            if (settings == null)
            {
                throw new ConfigurationException(SR.ExceptionCryptoSettingsMissing);
            }

            HashProviderData hashProviderData = settings.HashProviders[hashProviderName];
            if (hashProviderData == null)
            {
                throw new ConfigurationException(SR.ExceptionNoCryptoProvider(hashProviderName));
            }

            return hashProviderData;
        }

        /// <summary>
        /// <para>Gets the <see cref="CryptographySettings"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CryptographySettings"/>.</para>
        /// </returns>
        public virtual CryptographySettings GetCryptographySettings()
        {
            return ConfigurationContext.GetConfiguration(CryptographySettings.SectionName) as CryptographySettings;
        }

        /// <summary>
        /// <para>Gets the named <see cref="SymmetricCryptoProviderData"/> from the <see cref="CryptographySettings"/>.</para>
        /// </summary>
        /// <param name="symmetricProviderName">
        /// <para>The name of the <see cref="SymmetricCryptoProviderData"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="SymmetricCryptoProviderData"/> from the <see cref="CryptographySettings"/>.</para>
        /// </returns>
        public virtual SymmetricCryptoProviderData GetSymmetricCryptoProviderData(string symmetricProviderName)
        {
            CryptographySettings settings = ConfigurationContext.GetConfiguration(CryptographySettings.SectionName) as CryptographySettings;
            if (settings == null)
            {
                throw new ConfigurationException(SR.ExceptionCryptoSettingsMissing);
            }

            SymmetricCryptoProviderData symmetricCryptoProviderData = settings.SymmetricCryptoProviders[symmetricProviderName];
            if (symmetricCryptoProviderData == null)
            {
                throw new ConfigurationException(SR.ExceptionNoCryptoProvider(symmetricProviderName));
            }

            return symmetricCryptoProviderData;
        }
    }
}