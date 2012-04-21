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

using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a node for a <see cref="ConfigurationSectionData"/> of the current configuration. This makes the name read-only.</para>
    /// </summary>
    [Image(typeof(ConfigurationSectionNode))]
    public class ReadOnlyConfigurationSectionNode : ConfigurationSectionNode
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ReadOnlyConfigurationSectionNode"/> class with the runtime configuration.</para>
        /// </summary>
        /// <param name="configurationSectionData">
        /// <para>The runtime version of the configuration data.</para>
        /// </param>        
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="configurationSectionData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ReadOnlyConfigurationSectionNode(ReadOnlyConfigurationSectionData configurationSectionData) : base(configurationSectionData)
        {
        }

        /// <summary>
        /// <para>Gets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}