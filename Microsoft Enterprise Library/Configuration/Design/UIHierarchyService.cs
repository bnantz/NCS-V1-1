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
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides a container and management for IUIHierarchy objects.
    /// </devdoc>
    internal class UIHierarchyService : IUIHierarchyService, IDisposable
    {
        internal Hashtable hierarchies;
        private IUIHierarchy selectedHierarchy;
        private static object hierarchyAddedEvent = new object();
        private static object hierarchyRemovedEvent = new object();
        private EventHandlerList handlerList;
        
        public UIHierarchyService()
        {
            hierarchies = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            handlerList = new EventHandlerList();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                handlerList.Dispose();
                foreach (object hierarchy in hierarchies.Values)
                {
                    IDisposable disposable = hierarchy as IDisposable;
                    if (disposable != null) disposable.Dispose();
                }
            }
        }

        /// <devdoc>
        /// Occurs after a IUIHierarchy is added.
        /// </devdoc>
        public event HierarchyAddedEventHandler HierarchyAdded
        {
            add { handlerList.AddHandler(hierarchyAddedEvent, value); }
            remove { handlerList.RemoveHandler(hierarchyAddedEvent, value); }
        }

        /// <devdoc>
        /// Occurs after a IUIHierarchy is added.
        /// </devdoc>
        public event HierarchyRemovedEventHandler HierarchyRemoved
        {
            add { handlerList.AddHandler(hierarchyRemovedEvent, value); }
            remove { handlerList.RemoveHandler(hierarchyRemovedEvent, value); }
        }

        /// <devdoc>
        /// Gets or sets the current selected hierarchy.
        /// </devdoc>
        public IUIHierarchy SelectedHierarchy
        {
            get { return selectedHierarchy; }
            set { selectedHierarchy = value; }
        }

        /// <devdoc>
        /// Adds a hierarchy to the container.
        /// </devdoc>
        public void AddHierarchy(IUIHierarchy hierarchy)
        {
            hierarchies.Add(hierarchy.Id, hierarchy);
            if (selectedHierarchy == null)
            {
                selectedHierarchy = hierarchy;
            }
            OnHierarchyAdded(new HierarchyAddedEventArgs(hierarchy));
        }

        /// <devdoc>
        /// Gets a hierarchy from the container.
        /// </devdoc>
        public IUIHierarchy GetHierarchy(Guid id)
        {
            return hierarchies[id] as UIHierarchy;
        }

        /// <devdoc>
        /// Gets all the hierarchies in the service.
        /// </devdoc>
        public IUIHierarchy[] GetAllHierarchies()
        {
            ArrayList list = new ArrayList(hierarchies.Values);
            return (IUIHierarchy[])list.ToArray(typeof(IUIHierarchy));
        }

        /// <devdoc>
        /// Removes a hierarchy from the container.
        /// </devdoc>
        public void RemoveHierarchy(Guid id)
        {
            IUIHierarchy hierarchy = hierarchies[id] as IUIHierarchy;
            if (hierarchy == null)
            {
                return;
            }
            hierarchies.Remove(id);
            OnHierarchyRemoved(new HierarchyRemovedEventArgs(hierarchy));
        }

        /// <devdoc>
        /// Removes a hierarchy from the container.
        /// </devdoc>
        public void RemoveHierarchy(IUIHierarchy hierarchy)
        {
            RemoveHierarchy(hierarchy.Id);
        }

        /// <devdoc>
        /// Saves all the hierarchies by calling save on all IUIHierarchy.
        /// </devdoc>
        public void SaveAll()
        {
            foreach (DictionaryEntry entry in hierarchies)
            {
                IUIHierarchy hierarchy = entry.Value as IUIHierarchy;
                if (hierarchy != null)
                {
                    hierarchy.Save();
                }
            }
        }

        private void OnHierarchyAdded(HierarchyAddedEventArgs e)
        {
            HierarchyAddedEventHandler handler = (HierarchyAddedEventHandler)handlerList[hierarchyAddedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnHierarchyRemoved(HierarchyRemovedEventArgs e)
        {
            HierarchyRemovedEventHandler handler = (HierarchyRemovedEventHandler)handlerList[hierarchyRemovedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}