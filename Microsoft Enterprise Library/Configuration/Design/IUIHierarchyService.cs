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
    /// <para>Provides a container and management for <see cref="IUIHierarchy"/> objects.</para>
    /// </summary>
    public interface IUIHierarchyService
    {
        /// <summary>
        /// <para>When implemented by a class, occurs after an <see cref="IUIHierarchy"/> is added.</para>
        /// </summary>
        event HierarchyAddedEventHandler HierarchyAdded;

        /// <summary>
        /// <para>When implemented by a class, occurs after an <see cref="IUIHierarchy"/> is removed.</para>
        /// </summary>
        event HierarchyRemovedEventHandler HierarchyRemoved;

        /// <summary>
        /// <para>When implemented by a class, gets or sets the current selected hierarchy.</para>
        /// </summary>
        /// <value>
        /// <para>The current selected hierarchy.</para>
        /// </value>
        IUIHierarchy SelectedHierarchy { get; set; }

        /// <summary>
        /// <para>When implemented by a class, adds a hierarchy to the container.</para>
        /// </summary>
        /// <param name="hierarchy">
        /// <para>The hierarchy to add.</para>
        /// </param>
        void AddHierarchy(IUIHierarchy hierarchy);

        /// <summary>
        /// <para>When implemented by a class, gets a hierarchy from the container.</para>
        /// </summary>
        /// <param name="id">The identifier for the hierarchy.</param>
        /// <returns>
        /// <para>The <see cref="IUIHierarchy"/> if found, or <see langword="null"/> if not found.</para>
        /// </returns>
        IUIHierarchy GetHierarchy(Guid id);

        /// <summary>
        /// <para>When implemented by a class, gets all the hierarchies in the service.</para>
        /// </summary>
        /// <returns>
        /// <para>All the hierarchies in the service.</para>
        /// </returns>
        IUIHierarchy[] GetAllHierarchies();

        /// <summary>
        /// <para>When implemented by a class, removes a hierarchy from the container.</para>
        /// </summary>
        /// <param name="id">
        /// <para>The identifier of the hierarchy to remove.</para>
        /// </param>
        void RemoveHierarchy(Guid id);

        /// <summary>
        /// <para>When implemented by a class, removes a hierarchy from the container.</para>
        /// </summary>
        /// <param name="hierarchy">
        /// <para>The ihierarchy to remove.</para>
        /// </param>
        void RemoveHierarchy(IUIHierarchy hierarchy);

        /// <summary>
        /// <para>When implemented by a class, saves all the hierarchies by calling save on all <see cref="IUIHierarchy"/>.</para>
        /// </summary>
        void SaveAll();
    }
}