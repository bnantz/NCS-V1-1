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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// The root designtime configuration node for Exception Handling.
    /// </summary>
    [Image(typeof (ExceptionHandlingSettingsNode))]
    public class ExceptionHandlingSettingsNode : ConfigurationNode
    {
        private ExceptionHandlingSettings exceptionHandlingSettings;

        /// <summary>
        /// Creates the root node for Exception Handling.
        /// </summary>
        public ExceptionHandlingSettingsNode() : this(new ExceptionHandlingSettings())
        {
            
        }

        /// <summary>
        /// Creates the root node for Exception Handling and builds the designtime tree
        /// based on the supplied runtime settings.
        /// </summary>
        /// <param name="exceptionHandlingSettings">The Exception Handling runtime settings.</param>
        public ExceptionHandlingSettingsNode(ExceptionHandlingSettings exceptionHandlingSettings) : base()
        {
            this.exceptionHandlingSettings = exceptionHandlingSettings;
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
        public virtual ExceptionHandlingSettings ExceptionHandlingSettings
        {
            get
            {
                exceptionHandlingSettings.ExceptionPolicies.Clear();
                foreach (ExceptionPolicyNode node in this.Nodes)
                {
                    exceptionHandlingSettings.ExceptionPolicies.Add(node.ExceptionPolicyData);
                }
                return exceptionHandlingSettings;
            }
        }

        protected override void OnSited()
        {
            base.OnSited ();
            Site.Name = SR.DefaultExceptionHandlingSettingsNodeName;
            foreach (ExceptionPolicyData policyData in exceptionHandlingSettings.ExceptionPolicies)
            {
                Nodes.Add(new ExceptionPolicyNode(policyData));
            }
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.ExceptionPolicyNodeMenuText, 
                new AddChildNodeCommand(Site, typeof(ExceptionPolicyNode)),
                this,
                Shortcut.None,
                SR.ExceptionPolicyNodeStatusText,
                InsertionPoint.New);
            AddMenuItem(item);
        }
    }
}
