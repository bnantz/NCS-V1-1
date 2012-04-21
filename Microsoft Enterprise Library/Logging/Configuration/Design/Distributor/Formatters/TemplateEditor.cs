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
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters
{
    /// <summary>
    /// Editor for formatter templates.
    /// </summary>
    internal class TemplateEditor : UITypeEditor
    {
        /// <summary>
        /// Initializes and draws the template editor dialog and returns the results to the calling object.
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
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));

                    TemplateEditorUI dialog = new TemplateEditorUI();

                    string originalText = ((Template)value).Text;
                    dialog.UserText = originalText;

                    DialogResult dialogResult = service.ShowDialog(dialog);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return new Template(originalText);
                    }

                    return new Template(dialog.UserText);
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