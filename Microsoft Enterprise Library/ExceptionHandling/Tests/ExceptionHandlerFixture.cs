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

#if    UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestFixture]
	public class ExceptionHandlerFixture
	{
		public const string ExceptionPolicyName = "Default Policy";

		[SetUp]
		public void Setup()
		{
			MockExceptionHandler.Clear();
		}

		[Test]
		public void HandleTest()
		{
			ExceptionPolicy.HandleException(new MockException(), ExceptionPolicyName, new TestConfigurationContext());
			Assert.AreEqual(3, MockExceptionHandler.handleExceptionCount);
			ExceptionPolicy.HandleException(new MockException(), ExceptionPolicyName, new TestConfigurationContext());
			Assert.AreEqual(6, MockExceptionHandler.handleExceptionCount);
		}

        [Test]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void ThrowsExceptionIfBadExceptionTypeFoundInConfigurationDuringInitialization()
        {
            ExceptionPolicy.HandleException(new Exception(), "Policy With Bad Exception Type", new TestConfigurationContext());
        }

		[Test]
		[ExpectedException(typeof (ExceptionHandlingException))]
		public void HandlerFailTest()
		{
			ExceptionPolicy.HandleException(new MockException(), "Error Policy", new TestConfigurationContext());
		}

		[Test]
		public void RethrowTest()
		{
			try
			{
				ExceptionPolicy.HandleException(new MockException(), "Rethrow Policy", new TestConfigurationContext());
			}
			catch (MockException)
			{
				return;
			}
			Assert.Fail("Expected an ArgumentException to be thrown");
		}

		[Test]
		[ExpectedException(typeof (ExceptionHandlingException))]
		public void NullExceptionTest()
		{
			ExceptionPolicy.HandleException(new MockException(), "Null Replace Policy", new TestConfigurationContext());
		}

		[Test]
		public void FormatExceptionMessageTest()
		{
			string message = "Test Message: {handlingInstanceID}";
			Guid handlingInstanceID = Guid.NewGuid();
			message = MockExceptionHandler.FormatExceptionMessage(message, handlingInstanceID);

			Assert.AreEqual("Test Message: " + handlingInstanceID.ToString(), message);
		}

		public class InnerExceptionHandler : ExceptionHandler
		{
			public override Exception HandleException(Exception exception, string policyName, Guid handlingInstanceId)
			{
				return new ArgumentException();
			}

			public override void Initialize(ConfigurationView configurationView)
			{
			}
		}
	}
}

#endif