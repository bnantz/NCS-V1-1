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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a service to link nodes to other nodes.</para>
    /// </summary>
    public interface ILinkNodeService
    {
        /// <summary>
        /// <para>When implemented by a class, gets the reference to a <see cref="ConfigurationNode"/> wiring up the <seealso cref="ConfigurationNode.Removed"/> event.</para>
        /// </summary>
        /// <param name="oldReference"><para>The old reference.</para></param>
        /// <param name="newReference"><para>The new reference.</para></param>
        /// <param name="referenceRemovedHandler"><para>The delegate that will execute when the reference is removed.</para></param>
        /// <returns>
        /// <para>The reference to the <see cref="ConfigurationNode"/>.</para>
        /// </returns>
        ConfigurationNode CreateReference(ConfigurationNode oldReference, ConfigurationNode newReference, ConfigurationNodeChangedEventHandler referenceRemovedHandler);

        /// <summary>
        /// <para>When implemented by a class, gets the reference to a <see cref="ConfigurationNode"/> wiring up the <seealso cref="ConfigurationNode.Removed"/> and <seealso cref="ConfigurationNode.Renamed"/> events.</para>
        /// </summary>
        /// <param name="oldReference"><para>The old reference.</para></param>
        /// <param name="newReference"><para>The new reference.</para></param>
        /// <param name="referenceRemovedHandler"><para>The delegate that will execute when the reference is removed.</para></param>
        /// <param name="referenceRenamedHandler"><para>The delegate that will execute when the reference is renamed.</para></param>
        /// <returns>
        /// <para>The reference to the <see cref="ConfigurationNode"/>.</para>
        /// </returns>
        ConfigurationNode CreateReference(ConfigurationNode oldReference, ConfigurationNode newReference, ConfigurationNodeChangedEventHandler referenceRemovedHandler, ConfigurationNodeChangedEventHandler referenceRenamedHandler);
    }
}