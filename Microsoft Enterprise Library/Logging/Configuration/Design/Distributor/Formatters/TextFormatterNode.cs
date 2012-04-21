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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Node for a Text Formatter.
    /// </summary>
    public class TextFormatterNode : FormatterNode
    {
        private TextFormatterData textFormatterData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public TextFormatterNode() : this(new TextFormatterData(SR.Formatter, DefaultValues.TextFormatterFormat))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="textFormatterData">Configuration data.</param>
        public TextFormatterNode(TextFormatterData textFormatterData) : base(textFormatterData)
        {
            this.textFormatterData = textFormatterData;
        }

        /// <summary>
        /// Read only.  Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.TextFormatter"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return textFormatterData.TypeName; }
        }

        /// <summary>
        /// The template for the formatter.
        /// </summary>
        [Required, Editor(typeof(TemplateEditor), typeof(UITypeEditor))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public Template Template
        {
            get { return new Template(textFormatterData.Template.Value); }
            set { textFormatterData.Template.Value = value.Text; }
        }
    }
}