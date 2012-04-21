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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Import password dialog.
    /// </summary>
    public class ImportPassword : Form
    {
        private string password;

        #region Form Designer
        private Label lblPassword1;
        private Label lblTitle;
        private Button btnCancel;
        private Button btnOK;
        private TextBox txtPassword;
        private Container components = null;

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        public ImportPassword()
        {
            InitializeComponent();
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImportPassword));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword1 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.AccessibleDescription = resources.GetString("txtPassword.AccessibleDescription");
            this.txtPassword.AccessibleName = resources.GetString("txtPassword.AccessibleName");
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtPassword.Anchor")));
            this.txtPassword.AutoSize = ((bool)(resources.GetObject("txtPassword.AutoSize")));
            this.txtPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtPassword.BackgroundImage")));
            this.txtPassword.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtPassword.Dock")));
            this.txtPassword.Enabled = ((bool)(resources.GetObject("txtPassword.Enabled")));
            this.txtPassword.Font = ((System.Drawing.Font)(resources.GetObject("txtPassword.Font")));
            this.txtPassword.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtPassword.ImeMode")));
            this.txtPassword.Location = ((System.Drawing.Point)(resources.GetObject("txtPassword.Location")));
            this.txtPassword.MaxLength = ((int)(resources.GetObject("txtPassword.MaxLength")));
            this.txtPassword.Multiline = ((bool)(resources.GetObject("txtPassword.Multiline")));
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = ((char)(resources.GetObject("txtPassword.PasswordChar")));
            this.txtPassword.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtPassword.RightToLeft")));
            this.txtPassword.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtPassword.ScrollBars")));
            this.txtPassword.Size = ((System.Drawing.Size)(resources.GetObject("txtPassword.Size")));
            this.txtPassword.TabIndex = ((int)(resources.GetObject("txtPassword.TabIndex")));
            this.txtPassword.Text = resources.GetString("txtPassword.Text");
            this.txtPassword.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtPassword.TextAlign")));
            this.txtPassword.Visible = ((bool)(resources.GetObject("txtPassword.Visible")));
            this.txtPassword.WordWrap = ((bool)(resources.GetObject("txtPassword.WordWrap")));
            // 
            // lblPassword1
            // 
            this.lblPassword1.AccessibleDescription = resources.GetString("lblPassword1.AccessibleDescription");
            this.lblPassword1.AccessibleName = resources.GetString("lblPassword1.AccessibleName");
            this.lblPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPassword1.Anchor")));
            this.lblPassword1.AutoSize = ((bool)(resources.GetObject("lblPassword1.AutoSize")));
            this.lblPassword1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPassword1.Dock")));
            this.lblPassword1.Enabled = ((bool)(resources.GetObject("lblPassword1.Enabled")));
            this.lblPassword1.Font = ((System.Drawing.Font)(resources.GetObject("lblPassword1.Font")));
            this.lblPassword1.Image = ((System.Drawing.Image)(resources.GetObject("lblPassword1.Image")));
            this.lblPassword1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPassword1.ImageAlign")));
            this.lblPassword1.ImageIndex = ((int)(resources.GetObject("lblPassword1.ImageIndex")));
            this.lblPassword1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPassword1.ImeMode")));
            this.lblPassword1.Location = ((System.Drawing.Point)(resources.GetObject("lblPassword1.Location")));
            this.lblPassword1.Name = "lblPassword1";
            this.lblPassword1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPassword1.RightToLeft")));
            this.lblPassword1.Size = ((System.Drawing.Size)(resources.GetObject("lblPassword1.Size")));
            this.lblPassword1.TabIndex = ((int)(resources.GetObject("lblPassword1.TabIndex")));
            this.lblPassword1.Text = resources.GetString("lblPassword1.Text");
            this.lblPassword1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPassword1.TextAlign")));
            this.lblPassword1.Visible = ((bool)(resources.GetObject("lblPassword1.Visible")));
            // 
            // lblTitle
            // 
            this.lblTitle.AccessibleDescription = resources.GetString("lblTitle.AccessibleDescription");
            this.lblTitle.AccessibleName = resources.GetString("lblTitle.AccessibleName");
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblTitle.Anchor")));
            this.lblTitle.AutoSize = ((bool)(resources.GetObject("lblTitle.AutoSize")));
            this.lblTitle.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblTitle.Dock")));
            this.lblTitle.Enabled = ((bool)(resources.GetObject("lblTitle.Enabled")));
            this.lblTitle.Font = ((System.Drawing.Font)(resources.GetObject("lblTitle.Font")));
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTitle.ImageAlign")));
            this.lblTitle.ImageIndex = ((int)(resources.GetObject("lblTitle.ImageIndex")));
            this.lblTitle.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblTitle.ImeMode")));
            this.lblTitle.Location = ((System.Drawing.Point)(resources.GetObject("lblTitle.Location")));
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblTitle.RightToLeft")));
            this.lblTitle.Size = ((System.Drawing.Size)(resources.GetObject("lblTitle.Size")));
            this.lblTitle.TabIndex = ((int)(resources.GetObject("lblTitle.TabIndex")));
            this.lblTitle.Text = resources.GetString("lblTitle.Text");
            this.lblTitle.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTitle.TextAlign")));
            this.lblTitle.Visible = ((bool)(resources.GetObject("lblTitle.Visible")));
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
            // ImportPassword
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
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword1);
            this.Controls.Add(this.lblTitle);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "ImportPassword";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.ImportPassword_Load);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        /// <summary>
        /// Gets the password used for importing.
        /// </summary>
        public string Password
        {
            get { return password; }
        }

        private void ImportPassword_Load(object sender, EventArgs e)
        {
            this.password = string.Empty;
            txtPassword.Text = String.Empty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.password = string.Empty;

            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show(SR.ImportPasswordInvalid, SR.ImportPasswordInvalidTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.password = txtPassword.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.password = string.Empty;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}