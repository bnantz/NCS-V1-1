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
using System.Collections;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
	/// Represents a policy with exception types and
	/// exception handlers. 
	/// </summary>
    public sealed class ExceptionPolicy 
	{
		private Hashtable policyEntries;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionPolicy"/> class. This class
        /// cannot be inherited from.</para>
        /// </summary>
        public ExceptionPolicy()
		{
		}

		/// <summary>
		/// The main entry point into the Exception Handling
		/// framework. Handles the specified <see cref="Exception"/>
		/// object according the current configuration.
		/// </summary>
		/// <param name="ex">An <see cref="Exception"/> object.</param>
		/// <param name="policyName">The name of the policy to handle.</param>
		/// <returns>
		/// Whether or not a rethrow is recommended.
		/// </returns>
		/// <example>
		/// The following code shows the usage of the 
		/// exception handling framework.
		/// <code>
		/// try
		///	{
		///		Foo();
		///	}
		///	catch (Exception e)
		///	{
		///		if (ExceptionPolicy.HandleException(e, policyName)) throw;
		///	}
		/// </code>
		/// </example>
		public static bool HandleException(Exception ex, string policyName)
		{
			ConfigurationContext configurationContext = ConfigurationManager.GetCurrentContext();
			return HandleException(ex, policyName, configurationContext);
		}

        /// <summary>
        /// The main entry point into the Exception Handling
        /// framework. Handles the specified <see cref="Exception"/>
        /// object according the given <paramref name="configurationContext"></paramref>.
        /// </summary>
        /// <param name="ex">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <param name="configurationContext">The configuration to be used in handling this exception</param>
        /// <returns>
        /// Whether or not a rethrow is recommended.
        /// </returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        /// try
        ///	{
        ///		Foo();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///		if (ExceptionPolicy.HandleException(e, policyName)) throw;
        ///	}
        /// </code>
        /// </example>
        public static bool HandleException(Exception ex, string policyName, ConfigurationContext configurationContext)
		{
			ArgumentValidation.CheckForNullReference(ex, "ex");
			ArgumentValidation.CheckForNullReference(policyName, "policyName");

			if (policyName == null || policyName.Length == 0)
			{
				throw new ArgumentNullException("policyName");
			}

			ExceptionPolicy policy = GetExceptionPolicy(ex, policyName, configurationContext);
			policy.Initialize(configurationContext, policyName);
			return policy.HandleException(ex);
		}

		/// <summary>
		/// Checks if there is a policy entry that matches
		/// the type of the exception object specified by the
		/// <see cref="Exception"/> parameter
		/// and if so, invokes the handlers associated with that entry.
		/// </summary>
		/// <param name="ex">The <c>Exception</c> to handle.</param>
		/// <returns>Whether or not a rethrow is recommended.</returns>
		/// <remarks>
		/// The algorithm for matching the exception object to a 
		/// set of handlers mimics that of a standard .NET exception policy.
		/// The specified exception object will be matched to a single 
		/// exception policy entry by traversing its inheritance hierarchy. 
		/// This means that if a <c>FileNotFoundException</c>, for example, is 
		/// caught, but the only exception type that the exception policy 
		/// knows how to handle is System.Exception, the event handlers 
		/// for <c>System.Exception</c> will be invoked because 
		/// <c>FileNotFoundException</c> ultimately derives from <c>System.Exception</c>.
		/// </remarks>
		private bool HandleException(Exception ex)
		{
			Type exceptionType = ex.GetType();
            ExceptionPolicyEntry entry = this.FindExceptionPolicyEntry(exceptionType);
			
            bool recommendRethrow = false;

			if (entry == null)
			{
				// If there is no Entry for this type / policy than we recommend a rethrow.
				recommendRethrow = true;
				ExceptionNotHandledEvent.Fire();
			}
			else
			{
				try
				{
				    recommendRethrow = entry.Handle(ex);
				    ExceptionHandledEvent.Fire();
				}
                catch (ExceptionHandlingException)
                {
                    ExceptionNotHandledEvent.Fire();
                    throw;
			    }
		    }
            return recommendRethrow;
        }

		private static ExceptionPolicy GetExceptionPolicy(Exception exception, string policyName, ConfigurationContext configurationContext)
		{
			try
			{
                ExceptionPolicyFactory factory = new ExceptionPolicyFactory(configurationContext);
				return factory.CreateExceptionPolicy(policyName, exception);
			}
            catch (ConfigurationException ex)
			{
                ExceptionUtility.LogHandlingException(policyName, ex, null, exception);
				ExceptionNotHandledEvent.Fire();
                throw new ExceptionHandlingException(ex.Message, ex);
			}
			catch (InvalidOperationException ex)
			{
				ExceptionUtility.LogHandlingException(policyName, ex, null, exception);
				ExceptionNotHandledEvent.Fire();
                throw new ExceptionHandlingException(ex.Message, ex);
			}
		}

        private void Initialize(ConfigurationContext context, string policyName)
		{
        	ExceptionHandlingConfigurationView exceptionHandlingConfigurationView = new ExceptionHandlingConfigurationView(context);
            ExceptionPolicyData policyData = exceptionHandlingConfigurationView.GetExceptionPolicyData(policyName);
            this.AddPolicyEntriesToCache(policyData, exceptionHandlingConfigurationView.ConfigurationContext);
		}

	    private void AddPolicyEntriesToCache(ExceptionPolicyData policyData, ConfigurationContext context)
	    {
	        this.policyEntries = new Hashtable(policyData.ExceptionTypes.Count);

	        foreach (ExceptionTypeData typeData in policyData.ExceptionTypes)
	        {
	            Type exceptionType = GetExceptionType(typeData, policyData.Name);
	            ExceptionPolicyEntry exceptionEntry = new ExceptionPolicyEntry(policyData.Name, typeData, context);
	            this.policyEntries.Add(exceptionType, exceptionEntry);
	        }
	    }

	    private Type GetExceptionType(ExceptionTypeData typeData, string policyName)
	    {
	        try
	        {
	            return Type.GetType(typeData.TypeName, true);
	        }
	        catch (TypeLoadException e)
	        {
                ExceptionUtility.LogHandlingException(policyName, null, null, e);
                ExceptionNotHandledEvent.Fire();
                throw new ExceptionHandlingException(SR.ExceptionUnknownExceptionTypeInConfiguration(typeData.TypeName), e);
	        }
	    }

	    /// <devDoc>
		/// Gets the policy entry associated with the specified key.
		/// </devDoc>
		private ExceptionPolicyEntry GetPolicyEntry(Type exceptionType)
		{
			return (ExceptionPolicyEntry) this.policyEntries[exceptionType];
		}

		/// <devDoc>
		/// Traverses the specified type's inheritance hiearchy
		/// </devDoc>
		private ExceptionPolicyEntry FindExceptionPolicyEntry(Type exceptionType)
		{
			ExceptionPolicyEntry entry = null;

			while (exceptionType != typeof (Object))
			{
				entry = this.GetPolicyEntry(exceptionType);

				if (entry == null)
				{
					exceptionType = exceptionType.BaseType;
				}
				else
				{
					//we've found the handlers, now continue on
					break;
				}
			}

			return entry;
		}
	}
}
