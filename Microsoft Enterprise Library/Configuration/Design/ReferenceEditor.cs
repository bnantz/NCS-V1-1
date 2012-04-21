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
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a user interface that can edit a reference to another node at design time.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class ReferenceEditor : UITypeEditor
    {
        /// <summary>
        /// <para>Edits the value of the specified object using the editor style indicated by <seealso cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.</para>
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information. </param>
        /// <param name="provider">An <see cref="IServiceProvider"/> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                Debug.Assert(service != null, "No editor service; we cannot edit the value");
                if (service != null)
                {
                    ConfigurationNode currentNode = (ConfigurationNode)context.Instance;
                    ApplicationConfigurationNode appNode = GetApplicationNode(currentNode);
                    ReferenceEditorUI control = new ReferenceEditorUI(appNode, GetReferenceType(context), (ConfigurationNode)value, service, IsRequired(context));
                    service.DropDownControl(control);
                    if (control.SelectedNode != null)
                    {
                        Type propertyType = context.PropertyDescriptor.PropertyType;
                        Type selectedNodeType = control.SelectedNode.GetType();
                        if (propertyType == selectedNodeType || selectedNodeType.IsSubclassOf(propertyType))
                        {
                            return control.SelectedNode;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the editor style used by the <seealso cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/> method.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
        /// <returns><see cref="UITypeEditorEditStyle.DropDown"/></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private ApplicationConfigurationNode GetApplicationNode(ConfigurationNode node)
        {
            if (node is ApplicationConfigurationNode)
            {
                return (ApplicationConfigurationNode)node;
            }

            do
            {
                node = node.Parent;
            } while (node != null && !(node is ApplicationConfigurationNode));

            return (ApplicationConfigurationNode)node;
        }

        private bool IsRequired(ITypeDescriptorContext context)
        {
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                if (attribute is RequiredAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        private Type GetReferenceType(ITypeDescriptorContext context)
        {
            Type referenceType = null;
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
                if (attribute is ReferenceTypeAttribute)
                {
                    referenceType = ((ReferenceTypeAttribute)attribute).ReferenceType;
                    break;
                }
            }
            if (referenceType == null)
            {
                throw new InvalidOperationException(SR.ExceptionNoRefPathAttribute);
            }
            return referenceType;
        }
    }
}