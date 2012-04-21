//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	/// <summary>
	/// Contains factory methods for creating Database objects
	/// </summary>
	public sealed class DatabaseFactory
	{
		private DatabaseFactory()
		{
		}

		/// <summary>
		/// Method for invoking a default Database object.  Reads default settings
		/// from the ConnectionSettings.config file.
		/// </summary>
		/// <example>
		/// <code>
		/// Database dbSvc = DatabaseFactory.CreateDatabase();
		/// </code>
		/// </example>
		/// <returns>Database</returns>
		/// <exception cref="System.Configuration.ConfigurationException">
		/// <para>A error occured while reading the configuration.</para>
		/// </exception>
		public static Database CreateDatabase()
		{
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(context);
            return factory.CreateDefaultDatabase();
		}

		/// <summary>
		/// Method for invoking a specified Database service object.  Reads service settings
		/// from the ConnectionSettings.config file.
		/// </summary>
		/// <example>
		/// <code>
		/// Database dbSvc = DatabaseFactory.CreateDatabase("SQL_Customers");
		/// </code>
		/// </example>
		/// <param name="instanceName">configuration key for database service</param>
		/// <returns>Database</returns>
		/// <exception cref="System.Configuration.ConfigurationException">
		/// <para><paramref name="instanceName"/> is not defined in configuration.</para>
		/// <para>- or -</para>
		/// <para>An error exists in the configuration.</para>
		/// <para>- or -</para>
		/// <para>A error occured while reading the configuration.</para>        
		/// </exception>
		/// <exception cref="System.Reflection.TargetInvocationException">
		/// <para>The constructor being called throws an exception.</para>
		/// </exception>
		public static Database CreateDatabase(string instanceName)
		{
			ConfigurationContext context = ConfigurationManager.GetCurrentContext();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(context);
            return factory.CreateDatabase(instanceName);
		}
	}

}