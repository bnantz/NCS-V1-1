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

#if  LONG_RUNNING_TESTS

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests 
{
    public class Db2TestConfigurationContext  : ConfigurationContext
    {
        private static ConfigurationDictionary dictionary;

		public Db2TestConfigurationContext()
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
            settings.DatabaseTypes.Add( new DatabaseTypeData("DB2", typeof(DB2Database).AssemblyQualifiedName));
            
            ConnectionStringData data = new ConnectionStringData("DB2Test");
            data.Parameters.Add(new ParameterData("server","entlib01"));
            data.Parameters.Add(new ParameterData("database","sample"));
            data.Parameters.Add(new ParameterData("user id","administrator"));
            data.Parameters.Add(new ParameterData("password","Pag$1Lab"));
            settings.ConnectionStrings.Add(data);
            
            settings.Instances.Add( new InstanceData("DB2Test", "DB2", "DB2Test"));
            return settings;
        }
    }
}
#endif