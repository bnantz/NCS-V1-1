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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// <para>Represents a view for navigating the <see cref="ExceptionHandlingSettings"/> configuration data.</para>
    /// </summary>
    public class ExceptionHandlingConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionHandlingConfigurationView"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public ExceptionHandlingConfigurationView(ConfigurationContext configurationContext) : base(configurationContext)
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="ExceptionHandlingSettings"/> configuration data.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="ExceptionHandlingSettings"/> configuration data.</para>
        /// </returns>
        public virtual ExceptionHandlingSettings GetExceptionHandlingSettings()
        {
            return (ExceptionHandlingSettings)ConfigurationContext.GetConfiguration(ExceptionHandlingSettings.SectionName);
        }

        /// <summary>
        /// <para>Gets the <see cref="ExceptionPolicyData"/> from configuration by name.</para>
        /// </summary>
        /// <param name="policyName">
        /// <para>The name of the policy in configuration.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="ExceptionPolicyData"/> object.</para>
        /// </returns>
        public virtual ExceptionPolicyData GetExceptionPolicyData(string policyName)
        {
            ValidatePolicyName(policyName);

            ExceptionHandlingSettings settings = GetExceptionHandlingSettings();
            ExceptionPolicyData data = settings.ExceptionPolicies[policyName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionSimpleProviderNotFound(policyName));
            }
            return data;
        }

        
        /// <summary>
        /// <para>Gets the <see cref="ExceptionHandlerData"/> from configuration for the policy and specific exception type.</para>
        /// </summary>
        /// <param name="policyName">
        /// <para>The name of the <see cref="ExceptionPolicy"/> for the data.</para>
        /// </param>
        /// <param name="exceptionTypeName">
        /// <para>The <see cref="Exception"/> type that will be handled.</para>
        /// </param>
        /// <param name="handlerName"><para>The name of the handler to retrieve from configuration.</para></param>
        /// <returns>
        /// <para>An <see cref="ExceptionHandlerData"/> object.</para>
        /// </returns>
        public virtual ExceptionHandlerData GetExceptionHandlerData(string policyName, string exceptionTypeName, string handlerName)
        {
            ValidatePolicyName(policyName);
            ValidateExceptionTypeName(exceptionTypeName);
            ValidateHandlerName(handlerName);

            ExceptionHandlingSettings settings = (ExceptionHandlingSettings)ConfigurationContext.GetConfiguration(ExceptionHandlingSettings.SectionName);
            return GetExceptionHandlerData(settings, policyName, exceptionTypeName, handlerName);
        }

        /// <summary>
        /// <para>Gets the collection of <see cref="ExceptionHandlerData"/> objects for a policy based on an excetion type.</para>
        /// </summary>
        /// <param name="policyName">
        /// <para>The name of the <see cref="ExceptionPolicy"/> for the data.</para>
        /// </param>
        /// <param name="exceptionTypeName">
        /// <para>The <see cref="Exception"/> type that will be handled.</para>
        /// </param>
        /// <returns><para>An <see cref="ExceptionHandlerDataCollection"/> object.</para></returns>
        public virtual ExceptionHandlerDataCollection GetExceptionHandlerDataCollection(string policyName, string exceptionTypeName)
        {
            ValidatePolicyName(policyName);
            ValidateExceptionTypeName(exceptionTypeName);

            ExceptionHandlingSettings settings = GetExceptionHandlingSettings();
            return GetExceptionHandlerDataCollection(settings, policyName, exceptionTypeName);
        }

        private ExceptionHandlerData GetExceptionHandlerData(ExceptionHandlingSettings settings, string policyName, string exceptionTypeName, string handlerName)
        {
            ExceptionHandlerDataCollection exceptionHandlerDataCollection = GetExceptionHandlerDataCollection(settings, policyName, exceptionTypeName);
            ExceptionHandlerData data = exceptionHandlerDataCollection[handlerName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionExceptionHanlderNotFoun(handlerName, exceptionTypeName, policyName));
            }

            return data;
        }

        private static ExceptionHandlerDataCollection GetExceptionHandlerDataCollection(ExceptionHandlingSettings settings, string policyName, string exceptionTypeName)
        {
            ExceptionTypeData exceptionTypeData = GetExceptionTypeData(settings, policyName, exceptionTypeName);
            return exceptionTypeData.ExceptionHandlers;
        }

        private static ExceptionTypeData GetExceptionTypeData(ExceptionHandlingSettings settings, string policyName, string exceptionTypeName)
        {
            ExceptionTypeDataCollection exceptionTypeDataCollection = GetExceptionTypeDataCollection(settings, policyName);
            ExceptionTypeData exceptionTypeData = exceptionTypeDataCollection[exceptionTypeName];
            if (exceptionTypeData == null)
            {
                throw new ConfigurationException(SR.ExceptionExceptionTypeNotFound(exceptionTypeName, policyName));
            }

            return exceptionTypeData;
        }

        private static ExceptionTypeDataCollection GetExceptionTypeDataCollection(ExceptionHandlingSettings settings, string policyName)
        {
            ExceptionPolicyData exceptionPolicyData = GetExceptionPolicyData(settings, policyName);
            return exceptionPolicyData.ExceptionTypes;
        }

        private static ExceptionPolicyData GetExceptionPolicyData(ExceptionHandlingSettings settings, string policyName)
        {
            ExceptionPolicyDataCollection exceptionPolicyDataCollection = settings.ExceptionPolicies;
            ExceptionPolicyData exceptionPolicyData = exceptionPolicyDataCollection[policyName];
            if (exceptionPolicyData == null)
            {
                throw new ConfigurationException(SR.ExceptionSimpleProviderNotFound(policyName));
            }

            return exceptionPolicyData;
        }

        private static void ValidatePolicyName(string policyName)
        {
            ArgumentValidation.CheckForNullReference(policyName, "policyName");
            ArgumentValidation.CheckForEmptyString(policyName, "policyName");
        }

        private static void ValidateHandlerName(string handlerName)
        {
            ArgumentValidation.CheckForNullReference(handlerName, "policyName");
            ArgumentValidation.CheckForEmptyString(handlerName, "policyName");
        }

        private static void ValidateExceptionTypeName(string exceptionTypeName)
        {
            ArgumentValidation.CheckForNullReference(exceptionTypeName, "exceptionTypeName");
            ArgumentValidation.CheckForEmptyString(exceptionTypeName, "exceptionTypeName");
        }
    }
}