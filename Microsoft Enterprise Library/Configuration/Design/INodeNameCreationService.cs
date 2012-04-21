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

using System.ComponentModel.Design.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a service that can generate unique names for <see cref="ConfigurationNode"/> objects.</para>
    /// </summary>
    public interface INodeNameCreationService : INameCreationService
    {
        /// <summary>
        /// <para>When implemented by a class, gets a unique display name for the current node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to get the unique name.</para>
        /// </param>
        /// <param name="newName"><para>The name to check for uniquness.</para></param>
        /// <returns><para>The unique display name.</para></returns>
        string GetUniqueDisplayName(ConfigurationNode node, string newName);
    }

}