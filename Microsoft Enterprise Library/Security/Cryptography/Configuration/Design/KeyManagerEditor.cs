//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Editor for a <c>KeyInfo</c>.
    /// </summary>
    internal class KeyManagerEditor : UITypeEditor
    {
        /// <summary>
        /// See <see cref="UITypeEditor.EditValue"/>.
        /// </summary>
        /// <param name="context">See <see cref="UITypeEditor.EditValue"/>.</param>
        /// <param name="provider">See <see cref="UITypeEditor.EditValue"/>.</param>
        /// <param name="value">See <see cref="UITypeEditor.EditValue"/>.</param>
        /// <returns>See <see cref="UITypeEditor.EditValue"/>.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    KeySettings keySettings = value as KeySettings;
                    if (keySettings == null)
                    {
                        throw new ArgumentException(SR.KeyManagerEditorRequiresKeyInfoProperty);
                    }

                    KeyManagerEditorUI dialog = new KeyManagerEditorUI(keySettings.Clone());
                    DialogResult dialogResult = service.ShowDialog(dialog);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return keySettings;
                    }
                    else
                    {
                        return dialog.KeySettings;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.
        /// </summary>
        /// <param name="context">See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.</param>
        /// <returns>See <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}