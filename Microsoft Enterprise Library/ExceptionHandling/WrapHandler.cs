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
    /// Wraps the current exception in the handling chain with a new exception of a specified type.
    /// </summary>
    public class WrapHandler : ExceptionHandler
    {
        private WrapHandlerData wrapHandlerData;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="WrapHandler"/> object.</para>
        /// </summary>
        public WrapHandler() : base()
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
            ArgumentValidation.CheckExpectedType(exceptionHandlerData, typeof(WrapHandlerData));

            wrapHandlerData = (WrapHandlerData)exceptionHandlerData;
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of exception to wrap the original exception with.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> of exception to wrap the original exception with.</para>
        /// </value>
        public Type WrapExceptionType
        {
            [ReflectionPermission(SecurityAction.Demand, MemberAccess=true)]
            get { return Type.GetType(wrapHandlerData.WrapExceptionTypeName); }
        }

        /// <summary>
        /// <para>Gets the message of the wrapped exception.</para>
        /// </summary>
        /// <value>
        /// <para>The message of the wrapped exception.</para>
        /// </value>
        public string WrapExceptionMessage
        {
            get { return wrapHandlerData.ExceptionMessage; }
        }

        /// <summary>
        /// <para>Wraps the <see cref="Exception"/> with the configuration exception type.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>
        /// <param name="policyName"><para>The name of the <see cref="ExceptionPolicy"/>.</para></param>
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next handler in the chain.</para></returns>
        [ReflectionPermission(SecurityAction.Demand, MemberAccess=true)]
        public override Exception HandleException(Exception exception, string policyName, Guid handlingInstanceId)
        {
            return WrapException(
                exception,
                WrapExceptionType,
                ExceptionUtility.FormatExceptionMessage(WrapExceptionMessage, handlingInstanceId));
        }

        /// <summary>
        /// Wraps an exception with a new exception of a specified type.
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        /// <param name="wrapExceptionType">The type of exception to wrap the original exception with.</param>
        /// <param name="wrapExceptionMessage">The message for the new exception.</param>
        /// <returns>An exception with the innerException set to the original exception.  Returns null if unable to wrap the exception.</returns>
        private Exception WrapException(Exception originalException, Type wrapExceptionType, string wrapExceptionMessage)
        {
            if (wrapExceptionMessage == null)
            {
                wrapExceptionMessage = String.Empty;
            }

            try
            {
                object[] extraParameters = new object[2];
                extraParameters[0] = wrapExceptionMessage;
                extraParameters[1] = originalException;
                return (Exception)Activator.CreateInstance(wrapExceptionType, extraParameters);
            }
            catch (Exception ex)
            {
                throw new ExceptionHandlingException(SR.ExceptionUnableToWrapException(wrapExceptionType.Name), ex);
            }
        }
    }
}