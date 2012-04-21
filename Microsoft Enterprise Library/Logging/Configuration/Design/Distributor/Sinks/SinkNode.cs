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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
    /// <summary>
    /// Base class for nodes which represent sinks.
    /// </summary>
    [Image(typeof (SinkNode))]
    public abstract class SinkNode : ConfigurationNode
    {
        private SinkData sinkData;

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        protected SinkNode(SinkData sinkData) : base()
        {
            if (sinkData == null) throw new ArgumentNullException("sinkData");
            this.sinkData = sinkData;
        }
        
        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual SinkData SinkData
        {
            get { return sinkData; }
        }

        /// <summary>
        /// The fully qualified assembly name of the <see cref="LogSink"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(LogSink))]
        [SRDescription(SR.Keys.SinkTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return sinkData.TypeName; }
            set { sinkData.TypeName = value; }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = sinkData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            sinkData.Name = e.Node.Name;
        }


    }
}