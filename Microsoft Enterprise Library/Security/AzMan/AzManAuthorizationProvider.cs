//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Microsoft.Interop.Security.AzRoles;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan
{
    /// <summary>
    /// Represents the AzMan authorization provider.  
    /// Authorize method checks to see if the specified identity has access to a task.
    /// </summary>
    /// <remarks>
    /// Supports Windows authentication only.
    /// Requires AzMan on Windows Server 2003, or Windows XP users will need to install the 
    /// Windows Server 2003 Administration Pack.
    /// Implements the <see cref="IAuthorizationProvider"/> interface.</remarks>
    public class AzManAuthorizationProvider : ConfigurationProvider, IAuthorizationProvider
    {
        private const string OperationContextPrefix = "O:";
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// Read the configuration settings.
        /// </summary>
        /// <param name="configurationView">A <see cref="SecurityConfigurationView"></see> object</param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (SecurityConfigurationView));

            securityConfigurationView = (SecurityConfigurationView) configurationView;
        }

        /// <summary>
        /// Evaluates the specified authority against the specified context that is either a task or operation in Authorization Manager. If the context is an operation it should be prefixed by "O".
        /// </summary>
        /// <param name="principal">Principal object containing a windows identity.</param>
        /// <param name="context">Name of the task or operation to evaluate.</param>
        /// <returns><strong>True</strong> if AzMan evaluates to true,
        /// otherwise <strong>false</strong>.</returns>
        public bool Authorize(IPrincipal principal, string context)
        {
            ArgumentValidation.CheckForNullReference(principal, "principal");
            ArgumentValidation.CheckForNullReference(context, "context");

            SecurityAuthorizationCheckEvent.Fire(principal.Identity.Name, context);
            AzManAuthorizationProviderData data = GetConfigurationData();

            string auditIdentifier = data.AuditIdentifierPrefix + principal.Identity.Name + ":" + context;

            bool result = false;
            bool operation = false;
            if (context.IndexOf(OperationContextPrefix) == 0)
            {
                operation = true;
                context = context.Substring(OperationContextPrefix.Length);
            }

            if (operation)
            {
                string[] operations = new string[] {context};
                result = CheckAccessOperations(data, auditIdentifier, principal.Identity, operations);
            }
            else
            {
                string[] tasks = new string[] {context};
                result = CheckAccessTasks(data, auditIdentifier, principal.Identity, tasks);
            }

            if (result == false)
            {
                SecurityAuthorizationFailedEvent.Fire(principal.Identity.Name, context);
            }
            return result;
        }

        /// <devdoc>
        /// Checks access to specified a set of tasks in a specified application in a specified scope.
        /// </devdoc>      
        private bool CheckAccessTasks(AzManAuthorizationProviderData data, string auditIdentifier, IIdentity identity, string[] tasks)
        {
            string[] scopes = new string[] {data.Scope};

            IAzApplication azApp = null;
            try
            {
                IAzClientContext clientCtx = GetClientContext(data, identity, data.Application, out azApp);
                Debug.Assert(azApp != null);

                object[] operationIds = GetTaskOperations(data, azApp, tasks);

                object[] internalScopes = null;
                if (scopes != null)
                {
                    internalScopes = new object[1];
                    internalScopes[0] = scopes[0];
                }

                object[] result = (object[]) clientCtx.AccessCheck(auditIdentifier,
                                                                   internalScopes, operationIds, null, null, null, null, null);
                foreach (int accessAllowed in result)
                {
                    if (accessAllowed != 0)
                    {
                        return false;
                    }
                }
            }
            catch (COMException comEx)
            {
                throw new SecurityException(comEx.Message, comEx);
            }
            return true;
        }

        private object[] GetTaskOperations(AzManAuthorizationProviderData data, IAzApplication azApp, string[] tasks)
        {
            string[] scopes = new string[] {data.Scope};
            StringCollection operations = new StringCollection();
            foreach (String task in tasks)
            {
                IAzScope scope = null;
                if ((scopes != null) && (scopes[0].Length > 0))
                {
                    scope = azApp.OpenScope(scopes[0], null);
                }

                IAzTask azTask = null;
                if (scope != null)
                {
                    azTask = scope.OpenTask(task, null);
                }
                else
                {
                    azTask = azApp.OpenTask(task, null);
                }

                Array ops = azTask.Operations as Array;
                Debug.Assert(ops != null);
                foreach (String op in ops)
                {
                    operations.Add(op);
                }
            }

            if (operations.Count == 0)
            {
                throw new ConfigurationException(SR.NoOperations);
            }

            object[] operationIds = new object[operations.Count];
            for (int index = 0; index < operations.Count; index++)
            {
                operationIds[index] = azApp.OpenOperation(operations[index], null).OperationID;
            }

            return operationIds;
        }

        /// <devdoc>
        /// Checks access to specified a set of operations in a specified application in a specified scope.
        /// </devdoc>        
        private bool CheckAccessOperations(AzManAuthorizationProviderData data, string auditIdentifier, IIdentity identity, string[] operations)
        {
            string[] scopes = new string[] {data.Scope};
            IAzApplication azApp = null;
            try
            {
                IAzClientContext clientCtx = GetClientContext(data, identity, data.Application, out azApp);
                Debug.Assert(azApp != null);

                object[] operationIds = new object[operations.Length];
                for (int index = 0; index < operations.Length; index++)
                {
                    operationIds[index] = azApp.OpenOperation(operations[index], null).OperationID;
                }

                object[] internalScopes = null;
                if (scopes != null)
                {
                    internalScopes = new object[1];
                    internalScopes[0] = scopes[0];
                }

                object[] result = (object[]) clientCtx.AccessCheck(auditIdentifier,
                                                                   internalScopes, operationIds, null, null, null, null, null);
                foreach (int accessAllowed in result)
                {
                    if (accessAllowed != 0)
                    {
                        return false;
                    }
                }
            }
            catch (COMException comEx)
            {
                throw new SecurityException(comEx.Message, comEx);
            }
            return true;
        }

        /// <devdoc>
        /// Gets the client context for the call based on the identity, system and parameters.
        /// </devdoc>        
        private IAzClientContext GetClientContext(AzManAuthorizationProviderData data, IIdentity identity, String applicationName, out IAzApplication azApp)
        {
            WindowsIdentity winIdentity = identity as WindowsIdentity;
            if (winIdentity == null)
            {
                throw new ArgumentException(SR.WindowsIdentityOnly);
            }

            AzAuthorizationStoreClass store = new AzAuthorizationStoreClass();
            store.Initialize(0, data.StoreLocation, null);
            azApp = store.OpenApplication(applicationName, null);
            Debug.Assert(azApp != null, "could not open the application");

            ulong tokenHandle = (ulong) winIdentity.Token.ToInt64();
            IAzClientContext clientCtx = azApp.InitializeClientContextFromToken(tokenHandle, null);
            Debug.Assert(clientCtx != null, "could not get the context");
            return clientCtx;
        }

        private AzManAuthorizationProviderData GetConfigurationData()
        {
            AuthorizationProviderData authorizationProviderData = securityConfigurationView.GetAuthorizationProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(authorizationProviderData, typeof (AzManAuthorizationProviderData));
            return (AzManAuthorizationProviderData) authorizationProviderData;
        }
    }
}