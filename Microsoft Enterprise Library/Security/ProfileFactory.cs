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
    /// Static factory class used to get instances of a specified IProfileProvider
    /// </summary>
    public sealed class ProfileFactory
    {
        private ProfileFactory()
        {
        }

        /// <summary>
        /// Returns the default IProfileProvider instance. 
        /// Guaranteed to return an intialized IProfileProvider if no exception thrown
        /// </summary>
        /// <returns>Default Profile provider instance.</returns>
        /// <exception cref="ConfigurationException">Unable to create default IProfileProvider</exception>
        public static IProfileProvider GetProfileProvider()
        {
            ProfileProviderFactory factory = new ProfileProviderFactory(ConfigurationManager.GetCurrentContext());
                return factory.GetProfileProvider();
            }

        /// <summary>
        /// Returns the named IProfileProvider instance. Guaranteed to return an initialized IProfileProvider if no exception thrown.
        /// </summary>
        /// <param name="profileProviderName">Name defined in configuration for the Profile provider to instantiate</param>
        /// <returns>Named Profile provider instance</returns>
        /// <exception cref="ArgumentNullException">providerName is null</exception>
        /// <exception cref="ArgumentException">providerName is empty</exception>
        /// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
        /// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
        public static IProfileProvider GetProfileProvider(string profileProviderName)
        {
            ProfileProviderFactory factory = new ProfileProviderFactory(ConfigurationManager.GetCurrentContext());
            return factory.GetProfileProvider(profileProviderName);
            }
        }
    }
