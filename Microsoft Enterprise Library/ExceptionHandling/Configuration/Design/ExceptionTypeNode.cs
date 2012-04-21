//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// The designtime configuration node for an exception type for a <see cref="EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry"/>.
    /// </summary>
    [Image(typeof(ExceptionTypeNode))]
    public class ExceptionTypeNode : ConfigurationNode
    {
        private ExceptionTypeData exceptionTypeData;

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptionTypeNode"/> object.
        /// </summary>
        public ExceptionTypeNode() : this(new ExceptionTypeData(SR.DefaultExceptionTypeNodeName, string.Empty))
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptionTypeNode"/> object with
        /// a corresponding runtime configuration data.
        /// </summary>
        /// <param name="exceptionTypeData">The corresponding runtime configuration data.</param>
        public ExceptionTypeNode(ExceptionTypeData exceptionTypeData) : base()
        {
            if (exceptionTypeData == null)
            {
                throw new ArgumentNullException("exceptionTypeData");
            }

            this.exceptionTypeData = exceptionTypeData;
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
        /// The fully qualified assembly name of the <see cref="System.Exception"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.ExceptionTypeNodeNameDescription)]
        [ReadOnly(true)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string TypeName
        {
            get { return exceptionTypeData.TypeName; }
            set { exceptionTypeData.TypeName = value; }
        }

        /// <summary>
        /// <para>Determines how a rethrow is handled.</para>
        /// <list type="table">
        ///		<listheader>
        ///			<item>Enumeration</item>
        ///			<description>Description</description>
        ///		</listheader>
        ///		<item>
        ///			<term>None</term>
        ///			<description>
        ///			Indicates that no rethrow should occur.
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Notify</term>
        ///			<description>
        ///			Notify the caller that a Rethrow is recommended.
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Throw</term>
        ///			<description>
        ///			Throws the exception after the exception has been handled by all handlers in the chain.
        ///			</description>
        ///		</item>
        /// </list>
        /// </summary>
        [SRDescription(SR.Keys.ExceptionTypePostHandlingActionDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [Required]
        public PostHandlingAction PostHandlingAction
        {
            get { return exceptionTypeData.PostHandlingAction; }
            set { exceptionTypeData.PostHandlingAction = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual ExceptionTypeData ExceptionTypeData
        {
            get
            {
                exceptionTypeData.ExceptionHandlers.Clear();
                foreach (ExceptionHandlerNode node in this.Nodes)
                {
                    exceptionTypeData.ExceptionHandlers.Add(node.ExceptionHandlerData);
                }
                return exceptionTypeData;
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = exceptionTypeData.Name;
            CreateDynamicNodes(exceptionTypeData.ExceptionHandlers);
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            exceptionTypeData.Name = e.Node.Name;
        }


        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(ExceptionHandlerNode));
        }

        public override bool SortChildren
        {
            get { return false; }
        }
    }
}