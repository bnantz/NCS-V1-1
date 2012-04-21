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

using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a view into configuration data.</para>
    /// </summary>
    public abstract class ConfigurationView
    {
        private readonly ConfigurationContext context;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationView"/> with a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="context">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        protected ConfigurationView(ConfigurationContext context)
        {
            ArgumentValidation.CheckForNullReference(context, "context");

            this.context = context;
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationContext"/> for the current view.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationContext"/> for the current view.</para>
        /// </value>
        public ConfigurationContext ConfigurationContext
        {
            get { return context; }
        }
    }
}