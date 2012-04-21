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
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Provides a user interface that can edit <see cref="DpapiSettings"/> at design time for a <see cref="FileKeyAlgorithmPairStorageProvider"/>.</para>
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    public class FileKeyAlgorithmDpapiSettingsEditor : DpapiSettingsEditor
    {
        private FileKeyAlgorithmPairStorageProviderNode currentNode;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmDpapiSettingsEditor"/>.</para>
        /// </summary>
        public FileKeyAlgorithmDpapiSettingsEditor() : base(false)
        {
        }

        /// <summary>
        /// <para>Edits the specified object's value using the editor style indicated by <see cref="UITypeEditor.GetEditStyle"/>. This should be a <see cref="DpapiSettings"/> object.</para>
        /// </summary>
        /// <param name="context"><para>An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</para></param>
        /// <param name="provider"><para>An <see cref="IServiceProvider"/> that this editor can use to obtain services.</para></param>
        /// <param name="value"><para>The object to edit. This should be a <see cref="DpapiSettings"/> object.</para></param>
        /// <returns><para>The new value of the <see cref="DpapiSettings"/> object.</para></returns>
        /// <seealso cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ConfirmMessage = SR.DpapiConvertKeyConfirmMessage;
            currentNode = (FileKeyAlgorithmPairStorageProviderNode)context.Instance;
            return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// <para>If DPAPI settings change, this saves the key algorithm out with the new settings.</para>
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
        protected override object HandleResult(DialogResult dialogResult, DpapiSettings originalSettings, DpapiSettingsData newSettingsData)
        {
            bool returnBaseResult = true;

            if (dialogResult == DialogResult.OK)
            {
                DpapiSettings newDpapiSettings = null;
                if (newSettingsData != null)
                {
                    newDpapiSettings = new DpapiSettings(newSettingsData);
                }

                if (newDpapiSettings != originalSettings)
                {
                    returnBaseResult = SaveKeyAlgorithmPairWithNewDapiSettings(newDpapiSettings, originalSettings);
                }
            }

            if (returnBaseResult)
            {
                return base.HandleResult(dialogResult, originalSettings, newSettingsData);
            }
            else
            {
                return originalSettings;
            }
        }

        private ConfigurationContext GetContext()
        {
            ConfigurationContext context = currentNode.Hierarchy.BuildConfigurationContext();
            ConfigurationSettings settings = context.GetMetaConfiguration();
            ConfigurationDictionary config = new ConfigurationDictionary();
            config.Add(ConfigurationSettings.SectionName, settings);
            context = new ConfigurationContext(config);
            return context;
        }

        private bool SaveKeyAlgorithmPairWithNewDapiSettings(DpapiSettings newDpapiSettings, DpapiSettings originalDpapiSettings)
        {
            ConfigurationContext context = GetContext();

            ConfigurationSettings settings = context.GetMetaConfiguration();

            FileKeyAlgorithmPairStorageProvider loadProvider = new FileKeyAlgorithmPairStorageProvider();
            FileKeyAlgorithmPairStorageProviderData loadData = new FileKeyAlgorithmPairStorageProviderData(
                SR.DefaultFileKeyAlgorithmStorageProviderNodeName, currentNode.File, GetDpapiSettingsData(originalDpapiSettings));
            settings.KeyAlgorithmPairStorageProviderData = loadData;
            loadProvider.ConfigurationName = loadData.Name;
            loadProvider.Initialize(new RuntimeConfigurationView(context));


            FileKeyAlgorithmPairStorageProvider saveProvider = new FileKeyAlgorithmPairStorageProvider();
            FileKeyAlgorithmPairStorageProviderData saveData = new FileKeyAlgorithmPairStorageProviderData(
                SR.DefaultFileKeyAlgorithmStorageProviderNodeName, currentNode.File, GetDpapiSettingsData(newDpapiSettings));
            settings.KeyAlgorithmPairStorageProviderData = saveData;
            saveProvider.ConfigurationName = saveData.Name;
            saveProvider.Initialize(new RuntimeConfigurationView(context));

            try
            {
                KeyAlgorithmPair key = loadProvider.Load();
                saveProvider.Save(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    SR.FileKeyAlgorithmDpapiSettingsEditorUnableToSaveNewDpapiSettingsErrorMessage(ex.Message),
                    SR.FileKeyAlgorithmDpapiSettingsEditorUnableToSaveNewDpapiSettingsCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return false;
            }

            return true;
        }

        private DpapiSettingsData GetDpapiSettingsData(DpapiSettings settings)
        {
            if (settings == null)
            {
                return null;
            }
            else
            {
                return new DpapiSettingsData(settings.Entropy, settings.Mode);
            }
        }

    }
}