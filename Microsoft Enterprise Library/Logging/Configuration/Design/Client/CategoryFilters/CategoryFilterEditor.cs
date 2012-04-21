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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters
{
    /// <summary>
    /// A rich editor for category filters.
    /// </summary>
    internal class CategoryFilterEditor : UITypeEditor
    {
        /// <summary>
        /// Initializes and draws the category filter editor dialog and returns the results to the calling object.
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

                    CategoryFilterEditorUI dialog = new CategoryFilterEditorUI();
                    ClientSettingsNode currentSettings = (ClientSettingsNode) context.Instance;
                    InitializeDialog(dialog, currentSettings);

                    DialogResult dialogResult = service.ShowDialog(dialog);

                    if (dialogResult != DialogResult.Cancel)
                    {
                        CategoryFilterMode categoryFilterMode = dialog.FilterMode;

                        CategoryFilterDataCollection categoryFilters = new CategoryFilterDataCollection();
                        foreach (string category in dialog.GetSelectedCategoryNames())
                        {
                            CategoryFilterData categoryFilter = new CategoryFilterData();
                            categoryFilter.Name = category;
                            categoryFilters.Add(categoryFilter);
                        }

                        return new CategoryFilterSettings(categoryFilterMode, categoryFilters);
                    }
                    else
                    {
                        return currentSettings.CategoryFilterSettings;
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

        private void InitializeDialog(CategoryFilterEditorUI dialog, ClientSettingsNode node)
        {
            dialog.FilterMode = node.CategoryFilterSettings.CategoryFilterMode;

            SetCategoryFilterSettings(dialog, node);

            SetAvailableCategoryNames(dialog, node);
        }

        private void SetCategoryFilterSettings(CategoryFilterEditorUI dialog, ClientSettingsNode node)
        {
            ArrayList selectedCategoryNames = new ArrayList();
            foreach (CategoryFilterData categoryFilter in node.CategoryFilterSettings.CategoryFilters)
            {
                selectedCategoryNames.Add(categoryFilter.Name);
            }

            dialog.SetSelectedCategoryNames(selectedCategoryNames);
        }

        private void SetAvailableCategoryNames(CategoryFilterEditorUI dialog, ConfigurationNode node)
        {
            ConfigurationNode rootNode = node.Parent.Parent;
            ConfigurationNode[] nodes = rootNode.Hierarchy.FindNodesByType(rootNode, typeof(CategoryNode));
            ArrayList categoryNames = new ArrayList();
            if (nodes != null)
            {
                ConfigurationNode[] sortedNodes = nodes;
                Array.Sort(sortedNodes);
                foreach (ConfigurationNode categoryNode in sortedNodes)
                {
                    categoryNames.Add(categoryNode.Name);
                }
            }

            dialog.AvailableCategoryNames = categoryNames;
        }
    }
}