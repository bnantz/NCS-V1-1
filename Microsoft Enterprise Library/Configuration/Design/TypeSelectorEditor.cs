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
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a user interface for seleting a <see cref="Type"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para><see cref="TypeSelectorEditor"/> is a <see cref="UITypeEditor"/> that provides a dialog box for selecting a <see cref="Type"/>.</para>
    /// </remarks>
    [PermissionSet(SecurityAction.InheritanceDemand, Name="FullTrust")]
    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public class TypeSelectorEditor : UITypeEditor
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TypeSelectorEditor"/> class.</para>
        /// </summary>
        public TypeSelectorEditor() : base()
        {
        }

        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by <seealso cref="UITypeEditor.GetEditStyle"/>.</para>
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
        /// The fully qualifed type name for the chosen type.
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

                    IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    string currentTypeName = (string)value;
                    Type currentType = null;
                    if ((currentTypeName != null) && (currentTypeName.Length != 0))
                    {
                        currentType = Type.GetType(currentTypeName, false);
                    }
                    BaseTypeAttribute baseTypeAttribute = this.GetBaseType(context);
                    using (TypeSelectorUI form = new TypeSelectorUI(currentType, baseTypeAttribute.BaseType, baseTypeAttribute.IncludeFlags))
                    {
                        if (service.ShowDialog(form) == DialogResult.OK)
                        {
                            if (form.SelectedType != null)
                            {
                                currentTypeName = form.SelectedType.AssemblyQualifiedName;
                            }
                        }
                    }
                    return currentTypeName;
                }
            }
            return value;
        }

        /// <summary>
        /// <para>Gets the editor style used by the <seealso cref="UITypeEditor.EditValue"/> method.</para>
        /// </summary>
        /// <param name="context">
        /// <para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information</para>
        /// </param>
        /// <returns>
        /// <see cref="UITypeEditorEditStyle.Modal"/> for this editor.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <devdoc>
        /// Get the base type of the object to use to filter possible types.
        /// </devdoc>        
        private BaseTypeAttribute GetBaseType(ITypeDescriptorContext context)
        {
            BaseTypeAttribute baseTypeAttribute = null;
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                if (attribute is BaseTypeAttribute)
                {
                    baseTypeAttribute = ((BaseTypeAttribute)attribute);
                    break;
                }
            }
            if (baseTypeAttribute == null)
            {
                throw new InvalidOperationException(SR.ExceptionNoBaseTypeAttribute);
            }
            return baseTypeAttribute;
        }
    }
}