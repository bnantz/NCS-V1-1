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
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Replaces the exception in the chain of handlers with a cleansed exception.
    /// </summary>
    public class ReplaceHandler : ExceptionHandler
    {
        private ReplaceHandlerData replaceHandlerData;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ReplaceHandler"/> class.</para>
        /// </summary>
        public ReplaceHandler() : base()
        {
        }

        /// <summary>
        /// <para>Initializes the provider with a <see cref="ExceptionHandlingConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="ExceptionHandlingConfigurationView"/> object.</para>
        /// </param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(ExceptionHandlingConfigurationView));

            ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = (ExceptionHandlingConfigurationView)configurationView;
            ExceptionHandlerData exceptionHandlerData = exceptionHandlingConfigurationView.GetExceptionHandlerData(CurrentPolicyName, CurrentExceptionTypeName, ConfigurationName);
            ArgumentValidation.CheckExpectedType(exceptionHandlerData, typeof(ReplaceHandlerData));

            replaceHandlerData = (ReplaceHandlerData)exceptionHandlerData;
        }

        /// <summary>
        /// The type of exception to replace.
        /// </summary>
        public Type ReplaceExceptionType
        {
            [ReflectionPermission(SecurityAction.Demand, MemberAccess=true)]
            get { return Type.GetType(replaceHandlerData.ReplaceExceptionTypeName); }
        }

        /// <summary>
        /// Gets the message for the new exception.
        /// </summary>
        public string ExceptionMessage
        {
            get { return replaceHandlerData.ExceptionMessage; }
        }

        /// <summary>
        /// Replaces the exception with the configured type for the specified policy.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="policyName">The name of the <see cref="ExceptionPolicy"/>.</param>
        /// <param name="handlingInstanceID">The unique ID attached to the handling chain for this handling instance.</param>
        /// <returns>Modified exception to pass to the next handler in the chain.</returns>
        public override Exception HandleException(Exception exception, string policyName, Guid handlingInstanceID)
        {
            return ReplaceException(
                exception,
                ReplaceExceptionType,
                ExceptionUtility.FormatExceptionMessage(ExceptionMessage, handlingInstanceID));
        }

        /// <summary>
        /// Replaces an exception with a new exception of a specified type.
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        /// <param name="replaceExceptionType">The type of exception to replace.</param>
        /// <param name="replaceExceptionMessage">The message for the new exception.</param>
        /// <returns>The replaced or "cleansed" exception.  Returns null if unable to replace the exception.</returns>
        private static Exception ReplaceException(Exception originalException, Type replaceExceptionType, string replaceExceptionMessage)
        {
            if (replaceExceptionMessage == null)
            {
                replaceExceptionMessage = String.Empty;
            }

            try
            {
                object[] extraParameters = new object[] {replaceExceptionMessage};

                return (Exception)Activator.CreateInstance(replaceExceptionType, extraParameters);
            }
            catch (Exception ex)
            {
                throw new ExceptionHandlingException(SR.ExceptionUnableToReplaceException(replaceExceptionType.Name), ex);
            }
        }
    }
}