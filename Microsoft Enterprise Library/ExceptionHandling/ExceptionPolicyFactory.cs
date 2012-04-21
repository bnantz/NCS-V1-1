//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
	/// Represents a factory for creating instances of the <see cref="ExceptionPolicy"/> class.
	/// </summary>
    internal sealed class ExceptionPolicyFactory : ConfigurationFactory
	{
		private Exception currentException = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionPolicyFactory"/>
		/// class from the specified <see cref="ConfigurationContext"/> class.
		/// </summary>
		/// <param name="context">A <see cref="ConfigurationContext"/>.</param>
        public ExceptionPolicyFactory(ConfigurationContext context) : base(SR.PolicyFactoryName, context)
		{
		}

		/// <summary>
        /// Creates an <see cref="ExceptionPolicy"/> object from the configuration data associated with the specified name.
		/// </summary>
		/// <param name="policyName">
		/// <para>The name of the policy to create.</para>
		/// </param>
        /// <param name="exception">
        /// <para>The <see cref="Exception"/> used when creating the policy. It will be wrapped in a ? if the policy is not found.</para>
        /// </param>
		/// <returns>An <see cref="ExceptionPolicy"/>.</returns>
        public ExceptionPolicy CreateExceptionPolicy(string policyName, Exception exception)
		{
            ArgumentValidation.CheckForNullReference(exception, "exception");

			currentException = exception;
            return (ExceptionPolicy)CreateInstance(policyName);
		}

        protected override Type GetConfigurationType(string policyName)
		{
        	ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = new ExceptionHandlingConfigurationView(ConfigurationContext);
            ExceptionPolicyData policy = exceptionHandlingConfigurationView.GetExceptionPolicyData(policyName);
            return GetType(policy.TypeName);
		}

		/// <summary>
		/// Fires an instrumentation event.
		/// </summary>
		/// <param name="name">The name of the exception policy.</param>
		/// <param name="e">The caught exception.</param>
		protected override void PublishFailureEvent(string name, Exception e)
		{
            string message = CreateProviderNotFoundExceptionMessage(currentException, name);
            ExceptionHandlingConfigFailureEvent.Fire(message);
			ExceptionUtility.LogHandlingException(name, e, null, currentException);
		}

        private static string CreateProviderNotFoundExceptionMessage(Exception currentException, string policyName)
		{
            if (currentException == null)
			{
                return SR.ExceptionSimpleProviderNotFound(policyName);
			}
			else
			{
                return SR.ExceptionExtendedProviderNotFound(policyName, currentException.Message);
			}
		}
	}
}
