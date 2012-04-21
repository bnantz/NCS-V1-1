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
	/// Static factory class used to get instances of a specified ISecurityCacheProvider
	/// </summary>
	public sealed class SecurityCacheFactory
	{
		private SecurityCacheFactory()
		{
		}

		/// <summary>
		/// Returns the default ISecurityCacheProvider instance. 
		/// Guaranteed to return an intialized ISecurityCacheProvider if no exception thrown
		/// </summary>
		/// <returns>Default SecurityCache provider instance.</returns>
		/// <exception cref="ConfigurationException">Unable to create default ISecurityCacheProvider</exception>
		public static ISecurityCacheProvider GetSecurityCacheProvider()
		{
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return GetSecurityCacheProvider(context);
		}

		internal static ISecurityCacheProvider GetSecurityCacheProvider(ConfigurationContext context)
		{
			SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(context);
			return factory.GetSecurityCacheProvider();
		}

		/// <summary>
		/// Returns the named ISecurityCacheProvider instance. Guaranteed to return an initialized ISecurityCacheProvider if no exception thrown.
		/// </summary>
		/// <param name="securityCacheProviderName">Name defined in configuration for the SecurityCache provider to instantiate</param>
		/// <returns>Named SecurityCache provider instance</returns>
		/// <exception cref="ArgumentNullException">providerName is null</exception>
		/// <exception cref="ArgumentException">providerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static ISecurityCacheProvider GetSecurityCacheProvider(string securityCacheProviderName)
		{
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return GetSecurityCacheProvider(securityCacheProviderName, context);
		}

		internal static ISecurityCacheProvider GetSecurityCacheProvider(string securityCacheProviderName, ConfigurationContext context)
		{
			SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(context);
			return factory.GetSecurityCacheProvider(securityCacheProviderName);
		}
	}


}