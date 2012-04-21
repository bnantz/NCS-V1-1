//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
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
				dictionary.Add(LoggingSettings.SectionName, GenerateLoggingSettings());
				dictionary.Add(DistributorSettings.SectionName, GenerateDistributorSettings());
			}
			return dictionary;
		}

		private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			settings.ConfigurationSections.Add(new ConfigurationSectionData(LoggingSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.Logging.config"), new XmlSerializerTransformerData("DataBuilder")));
			settings.ConfigurationSections.Add(new ConfigurationSectionData(DistributorSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.LoggingDistributor.config"), new XmlSerializerTransformerData("DataBuilder")));
			return settings;
		}

		private static LoggingSettings GenerateLoggingSettings()
		{
			LoggingSettings result = new LoggingSettings();

			result.LoggingEnabled = true;
            result.TracingEnabled = true;
            result.MinimumPriority = 5;
            result.DistributionStrategy = "InProc";
            result.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;

			result.DistributionStrategies.Add( new InProcDistributionStrategyData("InProc"));

			return result;
		}

		private static DistributorSettings GenerateDistributorSettings()
		{
			DistributorSettings result = new DistributorSettings();			
			
			result.DefaultCategory="AppError" ;
			result.DefaultFormatter="XmlFormat";

			result.DistributorService = new MsmqDistributorServiceData("Enterprise Library Logging Distributor Service", @".\Private$\entlib", 1000 );

			result.SinkDataCollection.Add( new CustomSinkData("MockSink", typeof(MockLogSink).AssemblyQualifiedName ) );

			result.Formatters.Add( new TextFormatterData("XmlFormat", "<![CDATA[<EntLibLog>{newline}{tab}<message>{message}</message>{newline}{tab}<timestamp>{timestamp}</timestamp>{newline}{tab}<title>{title}</title>{newline}</EntLibLog>]]>"));

			CategoryData data = new CategoryData();
			data.Name = "AppError";
			data.DestinationDataCollection.Add( new DestinationData("MockSink", "MockSink"));
			result.CategoryDataCollection.Add(data);

			return result;
		}
	}
}
#endif
