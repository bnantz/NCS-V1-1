//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Globalization;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication
{
    /// <summary>
    /// Defines the functionality for a Database-based Authentication Provider.
    /// </summary>
    public class DbAuthenticationProvider : ConfigurationProvider, IAuthenticationProvider
    {
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbAuthenticationProvider"/> class.</para>
        /// </summary>
        public DbAuthenticationProvider()
        {}

        /// <summary>
        /// <para>Initializes the provider with a <see cref="SecurityConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="SecurityConfigurationView"/> object.</para>
        /// </param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(SecurityConfigurationView));

            this.securityConfigurationView = (SecurityConfigurationView)configurationView;
        }

        /// <summary>
        /// Authenticate the credentials against the configuration database.
        /// </summary>
        /// <param name="credentials">A credential object such as a <see cref="NamePasswordCredential"/>.</param>
        /// <param name="userIdentity">An <see cref="IIdentity"/> object representing 
        /// authenticated credentials returned if successfull.</param>
        /// <returns><strong>True</strong> if authentication was
        /// successful, otherwise false.</returns>
        /// <remarks>
        /// This method call is intended to be overloaded to support additional 
        /// credential objects if/when they are brought online.
        /// </remarks>
        public bool Authenticate(object credentials, out IIdentity userIdentity)
        {
            bool result = false;
            userIdentity = null;

            NamePasswordCredential namePasswordCredentials = credentials as NamePasswordCredential;

            if (namePasswordCredentials != null && namePasswordCredentials.Name.Length > 0)
            {
                SecurityAuthenticationCheckEvent.Fire(namePasswordCredentials.Name);

                result = PasswordsMatch(namePasswordCredentials.PasswordBytes, namePasswordCredentials.Name);

                if (result)
                {
                    userIdentity = new GenericIdentity(namePasswordCredentials.Name, GetAuthenticationType());
                }
                else
                {
                    SecurityAuthenticationFailedEvent.Fire(namePasswordCredentials.Name);
                }
            }

            return result;
        }

        /// <devdoc>
        /// Compares the password passed in against the password stored in the database.
        /// </devdoc>
        private bool PasswordsMatch(byte[] password, string userName)
        {
            DbAuthenticationProviderData dbAuthenticationProviderData = (DbAuthenticationProviderData)securityConfigurationView.GetAuthenticationProviderData(ConfigurationName);
            bool result = false;
            UserRoleManager manager = new UserRoleManager(dbAuthenticationProviderData.Database, securityConfigurationView.ConfigurationContext);
            byte[] hashedPassword = manager.GetPassword(userName);

            if (hashedPassword != null)
            {
                HashProviderFactory hashFactory = new HashProviderFactory(securityConfigurationView.ConfigurationContext);
                IHashProvider hashProvider = hashFactory.CreateHashProvider(dbAuthenticationProviderData.HashProvider);
                result = hashProvider.CompareHash(password, hashedPassword);
            }
            return result;
        }

        private string GetAuthenticationType()
        {
            DbAuthenticationProviderData dbAuthenticationProviderData = (DbAuthenticationProviderData)securityConfigurationView.GetAuthenticationProviderData(ConfigurationName);
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", typeof(DbAuthenticationProvider).FullName, dbAuthenticationProviderData.Name);
        }
    }
}