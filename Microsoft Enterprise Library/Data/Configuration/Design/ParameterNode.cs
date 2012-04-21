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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a parameter to the connection string for a database in the application.
    /// </para>
    /// </summary>
    [Image(typeof(ParameterNode))]
    public class ParameterNode : ConfigurationNode
    {
        private ParameterData parameterData;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ParameterNode"/> class.
        /// </para>
        /// </summary>
        public ParameterNode() : this(new ParameterData(SR.DefaultParameterNodeName, string.Empty))
        {
        }

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ParameterNode"/> class with a <see cref="ParameterData"/> object.
        /// </para>
        /// </summary>
        /// <param name="parameter">
        /// <para>The <see cref="ParameterData"/> runtime configuration.</para>
        /// </param> 
        public ParameterNode(ParameterData parameter) : base()
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }
            this.parameterData = parameter;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the value of the parameter.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>
        /// The name of the parameter.
        /// </para>
        /// </value>
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.ParameterValueDescription)]
        public virtual string Value
        {
            get { return this.parameterData.Value; }
            set { this.parameterData.Value = value; }
        }

        /// <summary>
        /// <para>Gets the runtime configuration <see cref="ParameterData"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The runtime configuration <see cref="ParameterData"/>.</para>
        /// </value>
        [Browsable(false)]
        public virtual ParameterData Parameter
        {
            get { return this.parameterData; }
        }

        /// <summary>
        /// <para>Sets the <seeals cref="ConfigurationNode.Name"/> to the <seealso cref="ParameterData.Name"/></para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = parameterData.Name;
        }

        /// <summary>
        /// <para>Sets the <seeals cref="ParameterData.Name"/> to the <seealso cref="ConfigurationNode.Name"/></para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            parameterData.Name = e.Node.Name;
        }
    }
}