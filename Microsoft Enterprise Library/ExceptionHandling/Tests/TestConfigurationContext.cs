//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	public class TestConfigurationContext : ConfigurationContext
	{
		public TestConfigurationContext()
			: base( GenerateConfigurationDictionary())
		{
		}

		private static ConfigurationDictionary GenerateConfigurationDictionary()
		{
		    ConfigurationDictionary dictionary = new ConfigurationDictionary();
		    dictionary = new ConfigurationDictionary();
		    dictionary.Add(ConfigurationSettings.SectionName, GenerateConfigurationSettings());
		    dictionary.Add(ExceptionHandlingSettings.SectionName, GenerateExceptionHandlingSettings());
		    return dictionary;
		}

	    private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			settings.ConfigurationSections.Add(new ConfigurationSectionData(ExceptionHandlingSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.ExceptionHandling.config"), new XmlSerializerTransformerData("exceptionHandlingConfiguration")));
			return settings;
		}

		private static ExceptionHandlingSettings GenerateExceptionHandlingSettings()
		{
			ExceptionHandlingSettings settings = new ExceptionHandlingSettings();

			ExceptionPolicyData policyData = new ExceptionPolicyData("Default Policy");
			ExceptionTypeData exceptionType = new ExceptionTypeData("ArgumentNullException", typeof(ArgumentNullException).AssemblyQualifiedName, PostHandlingAction.None );
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockExceptionHandler", typeof(MockExceptionHandler).AssemblyQualifiedName ));
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockExceptionHandler1", typeof(MockExceptionHandler).AssemblyQualifiedName ));
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockExceptionHandler2", typeof(MockExceptionHandler).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Error Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockBadExceptionHandler", typeof(MockBadExceptionHandler).AssemblyQualifiedName ));
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockExceptionHandler", typeof(MockExceptionHandler).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Rethrow Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockExceptionHandler", typeof(MockExceptionHandler).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Null Replace Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockNullReplaceExceptionHandler", typeof(MockNullReplaceExceptionHandler).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Fake Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new CustomHandlerData("MockNullReplaceExceptionHandler", "GARBAGE" ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Fake Rethrow Type Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new ReplaceHandlerData("ReplaceHandler", "", "GARBAGE" ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Wrap Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new WrapHandlerData("WrapHandler", "Test Message", typeof(ApplicationException).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

			policyData = new ExceptionPolicyData("Replace Policy");
			exceptionType = new ExceptionTypeData("Exception", typeof(Exception).AssemblyQualifiedName, PostHandlingAction.ThrowNewException );
			exceptionType.ExceptionHandlers.Add( new ReplaceHandlerData("ReplaceHandler", "Test Message", typeof(ApplicationException).AssemblyQualifiedName ));
			policyData.ExceptionTypes.Add(exceptionType);
			settings.ExceptionPolicies.Add(policyData);

            policyData = new ExceptionPolicyData("Policy With Bad Exception Type");
            exceptionType = new ExceptionTypeData("BadException", "thisIsABadExceptionType", PostHandlingAction.ThrowNewException );
            exceptionType.ExceptionHandlers.Add( new ReplaceHandlerData("ReplaceHandler", "Test Message", typeof(ApplicationException).AssemblyQualifiedName ));
            policyData.ExceptionTypes.Add(exceptionType);
            settings.ExceptionPolicies.Add(policyData);

			return settings;
		}
	}
}
#endif