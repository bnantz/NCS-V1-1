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
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a factory for creating objects from configuration.</para>
    /// </summary>
    [ReflectionPermission(SecurityAction.Demand, Flags=ReflectionPermissionFlag.MemberAccess)]
	public abstract class ConfigurationFactory
	{
        private readonly ConfigurationContext configurationContext;
        private readonly string factoryName;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationFactory"/> class with the factory name and a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="factoryName">
        /// <para>The name of the factory.</para>
        /// </param>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object</para>
        /// </param>
		protected ConfigurationFactory(string factoryName, ConfigurationContext configurationContext)
		{
            ArgumentValidation.CheckForNullReference(configurationContext, "configurationContext");
            ArgumentValidation.CheckForNullReference(factoryName, "providerName");
            ArgumentValidation.CheckForEmptyString(factoryName, "factoryName");

            this.configurationContext = configurationContext;
            this.factoryName = factoryName;
		}

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationContext"/> for the factory.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationContext"/> for the factory.</para>
        /// </value>
	    protected ConfigurationContext ConfigurationContext
	    {
	        get { return configurationContext; }
	    }

        /// <summary>
        /// <para>Gets the name of the factory.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the factory.</para>
        /// </value>
	    protected string FactoryName
	    {
	        get { return factoryName; }
	    }

        /// <summary>
        /// <para>When overriden by a derived class, gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="configurationName"/>.</para>
        /// </summary>
        /// <param name="configurationName">
        /// <para>The name of the configuration object to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the configuration object.</para>
        /// </returns>
        protected abstract Type GetConfigurationType(string configurationName);

        /// <summary>
        /// <para>Publish an instrumentation event that indicates there was an error while attempting to create a provider.</para>
        /// </summary>
        /// <param name="configurationName"><para>The name of the configuration object.</para></param>
        /// <param name="e"><para>The <see cref="Exception"/> to publish.</para></param>
        protected virtual void PublishFailureEvent(string configurationName, Exception e)
        {
        }

        /// <summary>
        /// <para>Creates an instance of the named configuration object.</para>
        /// </summary>
        /// <param name="configurationName">
        /// <para>The name of the configuration object.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the configuration object.</para>
        /// </returns>
        protected virtual object CreateInstance(string configurationName)
        {
            ArgumentValidation.CheckForNullReference(configurationName, "configurationName");
            ArgumentValidation.CheckForEmptyString(configurationName, "configurationName");

            try
            {
                Type type = GetConfigurationType(configurationName);
                return CreateObject(configurationName, type);
            }
            catch (ConfigurationException e)
            {
                PublishFailureEvent(configurationName, e);
                throw;
            }
        }

        /// <summary>
        /// <para>Construct an instance of a named configuration object by the <paramref name="type"/>.</para>
        /// </summary>
        /// <param name="configurationName">
        /// <para>The name of the configuration object.</para>
        /// </param>
        /// <param name="type"><para>The <see cref="Type"/> to create.</para></param>
        /// <returns>An instance of the type.</returns>
        protected virtual object CreateObject(string configurationName, Type type)
        {
            ArgumentValidation.CheckForNullReference(type, "type");
            ArgumentValidation.CheckForNullReference(configurationName, "configurationName");
            ArgumentValidation.CheckForEmptyString(configurationName, "configurationName");

            ConstructorInfo constructor = type.GetConstructor(new Type[] {});
            if (constructor == null)
            {
                throw new ConfigurationException(SR.ExceptionProviderMissingConstructor(type.FullName));
            }
            object createdObject = null;
            try
            {
                createdObject = constructor.Invoke(null);
            }
            catch (MethodAccessException ex)
            {
                throw new ConfigurationException(ex.Message, ex);   
            }
            catch (TargetInvocationException ex)
            {
                throw new ConfigurationException(ex.Message, ex);   
            }
            catch (TargetParameterCountException ex)
            {
                throw new ConfigurationException(ex.Message, ex);   
            }
            return createdObject;
        }

	    /// <summary>
	    /// <para>Gets the <see cref="Type"/> based on a qualified name.</para>
	    /// </summary>
	    /// <param name="typeName">
	    /// <para>The qualified name.</para>
	    /// </param>
	    /// <returns>
	    /// <para>The <see cref="Type"/> based on the qualified name.</para>
	    /// </returns>
        protected Type GetType(string typeName)
        {
            ArgumentValidation.CheckForNullReference(typeName, "typeName");

            try
            {
                return Type.GetType(typeName, true, false);
            }
            catch (TypeLoadException e)
            {
                throw new ConfigurationException(SR.ExceptionFactoryTypeLoadError(typeName, FactoryName), e);
            }
            catch (FileNotFoundException ex)
            {
                throw new ConfigurationException(SR.ExceptionTypeCreateError(typeName), ex);
            }
        }
	}
}
