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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>A utility class to help with common service related activities.</para>
    /// </summary>
    public sealed class ServiceHelper
    {
        private ServiceHelper()
        {
        }

        /// <summary>
        /// <para>Log an error to the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="node"><para>The configuration node associated with the error.</para></param>
        /// <param name="e"><para>The <see cref="Exception"/> to log.</para></param>
        public static void LogError(IServiceProvider serviceProvider, ConfigurationNode node, Exception e)
        {
            Exception exception = e;
            while (exception != null)
            {
                LogError(serviceProvider, new ConfigurationError(node, exception.Message));    
                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// <para>Log a <see cref="ValidationError"/> to the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="error">
        /// <para>The <see cref="ValidationError"/> to log.</para>
        /// </param>
        public static void LogError(IServiceProvider serviceProvider, ValidationError error)
        {
            ArgumentValidation.CheckForNullReference(error, "error");

            IConfigurationErrorLogService configurationErrorLogService = GetConfigurationErrorService(serviceProvider);
            configurationErrorLogService.LogError(error);
        }

        /// <summary>
        /// <para>Log a <see cref="ConfigurationError"/> to the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="error">
        /// <para>The <see cref="ConfigurationError"/> to log.</para>
        /// </param>
        public static void LogError(IServiceProvider serviceProvider, ConfigurationError error)
        {
            ArgumentValidation.CheckForNullReference(error, "error");

            IConfigurationErrorLogService configurationErrorLogService = GetConfigurationErrorService(serviceProvider);
            configurationErrorLogService.LogError(error);
        }

        /// <summary>
        /// <para>Display the errors from the <see cref="IConfigurationErrorLogService"/> in the user interface.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public static void DisplayErrors(IServiceProvider serviceProvider)
        {
            IUIService uiService = GetUIService(serviceProvider);
            uiService.DisplayErrorLog(GetConfigurationErrorService(serviceProvider));
        }

        /// <summary>
        /// <para>Gets the registered <see cref="IUIService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The registered <see cref="IUIService"/>.</returns>
        public static IUIService GetUIService(IServiceProvider serviceProvider)
        {
            ArgumentValidation.CheckForNullReference(serviceProvider, "serviceProvider");

            IUIService uiService = serviceProvider.GetService(typeof(IUIService)) as IUIService;
            Debug.Assert(uiService != null, "Could not get the IUIService");
            return uiService;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="IUIHierarchyService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The registered <see cref="IUIHierarchyService"/>.</returns>
        public static IUIHierarchyService GetUIHierarchyService(IServiceProvider serviceProvider)
        {
            IUIHierarchyService hierarchyService = serviceProvider.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(hierarchyService != null, "Could not get the IUIHierarchyService");
            return hierarchyService;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="IXmlIncludeTypeService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The registered <see cref="IXmlIncludeTypeService"/>.</returns>
        public static IXmlIncludeTypeService GetXmlIncludeTypeService(IServiceProvider serviceProvider)
        {
            IXmlIncludeTypeService xmlIncludeTypeService = serviceProvider.GetService(typeof(IXmlIncludeTypeService)) as IXmlIncludeTypeService;
            Debug.Assert(xmlIncludeTypeService != null, "Could not get the IXmlIncludeTypeService");
            return xmlIncludeTypeService;
        }

        /// <summary>
        /// <para>Gets the current selected <see cref="IUIHierarchy"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The current selected <see cref="IUIHierarchy"/>.</returns>
        public static IUIHierarchy GetCurrentHierarchy(IServiceProvider serviceProvider)
        {
            IUIHierarchyService uiHierarchyService = GetUIHierarchyService(serviceProvider);
            return uiHierarchyService.SelectedHierarchy;
        }

        /// <summary>
        /// <para>Gets the current selected <see cref="IUIHierarchy"/>'s <see cref="IStorageTable"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The current selected <see cref="IUIHierarchy"/>'s <see cref="IStorageTable"/>.</returns>
        public static IStorageTable GetCurrentStorageTable(IServiceProvider serviceProvider)
        {
            return GetCurrentHierarchy(serviceProvider).StorageTable;
        }

        /// <summary>
        /// <para>Gets the current selected <see cref="IUIHierarchy"/>'s root <see cref="ConfigurationNode"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The current selected <see cref="IUIHierarchy"/>'s root <see cref="ConfigurationNode"/>.</returns>
        public static ConfigurationNode GetCurrentRootNode(IServiceProvider serviceProvider)
        {
            return GetCurrentHierarchy(serviceProvider).RootNode;
        }

        /// <summary>
        /// <para>Gets the current selected <see cref="IUIHierarchy"/>'s <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The current selected <see cref="IUIHierarchy"/>'s <see cref="ConfigurationContext"/>.</returns>
        public static ConfigurationContext GetCurrentConfigurationContext(IServiceProvider serviceProvider)
        {
            return GetCurrentHierarchy(serviceProvider).ConfigurationContext;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="IMenuContainerService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>The registered <see cref="IMenuContainerService"/>.</returns>
        public static IMenuContainerService GetMenuContainerService(IServiceProvider serviceProvider)
        {
            ArgumentValidation.CheckForNullReference(serviceProvider, "serviceProvider");

            IMenuContainerService menuService = serviceProvider.GetService(typeof(IMenuContainerService)) as IMenuContainerService;
            Debug.Assert(menuService != null, "Could not get the IMenuContainerService");
            return menuService;
        }

        /// <summary>
        /// Checks that all declared dependencies for the object are present after it has been sited.
        /// </devdoc>
        internal static void CheckDependencies(IComponent component)
        {
            ServiceDependencyAttribute[] deps = (ServiceDependencyAttribute[])component.GetType().GetCustomAttributes(typeof(ServiceDependencyAttribute), true);
            if ((null == deps) || 0 == (deps.Length))
            {
                return;
            }
            if (null == component.Site)
            {
                throw new InvalidOperationException(SR.ExceptionComponentNotSited);
            }
            foreach (ServiceDependencyAttribute dep in deps)
            {
                if (null == component.Site.GetService(dep.ServiceType))
                {
                    throw new InvalidOperationException(SR.ExceptionServiceUnavailable(dep.ServiceType.Name));
                }
            }
        }

        /// <summary>
        /// <para>Gets the registered <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>
        /// <para>The registered <see cref="IConfigurationErrorLogService"/>.</para>
        /// </returns>
        public static IConfigurationErrorLogService GetConfigurationErrorService(IServiceProvider serviceProvider)
        {
            ArgumentValidation.CheckForNullReference(serviceProvider, "serviceProvider");

            IConfigurationErrorLogService errorLogService = serviceProvider.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
            Debug.Assert(errorLogService != null, "Could not get the IConfigurationErrorLogService");
            return errorLogService;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="ILinkNodeService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>
        /// <para>The registered <see cref="ILinkNodeService"/>.</para>
        /// </returns>
        public static ILinkNodeService GetLinkService(IServiceProvider serviceProvider)
        {
            ILinkNodeService linkNodeService = serviceProvider.GetService(typeof(ILinkNodeService)) as ILinkNodeService;
            Debug.Assert(linkNodeService != null, "Could not get the ILinkNodeService");
            return linkNodeService;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="INodeNameCreationService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>
        /// <para>The registered <see cref="INodeNameCreationService"/>.</para>
        /// </returns>
        public static INodeNameCreationService GetNodeNameCreationService(IServiceProvider serviceProvider)
        {
            INodeNameCreationService nodeNameCreationService = serviceProvider.GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
            Debug.Assert(nodeNameCreationService != null, "Could not get the INodeNameCreationService");
            return nodeNameCreationService;
        }

        /// <summary>
        /// <para>Gets the registered <see cref="INodeCreationService"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <returns>
        /// <para>The registered <see cref="INodeCreationService"/>.</para>
        /// </returns>
        public static INodeCreationService GetNodeCreationService(IServiceProvider serviceProvider)
        {
            INodeCreationService nodeCreationService = serviceProvider.GetService(typeof(INodeCreationService)) as INodeCreationService;
            Debug.Assert(nodeCreationService != null, "Could not get the INodeCreationService");
            return nodeCreationService;
        }
    }
}