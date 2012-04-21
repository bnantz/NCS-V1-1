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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents an application configuration. It is also the root node in the <see cref="IUIHierarchy"/> for an application.</para>
    /// </summary>
    [Image(typeof(ApplicationConfigurationNode))]
    [SelectedImage(typeof(ApplicationConfigurationNode))]
    public class ApplicationConfigurationNode : ConfigurationNode
    {
        private ApplicationData applicationData;

        /// <summary>
        /// <para>Initializes a new instance of the  <see cref="ApplicationConfigurationNode"/> class with an <see cref="ApplicationData"/> object containing the data for the application.</para>
        /// </summary>
        /// <param name="applicationData">
        /// <para>An <see cref="ApplicationData"/> object.</para>
        /// </param>
        public ApplicationConfigurationNode(ApplicationData applicationData) : base()
        {
            if (applicationData == null)
            {
                throw new ArgumentNullException("applicationData");
            }
            this.applicationData = applicationData;
        }

        /// <summary>
        /// <para>Gets the <see cref="ApplicationData"/> for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ApplicationData"/> for the node.</para>
        /// </value>
        [Browsable(false)]
        public ApplicationData ApplicationData
        {
            get { return applicationData; }
        }

        /// <summary>
        /// <para>Gets the current configuration file.</para>
        /// </summary>
        /// <value>
        /// <para>The current configuration file.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.ConfigurationFilePathDescription)]
        [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
        [FilteredFileNameEditor("Configuration files (*.config)|*.config|All files|*.*")]
        [FileValidation]
        [Browsable(true)]
        public string ConfigurationFile
        {
            get { return applicationData.ConfigurationFilePath; }
            set { applicationData.ConfigurationFilePath = value; }
        }

        /// <summary>
        /// <para>Sets the name of the node and updates the name of the <see cref="IStorageTable.MetaConfigurationFile"/> with the <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ApplicationData.ConfigurationFilePath"/>.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = applicationData.Name;
            Hierarchy.StorageTable.MetaConfigurationFile = applicationData.ConfigurationFilePath;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ApplicationData.Name"/> to the new node name.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            applicationData.Name = e.Node.Name;
        }

        /// <summary>
        /// <para>Adds the close and validate menu items to the user interface menu system.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            AddMenuItem(ConfigurationMenuItem.CreateValidateNodeCommand(Site, this));
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.CloseApplicationMenuItemText, new CloseApplicationConfigurationCommand(Site),
                                                                   this, Shortcut.None, SR.CloseApplicationStatusText, InsertionPoint.Action);
            AddMenuItem(item);
            Hierarchy.Load();
        }
    }
}