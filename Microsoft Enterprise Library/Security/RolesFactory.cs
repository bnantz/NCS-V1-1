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
	/// Static factory class used to get instances of a specified IRolesProvider
	/// </summary>
	public sealed class RolesFactory
	{
		private RolesFactory()
		{
		}

		/// <summary>
		/// Returns the default IRolesProvider instance. 
		/// Guaranteed to return an intialized IRolesProvider if no exception thrown
		/// </summary>
		/// <returns>Default Roles provider instance.</returns>
		/// <exception cref="ConfigurationException">Unable to create default IRolesProvider</exception>
		public static IRolesProvider GetRolesProvider()
		{
			RolesProviderFactory factory = new RolesProviderFactory(ConfigurationManager.GetCurrentContext());
			return factory.GetRolesProvider();
		}

		/// <summary>
		/// Returns the named IRolesProvider instance. Guaranteed to return an initialized IRolesProvider if no exception thrown.
		/// </summary>
		/// <param name="rolesProviderName">Name defined in configuration for the Roles provider to instantiate</param>
		/// <returns>Named Roles provider instance</returns>
		/// <exception cref="ArgumentNullException">providerName is null</exception>
		/// <exception cref="ArgumentException">providerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static IRolesProvider GetRolesProvider(string rolesProviderName)
		{
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
			return GetRolesProvider(rolesProviderName, context);
		}

		/// <summary>
		/// Returns the named IRolesProvider instance. Guaranteed to return an initialized IRolesProvider if no exception thrown.
		/// </summary>
		/// <param name="rolesProviderName">Name defined in configuration for the Roles provider to instantiate</param>
		/// <param name="context">The configuration context to use.</param>
		/// <returns>Named Roles provider instance</returns>
		/// <exception cref="ArgumentNullException">providerName is null</exception>
		/// <exception cref="ArgumentException">providerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static IRolesProvider GetRolesProvider(string rolesProviderName, ConfigurationContext context)
		{
			RolesProviderFactory factory = new RolesProviderFactory(context);
			return factory.GetRolesProvider(rolesProviderName);
		}
	}
}