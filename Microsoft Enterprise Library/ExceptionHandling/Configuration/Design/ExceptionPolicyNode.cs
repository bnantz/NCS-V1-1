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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// The designtime configuration node for an <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy"/>.
    /// </summary>
    [Image(typeof(ExceptionPolicyNode))]
    public class ExceptionPolicyNode : ConfigurationNode
    {
        private ExceptionPolicyData exceptionPolicyData;

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptionPolicyNode"/> object.
        /// </summary>
        public ExceptionPolicyNode() : this(new ExceptionPolicyData(SR.DefaultExceptionPolicyNodeName))
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptionPolicyNode"/> object
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="exceptionPolicyData">The corresponding runtime configuration data.</param>
        public ExceptionPolicyNode(ExceptionPolicyData exceptionPolicyData) : base()
        {
            if (exceptionPolicyData == null)
            {
                throw new ArgumentNullException("exceptionPolicyData");
            }
            this.exceptionPolicyData = exceptionPolicyData;

        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual ExceptionPolicyData ExceptionPolicyData
        {
            get
            {
                exceptionPolicyData.ExceptionTypes.Clear();
                foreach (ExceptionTypeNode node in this.Nodes)
                {
                    exceptionPolicyData.ExceptionTypes.Add(node.ExceptionTypeData);
                }
                return exceptionPolicyData;    
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = exceptionPolicyData.Name;
            foreach (ExceptionTypeData typeData in exceptionPolicyData.ExceptionTypes)
            {
                Nodes.Add(new ExceptionTypeNode(typeData));
            }
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            exceptionPolicyData.Name = e.Node.Name;
        }


        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.ExceptionTypeNodeMenuText, 
                new AddExceptionTypeNodeCommand(Site, typeof(ExceptionTypeNode)),
                this,
                Shortcut.None,
                SR.ExceptionTypeNodeStatusText,
                InsertionPoint.New);
            AddMenuItem(item);
        }


    }
}