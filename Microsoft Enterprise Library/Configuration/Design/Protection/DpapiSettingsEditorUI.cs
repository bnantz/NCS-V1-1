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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// The user interface for the <see cref="DpapiSettingsEditor"/>.
    /// </summary>
    public class DpapiSettingsEditorUI : Form
    {
        private DpapiSettingsData data;
        private bool firstRun = true;
        private bool required = true;
        private DpapiSettingsControl dpapiSettingsControl1;
        private DpapiSettingsControl dpapiSettingsControl;

        private const int EntropySize = 16;
        private string confirmMessage = SR.DpapiChangeConfirmMessage;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DpapiSettingsEditorUI"/> class determining if seettings are required or not.</para>
        /// </summary>
        /// <param name="required">Determines if DPAPI settings are required.</param>
        public DpapiSettingsEditorUI(bool required)
        {
            //dpapiSettingsControl = new DpapiSettingsControl(required);
            InitializeComponent();

            this.required = required;
            dpapiSettingsControl.ModeRequired = required;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DpapiSettingsEditorUI"/> class.</para>
        /// </summary>
        public DpapiSettingsEditorUI() : this(true)
        {
        }

        #region Form Designer
        private Container components = null;
        private GroupBox grpDpapiMode;
        private Button btnOK;
        private Label lblInstructions;
        private Button btnCancel;

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="DpapiSettingsEditorUI"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DpapiSettingsEditorUI));
            this.grpDpapiMode = new System.Windows.Forms.GroupBox();
            this.dpapiSettingsControl = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.DpapiSettingsControl();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.dpapiSettingsControl1 = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.DpapiSettingsControl();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpDpapiMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDpapiMode
            // 
            this.grpDpapiMode.AccessibleDescription = resources.GetString("grpDpapiMode.AccessibleDescription");
            this.grpDpapiMode.AccessibleName = resources.GetString("grpDpapiMode.AccessibleName");
            this.grpDpapiMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDpapiMode.Anchor")));
            this.grpDpapiMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDpapiMode.BackgroundImage")));
            this.grpDpapiMode.Controls.Add(this.dpapiSettingsControl);
            this.grpDpapiMode.Controls.Add(this.lblInstructions);
            this.grpDpapiMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDpapiMode.Dock")));
            this.grpDpapiMode.Enabled = ((bool)(resources.GetObject("grpDpapiMode.Enabled")));
            this.grpDpapiMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpDpapiMode.Font = ((System.Drawing.Font)(resources.GetObject("grpDpapiMode.Font")));
            this.grpDpapiMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDpapiMode.ImeMode")));
            this.grpDpapiMode.Location = ((System.Drawing.Point)(resources.GetObject("grpDpapiMode.Location")));
            this.grpDpapiMode.Name = "grpDpapiMode";
            this.grpDpapiMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDpapiMode.RightToLeft")));
            this.grpDpapiMode.Size = ((System.Drawing.Size)(resources.GetObject("grpDpapiMode.Size")));
            this.grpDpapiMode.TabIndex = ((int)(resources.GetObject("grpDpapiMode.TabIndex")));
            this.grpDpapiMode.TabStop = false;
            this.grpDpapiMode.Text = resources.GetString("grpDpapiMode.Text");
            this.grpDpapiMode.Visible = ((bool)(resources.GetObject("grpDpapiMode.Visible")));
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
            this.dpapiSettingsControl.DpapiStorageMode = DpapiStorageMode.User;
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
            // lblInstructions
            // 
            this.lblInstructions.AccessibleDescription = resources.GetString("lblInstructions.AccessibleDescription");
            this.lblInstructions.AccessibleName = resources.GetString("lblInstructions.AccessibleName");
            this.lblInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblInstructions.Anchor")));
            this.lblInstructions.AutoSize = ((bool)(resources.GetObject("lblInstructions.AutoSize")));
            this.lblInstructions.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblInstructions.Dock")));
            this.lblInstructions.Enabled = ((bool)(resources.GetObject("lblInstructions.Enabled")));
            this.lblInstructions.Font = ((System.Drawing.Font)(resources.GetObject("lblInstructions.Font")));
            this.lblInstructions.Image = ((System.Drawing.Image)(resources.GetObject("lblInstructions.Image")));
            this.lblInstructions.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblInstructions.ImageAlign")));
            this.lblInstructions.ImageIndex = ((int)(resources.GetObject("lblInstructions.ImageIndex")));
            this.lblInstructions.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblInstructions.ImeMode")));
            this.lblInstructions.Location = ((System.Drawing.Point)(resources.GetObject("lblInstructions.Location")));
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblInstructions.RightToLeft")));
            this.lblInstructions.Size = ((System.Drawing.Size)(resources.GetObject("lblInstructions.Size")));
            this.lblInstructions.TabIndex = ((int)(resources.GetObject("lblInstructions.TabIndex")));
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            this.lblInstructions.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblInstructions.TextAlign")));
            this.lblInstructions.Visible = ((bool)(resources.GetObject("lblInstructions.Visible")));
            // 
            // dpapiSettingsControl1
            // 
            this.dpapiSettingsControl1.AccessibleDescription = resources.GetString("dpapiSettingsControl1.AccessibleDescription");
            this.dpapiSettingsControl1.AccessibleName = resources.GetString("dpapiSettingsControl1.AccessibleName");
            this.dpapiSettingsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("dpapiSettingsControl1.Anchor")));
            this.dpapiSettingsControl1.AutoScroll = ((bool)(resources.GetObject("dpapiSettingsControl1.AutoScroll")));
            this.dpapiSettingsControl1.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl1.AutoScrollMargin")));
            this.dpapiSettingsControl1.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl1.AutoScrollMinSize")));
            this.dpapiSettingsControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dpapiSettingsControl1.BackgroundImage")));
            this.dpapiSettingsControl1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("dpapiSettingsControl1.Dock")));
            this.dpapiSettingsControl1.DpapiStorageMode = DpapiStorageMode.User;
            this.dpapiSettingsControl1.Enabled = ((bool)(resources.GetObject("dpapiSettingsControl1.Enabled")));
            this.dpapiSettingsControl1.Entropy = new System.Byte[0];
            this.dpapiSettingsControl1.Font = ((System.Drawing.Font)(resources.GetObject("dpapiSettingsControl1.Font")));
            this.dpapiSettingsControl1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("dpapiSettingsControl1.ImeMode")));
            this.dpapiSettingsControl1.Location = ((System.Drawing.Point)(resources.GetObject("dpapiSettingsControl1.Location")));
            this.dpapiSettingsControl1.ModeRequired = true;
            this.dpapiSettingsControl1.Name = "dpapiSettingsControl1";
            this.dpapiSettingsControl1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("dpapiSettingsControl1.RightToLeft")));
            this.dpapiSettingsControl1.Size = ((System.Drawing.Size)(resources.GetObject("dpapiSettingsControl1.Size")));
            this.dpapiSettingsControl1.TabIndex = ((int)(resources.GetObject("dpapiSettingsControl1.TabIndex")));
            this.dpapiSettingsControl1.Visible = ((bool)(resources.GetObject("dpapiSettingsControl1.Visible")));
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
            this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnOK.Anchor")));
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnOK.Dock")));
            this.btnOK.Enabled = ((bool)(resources.GetObject("btnOK.Enabled")));
            this.btnOK.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnOK.FlatStyle")));
            this.btnOK.Font = ((System.Drawing.Font)(resources.GetObject("btnOK.Font")));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.ImageAlign")));
            this.btnOK.ImageIndex = ((int)(resources.GetObject("btnOK.ImageIndex")));
            this.btnOK.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnOK.ImeMode")));
            this.btnOK.Location = ((System.Drawing.Point)(resources.GetObject("btnOK.Location")));
            this.btnOK.Name = "btnOK";
            this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnOK.RightToLeft")));
            this.btnOK.Size = ((System.Drawing.Size)(resources.GetObject("btnOK.Size")));
            this.btnOK.TabIndex = ((int)(resources.GetObject("btnOK.TabIndex")));
            this.btnOK.Text = resources.GetString("btnOK.Text");
            this.btnOK.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.TextAlign")));
            this.btnOK.Visible = ((bool)(resources.GetObject("btnOK.Visible")));
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription");
            this.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName");
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnCancel.Anchor")));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnCancel.Dock")));
            this.btnCancel.Enabled = ((bool)(resources.GetObject("btnCancel.Enabled")));
            this.btnCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnCancel.FlatStyle")));
            this.btnCancel.Font = ((System.Drawing.Font)(resources.GetObject("btnCancel.Font")));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.ImageAlign")));
            this.btnCancel.ImageIndex = ((int)(resources.GetObject("btnCancel.ImageIndex")));
            this.btnCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnCancel.ImeMode")));
            this.btnCancel.Location = ((System.Drawing.Point)(resources.GetObject("btnCancel.Location")));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnCancel.RightToLeft")));
            this.btnCancel.Size = ((System.Drawing.Size)(resources.GetObject("btnCancel.Size")));
            this.btnCancel.TabIndex = ((int)(resources.GetObject("btnCancel.TabIndex")));
            this.btnCancel.Text = resources.GetString("btnCancel.Text");
            this.btnCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.TextAlign")));
            this.btnCancel.Visible = ((bool)(resources.GetObject("btnCancel.Visible")));
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DpapiSettingsEditorUI
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnCancel;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpDpapiMode);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "DpapiSettingsEditorUI";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.DpapiSettingsUI_Load);
            this.grpDpapiMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        /// <summary>
        /// DPAPI settings for this instance.
        /// </summary>
        private void DpapiSettingsUI_Load(object sender, EventArgs e)
        {
            firstRun = (this.data == null);
            DisplaySettings();
        }

        /// <summary>
        /// Gets or sets the DPAPI settings data.
        /// </summary>
        public DpapiSettingsData DpapiSettingsData
        {
            get { return this.data; }
            set { this.data = value; }
        }

        /// <summary>
        /// Gets or sets the DPAPI confirmation message when
        /// changing the DPAPI settings on an existing item.
        /// </summary>
        public string ConfirmMessage
        {
            get { return this.confirmMessage; }
            set { this.confirmMessage = value; }
        }

        /// <summary>
        /// Controls the display of the settings depending on whether or not the settings can be displayed.
        /// </summary>
        public void DisplaySettings()
        {
            lblInstructions.Text = SR.DpapiSettingsFirstRunInstructionMessage;

            if (data == null)
            {
                dpapiSettingsControl.DpapiStorageMode = DpapiStorageMode.None;

            }
            else
            {
                switch (data.Mode)
                {
                    case DpapiStorageMode.Machine:
                        dpapiSettingsControl.DpapiStorageMode = DpapiStorageMode.Machine;
                        dpapiSettingsControl.Entropy = this.data.Entropy;
                        break;
                    case DpapiStorageMode.User:
                        dpapiSettingsControl.DpapiStorageMode = DpapiStorageMode.User;
                        break;
                }
            }
        }

        private bool Save()
        {
            bool result = true;

            if (firstRun)
            {
                this.data = new DpapiSettingsData();
            }
            else
            {
                if (!ConfirmSave())
                {
                    return false;
                }
            }

            switch (dpapiSettingsControl.DpapiStorageMode)
            {
                case DpapiStorageMode.User:
                    this.data.Mode = DpapiStorageMode.User;
                    this.data.Entropy = null;
                    break;
                case DpapiStorageMode.Machine:
                    result = SaveMachineMode();
                    break;
                default:
                    if (required)
                    {
                        MessageBox.Show(SR.DpapiMissingModeErrorMessage, SR.DpapiMissingModeCaption,
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        result = false;
                    }
                    else
                    {
                        this.data = null;
                    }
                    break;
            }

            return result;
        }

        private bool ConfirmSave()
        {
            bool confirmed = false;

            DialogResult result = MessageBox.Show(ConfirmMessage, SR.DpapiChangeConfirmCaption,
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                confirmed = true;
            }
            return confirmed;
        }

        private bool SaveMachineMode()
        {
            this.data.Mode = DpapiStorageMode.Machine;
            this.data.Entropy = dpapiSettingsControl.Entropy;

            if (data.Entropy.Length != EntropySize)
            {
                MessageBox.Show(SR.DpapiEntropyConvertErrorMessage, SR.DpapiEntropyMinSizeCaption,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}