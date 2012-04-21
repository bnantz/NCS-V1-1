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
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a user interface for saving a file name.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class SaveFileEditor : UITypeEditor
    {
        private string file;
        private FilteredFileNameEditorAttribute editorAttribute;

        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by GetEditStyle.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para>
        /// </param>
        /// <param name="provider">
        /// <para>An <see cref="IServiceProvider"/> that this editor can use to obtain services.</para>
        /// </param>
        /// <param name="value">
        /// <para>The object to edit.</para>
        /// </param>
        /// <returns>
        /// <para>The new value of the object.</para>
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    file = value as string;
                    foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
                    {
                        editorAttribute = attribute as FilteredFileNameEditorAttribute;
                        if (editorAttribute != null)
                        {
                            break;
                        }
                    }

                    using(SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        InitializeDialog(saveFileDialog);
                        if (value is string)
                        {
                            saveFileDialog.FileName = (string)value;
                        }

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            value = saveFileDialog.FileName;
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// <para>Gets the editor style used by the <see cref="EditValue"/> method.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para>
        /// </param>
        /// <returns>
        /// <para><see cref="UITypeEditorEditStyle.Modal"/></para>
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// <para>Initializes the save file dialog when it is created.</para>
        /// </summary>
        /// <param name="saveFileDialog">
        /// <para>A <see cref="SaveFileDialog"/> instance.</para>
        /// </param>
        protected virtual void InitializeDialog(SaveFileDialog saveFileDialog)
        {
            if (editorAttribute == null)
            {
                saveFileDialog.Filter = SR.GenericFileFilter;
            }
            else
            {
                saveFileDialog.Filter = editorAttribute.Filter;
            }
            saveFileDialog.Title = SR.GenericSaveFile;
            if (file != null)
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
            }
        }
    }
}