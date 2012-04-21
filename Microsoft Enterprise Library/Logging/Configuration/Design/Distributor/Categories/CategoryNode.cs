//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories
{
    /// <summary>
    /// Node representing a category.
    /// </summary>
    [Image(typeof (CategoryNode))]
    public class CategoryNode : ConfigurationNode
    {
        private CategoryData categoryData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public CategoryNode() : this(new CategoryData(SR.Category))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="categoryData">The specified configuration data.</param>
        public CategoryNode(CategoryData categoryData) : base()
        {
            if (categoryData == null) throw new ArgumentNullException("categoryData");
            this.categoryData = categoryData;
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CategoryData CategoryData
        {
            get
            {
                categoryData.DestinationDataCollection.Clear();
                foreach (DestinationNode destinationNode in Nodes)
                {
                    categoryData.DestinationDataCollection[destinationNode.DestinationData.Name] = destinationNode.DestinationData;
                }
                return categoryData;
            }
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.Destination,
                new AddChildNodeCommand(Site, typeof(DestinationNode)),
                this, 
                                                                   Shortcut.None,
                SR.GenericCreateStatusText(SR.Destination),
                InsertionPoint.New);
            AddMenuItem(item);
        }


        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = categoryData.Name;
            if (categoryData.DestinationDataCollection != null)
            {
                foreach (DestinationData destination in categoryData.DestinationDataCollection)
                {
                    Nodes.Add(new DestinationNode(destination));
                }
            }
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            categoryData.Name = e.Node.Name;
        }


    }
}