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
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a user interface for selecting a file name.</para>
    /// </summary>
    /// <seealso cref="FileNameEditor"/>
    public class FilteredFileNameEditor : FileNameEditor
    {
        private FilteredFileNameEditorAttribute editorAttribute;
        private string file;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FilteredFileNameEditor"/> class.</para>
        /// </summary>
        public FilteredFileNameEditor()
        {
        }

        /// <summary>
        /// <para>Edits the specified object using the editor style provided by the GetEditStyle method.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <para>ITypeDescriptorContext</para> that can be used to gain additional context information.</para>
        /// </param>
        /// <param name="serviceProvider">
        /// <para>A service provider object through which editing services may be obtained.</para>
        /// </param>
        /// <param name="value">
        /// <para>An instance of the value being edited.</para>
        /// </param>
        /// <returns>
        /// <para>The new value of the object. If the value of the object hasn't changed, this should return the same object it was passed.</para>
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            if (serviceProvider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)serviceProvider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
                    {
                        editorAttribute = attribute as FilteredFileNameEditorAttribute;
                        if (editorAttribute != null)
                        {
                            break;
                        }
                    }
                }
            }
            file = value as string;
            return base.EditValue(context, serviceProvider, value);
        }

        /// <summary>
        /// <para>Initializes the open file dialog when it is created.</para>
        /// </summary>
        /// <param name="openFileDialog">
        /// <para>The <see cref="OpenFileDialog"/> to use to select a file name. </para>
        /// </param>
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            if (this.editorAttribute != null)
            {
                openFileDialog.Filter = this.editorAttribute.Filter;
                if (file != null)
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
                }
            }
        }
    }
}