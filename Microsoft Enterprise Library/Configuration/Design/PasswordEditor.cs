//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a user interface that can edit a <see cref="Password"/> at design time.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class PasswordEditor : UITypeEditor
    {
        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by <see cref="GetEditStyle"/>. This should be a <see cref="DpapiSettings"/> object.</para>
        /// </summary>
        /// <param name="context"><para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para></param>
        /// <param name="provider"><para>An <see cref="IServiceProvider"/> that this editor can use to obtain services.</para></param>
        /// <param name="value"><para>The object to edit. This should be a <see cref="Password"/> object.</para></param>
        /// <returns><para>The new value of the <see cref="Password"/> object.</para></returns>
        /// <seealso cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    IWindowsFormsEditorService service =(IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    using (PasswordEditorUI dialog = new PasswordEditorUI())
                    {
                        if (DialogResult.OK == service.ShowDialog(dialog))
                        {
                            return new Password(dialog.Password);
                        }
                        else
                        {
                            return value;
                        }
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// <para>Gets the editor style used by the <see cref="EditValue"/> method.</para>
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
        /// <returns><see cref="UITypeEditorEditStyle.Modal"/>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}