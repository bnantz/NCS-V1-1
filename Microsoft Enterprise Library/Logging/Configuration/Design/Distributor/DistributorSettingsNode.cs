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
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor
{
    /// <summary>
    /// Node representing distributor settings.
    /// </summary>
    [Image(typeof(DistributorSettingsNode))]
    public class DistributorSettingsNode : ConfigurationNode
    {
        private DistributorSettings distributorSettings;
        private CategoryNode defaultCategoryNode;
        private FormatterNode defaultFormatterNode;
        private ConfigurationNodeChangedEventHandler onDefaultCategoryNodeRemoved;
        private ConfigurationNodeChangedEventHandler onDefaultCategoryNodeRenamed;
        private ConfigurationNodeChangedEventHandler onDefaultFormatterNodeRemoved;
        private ConfigurationNodeChangedEventHandler onDefaultFormatterNodeRenamed;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public DistributorSettingsNode() : this(new DistributorSettings(DefaultValues.DistributorDefaultCategory, DefaultValues.DistributorDefaultFormatter))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="distributorSettings">The specified configuration data.</param>
        public DistributorSettingsNode(DistributorSettings distributorSettings) : base()
        {
            this.onDefaultCategoryNodeRemoved = new ConfigurationNodeChangedEventHandler(OnDefaultCategoryNodeRemoved);
            this.onDefaultCategoryNodeRenamed = new ConfigurationNodeChangedEventHandler(OnDefaultCategoryNodeRenamed);
            this.onDefaultFormatterNodeRemoved = new ConfigurationNodeChangedEventHandler(OnDefaultFormatterNodeRemoved);
            this.onDefaultFormatterNodeRenamed = new ConfigurationNodeChangedEventHandler(OnDefaultFormatterNodeRenamed);
            this.distributorSettings = distributorSettings;

        }

        /// <summary>
        /// The configured name.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Default category to use if a category is not defined or is typed incorrectly.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultCategoryDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CategoryNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public CategoryNode DefaultCategory
        {
            get { return defaultCategoryNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultCategoryNode = (CategoryNode)service.CreateReference(defaultCategoryNode, value, onDefaultCategoryNodeRemoved, onDefaultCategoryNodeRenamed);
                distributorSettings.DefaultCategory = defaultCategoryNode == null ? String.Empty : defaultCategoryNode.Name;

            }
        }

        /// <summary>
        /// Default formatter to use if a formatter is not defined or misconfigured.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultFormatterDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public FormatterNode DefaultFormatter
        {
            get { return defaultFormatterNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultFormatterNode = (FormatterNode)service.CreateReference(defaultFormatterNode, value, onDefaultFormatterNodeRemoved, onDefaultFormatterNodeRenamed);
                distributorSettings.DefaultFormatter = defaultFormatterNode == null ? String.Empty : defaultFormatterNode.Name;
            }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual DistributorSettings DistributorSettings
        {
            get
            {
                distributorSettings.DistributorService = null;
                GetFormatters();
                GetSinks();
                GetCategories();
                GetDistributorService();
                return distributorSettings;
            }
        }
        /// <summary>
        /// Adds the following nodes: <see cref="SinkCollectionNode"/>, <see cref="FormatterCollectionNode"/>, and <see cref="CategoryCollectionNode"/>.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            SinkCollectionNode sinksNode = new SinkCollectionNode();
            Nodes.AddWithDefaultChildren(sinksNode);
            FormatterCollectionNode fomattersNode = new FormatterCollectionNode();
            Nodes.AddWithDefaultChildren(fomattersNode);
            CategoryCollectionNode cateogoriesNode = new CategoryCollectionNode();
            Nodes.AddWithDefaultChildren(cateogoriesNode);
         }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();

            ResolveDefaultCategoryNodeReferences();

            ResolveDefaultFormatterNodeReferences();
        }

        

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DistributorSettings;
            if (distributorSettings.SinkDataCollection.Count > 0)
            {
                Nodes.Add(new SinkCollectionNode(distributorSettings.SinkDataCollection));
            }

            if (distributorSettings.Formatters.Count > 0)
            {
                Nodes.Add(new FormatterCollectionNode(distributorSettings.Formatters));
            }

            if (distributorSettings.CategoryDataCollection.Count > 0)
            {
                Nodes.Add(new CategoryCollectionNode(distributorSettings.CategoryDataCollection));
            }

            if (distributorSettings.DistributorService.ServiceName != null)
            {
                Nodes.Add(new MsmqDistributorServiceNode(distributorSettings.DistributorService));
            }
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            ConfigurationMenuItem item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(SinkCollectionNode), SR.Sinks, false);
            AddMenuItem(item);
            item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(FormatterCollectionNode), SR.Formatters, false);
            AddMenuItem(item);
            item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(CategoryCollectionNode), SR.Categories, false);
            AddMenuItem(item);
            item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(MsmqDistributorServiceNode), SR.DistributorService, false);
            AddMenuItem(item);
        }

        protected override void OnChildAdding(ConfigurationNodeChangedEventArgs e)
        {
            base.OnChildAdding (e);
            if (Hierarchy.ContainsNodeType(this, e.Node.GetType()))
            {
                throw new InvalidOperationException(SR.ExceptionCollectionAlreadyExists);
            }
        }


        private void ResolveDefaultCategoryNodeReferences()
        {
            CategoryCollectionNode categoryCollectionNode = Hierarchy.FindNodeByType(typeof(CategoryCollectionNode)) as CategoryCollectionNode;
            if (categoryCollectionNode == null) return;
            DefaultCategory = Hierarchy.FindNodeByName(categoryCollectionNode, distributorSettings.DefaultCategory) as CategoryNode;
        }

        private void ResolveDefaultFormatterNodeReferences()
        {
            FormatterCollectionNode formatterCollectionNodes = Hierarchy.FindNodeByType(typeof(FormatterCollectionNode)) as FormatterCollectionNode;
            if (formatterCollectionNodes == null) return;
            DefaultFormatter = Hierarchy.FindNodeByName(formatterCollectionNodes, distributorSettings.DefaultFormatter) as FormatterNode;
        }

        private void GetFormatters()
        {
            FormatterCollectionNode formatterCollectionNode = Hierarchy.FindNodeByType(this, typeof(FormatterCollectionNode)) as FormatterCollectionNode;
            if (formatterCollectionNode == null) return;
            
            FormatterDataCollection formatters = formatterCollectionNode.FormatterDataCollection;
            if (Object.ReferenceEquals(distributorSettings.Formatters, formatters)) return;

            distributorSettings.Formatters.Clear();
            foreach (FormatterData formatterData in formatters)
            {
                distributorSettings.Formatters[formatterData.Name] = formatterData;
            }
        }

        private void GetSinks()
        {
            SinkCollectionNode sinkCollectionNode = Hierarchy.FindNodeByType(this, typeof(SinkCollectionNode)) as SinkCollectionNode;
            if (sinkCollectionNode == null) return;
            
            SinkDataCollection sinks = sinkCollectionNode.SinkDataCollection;
            if (Object.ReferenceEquals(distributorSettings.SinkDataCollection, sinks)) return;

            distributorSettings.SinkDataCollection.Clear();
            foreach (SinkData sink in sinks)
            {
                distributorSettings.SinkDataCollection[sink.Name] = sink;
            }
        }

        private void GetCategories()
        {
            CategoryCollectionNode categoryCollectionNode = Hierarchy.FindNodeByType(this, typeof(CategoryCollectionNode)) as CategoryCollectionNode;
            if (categoryCollectionNode == null) return;

            CategoryDataCollection categories = categoryCollectionNode.CategoryDataCollection;
            if (Object.ReferenceEquals(distributorSettings.CategoryDataCollection, categories)) return;

            distributorSettings.CategoryDataCollection.Clear();
            foreach (CategoryData categoryData in categories)
            {
                distributorSettings.CategoryDataCollection[categoryData.Name] = categoryData;
            }
        }

        private void GetDistributorService()
        {
            MsmqDistributorServiceNode serviceNode = Hierarchy.FindNodeByType(this, typeof(MsmqDistributorServiceNode)) as MsmqDistributorServiceNode;
            if (serviceNode == null) return;

            distributorSettings.DistributorService = serviceNode.MsmqDistributorServiceData;
        }

        private void OnDefaultCategoryNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultCategoryNode = null;
        }

        private void OnDefaultCategoryNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.distributorSettings.DefaultCategory = e.Node.Name;
        }

        private void OnDefaultFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultFormatterNode.Removed -= new ConfigurationNodeChangedEventHandler(this.OnDefaultFormatterNodeRemoved);
            this.defaultFormatterNode = null;
        }

        private void OnDefaultFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.distributorSettings.DefaultFormatter = e.Node.Name;
        }
    }
}