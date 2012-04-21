//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents a sensitive parameter to the connection string for a database in the application.
    /// </summary>
    /// <remarks>
    /// The value of this parameter is masked in the UI to prevent "over the shoulder" viewing
    /// of sensitive information.
    /// </remarks>
    public class SensitiveParameterNode : ParameterNode
    {
        private ParameterData parameter;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="SensitiveParameterNode"/> class.
        /// </para>
        /// </summary>
        public SensitiveParameterNode() : this(new ParameterData(SR.DefaultParameterNodeName, string.Empty))
        {
        }

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="SensitiveParameterNode"/> class with a <see cref="ParameterData"/> object.
        /// </para>
        /// </summary>
        /// <param name="parameterData">
        /// <para>The <see cref="ParameterData"/> runtime configuration.</para>
        /// </param> 
        public SensitiveParameterNode(ParameterData parameterData) : base(parameterData)
        {
            this.parameter = parameterData;
            this.parameter.IsSensitive = true;
        }

        /// <summary>
        /// Hidden to the designer. Use <see cref="SensitiveParameterNode.MaskedValue"/>.
        /// </summary>
        [Browsable(false)]
        public override string Value
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets or sets the masked value of this property.
        /// </summary>
        [Editor(typeof(PasswordEditor), typeof(UITypeEditor))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.ParameterValueDescription)]
        public Password MaskedValue
        {
            get { return new Password(parameter.Value); }
            set { parameter.Value = value.PasswordText; }
        }
    }
}