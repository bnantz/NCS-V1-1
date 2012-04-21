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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration;


namespace Microsoft.Practices.EnterpriseLibrary.Tools.SecurityDatabaseConsole
{
    public class RoleForm : Form
    {
        private UserRoleManager userRoleMgr; 

        private Button btnCancel;
        private Button btnSave;
        private GroupBox gbxAddUser;
        private TextBox tbxRole;
        private DbAuthenticationProviderData dbAuthenticationproviderData;
        private bool editMode;
        private string originalRoleName;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public RoleForm(DbAuthenticationProviderData dbAuthenticationProviderData, ConfigurationContext context)
        {
            this.dbAuthenticationproviderData = dbAuthenticationProviderData;
            InitializeComponent();

            this.userRoleMgr = new UserRoleManager(dbAuthenticationProviderData.Database, context);

            UpdateSaveButtonEnabled();
        }

        public RoleForm (DbAuthenticationProviderData dbAuthenticationProviderData, ConfigurationContext context, string role) : this(dbAuthenticationProviderData, context) 
        {
            tbxRole.Text = role;
            originalRoleName = role;
            editMode = true;
            UpdateSaveButtonEnabled();
        }

        public string RoleName 
        {
            get { return tbxRole.Text; }
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RoleForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbxAddUser = new System.Windows.Forms.GroupBox();
            this.tbxRole = new System.Windows.Forms.TextBox();
            this.gbxAddUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription");
            this.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName");
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnCancel.Anchor")));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
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
            // gbxAddUser
            // 
            this.gbxAddUser.AccessibleDescription = resources.GetString("gbxAddUser.AccessibleDescription");
            this.gbxAddUser.AccessibleName = resources.GetString("gbxAddUser.AccessibleName");
            this.gbxAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gbxAddUser.Anchor")));
            this.gbxAddUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gbxAddUser.BackgroundImage")));
            this.gbxAddUser.Controls.Add(this.tbxRole);
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
            // tbxRole
            // 
            this.tbxRole.AccessibleDescription = resources.GetString("tbxRole.AccessibleDescription");
            this.tbxRole.AccessibleName = resources.GetString("tbxRole.AccessibleName");
            this.tbxRole.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tbxRole.Anchor")));
            this.tbxRole.AutoSize = ((bool)(resources.GetObject("tbxRole.AutoSize")));
            this.tbxRole.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbxRole.BackgroundImage")));
            this.tbxRole.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tbxRole.Dock")));
            this.tbxRole.Enabled = ((bool)(resources.GetObject("tbxRole.Enabled")));
            this.tbxRole.Font = ((System.Drawing.Font)(resources.GetObject("tbxRole.Font")));
            this.tbxRole.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tbxRole.ImeMode")));
            this.tbxRole.Location = ((System.Drawing.Point)(resources.GetObject("tbxRole.Location")));
            this.tbxRole.MaxLength = ((int)(resources.GetObject("tbxRole.MaxLength")));
            this.tbxRole.Multiline = ((bool)(resources.GetObject("tbxRole.Multiline")));
            this.tbxRole.Name = "tbxRole";
            this.tbxRole.PasswordChar = ((char)(resources.GetObject("tbxRole.PasswordChar")));
            this.tbxRole.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tbxRole.RightToLeft")));
            this.tbxRole.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("tbxRole.ScrollBars")));
            this.tbxRole.Size = ((System.Drawing.Size)(resources.GetObject("tbxRole.Size")));
            this.tbxRole.TabIndex = ((int)(resources.GetObject("tbxRole.TabIndex")));
            this.tbxRole.Text = resources.GetString("tbxRole.Text");
            this.tbxRole.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("tbxRole.TextAlign")));
            this.tbxRole.Visible = ((bool)(resources.GetObject("tbxRole.Visible")));
            this.tbxRole.WordWrap = ((bool)(resources.GetObject("tbxRole.WordWrap")));
            this.tbxRole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxRole_KeyPress);
            this.tbxRole.TextChanged += new System.EventHandler(this.tbxRole_TextChanged);
            // 
            // RoleForm
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
            this.Name = "RoleForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.AddNewRoleForm_Load);
            this.gbxAddUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (userRoleMgr.GetRoleIdFromRoleName(tbxRole.Text) > -1) 
            {
                MessageBox.Show(SR.RoleAlreadyExists, SR.RoleAlreadyExists, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 

            if (editMode && tbxRole.Text != originalRoleName) 
            {
                this.userRoleMgr.RenameRole(originalRoleName, tbxRole.Text.Trim());
            } 
            else 
            {
                this.userRoleMgr.CreateRole(tbxRole.Text.Trim());
            }

            DialogResult = DialogResult.OK;
        }

        private void tbxRole_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) this.btnSave.PerformClick();
        }

        private void AddNewRoleForm_Load(object sender, System.EventArgs e)
        {
            tbxRole.Focus();
        }

        private void UpdateSaveButtonEnabled() 
        {
            
            if (editMode) 
            {
                btnSave.Enabled = tbxRole.Text.Trim().Length > 0 && tbxRole.Text != originalRoleName;
            }
            else 
            {
                btnSave.Enabled = tbxRole.Text.Trim().Length > 0;
            }
        }

        private void tbxRole_TextChanged(object sender, System.EventArgs e)
        {
            UpdateSaveButtonEnabled();
        }
    }
}