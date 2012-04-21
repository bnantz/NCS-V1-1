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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Represents a collection of <see cref="SinkNode"/>s.
    /// </summary>
    [Image(typeof (SinkCollectionNode))]
    public class SinkCollectionNode : ConfigurationNode
    {
        private SinkDataCollection sinkDataCollection;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public SinkCollectionNode() : this(new SinkDataCollection())
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="data">The specified configuration data.</param>
        public SinkCollectionNode(SinkDataCollection data) : base()
        {
            if (data == null) throw new ArgumentNullException("data");
            this.sinkDataCollection = data;
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
        public virtual SinkDataCollection SinkDataCollection
        {
            get
            {
                sinkDataCollection.Clear();
                foreach (SinkNode node in this.Nodes)
                {
                    sinkDataCollection[node.SinkData.Name] = node.SinkData;
                }
                return sinkDataCollection;
            }
        }

        /// <summary>
        /// Adds the default sink EventLogSink
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();

            Nodes.Add(new EventLogSinkNode());
            Nodes.Add(new FlatFileSinkNode());
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.Sinks;
            CreateDynamicNodes(sinkDataCollection);
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(SinkNode));
        }
      
    }
}
