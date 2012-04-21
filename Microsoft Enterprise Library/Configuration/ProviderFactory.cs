//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Provides base functionality for creating named instances of a particular provider type.</para>
    /// </summary>  
    public abstract class ProviderFactory : ConfigurationFactory
    {
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFactory"/>
        /// </summary>
        /// <param name="factoryName"><para>A friendly name that will be included in exception messages.</para></param>
        /// <param name="configurationContext"><para>The <see cref="ConfigurationContext"/> to use.</para></param>
        /// <param name="type"><para>The base type of all providers that this factory will create.</para></param>
        /// <excpetion cref="ArgumentNullException">
        /// <para><paramref name="configurationContext"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="factoryName"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="type"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </excpetion>
        protected ProviderFactory(string factoryName, ConfigurationContext configurationContext, Type type) : base(factoryName, configurationContext)
        {
            ArgumentValidation.CheckForNullReference(type, "type");

            this.type = type;
        }
        
        /// <summary>
        /// <para>Gets the <see cref="Type"/> being created.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> being created.</para>
        /// </value>
        protected Type ProviderType
        {
            get { return type; }
        }

        /// <summary>
        /// <para>When overriden by a derived class, creates the <see cref="ConfigurationView"/> for the factory.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="ConfigurationView"/> for the factory.</para>
        /// </returns>
        protected abstract ConfigurationView CreateConfigurationView();

        /// <summary>
        /// <para>Create the default insance of the provider object.</para>
        /// </summary>
        /// <returns>
        /// <para>The default provider object instance.</para>
        /// </returns>
        protected virtual object CreateDefaultInstance()
        {
            string defaultName = GetDefaultInstanceName();
            if ((null == type))
            {
                ConfigurationException ex = new ConfigurationException(SR.ExceptionDefaultProviderNotSpecified(FactoryName));
                PublishFailureEvent(defaultName, ex);
                throw ex;
            }

            return CreateInstance(defaultName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected virtual string GetDefaultInstanceName()
        {
            return string.Empty;
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
        protected virtual void InitializeConfigurationProvider(string providerName, IConfigurationProvider provider)
        {
            provider.Initialize(CreateConfigurationView());
        }

        /// <summary>
        /// <para>Create the named <see cref="IConfigurationProvider"/> object.</para>
        /// </summary>
        /// <param name="providerName">
        /// <para>The name of the <see cref="IConfigurationProvider"/> to create.</para>
        /// </param>
        /// <param name="type"><see cref="Type"></see> of object to instantiate</param>
        /// <returns>Instantiated object</returns>
        protected override object CreateObject(string providerName, Type type)
        {
            ValidateTypeIsIConfigurationProvider(type);

            object createdObject = base.CreateObject(providerName, type);
            InitializeObject(providerName, createdObject);
            return createdObject;
        }

        private void InitializeObject(string providerName, object createdObject)
        {
            IConfigurationProvider provider = (IConfigurationProvider) createdObject;
            provider.ConfigurationName = providerName;
            InitializeConfigurationProvider(providerName, provider);
        }

        private void ValidateTypeIsIConfigurationProvider(Type type)
        {
            if (!ProviderType.IsAssignableFrom(type))
            {
                throw new ConfigurationException(SR.ExceptionProviderTypeMismatchExceptionMessage(ProviderType.AssemblyQualifiedName, type.AssemblyQualifiedName));
            }

            if (!typeof (IConfigurationProvider).IsAssignableFrom(type))
            {
                throw new ConfigurationException(SR.ExceptionProviderNotTypeOfConfigurationProvider(type.AssemblyQualifiedName));
            }
        }
    }
}