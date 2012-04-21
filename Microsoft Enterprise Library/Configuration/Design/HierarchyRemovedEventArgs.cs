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
    /// <para>Represents the method that will handle the <seealso cref="IUIHierarchyService.HierarchyRemoved"/> event of the <see cref="IUIHierarchyService"/>.</para>
    /// </summary>
    /// <param name="sender">
    /// <para>The source of the event.</para>
    /// </param>
    /// <param name="e">
    /// <para>A <see cref="HierarchyRemovedEventArgs"/> that contains the event data.</para>
    /// </param>
    public delegate void HierarchyRemovedEventHandler(object sender, HierarchyRemovedEventArgs e );

    /// <summary>
    /// <para>Provides data for the <seealso cref="IUIHierarchyService.HierarchyAdded"/> event of the <see cref="IUIHierarchyService"/>.</para>
    /// </summary>
    [Serializable]
    public class HierarchyRemovedEventArgs : EventArgs
    {
        [NonSerialized] private readonly IUIHierarchy uiHierarchy;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HierarchyAddedEventArgs"/> class with a <see cref="IUIHierarchy"/> object.</para>
        /// </summary>
        /// <param name="uiHierarchy">
        /// <para>An <see cref="IUIHierarchy"/> object.</para>
        /// </param>
        public HierarchyRemovedEventArgs(IUIHierarchy uiHierarchy)
        {
            this.uiHierarchy = uiHierarchy;
        }

        /// <summary>
        /// <para>Gets the <see cref="IUIHierarchy"/> that was added.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IUIHierarchy"/> that was added.</para>
        /// </value>
        public IUIHierarchy UIHierarchy
        {
            get { return uiHierarchy; }
        }

    }
}