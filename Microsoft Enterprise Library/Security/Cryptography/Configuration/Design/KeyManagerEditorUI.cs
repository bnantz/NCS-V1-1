//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// The form for the <c>KeyManagerEditor</c>.
    /// </summary>
    public class KeyManagerEditorUI : Form
    {
        private bool editMode;
        private KeySettings keySettings;

        #region Form Designer
        private Button importKeyButton;
        private Button exportKeyButton;
        private Button OKButton;
        private Button cancelButton;
        private Label keyHiddenLabel;
        private StatusBar statusBar;
        private StatusBarPanel statusPanel;
        private StatusBarPanel keySizePanel;
        private KeyEditor keyEditor;
        private Container components = null;

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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(KeyManagerEditorUI));
            this.keyHiddenLabel = new System.Windows.Forms.Label();
            this.exportKeyButton = new System.Windows.Forms.Button();
            this.importKeyButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusPanel = new System.Windows.Forms.StatusBarPanel();
            this.keySizePanel = new System.Windows.Forms.StatusBarPanel();
            this.keyEditor = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.KeyEditor();
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.keySizePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // keyHiddenLabel
            // 
            this.keyHiddenLabel.AccessibleDescription = resources.GetString("keyHiddenLabel.AccessibleDescription");
            this.keyHiddenLabel.AccessibleName = resources.GetString("keyHiddenLabel.AccessibleName");
            this.keyHiddenLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyHiddenLabel.Anchor")));
            this.keyHiddenLabel.AutoSize = ((bool)(resources.GetObject("keyHiddenLabel.AutoSize")));
            this.keyHiddenLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyHiddenLabel.Dock")));
            this.keyHiddenLabel.Enabled = ((bool)(resources.GetObject("keyHiddenLabel.Enabled")));
            this.keyHiddenLabel.Font = ((System.Drawing.Font)(resources.GetObject("keyHiddenLabel.Font")));
            this.keyHiddenLabel.Image = ((System.Drawing.Image)(resources.GetObject("keyHiddenLabel.Image")));
            this.keyHiddenLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyHiddenLabel.ImageAlign")));
            this.keyHiddenLabel.ImageIndex = ((int)(resources.GetObject("keyHiddenLabel.ImageIndex")));
            this.keyHiddenLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyHiddenLabel.ImeMode")));
            this.keyHiddenLabel.Location = ((System.Drawing.Point)(resources.GetObject("keyHiddenLabel.Location")));
            this.keyHiddenLabel.Name = "keyHiddenLabel";
            this.keyHiddenLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyHiddenLabel.RightToLeft")));
            this.keyHiddenLabel.Size = ((System.Drawing.Size)(resources.GetObject("keyHiddenLabel.Size")));
            this.keyHiddenLabel.TabIndex = ((int)(resources.GetObject("keyHiddenLabel.TabIndex")));
            this.keyHiddenLabel.Text = resources.GetString("keyHiddenLabel.Text");
            this.keyHiddenLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyHiddenLabel.TextAlign")));
            this.keyHiddenLabel.Visible = ((bool)(resources.GetObject("keyHiddenLabel.Visible")));
            // 
            // exportKeyButton
            // 
            this.exportKeyButton.AccessibleDescription = resources.GetString("exportKeyButton.AccessibleDescription");
            this.exportKeyButton.AccessibleName = resources.GetString("exportKeyButton.AccessibleName");
            this.exportKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("exportKeyButton.Anchor")));
            this.exportKeyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("exportKeyButton.BackgroundImage")));
            this.exportKeyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("exportKeyButton.Dock")));
            this.exportKeyButton.Enabled = ((bool)(resources.GetObject("exportKeyButton.Enabled")));
            this.exportKeyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("exportKeyButton.FlatStyle")));
            this.exportKeyButton.Font = ((System.Drawing.Font)(resources.GetObject("exportKeyButton.Font")));
            this.exportKeyButton.Image = ((System.Drawing.Image)(resources.GetObject("exportKeyButton.Image")));
            this.exportKeyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("exportKeyButton.ImageAlign")));
            this.exportKeyButton.ImageIndex = ((int)(resources.GetObject("exportKeyButton.ImageIndex")));
            this.exportKeyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("exportKeyButton.ImeMode")));
            this.exportKeyButton.Location = ((System.Drawing.Point)(resources.GetObject("exportKeyButton.Location")));
            this.exportKeyButton.Name = "exportKeyButton";
            this.exportKeyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("exportKeyButton.RightToLeft")));
            this.exportKeyButton.Size = ((System.Drawing.Size)(resources.GetObject("exportKeyButton.Size")));
            this.exportKeyButton.TabIndex = ((int)(resources.GetObject("exportKeyButton.TabIndex")));
            this.exportKeyButton.Text = resources.GetString("exportKeyButton.Text");
            this.exportKeyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("exportKeyButton.TextAlign")));
            this.exportKeyButton.Visible = ((bool)(resources.GetObject("exportKeyButton.Visible")));
            this.exportKeyButton.Click += new System.EventHandler(this.exportKeyButton_Click);
            // 
            // importKeyButton
            // 
            this.importKeyButton.AccessibleDescription = resources.GetString("importKeyButton.AccessibleDescription");
            this.importKeyButton.AccessibleName = resources.GetString("importKeyButton.AccessibleName");
            this.importKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("importKeyButton.Anchor")));
            this.importKeyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("importKeyButton.BackgroundImage")));
            this.importKeyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("importKeyButton.Dock")));
            this.importKeyButton.Enabled = ((bool)(resources.GetObject("importKeyButton.Enabled")));
            this.importKeyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("importKeyButton.FlatStyle")));
            this.importKeyButton.Font = ((System.Drawing.Font)(resources.GetObject("importKeyButton.Font")));
            this.importKeyButton.Image = ((System.Drawing.Image)(resources.GetObject("importKeyButton.Image")));
            this.importKeyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("importKeyButton.ImageAlign")));
            this.importKeyButton.ImageIndex = ((int)(resources.GetObject("importKeyButton.ImageIndex")));
            this.importKeyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("importKeyButton.ImeMode")));
            this.importKeyButton.Location = ((System.Drawing.Point)(resources.GetObject("importKeyButton.Location")));
            this.importKeyButton.Name = "importKeyButton";
            this.importKeyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("importKeyButton.RightToLeft")));
            this.importKeyButton.Size = ((System.Drawing.Size)(resources.GetObject("importKeyButton.Size")));
            this.importKeyButton.TabIndex = ((int)(resources.GetObject("importKeyButton.TabIndex")));
            this.importKeyButton.Text = resources.GetString("importKeyButton.Text");
            this.importKeyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("importKeyButton.TextAlign")));
            this.importKeyButton.Visible = ((bool)(resources.GetObject("importKeyButton.Visible")));
            this.importKeyButton.Click += new System.EventHandler(this.importKeyButton_Click);
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
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // statusBar
            // 
            this.statusBar.AccessibleDescription = resources.GetString("statusBar.AccessibleDescription");
            this.statusBar.AccessibleName = resources.GetString("statusBar.AccessibleName");
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("statusBar.Anchor")));
            this.statusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar.BackgroundImage")));
            this.statusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("statusBar.Dock")));
            this.statusBar.Enabled = ((bool)(resources.GetObject("statusBar.Enabled")));
            this.statusBar.Font = ((System.Drawing.Font)(resources.GetObject("statusBar.Font")));
            this.statusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("statusBar.ImeMode")));
            this.statusBar.Location = ((System.Drawing.Point)(resources.GetObject("statusBar.Location")));
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                         this.statusPanel,
                                                                                         this.keySizePanel});
            this.statusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("statusBar.RightToLeft")));
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = ((System.Drawing.Size)(resources.GetObject("statusBar.Size")));
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = ((int)(resources.GetObject("statusBar.TabIndex")));
            this.statusBar.Text = resources.GetString("statusBar.Text");
            this.statusBar.Visible = ((bool)(resources.GetObject("statusBar.Visible")));
            // 
            // statusPanel
            // 
            this.statusPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusPanel.Alignment")));
            this.statusPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusPanel.Icon")));
            this.statusPanel.MinWidth = ((int)(resources.GetObject("statusPanel.MinWidth")));
            this.statusPanel.Text = resources.GetString("statusPanel.Text");
            this.statusPanel.ToolTipText = resources.GetString("statusPanel.ToolTipText");
            this.statusPanel.Width = ((int)(resources.GetObject("statusPanel.Width")));
            // 
            // keySizePanel
            // 
            this.keySizePanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("keySizePanel.Alignment")));
            this.keySizePanel.Icon = ((System.Drawing.Icon)(resources.GetObject("keySizePanel.Icon")));
            this.keySizePanel.MinWidth = ((int)(resources.GetObject("keySizePanel.MinWidth")));
            this.keySizePanel.Text = resources.GetString("keySizePanel.Text");
            this.keySizePanel.ToolTipText = resources.GetString("keySizePanel.ToolTipText");
            this.keySizePanel.Width = ((int)(resources.GetObject("keySizePanel.Width")));
            // 
            // keyEditor
            // 
            this.keyEditor.AccessibleDescription = resources.GetString("keyEditor.AccessibleDescription");
            this.keyEditor.AccessibleName = resources.GetString("keyEditor.AccessibleName");
            this.keyEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyEditor.Anchor")));
            this.keyEditor.AutoScroll = ((bool)(resources.GetObject("keyEditor.AutoScroll")));
            this.keyEditor.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("keyEditor.AutoScrollMargin")));
            this.keyEditor.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("keyEditor.AutoScrollMinSize")));
            this.keyEditor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyEditor.BackgroundImage")));
            this.keyEditor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyEditor.Dock")));
            this.keyEditor.Enabled = ((bool)(resources.GetObject("keyEditor.Enabled")));
            this.keyEditor.Font = ((System.Drawing.Font)(resources.GetObject("keyEditor.Font")));
            this.keyEditor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyEditor.ImeMode")));
            this.keyEditor.Key = new System.Byte[0];
            this.keyEditor.Location = ((System.Drawing.Point)(resources.GetObject("keyEditor.Location")));
            this.keyEditor.Name = "keyEditor";
            this.keyEditor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyEditor.RightToLeft")));
            this.keyEditor.Size = ((System.Drawing.Size)(resources.GetObject("keyEditor.Size")));
            this.keyEditor.TabIndex = ((int)(resources.GetObject("keyEditor.TabIndex")));
            this.keyEditor.Visible = ((bool)(resources.GetObject("keyEditor.Visible")));
            // 
            // KeyManagerUI
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
            this.Controls.Add(this.keyHiddenLabel);
            this.Controls.Add(this.exportKeyButton);
            this.Controls.Add(this.importKeyButton);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.keyEditor);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "KeyManagerUI";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.KeyManagerUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.keySizePanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        /// <summary>
        /// Initializes with a <c>KeyInfo</c>.
        /// </summary>
        /// <param name="keySettings">The key to manage.</param>
        public KeyManagerEditorUI(KeySettings keySettings)
        {
            this.keySettings = keySettings;

            InitializeComponent();

            keyEditor.TextChanged += new EventHandler(keyEditor_TextChanged);
            keyEditor.KeySettings = keySettings;

        }

        /// <summary>
        /// Gets a <c>KeyInfo</c> based on the current settings in the dialog.
        /// </summary>
        public KeySettings KeySettings
        {
            get
            {
                //this.keyInfo.Key = GetKeyBox();
                return this.keySettings;
            }
        }

        private void KeyManagerUI_Load(object sender, EventArgs e)
        {
            if (this.keySettings.Key != null && this.keySettings.Key.Length > 0)
            {
                this.editMode = false;
                keyEditor.Enabled = false;
                keyHiddenLabel.Visible = true;
                keyHiddenLabel.Text = SR.KeyManagerUIKeyBoxEncrypted;
                importKeyButton.Enabled = false;
            }
            else
            {
                this.editMode = true;
                keyEditor.Enabled = true;
                exportKeyButton.Enabled = false;
                keyHiddenLabel.Visible = false;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (this.editMode)
            {
                this.keySettings.Key = keyEditor.Key;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void exportKeyButton_Click(object sender, EventArgs e)
        {
            ExportKey form = new ExportKey(this.keySettings.Key);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void importKeyButton_Click(object sender, EventArgs e)
        {
            ImportKey form = new ImportKey();
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                keyEditor.Key = form.ImportedKey;
            }
        }

        private void RefreshCurrentKeySize()
        {
            int length = keyEditor.KeyLength;
            statusBar.Panels[1].Text = SR.KeyManagerUICurrentKeySizeCaption + " " + length.ToString(CultureInfo.CurrentCulture) + " " + SR.Bit;
        }

        private void keyEditor_TextChanged(object sender, EventArgs e)
        {
            string message = String.Empty;
            bool valid = keyEditor.KeyIsValid(out message);
            statusBar.Panels[0].Text = message;
            OKButton.Enabled = valid;
            if (!valid)
            {
                statusBar.Panels[1].Text = SR.KeyManagerUIInvalidKey;
            }
            else
            {
                RefreshCurrentKeySize();
            }
        }
    }
}