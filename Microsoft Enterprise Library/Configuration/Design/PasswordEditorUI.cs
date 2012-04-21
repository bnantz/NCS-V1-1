//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// The ui for the PasswordEditor.
    /// </summary>
    internal class PasswordEditorUI : Form
    {
        private TextBox passwordTextBox;
        private TextBox confirmPasswordTextBox;
        private Label passwordLabel;
        private Label confirmPasswordLabel;
        private Button cancelButton;
        private Button OKButton;
        private IContainer components = null;

        public PasswordEditorUI()
        {
            InitializeComponent();
        }
        
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

        public string Password
        {
            get { return passwordTextBox.Text; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad (e);
            passwordTextBox.Select();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == confirmPasswordTextBox.Text)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(SR.MismatchedPasswordMessage, SR.MismatchedPasswordCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                passwordTextBox.Clear();
                confirmPasswordTextBox.Clear();
                passwordTextBox.Select();
            }
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PasswordEditorUI));
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.AccessibleDescription = resources.GetString("passwordTextBox.AccessibleDescription");
            this.passwordTextBox.AccessibleName = resources.GetString("passwordTextBox.AccessibleName");
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("passwordTextBox.Anchor")));
            this.passwordTextBox.AutoSize = ((bool)(resources.GetObject("passwordTextBox.AutoSize")));
            this.passwordTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("passwordTextBox.BackgroundImage")));
            this.passwordTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("passwordTextBox.Dock")));
            this.passwordTextBox.Enabled = ((bool)(resources.GetObject("passwordTextBox.Enabled")));
            this.passwordTextBox.Font = ((System.Drawing.Font)(resources.GetObject("passwordTextBox.Font")));
            this.passwordTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("passwordTextBox.ImeMode")));
            this.passwordTextBox.Location = ((System.Drawing.Point)(resources.GetObject("passwordTextBox.Location")));
            this.passwordTextBox.MaxLength = ((int)(resources.GetObject("passwordTextBox.MaxLength")));
            this.passwordTextBox.Multiline = ((bool)(resources.GetObject("passwordTextBox.Multiline")));
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = ((char)(resources.GetObject("passwordTextBox.PasswordChar")));
            this.passwordTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("passwordTextBox.RightToLeft")));
            this.passwordTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("passwordTextBox.ScrollBars")));
            this.passwordTextBox.Size = ((System.Drawing.Size)(resources.GetObject("passwordTextBox.Size")));
            this.passwordTextBox.TabIndex = ((int)(resources.GetObject("passwordTextBox.TabIndex")));
            this.passwordTextBox.Text = resources.GetString("passwordTextBox.Text");
            this.passwordTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("passwordTextBox.TextAlign")));
            this.passwordTextBox.Visible = ((bool)(resources.GetObject("passwordTextBox.Visible")));
            this.passwordTextBox.WordWrap = ((bool)(resources.GetObject("passwordTextBox.WordWrap")));
            // 
            // confirmPasswordTextBox
            // 
            this.confirmPasswordTextBox.AccessibleDescription = resources.GetString("confirmPasswordTextBox.AccessibleDescription");
            this.confirmPasswordTextBox.AccessibleName = resources.GetString("confirmPasswordTextBox.AccessibleName");
            this.confirmPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("confirmPasswordTextBox.Anchor")));
            this.confirmPasswordTextBox.AutoSize = ((bool)(resources.GetObject("confirmPasswordTextBox.AutoSize")));
            this.confirmPasswordTextBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("confirmPasswordTextBox.BackgroundImage")));
            this.confirmPasswordTextBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("confirmPasswordTextBox.Dock")));
            this.confirmPasswordTextBox.Enabled = ((bool)(resources.GetObject("confirmPasswordTextBox.Enabled")));
            this.confirmPasswordTextBox.Font = ((System.Drawing.Font)(resources.GetObject("confirmPasswordTextBox.Font")));
            this.confirmPasswordTextBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("confirmPasswordTextBox.ImeMode")));
            this.confirmPasswordTextBox.Location = ((System.Drawing.Point)(resources.GetObject("confirmPasswordTextBox.Location")));
            this.confirmPasswordTextBox.MaxLength = ((int)(resources.GetObject("confirmPasswordTextBox.MaxLength")));
            this.confirmPasswordTextBox.Multiline = ((bool)(resources.GetObject("confirmPasswordTextBox.Multiline")));
            this.confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            this.confirmPasswordTextBox.PasswordChar = ((char)(resources.GetObject("confirmPasswordTextBox.PasswordChar")));
            this.confirmPasswordTextBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("confirmPasswordTextBox.RightToLeft")));
            this.confirmPasswordTextBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("confirmPasswordTextBox.ScrollBars")));
            this.confirmPasswordTextBox.Size = ((System.Drawing.Size)(resources.GetObject("confirmPasswordTextBox.Size")));
            this.confirmPasswordTextBox.TabIndex = ((int)(resources.GetObject("confirmPasswordTextBox.TabIndex")));
            this.confirmPasswordTextBox.Text = resources.GetString("confirmPasswordTextBox.Text");
            this.confirmPasswordTextBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("confirmPasswordTextBox.TextAlign")));
            this.confirmPasswordTextBox.Visible = ((bool)(resources.GetObject("confirmPasswordTextBox.Visible")));
            this.confirmPasswordTextBox.WordWrap = ((bool)(resources.GetObject("confirmPasswordTextBox.WordWrap")));
            // 
            // passwordLabel
            // 
            this.passwordLabel.AccessibleDescription = resources.GetString("passwordLabel.AccessibleDescription");
            this.passwordLabel.AccessibleName = resources.GetString("passwordLabel.AccessibleName");
            this.passwordLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("passwordLabel.Anchor")));
            this.passwordLabel.AutoSize = ((bool)(resources.GetObject("passwordLabel.AutoSize")));
            this.passwordLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("passwordLabel.Dock")));
            this.passwordLabel.Enabled = ((bool)(resources.GetObject("passwordLabel.Enabled")));
            this.passwordLabel.Font = ((System.Drawing.Font)(resources.GetObject("passwordLabel.Font")));
            this.passwordLabel.Image = ((System.Drawing.Image)(resources.GetObject("passwordLabel.Image")));
            this.passwordLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("passwordLabel.ImageAlign")));
            this.passwordLabel.ImageIndex = ((int)(resources.GetObject("passwordLabel.ImageIndex")));
            this.passwordLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("passwordLabel.ImeMode")));
            this.passwordLabel.Location = ((System.Drawing.Point)(resources.GetObject("passwordLabel.Location")));
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("passwordLabel.RightToLeft")));
            this.passwordLabel.Size = ((System.Drawing.Size)(resources.GetObject("passwordLabel.Size")));
            this.passwordLabel.TabIndex = ((int)(resources.GetObject("passwordLabel.TabIndex")));
            this.passwordLabel.Text = resources.GetString("passwordLabel.Text");
            this.passwordLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("passwordLabel.TextAlign")));
            this.passwordLabel.Visible = ((bool)(resources.GetObject("passwordLabel.Visible")));
            // 
            // confirmPasswordLabel
            // 
            this.confirmPasswordLabel.AccessibleDescription = resources.GetString("confirmPasswordLabel.AccessibleDescription");
            this.confirmPasswordLabel.AccessibleName = resources.GetString("confirmPasswordLabel.AccessibleName");
            this.confirmPasswordLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("confirmPasswordLabel.Anchor")));
            this.confirmPasswordLabel.AutoSize = ((bool)(resources.GetObject("confirmPasswordLabel.AutoSize")));
            this.confirmPasswordLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("confirmPasswordLabel.Dock")));
            this.confirmPasswordLabel.Enabled = ((bool)(resources.GetObject("confirmPasswordLabel.Enabled")));
            this.confirmPasswordLabel.Font = ((System.Drawing.Font)(resources.GetObject("confirmPasswordLabel.Font")));
            this.confirmPasswordLabel.Image = ((System.Drawing.Image)(resources.GetObject("confirmPasswordLabel.Image")));
            this.confirmPasswordLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("confirmPasswordLabel.ImageAlign")));
            this.confirmPasswordLabel.ImageIndex = ((int)(resources.GetObject("confirmPasswordLabel.ImageIndex")));
            this.confirmPasswordLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("confirmPasswordLabel.ImeMode")));
            this.confirmPasswordLabel.Location = ((System.Drawing.Point)(resources.GetObject("confirmPasswordLabel.Location")));
            this.confirmPasswordLabel.Name = "confirmPasswordLabel";
            this.confirmPasswordLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("confirmPasswordLabel.RightToLeft")));
            this.confirmPasswordLabel.Size = ((System.Drawing.Size)(resources.GetObject("confirmPasswordLabel.Size")));
            this.confirmPasswordLabel.TabIndex = ((int)(resources.GetObject("confirmPasswordLabel.TabIndex")));
            this.confirmPasswordLabel.Text = resources.GetString("confirmPasswordLabel.Text");
            this.confirmPasswordLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("confirmPasswordLabel.TextAlign")));
            this.confirmPasswordLabel.Visible = ((bool)(resources.GetObject("confirmPasswordLabel.Visible")));
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
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClick);
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
            this.OKButton.Click += new System.EventHandler(this.OnOkButtonClick);
            // 
            // SetPasswordDialog
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
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmPasswordLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.confirmPasswordTextBox);
            this.Controls.Add(this.passwordTextBox);
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
            this.Name = "SetPasswordDialog";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
        #endregion
    }
}