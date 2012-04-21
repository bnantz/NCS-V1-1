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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Base class for nodes which represent formatters.
    /// </summary>
    [Image(typeof(FormatterNode))]
    public abstract class FormatterNode : ConfigurationNode
    {
        private FormatterData formatterData;

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        protected FormatterNode(FormatterData formatterData) : base()
        {
            if (formatterData == null)
            {
                throw new ArgumentNullException("formatterData");
            }
            this.formatterData = formatterData;
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual FormatterData FormatterData
        {
            get { return formatterData; }
        }

        /// <summary>
        /// The fully qualified assembly name of the <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.ILogFormatter"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ILogFormatter), TypeSelectorIncludeFlags.AbstractTypes)]
        [SRDescription(SR.Keys.FormatterTypeDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return formatterData.TypeName; }
            set { formatterData.TypeName = value; }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = formatterData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            formatterData.Name = e.Node.Name;
        }


    }
}