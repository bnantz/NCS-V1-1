//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.SecurityDatabaseConsole
{
    public class UserForm : Form
    {
        private UserRoleManager userRoleMgr;
        private IHashProvider hashProvider;
        private Label label1;
        private Label label2;
        private Label label3;
        private GroupBox gbxAddUser;
        private TextBox tbxUser;
        private TextBox tbxPassword1;
        private TextBox tbxPassword2;
        private Button btnSave;
        private Button btnCancel;
        private bool editMode;
        private DbAuthenticationProviderData dbAuthenticationProvider;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public UserForm(DbAuthenticationProviderData dbAuthenticationProvider, ConfigurationContext context)
        {
            this.dbAuthenticationProvider = dbAuthenticationProvider;
            InitializeComponent();

            this.userRoleMgr = new UserRoleManager(dbAuthenticationProvider.Database, context);

            HashProviderFactory hashProviderFactory = new HashProviderFactory(context);
            this.hashProvider = hashProviderFactory.CreateHashProvider(dbAuthenticationProvider.HashProvider);    
            UpdateSaveButtonEnabled();
        }

        public UserForm(DbAuthenticationProviderData dbAuthenticationProvider, ConfigurationContext context, string username) : this(dbAuthenticationProvider, context) 
        {
            tbxUser.Text = username;
            tbxUser.Enabled = false;
            editMode = true;
            label3.Text = SR.NewPasswordLabel;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UserForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbxAddUser = new System.Windows.Forms.GroupBox();
            this.tbxPassword2 = new System.Windows.Forms.TextBox();
            this.tbxPassword1 = new System.Windows.Forms.TextBox();
            this.tbxUser = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxAddUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
            this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
            this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
            this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
            this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
            this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
            this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
            this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
            this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
            this.label1.Name = "label1";
            this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
            this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
            this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
            this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
            // 
            // label2
            // 
            this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
            this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
            this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
            this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
            this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
            this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
            this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
            this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
            this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
            this.label2.Name = "label2";
            this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
            this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
            this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
            this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
            // 
            // label3
            // 
            this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
            this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
            this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
            this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
            this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
            this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
            this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
            this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
            this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
            this.label3.Name = "label3";
            this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
            this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
            this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
            this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
            // 
            // gbxAddUser
            // 
            this.gbxAddUser.AccessibleDescription = resources.GetString("gbxAddUser.AccessibleDescription");
            this.gbxAddUser.AccessibleName = resources.GetString("gbxAddUser.AccessibleName");
            this.gbxAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gbxAddUser.Anchor")));
            this.gbxAddUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gbxAddUser.BackgroundImage")));
            this.gbxAddUser.Controls.Add(this.tbxPassword2);
            this.gbxAddUser.Controls.Add(this.tbxPassword1);
            this.gbxAddUser.Controls.Add(this.tbxUser);
            this.gbxAddUser.Controls.Add(this.label2);
            this.gbxAddUser.Controls.Add(this.label3);
            this.gbxAddUser.Controls.Add(this.label1);
            this.gbxAddUser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("gbxAddUser.Dock")));
            this.gbxAddUser.Enabled = ((bool)(resources.GetObject("gbxAddUser.Enabled")));
            this.gbxAddUser.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxAddUser.Font = ((System.Drawing.Font)(resources.GetObject("gbxAddUser.Font")));
            this.gbxAddUser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("gbxAddUser.ImeMode")));
            this.gbxAddUser.Location = ((System.Drawing.Point)(resources.GetObject("gbxAddUser.Location")));
            this.gbxAddUser.Name = "gbxAddUser";
            this.gbxAddUser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("gbxAddUser.RightToLeft")));
            this.gbxAddUser.Size = ((System.Drawing.Size)(resources.GetObject("gbxAddUser.Size")));
            this.gbxAddUser.TabIndex = ((int)(resources.GetObject("gbxAddUser.TabIndex")));
            this.gbxAddUser.TabStop = false;
            this.gbxAddUser.Text = resources.GetString("gbxAddUser.Text");
            this.gbxAddUser.Visible = ((bool)(resources.GetObject("gbxAddUser.Visible")));
            // 
            // tbxPassword2
            // 
            this.tbxPassword2.AccessibleDescription = resources.GetString("tbxPassword2.AccessibleDescription");
            this.tbxPassword2.AccessibleName = resources.GetString("tbxPassword2.AccessibleName");
            this.tbxPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbxPassword2.Anchor")));
            this.tbxPassword2.AutoSize = ((bool)(resources.GetObject("tbxPassword2.AutoSize")));
            this.tbxPassword2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbxPassword2.BackgroundImage")));
            this.tbxPassword2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbxPassword2.Dock")));
            this.tbxPassword2.Enabled = ((bool)(resources.GetObject("tbxPassword2.Enabled")));
            this.tbxPassword2.Font = ((System.Drawing.Font)(resources.GetObject("tbxPassword2.Font")));
            this.tbxPassword2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbxPassword2.ImeMode")));
            this.tbxPassword2.Location = ((System.Drawing.Point)(resources.GetObject("tbxPassword2.Location")));
            this.tbxPassword2.MaxLength = ((int)(resources.GetObject("tbxPassword2.MaxLength")));
            this.tbxPassword2.Multiline = ((bool)(resources.GetObject("tbxPassword2.Multiline")));
            this.tbxPassword2.Name = "tbxPassword2";
            this.tbxPassword2.PasswordChar = ((char)(resources.GetObject("tbxPassword2.PasswordChar")));
            this.tbxPassword2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbxPassword2.RightToLeft")));
            this.tbxPassword2.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("tbxPassword2.ScrollBars")));
            this.tbxPassword2.Size = ((System.Drawing.Size)(resources.GetObject("tbxPassword2.Size")));
            this.tbxPassword2.TabIndex = ((int)(resources.GetObject("tbxPassword2.TabIndex")));
            this.tbxPassword2.Text = resources.GetString("tbxPassword2.Text");
            this.tbxPassword2.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("tbxPassword2.TextAlign")));
            this.tbxPassword2.Visible = ((bool)(resources.GetObject("tbxPassword2.Visible")));
            this.tbxPassword2.WordWrap = ((bool)(resources.GetObject("tbxPassword2.WordWrap")));
            this.tbxPassword2.TextChanged += new System.EventHandler(this.tbxPassword2_TextChanged);
            // 
            // tbxPassword1
            // 
            this.tbxPassword1.AccessibleDescription = resources.GetString("tbxPassword1.AccessibleDescription");
            this.tbxPassword1.AccessibleName = resources.GetString("tbxPassword1.AccessibleName");
            this.tbxPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbxPassword1.Anchor")));
            this.tbxPassword1.AutoSize = ((bool)(resources.GetObject("tbxPassword1.AutoSize")));
            this.tbxPassword1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbxPassword1.BackgroundImage")));
            this.tbxPassword1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbxPassword1.Dock")));
            this.tbxPassword1.Enabled = ((bool)(resources.GetObject("tbxPassword1.Enabled")));
            this.tbxPassword1.Font = ((System.Drawing.Font)(resources.GetObject("tbxPassword1.Font")));
            this.tbxPassword1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbxPassword1.ImeMode")));
            this.tbxPassword1.Location = ((System.Drawing.Point)(resources.GetObject("tbxPassword1.Location")));
            this.tbxPassword1.MaxLength = ((int)(resources.GetObject("tbxPassword1.MaxLength")));
            this.tbxPassword1.Multiline = ((bool)(resources.GetObject("tbxPassword1.Multiline")));
            this.tbxPassword1.Name = "tbxPassword1";
            this.tbxPassword1.PasswordChar = ((char)(resources.GetObject("tbxPassword1.PasswordChar")));
            this.tbxPassword1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbxPassword1.RightToLeft")));
            this.tbxPassword1.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("tbxPassword1.ScrollBars")));
            this.tbxPassword1.Size = ((System.Drawing.Size)(resources.GetObject("tbxPassword1.Size")));
            this.tbxPassword1.TabIndex = ((int)(resources.GetObject("tbxPassword1.TabIndex")));
            this.tbxPassword1.Text = resources.GetString("tbxPassword1.Text");
            this.tbxPassword1.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("tbxPassword1.TextAlign")));
            this.tbxPassword1.Visible = ((bool)(resources.GetObject("tbxPassword1.Visible")));
            this.tbxPassword1.WordWrap = ((bool)(resources.GetObject("tbxPassword1.WordWrap")));
            this.tbxPassword1.TextChanged += new System.EventHandler(this.tbxPassword1_TextChanged);
            // 
            // tbxUser
            // 
            this.tbxUser.AccessibleDescription = resources.GetString("tbxUser.AccessibleDescription");
            this.tbxUser.AccessibleName = resources.GetString("tbxUser.AccessibleName");
            this.tbxUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbxUser.Anchor")));
            this.tbxUser.AutoSize = ((bool)(resources.GetObject("tbxUser.AutoSize")));
            this.tbxUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbxUser.BackgroundImage")));
            this.tbxUser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbxUser.Dock")));
            this.tbxUser.Enabled = ((bool)(resources.GetObject("tbxUser.Enabled")));
            this.tbxUser.Font = ((System.Drawing.Font)(resources.GetObject("tbxUser.Font")));
            this.tbxUser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbxUser.ImeMode")));
            this.tbxUser.Location = ((System.Drawing.Point)(resources.GetObject("tbxUser.Location")));
            this.tbxUser.MaxLength = ((int)(resources.GetObject("tbxUser.MaxLength")));
            this.tbxUser.Multiline = ((bool)(resources.GetObject("tbxUser.Multiline")));
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.PasswordChar = ((char)(resources.GetObject("tbxUser.PasswordChar")));
            this.tbxUser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbxUser.RightToLeft")));
            this.tbxUser.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("tbxUser.ScrollBars")));
            this.tbxUser.Size = ((System.Drawing.Size)(resources.GetObject("tbxUser.Size")));
            this.tbxUser.TabIndex = ((int)(resources.GetObject("tbxUser.TabIndex")));
            this.tbxUser.Text = resources.GetString("tbxUser.Text");
            this.tbxUser.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("tbxUser.TextAlign")));
            this.tbxUser.Visible = ((bool)(resources.GetObject("tbxUser.Visible")));
            this.tbxUser.WordWrap = ((bool)(resources.GetObject("tbxUser.WordWrap")));
            this.tbxUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxUser_KeyPress);
            this.tbxUser.TextChanged += new System.EventHandler(this.tbxUser_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleDescription = resources.GetString("btnSave.AccessibleDescription");
            this.btnSave.AccessibleName = resources.GetString("btnSave.AccessibleName");
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnSave.Anchor")));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnSave.Dock")));
            this.btnSave.Enabled = ((bool)(resources.GetObject("btnSave.Enabled")));
            this.btnSave.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnSave.FlatStyle")));
            this.btnSave.Font = ((System.Drawing.Font)(resources.GetObject("btnSave.Font")));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSave.ImageAlign")));
            this.btnSave.ImageIndex = ((int)(resources.GetObject("btnSave.ImageIndex")));
            this.btnSave.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnSave.ImeMode")));
            this.btnSave.Location = ((System.Drawing.Point)(resources.GetObject("btnSave.Location")));
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnSave.RightToLeft")));
            this.btnSave.Size = ((System.Drawing.Size)(resources.GetObject("btnSave.Size")));
            this.btnSave.TabIndex = ((int)(resources.GetObject("btnSave.TabIndex")));
            this.btnSave.Text = resources.GetString("btnSave.Text");
            this.btnSave.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSave.TextAlign")));
            this.btnSave.Visible = ((bool)(resources.GetObject("btnSave.Visible")));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // UserForm
            // 
            this.AcceptButton = this.btnSave;
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
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbxAddUser);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "UserForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.gbxAddUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbxPassword1.Text != tbxPassword2.Text)
            {
                MessageBox.Show(SR.PasswordsMustMatch);
                return;
            }

            byte[] password = hashProvider.CreateHash(Encoding.Unicode.GetBytes(tbxPassword1.Text));
            
            if (editMode) 
            {
                userRoleMgr.ChangeUserPassword(tbxUser.Text, password);
            } 
            else if (userRoleMgr.UserExists(tbxUser.Text)) 
            {
                MessageBox.Show(SR.UserAlreadyExists);
                return;
            } 
            else 
            {
                userRoleMgr.CreateUser(tbxUser.Text, password);
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void tbxUser_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) this.btnSave.PerformClick();
        }

        private void UserForm_Load(object sender, System.EventArgs e)
        {
            tbxUser.Focus();
        }

        private void tbxUser_TextChanged(object sender, System.EventArgs e)
        {
            UpdateSaveButtonEnabled();
        }

        private void UpdateSaveButtonEnabled() 
        {
            btnSave.Enabled = 
                tbxUser.Text.Trim().Length > 0 &&
                tbxPassword1.Text.Trim().Length > 0 &&
                tbxPassword2.Text.Trim().Length > 0;
        }

        private void tbxPassword1_TextChanged(object sender, System.EventArgs e)
        {
            UpdateSaveButtonEnabled();
        }

        private void tbxPassword2_TextChanged(object sender, System.EventArgs e)
        {
            UpdateSaveButtonEnabled();
        }
    }
}