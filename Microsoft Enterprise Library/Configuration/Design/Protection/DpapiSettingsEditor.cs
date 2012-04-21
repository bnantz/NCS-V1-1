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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Provides a user interface that can edit <see cref="DpapiSettings"/> at design time.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    public class DpapiSettingsEditor : UITypeEditor
    {
        
        private string confirmMessage;
        private bool required;

        /// <summary>
        /// Create an instance which requires dpapi settings.
        /// </summary>
        public DpapiSettingsEditor() : this(true)
        {
        }

        /// <summary>
        /// Creates an instance defining whether or not dpapi settings are required.
        /// </summary>
        /// <param name="required">Whether or not dpapi settings are requried.</param>
        public DpapiSettingsEditor(bool required)
        {
            this.required = required;
            confirmMessage = string.Empty;
        }

        /// <summary>
        /// <para>Gets or sets the confirmation message to display to the user.</para>
        /// </summary>
        /// <value>
        /// <para>The confirmation message to display to the user.</para>
        /// </value>
        protected string ConfirmMessage
        {
            get { return confirmMessage; }
            set { confirmMessage = value; }
        }

        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by <see cref="GetEditStyle"/>. This should be a <see cref="DpapiSettings"/> object.</para>
        /// </summary>
        /// <param name="context"><para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para></param>
        /// <param name="provider"><para>An <see cref="IServiceProvider"/> that this editor can use to obtain services.</para></param>
        /// <param name="value"><para>The object to edit. This should be a <see cref="DpapiSettings"/> object.</para></param>
        /// <returns><para>The new value of the <see cref="DpapiSettings"/> object.</para></returns>
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
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    using (DpapiSettingsEditorUI dialog = new DpapiSettingsEditorUI(required))
                    {
                        if (ConfirmMessage.Length > 0)
                        {
                            dialog.ConfirmMessage = this.ConfirmMessage;
                        }
                    
                        DpapiSettings dpapiSettings = value as DpapiSettings;
                        InitializeDialog(dialog, dpapiSettings);
                        DpapiSettingsData newSettingsData = dialog.DpapiSettingsData;
                        return HandleResult(service.ShowDialog(dialog), dpapiSettings, newSettingsData);
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// <para>Gets the editor style used by the <see cref="EditValue"/> method.</para>
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
        /// <returns>A <see cref="UITypeEditorEditStyle"/> value that indicates the style of editor used by <see cref="EditValue"/>. If the <see cref="UITypeEditor"/> does not support this method, then <see cref="GetEditStyle"/> will return <see cref="UITypeEditorEditStyle.None"/>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// <para>Handle the result from the editor.</para>
        /// </summary>
        /// <param name="dialogResult">
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </param>
        /// <param name="originalSettings">
        /// <para>The original <see cref="DpapiSettings"/> before editing.</para>
        /// </param>
        /// <param name="newSettingsData">
        /// <para>The new <see cref="DpapiSettingsData"/> from the editor.</para>
        /// </param>
        /// <returns>
        /// <para>If accepted, the new <see cref="DpapiSettings"/>; otherwise the <paramref name="originalSettings"/>.</para>
        /// </returns>
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
        protected virtual object HandleResult(DialogResult dialogResult, DpapiSettings originalSettings, DpapiSettingsData newSettingsData)
        {
            if (dialogResult == DialogResult.OK)
            {
                if (newSettingsData == null)
                {
                    return null;
                }

                return new DpapiSettings(newSettingsData);
            }
            else
            {
                return originalSettings;
            }
        }

        private void InitializeDialog(DpapiSettingsEditorUI dialog, DpapiSettings settings)
        {
            if (settings != null)
            {
                dialog.DpapiSettingsData = new DpapiSettingsData();
                dialog.DpapiSettingsData.Mode = settings.Mode;
                dialog.DpapiSettingsData.Entropy = settings.Entropy;
            }

            dialog.DisplaySettings();
        }
    }
}