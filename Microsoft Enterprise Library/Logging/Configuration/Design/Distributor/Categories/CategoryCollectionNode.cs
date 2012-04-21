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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories
{
    /// <summary>
    /// Node representing a collection of <see cref="CategoryNode"/>s.
    /// </summary>
    [Image(typeof (CategoryCollectionNode))]
    public class CategoryCollectionNode : ConfigurationNode
    {
        private CategoryDataCollection categoryDataCollection;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public CategoryCollectionNode() : this(new CategoryDataCollection())
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="categoryDataCollection">The specified configuration data.</param>
        public CategoryCollectionNode(CategoryDataCollection categoryDataCollection) : base()
        {
            this.categoryDataCollection = categoryDataCollection;
        }

        /// <summary>
        /// The configured name.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }


        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CategoryDataCollection CategoryDataCollection
        {
            get
            {
                categoryDataCollection.Clear();
                foreach (CategoryNode categoryNode in Nodes)
                {
                    categoryDataCollection[categoryNode.CategoryData.Name] = categoryNode.CategoryData;
                }
                return categoryDataCollection;
            }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.Categories;
            foreach (CategoryData category in categoryDataCollection)
            {
                Nodes.Add(new CategoryNode(category));
            }
        }


        /// <summary>
        /// Adds a default category node.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();

            Nodes.Add(CreateDefaultNode(SR.DefaultCategory, SR.DefaultCategoryDestination, SR.EventLogSink));
            Nodes.Add(CreateDefaultNode(SR.DefaultTracingCategory, SR.DefaultTracingDestination, SR.DefaultTracingFileSink));

            ResolveAllNodeReferences();
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.Category,
                new AddChildNodeCommand(Site, typeof(CategoryNode)),
                this, 
                Shortcut.None,
                SR.GenericCreateStatusText(SR.Category),
                InsertionPoint.New);
            AddMenuItem(item);
        }


        private void ResolveAllNodeReferences()
        {
            foreach (ConfigurationNode node in Nodes)
            {
                node.ResolveNodeReferences();
            }
            Parent.ResolveNodeReferences();
        }

        private CategoryNode CreateDefaultNode(string categoryName, string destinationName, string sinkNodeReference)
        {
            CategoryData data = new CategoryData(categoryName);
            DestinationData destination = new DestinationData(destinationName, sinkNodeReference, SR.DefaultFormatter);
            data.DestinationDataCollection.Add(destination);
            return new CategoryNode(data);
        }
    }
}
