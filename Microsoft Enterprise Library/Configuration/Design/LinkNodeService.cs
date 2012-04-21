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
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides a service to link nodes to other nodes.
    /// </devdoc>
    internal class LinkNodeService : ILinkNodeService
    {
        internal Hashtable referenceNodeMap;
        internal IServiceProvider serviceProvider;

        public LinkNodeService(IServiceProvider serviceProvider)
        {
            referenceNodeMap = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
            this.serviceProvider = serviceProvider;
        }

        /// <devdoc>
        /// Get the reference to a ConfigurationNode wiring up the ConfigurationNode.Removed event.
        /// </devdoc>
        public ConfigurationNode CreateReference(ConfigurationNode oldReference, ConfigurationNode newReference, ConfigurationNodeChangedEventHandler referenceRemovedHandler)
        {
            if (newReference == null)
            {
                if (oldReference != null)
                {
                    oldReference.Removed -= referenceRemovedHandler;
                }
            }
            else if (newReference != oldReference)
            {
                // unsubscribe from the old newReference
                // and subscribe to the new.
                if (oldReference != null)
                {
                    oldReference.Removed -= referenceRemovedHandler;
                }

                newReference.Removed += referenceRemovedHandler;
            }
            return newReference;
        }

        /// <devdoc>
        /// Get the reference to a ConfigurationNode wiring up the ConfigurationNode.Removed and ConfigurationNode.Renamed events.
        /// </devdoc>
        public ConfigurationNode CreateReference(ConfigurationNode oldReference, ConfigurationNode newReference, ConfigurationNodeChangedEventHandler referenceRemovedHandler, ConfigurationNodeChangedEventHandler referenceRenamedHandler)
        {
            ConfigurationNode node = CreateReference(oldReference, newReference, referenceRemovedHandler);
            if (node != null && node != oldReference)
            {
                if (oldReference != null)
                {
                    oldReference.Renamed -= referenceRenamedHandler;
                }
                node.Renamed += referenceRenamedHandler;
            }
            return node;
        }
    }
}