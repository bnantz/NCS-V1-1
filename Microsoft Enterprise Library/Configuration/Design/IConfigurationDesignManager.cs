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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents the object that will manage the design of configuration.</para>.
    /// </summary>
    public interface IConfigurationDesignManager
    {
        /// <summary>
        /// <para>
        /// When implemented by a class, allows the registration of configuration nodes and commands into the configuration tree.
        /// </para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        void Register(IServiceProvider serviceProvider);

        /// <summary>
        /// <para>When implemented by a class, saves the configuration data for the implementer.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        void Save(IServiceProvider serviceProvider);

        /// <summary>
        /// <para>When implemented by a class, opens the configuration for the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        void Open(IServiceProvider serviceProvider);

        /// <summary>
        /// <para>When implemented by a class, adds the configuration data for the current implementer to the <see cref="ConfigurationDictionary"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="configurationDictionary">
        /// <para>A <see cref="ConfigurationDictionary"/> object that will contain the configuration data.</para></param>
        void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary);
    }
}