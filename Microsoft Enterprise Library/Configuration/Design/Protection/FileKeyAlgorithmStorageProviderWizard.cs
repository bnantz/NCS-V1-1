//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    internal class FileKeyAlgorithmStorageProviderWizard : Form
    {
        private Label introLabel;
        private bool openMode;
        private GroupBox groupBox1;
        private Button selectFileButton;
        private Label keyFileLabel;
        private Button OKButton;
        private Button cancelButton;
        private CheckBox dpapiCheckBox;
        private GroupBox dpapiGroup;
        private LinkLabel dpapiHelpLink;

        private string path = String.Empty;
        private DpapiSettings dpapiSettings;
        private KeyAlgorithmPair keyAlgorithmPair;
        private Label dpapiTextLabel;
        private DpapiSettingsControl dpapiSettingsControl;

        private const int EntropySize = 16;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public IServiceProvider provider;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="keyAlgorithmPair">The existing key algorithm pair. Pass <c>null</c> for opening an existing key algorithm pair.</param>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public FileKeyAlgorithmStorageProviderWizard(KeyAlgorithmPair keyAlgorithmPair, IServiceProvider serviceProvider)
        {
            openMode = (keyAlgorithmPair == null);
            this.keyAlgorithmPair = keyAlgorithmPair;
            this.provider = serviceProvider;

            InitializeComponent();

            if (openMode)
            {
                introLabel.Text = SR.FileKeyAlgorithmWizardLoadIntroText;
                dpapiCheckBox.Text = SR.FileKeyAlgorithmWizardDpapiLoadLabelText;
            }
            else
            {
                introLabel.Text = SR.FileKeyAlgorithmWizardSaveIntroText;
                dpapiCheckBox.Text = SR.FileKeyAlgorithmWizardDpapiSaveLabelText;
            }

            dpapiTextLabel.Text = SR.DpapiSettingsFirstRunInstructionMessage;
            dpapiTextLabel.Enabled = false;
            dpapiSettingsControl.Enabled = false;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the selected file path.
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// Gets the selected DPAPI settings.
        /// </summary>
        /// <remarks>Will return <c>null</c> if not configured.</remarks>
        public DpapiSettings DpapiSettings
        {
            get { return dpapiSettings; }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            string filter = SR.KeyFilesString;

            if (openMode)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = filter;
                dialog.Title = SR.FileKeyAlgorithmWizardLoadIntroText;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.FileName;
                    keyFileLabel.Text = path;
                }
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = filter;
                dialog.Title = SR.FileKeyAlgorithmWizardSaveIntroText;
                dialog.CheckPathExists = true;
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.FileName;
                    keyFileLabel.Text = path;
        }
            }

            if (path != null && path.Length > 0)
            {
                OKButton.Enabled = true;
            }
        }

        private void dpapiHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                SR.DpapiInformationMessage,
                SR.DpapiInformationCaption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                if (dpapiCheckBox.Checked)
                {
                    if (dpapiSettingsControl.DpapiStorageMode == DpapiStorageMode.Machine && dpapiSettingsControl.Entropy.Length != EntropySize)
                    {
                        MessageBox.Show(SR.DpapiEntropyConvertErrorMessage, SR.DpapiEntropyMinSizeCaption,
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    this.dpapiSettings = new DpapiSettings(null);
                    this.dpapiSettings.Entropy = dpapiSettingsControl.Entropy;
                    this.dpapiSettings.Mode = dpapiSettingsControl.DpapiStorageMode;
                }
                else
                {
                    this.dpapiSettings = null;
                }

                if (openMode)
                {
                    // Make sure we can load the key
                    try
                    {
                        LoadKeyAlgorithmPair();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            SR.FileKeyAlgorithmWizardUnableToLoadKeyErrorMessage(ex.Message),
                            SR.FileKeyAlgorithmWizardUnableToLoadKeyCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );
                        return;
                    }
                }
                else
                {
                    try
                    {
                        SaveKeyAlgorithmPair();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            ex.Message,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SaveKeyAlgorithmPair()
        {
            FileKeyAlgorithmPairStorageProvider provider = GetCurrentProvider();

            provider.Save(this.keyAlgorithmPair);
        }

        private void LoadKeyAlgorithmPair()
        {
            FileKeyAlgorithmPairStorageProvider provider = GetCurrentProvider();

            if (provider.Load() == null)
            {
                throw new NullReferenceException();
            }
        }

        private ConfigurationContext GetContext()
        {
            IUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(provider);
            ConfigurationContext context = hierarchy.BuildConfigurationContext();
            ConfigurationSettings settings = context.GetMetaConfiguration();
            ConfigurationDictionary config = new ConfigurationDictionary();
            config.Add(ConfigurationSettings.SectionName, settings);
            context = new ConfigurationContext(config);
            return context;
        }

        private FileKeyAlgorithmPairStorageProvider GetCurrentProvider()
        {
            ConfigurationContext context = GetContext();
            FileKeyAlgorithmPairStorageProvider provider = new FileKeyAlgorithmPairStorageProvider();
            FileKeyAlgorithmPairStorageProviderData data = new FileKeyAlgorithmPairStorageProviderData();

            if (this.dpapiSettings != null)
            {
                data.DpapiSettings = new DpapiSettingsData(this.dpapiSettings.Entropy, this.dpapiSettings.Mode);
            }

            data.Path = this.path;
            data.Name = SR.DefaultFileKeyAlgorithmStorageProviderNodeName;
            ConfigurationSettings settings = context.GetMetaConfiguration();
            settings.KeyAlgorithmPairStorageProviderData = data;
            provider.ConfigurationName = data.Name;
            provider.Initialize(new RuntimeConfigurationView(context));

            return provider;
        }

        private void dpapiCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dpapiSettingsControl.Enabled = dpapiCheckBox.Checked;
            dpapiTextLabel.Enabled = dpapiCheckBox.Checked;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FileKeyAlgorithmStorageProviderWizard));
            this.introLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.keyFileLabel = new System.Windows.Forms.Label();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.dpapiGroup = new System.Windows.Forms.GroupBox();
            this.dpapiSettingsControl = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.DpapiSettingsControl();
            this.dpapiTextLabel = new System.Windows.Forms.Label();
            this.dpapiHelpLink = new System.Windows.Forms.LinkLabel();
            this.dpapiCheckBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.dpapiGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // introLabel
            // 
            this.introLabel.AccessibleDescription = resources.GetString("introLabel.AccessibleDescription");
            this.introLabel.AccessibleName = resources.GetString("introLabel.AccessibleName");
            this.introLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("introLabel.Anchor")));
            this.introLabel.AutoSize = ((bool)(resources.GetObject("introLabel.AutoSize")));
            this.introLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("introLabel.Dock")));
            this.introLabel.Enabled = ((bool)(resources.GetObject("introLabel.Enabled")));
            this.introLabel.Font = ((System.Drawing.Font)(resources.GetObject("introLabel.Font")));
            this.introLabel.Image = ((System.Drawing.Image)(resources.GetObject("introLabel.Image")));
            this.introLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("introLabel.ImageAlign")));
            this.introLabel.ImageIndex = ((int)(resources.GetObject("introLabel.ImageIndex")));
            this.introLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("introLabel.ImeMode")));
            this.introLabel.Location = ((System.Drawing.Point)(resources.GetObject("introLabel.Location")));
            this.introLabel.Name = "introLabel";
            this.introLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("introLabel.RightToLeft")));
            this.introLabel.Size = ((System.Drawing.Size)(resources.GetObject("introLabel.Size")));
            this.introLabel.TabIndex = ((int)(resources.GetObject("introLabel.TabIndex")));
            this.introLabel.Text = resources.GetString("introLabel.Text");
            this.introLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("introLabel.TextAlign")));
            this.introLabel.Visible = ((bool)(resources.GetObject("introLabel.Visible")));
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
            this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.keyFileLabel);
            this.groupBox1.Controls.Add(this.selectFileButton);
            this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
            this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
            this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
            this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
            this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
            this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = resources.GetString("groupBox1.Text");
            this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
            // 
            // keyFileLabel
            // 
            this.keyFileLabel.AccessibleDescription = resources.GetString("keyFileLabel.AccessibleDescription");
            this.keyFileLabel.AccessibleName = resources.GetString("keyFileLabel.AccessibleName");
            this.keyFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyFileLabel.Anchor")));
            this.keyFileLabel.AutoSize = ((bool)(resources.GetObject("keyFileLabel.AutoSize")));
            this.keyFileLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyFileLabel.Dock")));
            this.keyFileLabel.Enabled = ((bool)(resources.GetObject("keyFileLabel.Enabled")));
            this.keyFileLabel.Font = ((System.Drawing.Font)(resources.GetObject("keyFileLabel.Font")));
            this.keyFileLabel.Image = ((System.Drawing.Image)(resources.GetObject("keyFileLabel.Image")));
            this.keyFileLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyFileLabel.ImageAlign")));
            this.keyFileLabel.ImageIndex = ((int)(resources.GetObject("keyFileLabel.ImageIndex")));
            this.keyFileLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyFileLabel.ImeMode")));
            this.keyFileLabel.Location = ((System.Drawing.Point)(resources.GetObject("keyFileLabel.Location")));
            this.keyFileLabel.Name = "keyFileLabel";
            this.keyFileLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyFileLabel.RightToLeft")));
            this.keyFileLabel.Size = ((System.Drawing.Size)(resources.GetObject("keyFileLabel.Size")));
            this.keyFileLabel.TabIndex = ((int)(resources.GetObject("keyFileLabel.TabIndex")));
            this.keyFileLabel.Text = resources.GetString("keyFileLabel.Text");
            this.keyFileLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyFileLabel.TextAlign")));
            this.keyFileLabel.Visible = ((bool)(resources.GetObject("keyFileLabel.Visible")));
            // 
            // selectFileButton
            // 
            this.selectFileButton.AccessibleDescription = resources.GetString("selectFileButton.AccessibleDescription");
            this.selectFileButton.AccessibleName = resources.GetString("selectFileButton.AccessibleName");
            this.selectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("selectFileButton.Anchor")));
            this.selectFileButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectFileButton.BackgroundImage")));
            this.selectFileButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("selectFileButton.Dock")));
            this.selectFileButton.Enabled = ((bool)(resources.GetObject("selectFileButton.Enabled")));
            this.selectFileButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("selectFileButton.FlatStyle")));
            this.selectFileButton.Font = ((System.Drawing.Font)(resources.GetObject("selectFileButton.Font")));
            this.selectFileButton.Image = ((System.Drawing.Image)(resources.GetObject("selectFileButton.Image")));
            this.selectFileButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectFileButton.ImageAlign")));
            this.selectFileButton.ImageIndex = ((int)(resources.GetObject("selectFileButton.ImageIndex")));
            this.selectFileButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("selectFileButton.ImeMode")));
            this.selectFileButton.Location = ((System.Drawing.Point)(resources.GetObject("selectFileButton.Location")));
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("selectFileButton.RightToLeft")));
            this.selectFileButton.Size = ((System.Drawing.Size)(resources.GetObject("selectFileButton.Size")));
            this.selectFileButton.TabIndex = ((int)(resources.GetObject("selectFileButton.TabIndex")));
            this.selectFileButton.Text = resources.GetString("selectFileButton.Text");
            this.selectFileButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectFileButton.TextAlign")));
            this.selectFileButton.Visible = ((bool)(resources.GetObject("selectFileButton.Visible")));
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // dpapiGroup
            // 
            this.dpapiGroup.AccessibleDescription = resources.GetString("dpapiGroup.AccessibleDescription");
            this.dpapiGroup.AccessibleName = resources.GetString("dpapiGroup.AccessibleName");
            this.dpapiGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiGroup.Anchor")));
            this.dpapiGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dpapiGroup.BackgroundImage")));
            this.dpapiGroup.Controls.Add(this.dpapiSettingsControl);
            this.dpapiGroup.Controls.Add(this.dpapiTextLabel);
            this.dpapiGroup.Controls.Add(this.dpapiHelpLink);
            this.dpapiGroup.Controls.Add(this.dpapiCheckBox);
            this.dpapiGroup.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiGroup.Dock")));
            this.dpapiGroup.Enabled = ((bool)(resources.GetObject("dpapiGroup.Enabled")));
            this.dpapiGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dpapiGroup.Font = ((System.Drawing.Font)(resources.GetObject("dpapiGroup.Font")));
            this.dpapiGroup.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiGroup.ImeMode")));
            this.dpapiGroup.Location = ((System.Drawing.Point)(resources.GetObject("dpapiGroup.Location")));
            this.dpapiGroup.Name = "dpapiGroup";
            this.dpapiGroup.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiGroup.RightToLeft")));
            this.dpapiGroup.Size = ((System.Drawing.Size)(resources.GetObject("dpapiGroup.Size")));
            this.dpapiGroup.TabIndex = ((int)(resources.GetObject("dpapiGroup.TabIndex")));
            this.dpapiGroup.TabStop = false;
            this.dpapiGroup.Text = resources.GetString("dpapiGroup.Text");
            this.dpapiGroup.Visible = ((bool)(resources.GetObject("dpapiGroup.Visible")));
            // 
            // dpapiSettingsControl
            // 
            this.dpapiSettingsControl.AccessibleDescription = resources.GetString("dpapiSettingsControl.AccessibleDescription");
            this.dpapiSettingsControl.AccessibleName = resources.GetString("dpapiSettingsControl.AccessibleName");
            this.dpapiSettingsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiSettingsControl.Anchor")));
            this.dpapiSettingsControl.AutoScroll = ((bool)(resources.GetObject("dpapiSettingsControl.AutoScroll")));
            this.dpapiSettingsControl.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl.AutoScrollMargin")));
            this.dpapiSettingsControl.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl.AutoScrollMinSize")));
            this.dpapiSettingsControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dpapiSettingsControl.BackgroundImage")));
            this.dpapiSettingsControl.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiSettingsControl.Dock")));
            this.dpapiSettingsControl.DpapiStorageMode = DpapiStorageMode.None;
            this.dpapiSettingsControl.Enabled = ((bool)(resources.GetObject("dpapiSettingsControl.Enabled")));
            this.dpapiSettingsControl.Entropy = new System.Byte[0];
            this.dpapiSettingsControl.Font = ((System.Drawing.Font)(resources.GetObject("dpapiSettingsControl.Font")));
            this.dpapiSettingsControl.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiSettingsControl.ImeMode")));
            this.dpapiSettingsControl.Location = ((System.Drawing.Point)(resources.GetObject("dpapiSettingsControl.Location")));
            this.dpapiSettingsControl.ModeRequired = true;
            this.dpapiSettingsControl.Name = "dpapiSettingsControl";
            this.dpapiSettingsControl.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiSettingsControl.RightToLeft")));
            this.dpapiSettingsControl.Size = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl.Size")));
            this.dpapiSettingsControl.TabIndex = ((int)(resources.GetObject("dpapiSettingsControl.TabIndex")));
            this.dpapiSettingsControl.Visible = ((bool)(resources.GetObject("dpapiSettingsControl.Visible")));
            // 
            // dpapiTextLabel
            // 
            this.dpapiTextLabel.AccessibleDescription = resources.GetString("dpapiTextLabel.AccessibleDescription");
            this.dpapiTextLabel.AccessibleName = resources.GetString("dpapiTextLabel.AccessibleName");
            this.dpapiTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiTextLabel.Anchor")));
            this.dpapiTextLabel.AutoSize = ((bool)(resources.GetObject("dpapiTextLabel.AutoSize")));
            this.dpapiTextLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiTextLabel.Dock")));
            this.dpapiTextLabel.Enabled = ((bool)(resources.GetObject("dpapiTextLabel.Enabled")));
            this.dpapiTextLabel.Font = ((System.Drawing.Font)(resources.GetObject("dpapiTextLabel.Font")));
            this.dpapiTextLabel.Image = ((System.Drawing.Image)(resources.GetObject("dpapiTextLabel.Image")));
            this.dpapiTextLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiTextLabel.ImageAlign")));
            this.dpapiTextLabel.ImageIndex = ((int)(resources.GetObject("dpapiTextLabel.ImageIndex")));
            this.dpapiTextLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiTextLabel.ImeMode")));
            this.dpapiTextLabel.Location = ((System.Drawing.Point)(resources.GetObject("dpapiTextLabel.Location")));
            this.dpapiTextLabel.Name = "dpapiTextLabel";
            this.dpapiTextLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiTextLabel.RightToLeft")));
            this.dpapiTextLabel.Size = ((System.Drawing.Size)(resources.GetObject("dpapiTextLabel.Size")));
            this.dpapiTextLabel.TabIndex = ((int)(resources.GetObject("dpapiTextLabel.TabIndex")));
            this.dpapiTextLabel.Text = resources.GetString("dpapiTextLabel.Text");
            this.dpapiTextLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiTextLabel.TextAlign")));
            this.dpapiTextLabel.Visible = ((bool)(resources.GetObject("dpapiTextLabel.Visible")));
            // 
            // dpapiHelpLink
            // 
            this.dpapiHelpLink.AccessibleDescription = resources.GetString("dpapiHelpLink.AccessibleDescription");
            this.dpapiHelpLink.AccessibleName = resources.GetString("dpapiHelpLink.AccessibleName");
            this.dpapiHelpLink.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiHelpLink.Anchor")));
            this.dpapiHelpLink.AutoSize = ((bool)(resources.GetObject("dpapiHelpLink.AutoSize")));
            this.dpapiHelpLink.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiHelpLink.Dock")));
            this.dpapiHelpLink.Enabled = ((bool)(resources.GetObject("dpapiHelpLink.Enabled")));
            this.dpapiHelpLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dpapiHelpLink.Font = ((System.Drawing.Font)(resources.GetObject("dpapiHelpLink.Font")));
            this.dpapiHelpLink.Image = ((System.Drawing.Image)(resources.GetObject("dpapiHelpLink.Image")));
            this.dpapiHelpLink.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiHelpLink.ImageAlign")));
            this.dpapiHelpLink.ImageIndex = ((int)(resources.GetObject("dpapiHelpLink.ImageIndex")));
            this.dpapiHelpLink.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiHelpLink.ImeMode")));
            this.dpapiHelpLink.LinkArea = ((System.Windows.Forms.LinkArea)(resources.GetObject("dpapiHelpLink.LinkArea")));
            this.dpapiHelpLink.LinkColor = System.Drawing.Color.Blue;
            this.dpapiHelpLink.Location = ((System.Drawing.Point)(resources.GetObject("dpapiHelpLink.Location")));
            this.dpapiHelpLink.Name = "dpapiHelpLink";
            this.dpapiHelpLink.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiHelpLink.RightToLeft")));
            this.dpapiHelpLink.Size = ((System.Drawing.Size)(resources.GetObject("dpapiHelpLink.Size")));
            this.dpapiHelpLink.TabIndex = ((int)(resources.GetObject("dpapiHelpLink.TabIndex")));
            this.dpapiHelpLink.TabStop = true;
            this.dpapiHelpLink.Text = resources.GetString("dpapiHelpLink.Text");
            this.dpapiHelpLink.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiHelpLink.TextAlign")));
            this.dpapiHelpLink.Visible = ((bool)(resources.GetObject("dpapiHelpLink.Visible")));
            this.dpapiHelpLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.dpapiHelpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.dpapiHelpLink_LinkClicked);
            // 
            // dpapiCheckBox
            // 
            this.dpapiCheckBox.AccessibleDescription = resources.GetString("dpapiCheckBox.AccessibleDescription");
            this.dpapiCheckBox.AccessibleName = resources.GetString("dpapiCheckBox.AccessibleName");
            this.dpapiCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiCheckBox.Anchor")));
            this.dpapiCheckBox.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("dpapiCheckBox.Appearance")));
            this.dpapiCheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dpapiCheckBox.BackgroundImage")));
            this.dpapiCheckBox.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiCheckBox.CheckAlign")));
            this.dpapiCheckBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiCheckBox.Dock")));
            this.dpapiCheckBox.Enabled = ((bool)(resources.GetObject("dpapiCheckBox.Enabled")));
            this.dpapiCheckBox.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("dpapiCheckBox.FlatStyle")));
            this.dpapiCheckBox.Font = ((System.Drawing.Font)(resources.GetObject("dpapiCheckBox.Font")));
            this.dpapiCheckBox.Image = ((System.Drawing.Image)(resources.GetObject("dpapiCheckBox.Image")));
            this.dpapiCheckBox.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiCheckBox.ImageAlign")));
            this.dpapiCheckBox.ImageIndex = ((int)(resources.GetObject("dpapiCheckBox.ImageIndex")));
            this.dpapiCheckBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiCheckBox.ImeMode")));
            this.dpapiCheckBox.Location = ((System.Drawing.Point)(resources.GetObject("dpapiCheckBox.Location")));
            this.dpapiCheckBox.Name = "dpapiCheckBox";
            this.dpapiCheckBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiCheckBox.RightToLeft")));
            this.dpapiCheckBox.Size = ((System.Drawing.Size)(resources.GetObject("dpapiCheckBox.Size")));
            this.dpapiCheckBox.TabIndex = ((int)(resources.GetObject("dpapiCheckBox.TabIndex")));
            this.dpapiCheckBox.Text = resources.GetString("dpapiCheckBox.Text");
            this.dpapiCheckBox.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("dpapiCheckBox.TextAlign")));
            this.dpapiCheckBox.Visible = ((bool)(resources.GetObject("dpapiCheckBox.Visible")));
            this.dpapiCheckBox.CheckedChanged += new System.EventHandler(this.dpapiCheckBox_CheckedChanged);
            // 
            // OKButton
            // 
            this.OKButton.AccessibleDescription = resources.GetString("OKButton.AccessibleDescription");
            this.OKButton.AccessibleName = resources.GetString("OKButton.AccessibleName");
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("OKButton.Anchor")));
            this.OKButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OKButton.BackgroundImage")));
            this.OKButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("OKButton.Dock")));
            this.OKButton.Enabled = ((bool)(resources.GetObject("OKButton.Enabled")));
            this.OKButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("OKButton.FlatStyle")));
            this.OKButton.Font = ((System.Drawing.Font)(resources.GetObject("OKButton.Font")));
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OKButton.ImageAlign")));
            this.OKButton.ImageIndex = ((int)(resources.GetObject("OKButton.ImageIndex")));
            this.OKButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("OKButton.ImeMode")));
            this.OKButton.Location = ((System.Drawing.Point)(resources.GetObject("OKButton.Location")));
            this.OKButton.Name = "OKButton";
            this.OKButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("OKButton.RightToLeft")));
            this.OKButton.Size = ((System.Drawing.Size)(resources.GetObject("OKButton.Size")));
            this.OKButton.TabIndex = ((int)(resources.GetObject("OKButton.TabIndex")));
            this.OKButton.Text = resources.GetString("OKButton.Text");
            this.OKButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OKButton.TextAlign")));
            this.OKButton.Visible = ((bool)(resources.GetObject("OKButton.Visible")));
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            // 
            // FileKeyAlgorithmStorageProviderWizard
            // 
            this.AcceptButton = this.OKButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.dpapiGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.introLabel);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "FileKeyAlgorithmStorageProviderWizard";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox1.ResumeLayout(false);
            this.dpapiGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
                }
            }
