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
    /// <para>Factory for creating <see cref="IUIHierarchy"/> objects for a configration hierarchy.</para>
    /// </summary>
    public sealed class ConfigurationUIHierarchyFactory
    {
        private ConfigurationUIHierarchyFactory()
        {
        }

        /// <summary>
        /// <para>Create an <see cref="IUIHierarchy"/> for building application configuration.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> that will be the root of the <see cref="IUIHierarchy"/>.</para>
        /// </param>
        /// <param name="serviceProvider">
        /// <para>The <see cref="IServiceProvider"/> for the heirarchy.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="IUIHierarchy"/> created.</para>
        /// </returns>
        public static IUIHierarchy Create(ConfigurationNode node, IServiceProvider serviceProvider)
        {
            return new UIHierarchy(node, serviceProvider);
        }
    }
}