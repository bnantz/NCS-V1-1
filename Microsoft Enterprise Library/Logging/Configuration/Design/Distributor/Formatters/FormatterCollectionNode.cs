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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Contains a collection of <see cref="FormatterNode"/>s.
    /// </summary>
    [Image(typeof (FormatterCollectionNode))]
    public class FormatterCollectionNode : ConfigurationNode
    {
        private FormatterDataCollection formatterDataCollection;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public FormatterCollectionNode() : this(new FormatterDataCollection())
        {
        }

        /// <summary>
        /// Creates node with defined configuration data.
        /// </summary>
        /// <param name="formatters">The defined configuration data.</param>
        public FormatterCollectionNode(FormatterDataCollection formatters) : base()
        {
            if (formatters == null)
            {
                throw new ArgumentNullException("formatters");
            }
            this.formatterDataCollection = formatters;
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
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual FormatterDataCollection FormatterDataCollection
        {
            get
            {
                formatterDataCollection.Clear();
                foreach (FormatterNode node in this.Nodes)
                {
                    formatterDataCollection.Add(node.FormatterData);
                }

                return formatterDataCollection;
            }
        }

        /// <summary>
        /// Adds a <see cref="TextFormatterNode"/>.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            TextFormatterData data = new TextFormatterData(SR.DefaultFormatter, SR.DefaultTextFormat);
            Nodes.Add(new TextFormatterNode(data));
        }

        

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.Formatters;
            CreateDynamicNodes(formatterDataCollection);
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(FormatterNode));
        }
    }
}
