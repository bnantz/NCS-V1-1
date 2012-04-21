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

#if   UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Tests
{
	[TestFixture]
	public class ExceptionHandlingSettingsFixture
	{
		private const string BadString = "SomeBunkString984t487y";
		private const string ExceptionPolicyName = "Default Policy";
		private const string ExceptionTypeName = "ArgumentNullException";
		private const string ExceptionHandlerName = "MockExceptionHandler";

		private ExceptionPolicyData DefaultPolicy
		{
			get
			{
				TestConfigurationContext context = new TestConfigurationContext();
				ExceptionHandlingSettings settings = (ExceptionHandlingSettings) context.GetConfiguration(ExceptionHandlingSettings.SectionName);
				return settings.ExceptionPolicies[ExceptionPolicyName];
			}
		}

		[Test]
		public void GetPolicyByNamePassTest()
		{
			ExceptionPolicyData testPolicy = DefaultPolicy;
			Assert.IsNotNull(testPolicy);
		}

		[Test]
		public void GetPolicyByNameFailTest()
		{
			TestConfigurationContext context = new TestConfigurationContext();
			ExceptionHandlingSettings settings = (ExceptionHandlingSettings) context.GetConfiguration(ExceptionHandlingSettings.SectionName);
			ExceptionPolicyData testPolicy = settings.ExceptionPolicies[BadString];
			Assert.IsNull(testPolicy);
		}

		[Test]
		public void GetTypeByNamePassTest()
		{
			ExceptionTypeData testType = DefaultPolicy.ExceptionTypes[ExceptionTypeName];
			Assert.IsNotNull(testType);
			Assert.AreEqual(PostHandlingAction.None, testType.PostHandlingAction);
		}

		[Test]
		public void GetTypeByNameFailTest()
		{
			ExceptionTypeData testType = DefaultPolicy.ExceptionTypes[BadString];
			Assert.IsNull(testType);
		}

		[Test]
		public void GetHandlerPassTest()
		{
			ExceptionTypeData testType = DefaultPolicy.ExceptionTypes[ExceptionTypeName];
			Assert.IsNotNull(testType);
			Assert.AreEqual(3, testType.ExceptionHandlers.Count);
			Assert.AreEqual(testType.ExceptionHandlers[0].Name, ExceptionHandlerName);
		}

		[Test]
		public void CustomHandlerPropertiesTest()
		{
			CustomHandlerData data = new CustomHandlerData();
			string name = "Test Name";
			string typeName = "Test TypeName";
			NameValueItemCollection attributes = new NameValueItemCollection();
			attributes.Add(new NameValueItem("TEST", "VALUE"));

			data.Name = name;
			data.TypeName = typeName;
			data.Attributes.Add(attributes[0]);

			Assert.AreEqual(name, data.Name);
			Assert.AreEqual(typeName, data.TypeName);
			Assert.AreEqual(attributes["TEST"], data.Attributes["TEST"]);
		}

		[Test]
		public void WrapHandlerPropertiesTest()
		{
			WrapHandlerData data = new WrapHandlerData();
			string exceptionMessage = "test message";
			string wrapExceptionTypeName = "System.Exception, mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

			data.ExceptionMessage = exceptionMessage;
			data.WrapExceptionTypeName = wrapExceptionTypeName;

			Assert.AreEqual(exceptionMessage, data.ExceptionMessage);
			Assert.AreEqual(wrapExceptionTypeName, data.WrapExceptionTypeName);
		}

		[Test]
		public void ReplaceHandlerPropertiesTest()
		{
			ReplaceHandlerData data = new ReplaceHandlerData();
			string exceptionMessage = "test message";
			string replaceExecptionTypeName = "Test Replace TypeName";

			data.ExceptionMessage = exceptionMessage;
			data.ReplaceExceptionTypeName = replaceExecptionTypeName;

			Assert.AreEqual(exceptionMessage, data.ExceptionMessage);
			Assert.AreEqual(replaceExecptionTypeName, data.ReplaceExceptionTypeName);
		}
	}
}

#endif