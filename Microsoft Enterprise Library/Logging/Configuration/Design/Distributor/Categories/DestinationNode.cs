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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories
{
    /// <summary>
    /// Node representing a destination.
    /// </summary>
    [Image(typeof (DestinationNode))]
    [ServiceDependency(typeof(ILinkNodeService))]
    public class DestinationNode : ConfigurationNode
    {
        private DestinationData destinationData;
        private FormatterNode formatterNode;
        private SinkNode sinkNode;
        private ConfigurationNodeChangedEventHandler onFormatterNodeRemoved;
        private ConfigurationNodeChangedEventHandler onFormatterNodeRenamed;
        private ConfigurationNodeChangedEventHandler onSinkNodeRemoved;
        private ConfigurationNodeChangedEventHandler onSinkNodeRenamed;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public DestinationNode() : this(new DestinationData(SR.Destination))
        {
        }

        /// <summary>
        /// Creates node with specified configuraiton data.
        /// </summary>
        /// <param name="data">The specified configuration data.</param>
        public DestinationNode(DestinationData data) : base()
        {
            this.destinationData = data;
            this.onFormatterNodeRemoved = new ConfigurationNodeChangedEventHandler(OnFormatterNodeRemoved);
            this.onFormatterNodeRenamed = new ConfigurationNodeChangedEventHandler(OnFormatterNodeRenamed);
            this.onSinkNodeRemoved = new ConfigurationNodeChangedEventHandler(OnSinkNodeRemoved);
            this.onSinkNodeRenamed = new ConfigurationNodeChangedEventHandler(OnSinkNodeRenamed);
        }

        /// <summary>
        /// The sink for this destination.
        /// </summary>
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [SRDescription(SR.Keys.SinkDescription)]
        [ReferenceType(typeof(SinkNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public SinkNode Sink
        {
            get { return sinkNode; }
            set
            {
                ILinkNodeService service = ServiceHelper.GetLinkService(Site);
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                sinkNode = (SinkNode)service.CreateReference(sinkNode, value, onSinkNodeRemoved, onSinkNodeRenamed);
                destinationData.Sink = sinkNode == null ? String.Empty : sinkNode.Name;
            }
        }

        /// <summary>
        /// The formatter for this destination.
        /// </summary>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [SRDescription(SR.Keys.FormatDescription)]
        [ReferenceType(typeof(FormatterNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                ILinkNodeService service = ServiceHelper.GetLinkService(Site);
                formatterNode = (FormatterNode)service.CreateReference(formatterNode, value, onFormatterNodeRemoved, onFormatterNodeRenamed);
                destinationData.Format = formatterNode == null ? String.Empty : formatterNode.Name;
            }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual DestinationData DestinationData
        {
            get { return destinationData; }
        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            ResolveSinkNode();
            ResolveFormatterNode();
            return;
        }

        private void ResolveFormatterNode()
        {
            FormatterCollectionNode formatterCollectionNode = Hierarchy.FindNodeByType(typeof(FormatterCollectionNode)) as FormatterCollectionNode;
            if (formatterCollectionNode == null) return;
            Formatter = Hierarchy.FindNodeByName(formatterCollectionNode, this.destinationData.Format) as FormatterNode;
        }

        private void ResolveSinkNode()
        {
            SinkCollectionNode sinkCollectionNode = Hierarchy.FindNodeByType(typeof(SinkCollectionNode)) as SinkCollectionNode;
            if (sinkCollectionNode == null) return;
            Sink = Hierarchy.FindNodeByName(sinkCollectionNode, this.destinationData.Sink) as SinkNode;
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = destinationData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            destinationData.Name = e.Node.Name;
        }

        
        private void OnSinkNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.sinkNode = null;
        }

        private void OnSinkNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.destinationData.Sink = e.Node.Name;
        }

        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.formatterNode = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.destinationData.Format = e.Node.Name;
        }
    }
}