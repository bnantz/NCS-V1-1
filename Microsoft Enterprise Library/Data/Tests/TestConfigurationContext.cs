//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
	public class TestConfigurationContext : ConfigurationContext
	{
		private static ConfigurationDictionary dictionary;

		public TestConfigurationContext()
			: base( GenerateConfigurationDictionary())
		{
		}

		private static ConfigurationDictionary GenerateConfigurationDictionary()
		{
			if( dictionary == null )
			{
				dictionary = new ConfigurationDictionary();
				dictionary.Add(ConfigurationSettings.SectionName, GenerateConfigurationSettings());
				dictionary.Add(DatabaseSettings.SectionName, GenerateDataSettings());
			}
			return dictionary;
		}

		private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			settings.ConfigurationSections.Add(new ConfigurationSectionData(DatabaseSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.Data.config"), new XmlSerializerTransformerData("DataBuilder")));
			return settings;
		}

		private static DatabaseSettings GenerateDataSettings()
		{
			DatabaseSettings settings = new DatabaseSettings();

			settings.DefaultInstance = "Service_Dflt";

			settings.DatabaseTypes.Add( new DatabaseTypeData("SqlServer", typeof(SqlDatabase).AssemblyQualifiedName));
			settings.DatabaseTypes.Add( new DatabaseTypeData("Oracle", typeof(OracleDatabase).AssemblyQualifiedName));

			ConnectionStringData data = new ConnectionStringData("OracleTest");
			data.Parameters.Add(new ParameterData("server","entlib"));
			data.Parameters.Add(new ParameterData("user id","testuser"));
			data.Parameters.Add(new ParameterData("password","testuser"));
			settings.ConnectionStrings.Add(data);

			data = new ConnectionStringData("NewDatabase");
			data.Parameters.Add(new ParameterData("server","localhost"));
			data.Parameters.Add(new ParameterData("database","Northwind"));
			data.Parameters.Add(new ParameterData("Integrated Security","true"));
			settings.ConnectionStrings.Add(data);

			data = new ConnectionStringData("DbWithSqlServerAuthn");
			data.Parameters.Add(new ParameterData("server","localhost"));
			data.Parameters.Add(new ParameterData("database","Northwind"));
			data.Parameters.Add(new ParameterData("uid","sa"));
			data.Parameters.Add(new ParameterData("pwd","mypassword"));
			settings.ConnectionStrings.Add(data);

			data = new ConnectionStringData("Northwind");
			data.Parameters.Add(new ParameterData("server","localhost"));
			data.Parameters.Add(new ParameterData("database","Northwind"));
			data.Parameters.Add(new ParameterData("Integrated Security","true"));
			settings.ConnectionStrings.Add(data);

			data = new ConnectionStringData("EntLibQuickStarts");
			data.Parameters.Add(new ParameterData("server","localhost"));
			data.Parameters.Add(new ParameterData("database","EntLibQuickStarts"));
			data.Parameters.Add(new ParameterData("Integrated Security","true"));
			settings.ConnectionStrings.Add(data);

			data = new ConnectionStringData("NorthwindPersistFalse");
			data.Parameters.Add(new ParameterData("server","localhost"));
			data.Parameters.Add(new ParameterData("database","Northwind"));
			data.Parameters.Add(new ParameterData("uid","entlib"));
			data.Parameters.Add(new ParameterData("pwd","sunrain"));
			data.Parameters.Add(new ParameterData("Persist Security Info", "false"));
			settings.ConnectionStrings.Add(data);

			settings.Instances.Add( new InstanceData("NewDatabase", "SqlServer", "NewDatabase"));
			settings.Instances.Add( new InstanceData("Service_Dflt", "SqlServer", "Northwind"));
			settings.Instances.Add( new InstanceData("EntLibQS", "SqlServer", "EntLibQuickStarts"));
			settings.Instances.Add( new InstanceData("DbWithSqlServerAuthn", "SqlServer", "DbWithSqlServerAuthn"));
			settings.Instances.Add( new InstanceData("NorthwindPersistFalse", "SqlServer", "NorthwindPersistFalse"));
			settings.Instances.Add( new InstanceData("OracleTest", "Oracle", "OracleTest"));

			return settings;
		}
	}
}
#endif
