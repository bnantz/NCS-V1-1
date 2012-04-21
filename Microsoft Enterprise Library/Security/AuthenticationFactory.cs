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

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Static factory class used to get instances of a specified IAuthenticationProvider
    /// </summary>
    public sealed class AuthenticationFactory
    {
        private AuthenticationFactory()
        {
        }

        /// <summary>
        /// Returns the default IAuthenticationProvider instance. 
        /// Guaranteed to return an intialized IAuthenticationProvider if no exception thrown
        /// </summary>
        /// <returns>Default authentication provider instance.</returns>
        /// <exception cref="ConfigurationException">Unable to create default IAuthenticationProvider</exception>
        public static IAuthenticationProvider GetAuthenticationProvider()
        {
            AuthenticationProviderFactory factory = new AuthenticationProviderFactory(ConfigurationManager.GetCurrentContext());
                return factory.GetAuthenticationProvider();
            }

        /// <summary>
        /// Returns the named IAuthenticationProvider instance. Guaranteed to return an initialized IAuthenticationProvider if no exception thrown.
        /// </summary>
        /// <param name="authenticationProvider">Name defined in configuration for the authentication provider to instantiate</param>
        /// <returns><see cref="IAuthenticationProvider"></see> created through configuration</returns>
        /// <exception cref="ArgumentNullException">providerName is null</exception>
        /// <exception cref="ArgumentException">providerName is empty</exception>
        /// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
        /// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
        public static IAuthenticationProvider GetAuthenticationProvider(string authenticationProvider)
        {
            AuthenticationProviderFactory factory = new AuthenticationProviderFactory(ConfigurationManager.GetCurrentContext());
            return factory.GetAuthenticationProvider(authenticationProvider);
            }
        }
    }
