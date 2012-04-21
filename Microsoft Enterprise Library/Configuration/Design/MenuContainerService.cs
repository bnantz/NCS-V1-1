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

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides support for adding menu items to the main menu.
    /// </devdoc>
    internal class MenuContainerService : IMenuContainerService
    {
        private ArrayList list;
        
        public MenuContainerService()
        {
            list = new ArrayList();
        }

        /// <devdoc>
        /// Gets the list of menu items for all external callers.
        /// </devdoc>
        public IList MenuItems
        {
            get { return list; }
        }
    }
}