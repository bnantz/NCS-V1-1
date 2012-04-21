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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents a factory for creating instances of classes which implement <see cref="IExceptionHandler"/>.
    /// </summary>
    public class ExceptionHandlerFactory : ProviderFactory
    {
        private string policyName;
        private string exceptionTypeName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionHandlerFactory"/> class.</para>
        /// </summary>
        public ExceptionHandlerFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="ExceptionHandlerFactory"/> class with the specified <see cref="ConfigurationContext"/>.
        /// </para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>Configuration context to use when creating factory</para>
        /// </param>
        public ExceptionHandlerFactory(ConfigurationContext configurationContext) : base(SR.HandlerFactoryName, configurationContext, typeof(IExceptionHandler))
        {
            this.policyName = string.Empty;
            this.exceptionTypeName = string.Empty;
        }

        /// <summary>
        /// Creates all configured <see cref="IExceptionHandler"/> objects.
        /// </summary>
        /// <param name="policyName"><para>The policy name to create the handler.</para></param>
        /// <param name="exceptionTypeName"><para>The type of exception requested to be handled.</para></param>
        /// <returns>An array of <see cref="IExceptionHandler"/> objects.</returns>
        public IExceptionHandler[] CreateExceptionHandlers(string policyName, string exceptionTypeName)
        {
            ValidatePolicyAndExceptionType(policyName, exceptionTypeName);

            this.policyName = policyName;
            this.exceptionTypeName = exceptionTypeName;
        	ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = (ExceptionHandlingConfigurationView)CreateConfigurationView();
            ExceptionHandlerDataCollection handlers = exceptionHandlingConfigurationView.GetExceptionHandlerDataCollection(policyName, exceptionTypeName);
            IExceptionHandler[] exceptionHandlers = new IExceptionHandler[handlers.Count];
            int index = 0;
            foreach (ExceptionHandlerData handler in handlers)
            {
                exceptionHandlers[index++] = CreateExceptionHandler(policyName, exceptionTypeName, handler.Name);
            }
            return exceptionHandlers;
        }

        /// <summary>
        /// <para>Create an <see cref="IExceptionHandler"/> for a policy and specific exception type.</para>
        /// </summary>
        /// <param name="policyName"><para>The policy name to create the handler.</para></param>
        /// <param name="exceptionTypeName"><para>The type of exception requested to be handled.</para></param>
        /// <param name="handlerName"><para>The name of the handler to create.</para></param>
        /// <returns>An <see cref="IExceptionHandler"/> object.</returns>
        public IExceptionHandler CreateExceptionHandler(string policyName, string exceptionTypeName, string handlerName)
        {
            ValidatePolicyAndExceptionType(policyName, exceptionTypeName);

            this.policyName = policyName;
            this.exceptionTypeName = exceptionTypeName;
            return (IExceptionHandler)base.CreateInstance(handlerName);
        }

        /// <summary>
        /// <para>Publish an instrumentation event that indicates there was an error while attempting to create a provider.</para>
        /// </summary>
        /// <param name="configurationName"><para>The name of the configuration object.</para></param>
        /// <param name="e"><para>The <see cref="Exception"/> to publish.</para></param>
        protected override void PublishFailureEvent(string configurationName, Exception e)
        {
            Exception wrappedException = new ExceptionHandlingException(SR.UnableToLoadExceptionHandlers(configurationName, this.policyName), e);
            ExceptionUtility.LogHandlingException(this.policyName, wrappedException, null, e);
            throw new ExceptionHandlingException(SR.UnableToLoadExceptionHandlers(configurationName, this.policyName));
        }

        /// <summary>
        /// <para>Creates the <see cref="ExceptionHandlingConfigurationView"/> object to navigate the <see cref="ExceptionHandlingSettings"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="ExceptionHandlingConfigurationView"/> object.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new ExceptionHandlingConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of <see cref="IExceptionHandler"/> to create based on the name in configuration.</para>
        /// </summary>
        /// <param name="handlerName">
        /// <para>The name in configuraiton of the <see cref="IExceptionHandler"/> to get the <see cref="Type"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IExceptionHandler"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string handlerName)
        {
            ExceptionHandlerData data = GetExceptionHandlerData(handlerName);
            return GetType(data.TypeName);
        }

        /// <summary>
        /// <para>Initialize the <see cref="IConfigurationProvider"/> by invoking the <see cref="IConfigurationProvider.Initialize"/> method.</para>
        /// </summary>
        /// <param name="providerName">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="provider">
        /// <para>The <see cref="IConfigurationProvider"/> to initialize.</para>
        /// </param>
        protected override void InitializeConfigurationProvider(string providerName, IConfigurationProvider provider)
        {
            ((IExceptionHandler)provider).CurrentPolicyName = policyName;
            ((IExceptionHandler)provider).CurrentExceptionTypeName = exceptionTypeName;
            base.InitializeConfigurationProvider (providerName, provider);
        }


        private ExceptionHandlerData GetExceptionHandlerData(string handlerName)
        {
        	ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = (ExceptionHandlingConfigurationView)CreateConfigurationView();
            return exceptionHandlingConfigurationView.GetExceptionHandlerData(policyName, exceptionTypeName, handlerName);
        }

        private static void ValidatePolicyAndExceptionType(string policyName, string exceptionTypeName)
        {
            ArgumentValidation.CheckForNullReference(policyName, "policyName");
            ArgumentValidation.CheckForEmptyString(policyName, "policyName");
            ArgumentValidation.CheckForNullReference(exceptionTypeName, "exceptionTypeName");
            ArgumentValidation.CheckForEmptyString(exceptionTypeName, "exceptionTypeName");
        }
    }
}