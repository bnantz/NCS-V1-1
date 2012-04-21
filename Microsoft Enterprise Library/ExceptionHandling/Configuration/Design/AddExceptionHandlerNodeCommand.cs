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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Intercepts creation of the WrapHandlerNode
    /// </summary>
    public class AddExceptionHandlerNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AddExceptionHandlerNodeCommand"/> class with a name, the <see cref="ConfigurationNode"/> the node is for, and the <see cref="Type"/> to create.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        public AddExceptionHandlerNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }
        
        protected override void ExecuteCore(ConfigurationNode node)
        {
            TypeSelectorUI selector = new TypeSelectorUI(null, typeof(System.Exception));
            selector.Text = SR.SelectTypeDialogCaption;
            DialogResult result = selector.ShowDialog();
            if (result == DialogResult.OK)
            {
                base.ExecuteCore(node);
                if (typeof(ITypeDependentExceptionHandler).IsInstanceOfType(ChildNode))
                {
                    ((ITypeDependentExceptionHandler)ChildNode).SetSelectedType(selector.SelectedType);
                }
            }
        }
    }
}
