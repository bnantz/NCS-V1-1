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
using System.Configuration;
using System.IO;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class ExceptionPolicyFixture
    {
        private const string ExceptionPolicyName = "Default Policy";

        [Test]
        public void HandleExceptionNoRethrowActionTest()
        {
            bool ret = ExceptionPolicy.HandleException(new ArgumentNullException("Test Message", "Test Param"), ExceptionPolicyName, new TestConfigurationContext());
            Assert.IsFalse(ret);
        }

        [Test]
        public void HandleExceptionRethrowTest()
        {
            bool ret = ExceptionPolicy.HandleException(new UnauthorizedAccessException(), ExceptionPolicyName, new TestConfigurationContext());
            Assert.IsTrue(ret);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionNullTest()
        {
            ExceptionPolicy.HandleException(null, ExceptionPolicyName, new TestConfigurationContext());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionNullPolicyNameTest()
        {
            ExceptionPolicy.HandleException(new Exception("foo"), null, new TestConfigurationContext());
        }

        [Test]
        public void HandleExceptionBadPolicyTest()
        {
            const string exceptionMessage = "The file is missing";
            const string policyName = "b5ywougvlathbysvg894tadv";

            try
            {
                ExceptionPolicy.HandleException(new FileNotFoundException(exceptionMessage, "thefile"), policyName, new TestConfigurationContext());
            }
            catch (ExceptionHandlingException ex)
            {
                string message = ex.Message;
                Assert.IsTrue(message.IndexOf(policyName) > -1, String.Format("Message recieved was '{0}'", message));
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionEmptyPolicyTest()
        {
            ExceptionPolicy.HandleException(new Exception(), String.Empty, new TestConfigurationContext());
        }

        [Test]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void FakeHandlerTest()
        {
            try
            {
                ExceptionPolicy.HandleException(new MockException(), "Fake Policy", new TestConfigurationContext());
                Assert.Fail();
            }
            catch (ConfigurationException e)
            {
                Assert.IsNotNull(e.InnerException);
                Assert.AreEqual(typeof(ExceptionHandlingException), e.InnerException.GetType());
                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void FakeRethrowTest()
        {
            ExceptionPolicy.HandleException(new MockException(), "Fake Rethrow Type Policy", new TestConfigurationContext());
        }

        [Test]
        public void WrapHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception wrappedException = null;

            try
            {
                ExceptionPolicy.HandleException(originalException, "Wrap Policy", new TestConfigurationContext());
            }
            catch (Exception ex)
            {
                wrappedException = ex;
            }

            Assert.IsNotNull(wrappedException);
            Assert.AreEqual("Test Message", wrappedException.Message);
            Assert.AreEqual(typeof(ApplicationException), wrappedException.GetType());
            Assert.IsNotNull(wrappedException.InnerException);
        }

        [Test]
        public void ReplaceHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception replacedException = null;

            try
            {
                ExceptionPolicy.HandleException(originalException, "Replace Policy", new TestConfigurationContext());
            }
            catch (Exception ex)
            {
                replacedException = ex;
            }

            Assert.IsNotNull(replacedException);
            Assert.AreEqual("Test Message", replacedException.Message);
            Assert.AreEqual(typeof(ApplicationException), replacedException.GetType());
            Assert.IsNull(replacedException.InnerException);
        }
    }
}

#endif