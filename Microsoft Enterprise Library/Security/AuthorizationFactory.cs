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
	/// Static factory class used to get instances of a specified IAuthorizationProvider
	/// </summary>
	public sealed class AuthorizationFactory
	{
		private AuthorizationFactory()
		{
		}

		/// <summary>
		/// Returns the default IAuthorizationProvider instance. 
		/// Guaranteed to return an intialized IAuthorizationProvider if no exception thrown
		/// </summary>
		/// <returns>Default Authorization provider instance.</returns>
		/// <exception cref="ConfigurationException">Unable to create default IAuthorizationProvider</exception>
		public static IAuthorizationProvider GetAuthorizationProvider()
		{
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(ConfigurationManager.GetCurrentContext());
			return factory.GetAuthorizationProvider();
		}

		/// <summary>
		/// Returns the named IAuthorizationProvider instance. Guaranteed to return an initialized IAuthorizationProvider if no exception thrown.
		/// </summary>
		/// <param name="authorizationProviderName">Name defined in configuration for the Authorization provider to instantiate</param>
		/// <returns>Named Authorization provider instance</returns>
		/// <exception cref="ArgumentNullException">providerName is null</exception>
		/// <exception cref="ArgumentException">providerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static IAuthorizationProvider GetAuthorizationProvider(string authorizationProviderName)
		{
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(ConfigurationManager.GetCurrentContext());
			return factory.GetAuthorizationProvider(authorizationProviderName);
		}
	}
}