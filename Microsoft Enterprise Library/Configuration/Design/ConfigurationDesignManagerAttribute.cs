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
    /// <para>Indicates the <see cref="IConfigurationDesignManager"/> defined in an assembly.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class ConfigurationDesignManagerAttribute : Attribute
    {
        private readonly Type configurationDesignManager;

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="ConfigurationDesignManagerAttribute"/> class with a <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
        /// </para>
        /// </summary>
        /// <param name="configurationDesignManager">
        /// <para>
        /// A <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
        /// </para>
        /// </param>        
        public ConfigurationDesignManagerAttribute(Type configurationDesignManager)
        {
            this.configurationDesignManager = configurationDesignManager;
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/></para>
        /// </value>
        public Type ConfigurationDesignManager
        {
            get { return this.configurationDesignManager; }
        }
    }
}