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
using System.Data;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.SecurityDatabaseConsole
{
    public class MainForm : Form
    {
		private UserRoleManager userRoleMgr; 

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private Label lblRoles;
        private Label lblUsers;
        private Label lblRoleUsers;
        private ListBox lbxRoleUsers;
        private ListBox lbxUsers;
        private GroupBox gbxUserRoles;
        private ComboBox cbxRoles;
        private ComboBox cbxDbInstances;
        private Button btnQuit;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnAddNewUser;
        private Button btnAddNewRole;
        private Button btnDelUser;
        private GroupBox databasesGroupBox;
        private Button selectDbButton;
        private System.Windows.Forms.Button editUserButton;
        private Button btnDelRole;
        private System.Windows.Forms.Button btnEditRole;
        private ConfigurationContext configurationContext;

        private static readonly string paragraphSeparator = System.Environment.NewLine + System.Environment.NewLine;

        public MainForm(ConfigurationContext configurationContext)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            this.configurationContext = configurationContext;

            InitializeComponent();

            UpdateEditRoleButtonEnabled();
            UpdateEditUserButtonEnabled();
            UpdateConnectButtonEnabled();
            UpdateUserToRoleButtonsEnabled();
        }

        public MainForm() : this(ConfigurationManager.GetCurrentContext()) {}

        public DbAuthenticationProviderData DbAuthenticationProvider 
        {
            get 
            {
                DbAuthenticationProviderData providerData = cbxDbInstances.SelectedItem as DbAuthenticationProviderData;
                return providerData;
            }
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.lbxRoleUsers = new System.Windows.Forms.ListBox();
            this.lblRoles = new System.Windows.Forms.Label();
            this.lbxUsers = new System.Windows.Forms.ListBox();
            this.cbxRoles = new System.Windows.Forms.ComboBox();
            this.lblRoleUsers = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.gbxUserRoles = new System.Windows.Forms.GroupBox();
            this.btnEditRole = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelUser = new System.Windows.Forms.Button();
            this.btnAddNewUser = new System.Windows.Forms.Button();
            this.btnAddNewRole = new System.Windows.Forms.Button();
            this.btnDelRole = new System.Windows.Forms.Button();
            this.editUserButton = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.cbxDbInstances = new System.Windows.Forms.ComboBox();
            this.databasesGroupBox = new System.Windows.Forms.GroupBox();
            this.selectDbButton = new System.Windows.Forms.Button();
            this.gbxUserRoles.SuspendLayout();
            this.databasesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxRoleUsers
            // 
            this.lbxRoleUsers.AccessibleDescription = resources.GetString("lbxRoleUsers.AccessibleDescription");
            this.lbxRoleUsers.AccessibleName = resources.GetString("lbxRoleUsers.AccessibleName");
            this.lbxRoleUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lbxRoleUsers.Anchor")));
            this.lbxRoleUsers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lbxRoleUsers.BackgroundImage")));
            this.lbxRoleUsers.ColumnWidth = ((int)(resources.GetObject("lbxRoleUsers.ColumnWidth")));
            this.lbxRoleUsers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lbxRoleUsers.Dock")));
            this.lbxRoleUsers.Enabled = ((bool)(resources.GetObject("lbxRoleUsers.Enabled")));
            this.lbxRoleUsers.Font = ((System.Drawing.Font)(resources.GetObject("lbxRoleUsers.Font")));
            this.lbxRoleUsers.HorizontalExtent = ((int)(resources.GetObject("lbxRoleUsers.HorizontalExtent")));
            this.lbxRoleUsers.HorizontalScrollbar = ((bool)(resources.GetObject("lbxRoleUsers.HorizontalScrollbar")));
            this.lbxRoleUsers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lbxRoleUsers.ImeMode")));
            this.lbxRoleUsers.IntegralHeight = ((bool)(resources.GetObject("lbxRoleUsers.IntegralHeight")));
            this.lbxRoleUsers.ItemHeight = ((int)(resources.GetObject("lbxRoleUsers.ItemHeight")));
            this.lbxRoleUsers.Location = ((System.Drawing.Point)(resources.GetObject("lbxRoleUsers.Location")));
            this.lbxRoleUsers.Name = "lbxRoleUsers";
            this.lbxRoleUsers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lbxRoleUsers.RightToLeft")));
            this.lbxRoleUsers.ScrollAlwaysVisible = ((bool)(resources.GetObject("lbxRoleUsers.ScrollAlwaysVisible")));
            this.lbxRoleUsers.Size = ((System.Drawing.Size)(resources.GetObject("lbxRoleUsers.Size")));
            this.lbxRoleUsers.TabIndex = ((int)(resources.GetObject("lbxRoleUsers.TabIndex")));
            this.lbxRoleUsers.Visible = ((bool)(resources.GetObject("lbxRoleUsers.Visible")));
            // 
            // lblRoles
            // 
            this.lblRoles.AccessibleDescription = resources.GetString("lblRoles.AccessibleDescription");
            this.lblRoles.AccessibleName = resources.GetString("lblRoles.AccessibleName");
            this.lblRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblRoles.Anchor")));
            this.lblRoles.AutoSize = ((bool)(resources.GetObject("lblRoles.AutoSize")));
            this.lblRoles.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblRoles.Dock")));
            this.lblRoles.Enabled = ((bool)(resources.GetObject("lblRoles.Enabled")));
            this.lblRoles.Font = ((System.Drawing.Font)(resources.GetObject("lblRoles.Font")));
            this.lblRoles.Image = ((System.Drawing.Image)(resources.GetObject("lblRoles.Image")));
            this.lblRoles.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRoles.ImageAlign")));
            this.lblRoles.ImageIndex = ((int)(resources.GetObject("lblRoles.ImageIndex")));
            this.lblRoles.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblRoles.ImeMode")));
            this.lblRoles.Location = ((System.Drawing.Point)(resources.GetObject("lblRoles.Location")));
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblRoles.RightToLeft")));
            this.lblRoles.Size = ((System.Drawing.Size)(resources.GetObject("lblRoles.Size")));
            this.lblRoles.TabIndex = ((int)(resources.GetObject("lblRoles.TabIndex")));
            this.lblRoles.Text = resources.GetString("lblRoles.Text");
            this.lblRoles.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRoles.TextAlign")));
            this.lblRoles.Visible = ((bool)(resources.GetObject("lblRoles.Visible")));
            // 
            // lbxUsers
            // 
            this.lbxUsers.AccessibleDescription = resources.GetString("lbxUsers.AccessibleDescription");
            this.lbxUsers.AccessibleName = resources.GetString("lbxUsers.AccessibleName");
            this.lbxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lbxUsers.Anchor")));
            this.lbxUsers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lbxUsers.BackgroundImage")));
            this.lbxUsers.ColumnWidth = ((int)(resources.GetObject("lbxUsers.ColumnWidth")));
            this.lbxUsers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lbxUsers.Dock")));
            this.lbxUsers.Enabled = ((bool)(resources.GetObject("lbxUsers.Enabled")));
            this.lbxUsers.Font = ((System.Drawing.Font)(resources.GetObject("lbxUsers.Font")));
            this.lbxUsers.HorizontalExtent = ((int)(resources.GetObject("lbxUsers.HorizontalExtent")));
            this.lbxUsers.HorizontalScrollbar = ((bool)(resources.GetObject("lbxUsers.HorizontalScrollbar")));
            this.lbxUsers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lbxUsers.ImeMode")));
            this.lbxUsers.IntegralHeight = ((bool)(resources.GetObject("lbxUsers.IntegralHeight")));
            this.lbxUsers.ItemHeight = ((int)(resources.GetObject("lbxUsers.ItemHeight")));
            this.lbxUsers.Location = ((System.Drawing.Point)(resources.GetObject("lbxUsers.Location")));
            this.lbxUsers.Name = "lbxUsers";
            this.lbxUsers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lbxUsers.RightToLeft")));
            this.lbxUsers.ScrollAlwaysVisible = ((bool)(resources.GetObject("lbxUsers.ScrollAlwaysVisible")));
            this.lbxUsers.Size = ((System.Drawing.Size)(resources.GetObject("lbxUsers.Size")));
            this.lbxUsers.TabIndex = ((int)(resources.GetObject("lbxUsers.TabIndex")));
            this.lbxUsers.Visible = ((bool)(resources.GetObject("lbxUsers.Visible")));
            this.lbxUsers.SelectedIndexChanged += new System.EventHandler(this.lbxUsers_SelectedIndexChanged);
            // 
            // cbxRoles
            // 
            this.cbxRoles.AccessibleDescription = resources.GetString("cbxRoles.AccessibleDescription");
            this.cbxRoles.AccessibleName = resources.GetString("cbxRoles.AccessibleName");
            this.cbxRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cbxRoles.Anchor")));
            this.cbxRoles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cbxRoles.BackgroundImage")));
            this.cbxRoles.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbxRoles.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cbxRoles.Dock")));
            this.cbxRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRoles.Enabled = ((bool)(resources.GetObject("cbxRoles.Enabled")));
            this.cbxRoles.Font = ((System.Drawing.Font)(resources.GetObject("cbxRoles.Font")));
            this.cbxRoles.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cbxRoles.ImeMode")));
            this.cbxRoles.IntegralHeight = ((bool)(resources.GetObject("cbxRoles.IntegralHeight")));
            this.cbxRoles.ItemHeight = ((int)(resources.GetObject("cbxRoles.ItemHeight")));
            this.cbxRoles.Location = ((System.Drawing.Point)(resources.GetObject("cbxRoles.Location")));
            this.cbxRoles.MaxDropDownItems = ((int)(resources.GetObject("cbxRoles.MaxDropDownItems")));
            this.cbxRoles.MaxLength = ((int)(resources.GetObject("cbxRoles.MaxLength")));
            this.cbxRoles.Name = "cbxRoles";
            this.cbxRoles.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cbxRoles.RightToLeft")));
            this.cbxRoles.Size = ((System.Drawing.Size)(resources.GetObject("cbxRoles.Size")));
            this.cbxRoles.TabIndex = ((int)(resources.GetObject("cbxRoles.TabIndex")));
            this.cbxRoles.Text = resources.GetString("cbxRoles.Text");
            this.cbxRoles.Visible = ((bool)(resources.GetObject("cbxRoles.Visible")));
            this.cbxRoles.SelectedIndexChanged += new System.EventHandler(this.cbxRoles_SelectedIndexChanged);
            // 
            // lblRoleUsers
            // 
            this.lblRoleUsers.AccessibleDescription = resources.GetString("lblRoleUsers.AccessibleDescription");
            this.lblRoleUsers.AccessibleName = resources.GetString("lblRoleUsers.AccessibleName");
            this.lblRoleUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblRoleUsers.Anchor")));
            this.lblRoleUsers.AutoSize = ((bool)(resources.GetObject("lblRoleUsers.AutoSize")));
            this.lblRoleUsers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblRoleUsers.Dock")));
            this.lblRoleUsers.Enabled = ((bool)(resources.GetObject("lblRoleUsers.Enabled")));
            this.lblRoleUsers.Font = ((System.Drawing.Font)(resources.GetObject("lblRoleUsers.Font")));
            this.lblRoleUsers.Image = ((System.Drawing.Image)(resources.GetObject("lblRoleUsers.Image")));
            this.lblRoleUsers.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRoleUsers.ImageAlign")));
            this.lblRoleUsers.ImageIndex = ((int)(resources.GetObject("lblRoleUsers.ImageIndex")));
            this.lblRoleUsers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblRoleUsers.ImeMode")));
            this.lblRoleUsers.Location = ((System.Drawing.Point)(resources.GetObject("lblRoleUsers.Location")));
            this.lblRoleUsers.Name = "lblRoleUsers";
            this.lblRoleUsers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblRoleUsers.RightToLeft")));
            this.lblRoleUsers.Size = ((System.Drawing.Size)(resources.GetObject("lblRoleUsers.Size")));
            this.lblRoleUsers.TabIndex = ((int)(resources.GetObject("lblRoleUsers.TabIndex")));
            this.lblRoleUsers.Text = resources.GetString("lblRoleUsers.Text");
            this.lblRoleUsers.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRoleUsers.TextAlign")));
            this.lblRoleUsers.Visible = ((bool)(resources.GetObject("lblRoleUsers.Visible")));
            // 
            // lblUsers
            // 
            this.lblUsers.AccessibleDescription = resources.GetString("lblUsers.AccessibleDescription");
            this.lblUsers.AccessibleName = resources.GetString("lblUsers.AccessibleName");
            this.lblUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblUsers.Anchor")));
            this.lblUsers.AutoSize = ((bool)(resources.GetObject("lblUsers.AutoSize")));
            this.lblUsers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblUsers.Dock")));
            this.lblUsers.Enabled = ((bool)(resources.GetObject("lblUsers.Enabled")));
            this.lblUsers.Font = ((System.Drawing.Font)(resources.GetObject("lblUsers.Font")));
            this.lblUsers.Image = ((System.Drawing.Image)(resources.GetObject("lblUsers.Image")));
            this.lblUsers.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblUsers.ImageAlign")));
            this.lblUsers.ImageIndex = ((int)(resources.GetObject("lblUsers.ImageIndex")));
            this.lblUsers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblUsers.ImeMode")));
            this.lblUsers.Location = ((System.Drawing.Point)(resources.GetObject("lblUsers.Location")));
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblUsers.RightToLeft")));
            this.lblUsers.Size = ((System.Drawing.Size)(resources.GetObject("lblUsers.Size")));
            this.lblUsers.TabIndex = ((int)(resources.GetObject("lblUsers.TabIndex")));
            this.lblUsers.Text = resources.GetString("lblUsers.Text");
            this.lblUsers.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblUsers.TextAlign")));
            this.lblUsers.Visible = ((bool)(resources.GetObject("lblUsers.Visible")));
            // 
            // gbxUserRoles
            // 
            this.gbxUserRoles.AccessibleDescription = resources.GetString("gbxUserRoles.AccessibleDescription");
            this.gbxUserRoles.AccessibleName = resources.GetString("gbxUserRoles.AccessibleName");
            this.gbxUserRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gbxUserRoles.Anchor")));
            this.gbxUserRoles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gbxUserRoles.BackgroundImage")));
            this.gbxUserRoles.Controls.Add(this.btnEditRole);
            this.gbxUserRoles.Controls.Add(this.btnRemove);
            this.gbxUserRoles.Controls.Add(this.btnAdd);
            this.gbxUserRoles.Controls.Add(this.btnDelUser);
            this.gbxUserRoles.Controls.Add(this.btnAddNewUser);
            this.gbxUserRoles.Controls.Add(this.btnAddNewRole);
            this.gbxUserRoles.Controls.Add(this.btnDelRole);
            this.gbxUserRoles.Controls.Add(this.cbxRoles);
            this.gbxUserRoles.Controls.Add(this.lblRoleUsers);
            this.gbxUserRoles.Controls.Add(this.lbxRoleUsers);
            this.gbxUserRoles.Controls.Add(this.lblRoles);
            this.gbxUserRoles.Controls.Add(this.lbxUsers);
            this.gbxUserRoles.Controls.Add(this.lblUsers);
            this.gbxUserRoles.Controls.Add(this.editUserButton);
            this.gbxUserRoles.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("gbxUserRoles.Dock")));
            this.gbxUserRoles.Enabled = ((bool)(resources.GetObject("gbxUserRoles.Enabled")));
            this.gbxUserRoles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbxUserRoles.Font = ((System.Drawing.Font)(resources.GetObject("gbxUserRoles.Font")));
            this.gbxUserRoles.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("gbxUserRoles.ImeMode")));
            this.gbxUserRoles.Location = ((System.Drawing.Point)(resources.GetObject("gbxUserRoles.Location")));
            this.gbxUserRoles.Name = "gbxUserRoles";
            this.gbxUserRoles.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("gbxUserRoles.RightToLeft")));
            this.gbxUserRoles.Size = ((System.Drawing.Size)(resources.GetObject("gbxUserRoles.Size")));
            this.gbxUserRoles.TabIndex = ((int)(resources.GetObject("gbxUserRoles.TabIndex")));
            this.gbxUserRoles.TabStop = false;
            this.gbxUserRoles.Text = resources.GetString("gbxUserRoles.Text");
            this.gbxUserRoles.Visible = ((bool)(resources.GetObject("gbxUserRoles.Visible")));
            // 
            // btnEditRole
            // 
            this.btnEditRole.AccessibleDescription = resources.GetString("btnEditRole.AccessibleDescription");
            this.btnEditRole.AccessibleName = resources.GetString("btnEditRole.AccessibleName");
            this.btnEditRole.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnEditRole.Anchor")));
            this.btnEditRole.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEditRole.BackgroundImage")));
            this.btnEditRole.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnEditRole.Dock")));
            this.btnEditRole.Enabled = ((bool)(resources.GetObject("btnEditRole.Enabled")));
            this.btnEditRole.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnEditRole.FlatStyle")));
            this.btnEditRole.Font = ((System.Drawing.Font)(resources.GetObject("btnEditRole.Font")));
            this.btnEditRole.Image = ((System.Drawing.Image)(resources.GetObject("btnEditRole.Image")));
            this.btnEditRole.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnEditRole.ImageAlign")));
            this.btnEditRole.ImageIndex = ((int)(resources.GetObject("btnEditRole.ImageIndex")));
            this.btnEditRole.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnEditRole.ImeMode")));
            this.btnEditRole.Location = ((System.Drawing.Point)(resources.GetObject("btnEditRole.Location")));
            this.btnEditRole.Name = "btnEditRole";
            this.btnEditRole.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnEditRole.RightToLeft")));
            this.btnEditRole.Size = ((System.Drawing.Size)(resources.GetObject("btnEditRole.Size")));
            this.btnEditRole.TabIndex = ((int)(resources.GetObject("btnEditRole.TabIndex")));
            this.btnEditRole.Text = resources.GetString("btnEditRole.Text");
            this.btnEditRole.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnEditRole.TextAlign")));
            this.btnEditRole.Visible = ((bool)(resources.GetObject("btnEditRole.Visible")));
            this.btnEditRole.Click += new System.EventHandler(this.btnEditRole_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.AccessibleDescription = resources.GetString("btnRemove.AccessibleDescription");
            this.btnRemove.AccessibleName = resources.GetString("btnRemove.AccessibleName");
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnRemove.Anchor")));
            this.btnRemove.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemove.BackgroundImage")));
            this.btnRemove.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnRemove.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnRemove.Dock")));
            this.btnRemove.Enabled = ((bool)(resources.GetObject("btnRemove.Enabled")));
            this.btnRemove.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnRemove.FlatStyle")));
            this.btnRemove.Font = ((System.Drawing.Font)(resources.GetObject("btnRemove.Font")));
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRemove.ImageAlign")));
            this.btnRemove.ImageIndex = ((int)(resources.GetObject("btnRemove.ImageIndex")));
            this.btnRemove.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnRemove.ImeMode")));
            this.btnRemove.Location = ((System.Drawing.Point)(resources.GetObject("btnRemove.Location")));
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnRemove.RightToLeft")));
            this.btnRemove.Size = ((System.Drawing.Size)(resources.GetObject("btnRemove.Size")));
            this.btnRemove.TabIndex = ((int)(resources.GetObject("btnRemove.TabIndex")));
            this.btnRemove.Text = resources.GetString("btnRemove.Text");
            this.btnRemove.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRemove.TextAlign")));
            this.btnRemove.Visible = ((bool)(resources.GetObject("btnRemove.Visible")));
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleDescription = resources.GetString("btnAdd.AccessibleDescription");
            this.btnAdd.AccessibleName = resources.GetString("btnAdd.AccessibleName");
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAdd.Anchor")));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAdd.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAdd.Dock")));
            this.btnAdd.Enabled = ((bool)(resources.GetObject("btnAdd.Enabled")));
            this.btnAdd.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAdd.FlatStyle")));
            this.btnAdd.Font = ((System.Drawing.Font)(resources.GetObject("btnAdd.Font")));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAdd.ImageAlign")));
            this.btnAdd.ImageIndex = ((int)(resources.GetObject("btnAdd.ImageIndex")));
            this.btnAdd.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAdd.ImeMode")));
            this.btnAdd.Location = ((System.Drawing.Point)(resources.GetObject("btnAdd.Location")));
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAdd.RightToLeft")));
            this.btnAdd.Size = ((System.Drawing.Size)(resources.GetObject("btnAdd.Size")));
            this.btnAdd.TabIndex = ((int)(resources.GetObject("btnAdd.TabIndex")));
            this.btnAdd.Text = resources.GetString("btnAdd.Text");
            this.btnAdd.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAdd.TextAlign")));
            this.btnAdd.Visible = ((bool)(resources.GetObject("btnAdd.Visible")));
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.AccessibleDescription = resources.GetString("btnDelUser.AccessibleDescription");
            this.btnDelUser.AccessibleName = resources.GetString("btnDelUser.AccessibleName");
            this.btnDelUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnDelUser.Anchor")));
            this.btnDelUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelUser.BackgroundImage")));
            this.btnDelUser.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDelUser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnDelUser.Dock")));
            this.btnDelUser.Enabled = ((bool)(resources.GetObject("btnDelUser.Enabled")));
            this.btnDelUser.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnDelUser.FlatStyle")));
            this.btnDelUser.Font = ((System.Drawing.Font)(resources.GetObject("btnDelUser.Font")));
            this.btnDelUser.Image = ((System.Drawing.Image)(resources.GetObject("btnDelUser.Image")));
            this.btnDelUser.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDelUser.ImageAlign")));
            this.btnDelUser.ImageIndex = ((int)(resources.GetObject("btnDelUser.ImageIndex")));
            this.btnDelUser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnDelUser.ImeMode")));
            this.btnDelUser.Location = ((System.Drawing.Point)(resources.GetObject("btnDelUser.Location")));
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnDelUser.RightToLeft")));
            this.btnDelUser.Size = ((System.Drawing.Size)(resources.GetObject("btnDelUser.Size")));
            this.btnDelUser.TabIndex = ((int)(resources.GetObject("btnDelUser.TabIndex")));
            this.btnDelUser.Text = resources.GetString("btnDelUser.Text");
            this.btnDelUser.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDelUser.TextAlign")));
            this.btnDelUser.Visible = ((bool)(resources.GetObject("btnDelUser.Visible")));
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.AccessibleDescription = resources.GetString("btnAddNewUser.AccessibleDescription");
            this.btnAddNewUser.AccessibleName = resources.GetString("btnAddNewUser.AccessibleName");
            this.btnAddNewUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAddNewUser.Anchor")));
            this.btnAddNewUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddNewUser.BackgroundImage")));
            this.btnAddNewUser.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAddNewUser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAddNewUser.Dock")));
            this.btnAddNewUser.Enabled = ((bool)(resources.GetObject("btnAddNewUser.Enabled")));
            this.btnAddNewUser.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAddNewUser.FlatStyle")));
            this.btnAddNewUser.Font = ((System.Drawing.Font)(resources.GetObject("btnAddNewUser.Font")));
            this.btnAddNewUser.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewUser.Image")));
            this.btnAddNewUser.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddNewUser.ImageAlign")));
            this.btnAddNewUser.ImageIndex = ((int)(resources.GetObject("btnAddNewUser.ImageIndex")));
            this.btnAddNewUser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAddNewUser.ImeMode")));
            this.btnAddNewUser.Location = ((System.Drawing.Point)(resources.GetObject("btnAddNewUser.Location")));
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAddNewUser.RightToLeft")));
            this.btnAddNewUser.Size = ((System.Drawing.Size)(resources.GetObject("btnAddNewUser.Size")));
            this.btnAddNewUser.TabIndex = ((int)(resources.GetObject("btnAddNewUser.TabIndex")));
            this.btnAddNewUser.Text = resources.GetString("btnAddNewUser.Text");
            this.btnAddNewUser.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddNewUser.TextAlign")));
            this.btnAddNewUser.Visible = ((bool)(resources.GetObject("btnAddNewUser.Visible")));
            this.btnAddNewUser.Click += new System.EventHandler(this.btnAddNewUser_Click);
            // 
            // btnAddNewRole
            // 
            this.btnAddNewRole.AccessibleDescription = resources.GetString("btnAddNewRole.AccessibleDescription");
            this.btnAddNewRole.AccessibleName = resources.GetString("btnAddNewRole.AccessibleName");
            this.btnAddNewRole.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAddNewRole.Anchor")));
            this.btnAddNewRole.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddNewRole.BackgroundImage")));
            this.btnAddNewRole.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAddNewRole.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAddNewRole.Dock")));
            this.btnAddNewRole.Enabled = ((bool)(resources.GetObject("btnAddNewRole.Enabled")));
            this.btnAddNewRole.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAddNewRole.FlatStyle")));
            this.btnAddNewRole.Font = ((System.Drawing.Font)(resources.GetObject("btnAddNewRole.Font")));
            this.btnAddNewRole.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewRole.Image")));
            this.btnAddNewRole.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddNewRole.ImageAlign")));
            this.btnAddNewRole.ImageIndex = ((int)(resources.GetObject("btnAddNewRole.ImageIndex")));
            this.btnAddNewRole.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAddNewRole.ImeMode")));
            this.btnAddNewRole.Location = ((System.Drawing.Point)(resources.GetObject("btnAddNewRole.Location")));
            this.btnAddNewRole.Name = "btnAddNewRole";
            this.btnAddNewRole.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAddNewRole.RightToLeft")));
            this.btnAddNewRole.Size = ((System.Drawing.Size)(resources.GetObject("btnAddNewRole.Size")));
            this.btnAddNewRole.TabIndex = ((int)(resources.GetObject("btnAddNewRole.TabIndex")));
            this.btnAddNewRole.Text = resources.GetString("btnAddNewRole.Text");
            this.btnAddNewRole.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAddNewRole.TextAlign")));
            this.btnAddNewRole.Visible = ((bool)(resources.GetObject("btnAddNewRole.Visible")));
            this.btnAddNewRole.Click += new System.EventHandler(this.btnAddNewRole_Click);
            // 
            // btnDelRole
            // 
            this.btnDelRole.AccessibleDescription = resources.GetString("btnDelRole.AccessibleDescription");
            this.btnDelRole.AccessibleName = resources.GetString("btnDelRole.AccessibleName");
            this.btnDelRole.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnDelRole.Anchor")));
            this.btnDelRole.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelRole.BackgroundImage")));
            this.btnDelRole.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDelRole.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnDelRole.Dock")));
            this.btnDelRole.Enabled = ((bool)(resources.GetObject("btnDelRole.Enabled")));
            this.btnDelRole.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnDelRole.FlatStyle")));
            this.btnDelRole.Font = ((System.Drawing.Font)(resources.GetObject("btnDelRole.Font")));
            this.btnDelRole.Image = ((System.Drawing.Image)(resources.GetObject("btnDelRole.Image")));
            this.btnDelRole.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDelRole.ImageAlign")));
            this.btnDelRole.ImageIndex = ((int)(resources.GetObject("btnDelRole.ImageIndex")));
            this.btnDelRole.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnDelRole.ImeMode")));
            this.btnDelRole.Location = ((System.Drawing.Point)(resources.GetObject("btnDelRole.Location")));
            this.btnDelRole.Name = "btnDelRole";
            this.btnDelRole.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnDelRole.RightToLeft")));
            this.btnDelRole.Size = ((System.Drawing.Size)(resources.GetObject("btnDelRole.Size")));
            this.btnDelRole.TabIndex = ((int)(resources.GetObject("btnDelRole.TabIndex")));
            this.btnDelRole.Text = resources.GetString("btnDelRole.Text");
            this.btnDelRole.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDelRole.TextAlign")));
            this.btnDelRole.Visible = ((bool)(resources.GetObject("btnDelRole.Visible")));
            this.btnDelRole.Click += new System.EventHandler(this.btnDelRole_Click);
            // 
            // editUserButton
            // 
            this.editUserButton.AccessibleDescription = resources.GetString("editUserButton.AccessibleDescription");
            this.editUserButton.AccessibleName = resources.GetString("editUserButton.AccessibleName");
            this.editUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("editUserButton.Anchor")));
            this.editUserButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("editUserButton.BackgroundImage")));
            this.editUserButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.editUserButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("editUserButton.Dock")));
            this.editUserButton.Enabled = ((bool)(resources.GetObject("editUserButton.Enabled")));
            this.editUserButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("editUserButton.FlatStyle")));
            this.editUserButton.Font = ((System.Drawing.Font)(resources.GetObject("editUserButton.Font")));
            this.editUserButton.Image = ((System.Drawing.Image)(resources.GetObject("editUserButton.Image")));
            this.editUserButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editUserButton.ImageAlign")));
            this.editUserButton.ImageIndex = ((int)(resources.GetObject("editUserButton.ImageIndex")));
            this.editUserButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("editUserButton.ImeMode")));
            this.editUserButton.Location = ((System.Drawing.Point)(resources.GetObject("editUserButton.Location")));
            this.editUserButton.Name = "editUserButton";
            this.editUserButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("editUserButton.RightToLeft")));
            this.editUserButton.Size = ((System.Drawing.Size)(resources.GetObject("editUserButton.Size")));
            this.editUserButton.TabIndex = ((int)(resources.GetObject("editUserButton.TabIndex")));
            this.editUserButton.Text = resources.GetString("editUserButton.Text");
            this.editUserButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("editUserButton.TextAlign")));
            this.editUserButton.Visible = ((bool)(resources.GetObject("editUserButton.Visible")));
            this.editUserButton.Click += new System.EventHandler(this.editUserButton_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleDescription = resources.GetString("btnQuit.AccessibleDescription");
            this.btnQuit.AccessibleName = resources.GetString("btnQuit.AccessibleName");
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnQuit.Anchor")));
            this.btnQuit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnQuit.BackgroundImage")));
            this.btnQuit.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnQuit.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnQuit.Dock")));
            this.btnQuit.Enabled = ((bool)(resources.GetObject("btnQuit.Enabled")));
            this.btnQuit.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnQuit.FlatStyle")));
            this.btnQuit.Font = ((System.Drawing.Font)(resources.GetObject("btnQuit.Font")));
            this.btnQuit.Image = ((System.Drawing.Image)(resources.GetObject("btnQuit.Image")));
            this.btnQuit.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnQuit.ImageAlign")));
            this.btnQuit.ImageIndex = ((int)(resources.GetObject("btnQuit.ImageIndex")));
            this.btnQuit.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnQuit.ImeMode")));
            this.btnQuit.Location = ((System.Drawing.Point)(resources.GetObject("btnQuit.Location")));
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnQuit.RightToLeft")));
            this.btnQuit.Size = ((System.Drawing.Size)(resources.GetObject("btnQuit.Size")));
            this.btnQuit.TabIndex = ((int)(resources.GetObject("btnQuit.TabIndex")));
            this.btnQuit.Text = resources.GetString("btnQuit.Text");
            this.btnQuit.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnQuit.TextAlign")));
            this.btnQuit.Visible = ((bool)(resources.GetObject("btnQuit.Visible")));
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // cbxDbInstances
            // 
            this.cbxDbInstances.AccessibleDescription = resources.GetString("cbxDbInstances.AccessibleDescription");
            this.cbxDbInstances.AccessibleName = resources.GetString("cbxDbInstances.AccessibleName");
            this.cbxDbInstances.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cbxDbInstances.Anchor")));
            this.cbxDbInstances.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cbxDbInstances.BackgroundImage")));
            this.cbxDbInstances.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbxDbInstances.DisplayMember = "Name";
            this.cbxDbInstances.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cbxDbInstances.Dock")));
            this.cbxDbInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDbInstances.Enabled = ((bool)(resources.GetObject("cbxDbInstances.Enabled")));
            this.cbxDbInstances.Font = ((System.Drawing.Font)(resources.GetObject("cbxDbInstances.Font")));
            this.cbxDbInstances.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cbxDbInstances.ImeMode")));
            this.cbxDbInstances.IntegralHeight = ((bool)(resources.GetObject("cbxDbInstances.IntegralHeight")));
            this.cbxDbInstances.ItemHeight = ((int)(resources.GetObject("cbxDbInstances.ItemHeight")));
            this.cbxDbInstances.Location = ((System.Drawing.Point)(resources.GetObject("cbxDbInstances.Location")));
            this.cbxDbInstances.MaxDropDownItems = ((int)(resources.GetObject("cbxDbInstances.MaxDropDownItems")));
            this.cbxDbInstances.MaxLength = ((int)(resources.GetObject("cbxDbInstances.MaxLength")));
            this.cbxDbInstances.Name = "cbxDbInstances";
            this.cbxDbInstances.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cbxDbInstances.RightToLeft")));
            this.cbxDbInstances.Size = ((System.Drawing.Size)(resources.GetObject("cbxDbInstances.Size")));
            this.cbxDbInstances.TabIndex = ((int)(resources.GetObject("cbxDbInstances.TabIndex")));
            this.cbxDbInstances.Text = resources.GetString("cbxDbInstances.Text");
            this.cbxDbInstances.ValueMember = "Name";
            this.cbxDbInstances.Visible = ((bool)(resources.GetObject("cbxDbInstances.Visible")));
            this.cbxDbInstances.SelectedIndexChanged += new System.EventHandler(this.cbxDbInstances_SelectedIndexChanged);
            // 
            // databasesGroupBox
            // 
            this.databasesGroupBox.AccessibleDescription = resources.GetString("databasesGroupBox.AccessibleDescription");
            this.databasesGroupBox.AccessibleName = resources.GetString("databasesGroupBox.AccessibleName");
            this.databasesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("databasesGroupBox.Anchor")));
            this.databasesGroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("databasesGroupBox.BackgroundImage")));
            this.databasesGroupBox.Controls.Add(this.selectDbButton);
            this.databasesGroupBox.Controls.Add(this.cbxDbInstances);
            this.databasesGroupBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("databasesGroupBox.Dock")));
            this.databasesGroupBox.Enabled = ((bool)(resources.GetObject("databasesGroupBox.Enabled")));
            this.databasesGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.databasesGroupBox.Font = ((System.Drawing.Font)(resources.GetObject("databasesGroupBox.Font")));
            this.databasesGroupBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("databasesGroupBox.ImeMode")));
            this.databasesGroupBox.Location = ((System.Drawing.Point)(resources.GetObject("databasesGroupBox.Location")));
            this.databasesGroupBox.Name = "databasesGroupBox";
            this.databasesGroupBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("databasesGroupBox.RightToLeft")));
            this.databasesGroupBox.Size = ((System.Drawing.Size)(resources.GetObject("databasesGroupBox.Size")));
            this.databasesGroupBox.TabIndex = ((int)(resources.GetObject("databasesGroupBox.TabIndex")));
            this.databasesGroupBox.TabStop = false;
            this.databasesGroupBox.Text = resources.GetString("databasesGroupBox.Text");
            this.databasesGroupBox.Visible = ((bool)(resources.GetObject("databasesGroupBox.Visible")));
            // 
            // selectDbButton
            // 
            this.selectDbButton.AccessibleDescription = resources.GetString("selectDbButton.AccessibleDescription");
            this.selectDbButton.AccessibleName = resources.GetString("selectDbButton.AccessibleName");
            this.selectDbButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("selectDbButton.Anchor")));
            this.selectDbButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectDbButton.BackgroundImage")));
            this.selectDbButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.selectDbButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("selectDbButton.Dock")));
            this.selectDbButton.Enabled = ((bool)(resources.GetObject("selectDbButton.Enabled")));
            this.selectDbButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("selectDbButton.FlatStyle")));
            this.selectDbButton.Font = ((System.Drawing.Font)(resources.GetObject("selectDbButton.Font")));
            this.selectDbButton.Image = ((System.Drawing.Image)(resources.GetObject("selectDbButton.Image")));
            this.selectDbButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectDbButton.ImageAlign")));
            this.selectDbButton.ImageIndex = ((int)(resources.GetObject("selectDbButton.ImageIndex")));
            this.selectDbButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("selectDbButton.ImeMode")));
            this.selectDbButton.Location = ((System.Drawing.Point)(resources.GetObject("selectDbButton.Location")));
            this.selectDbButton.Name = "selectDbButton";
            this.selectDbButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("selectDbButton.RightToLeft")));
            this.selectDbButton.Size = ((System.Drawing.Size)(resources.GetObject("selectDbButton.Size")));
            this.selectDbButton.TabIndex = ((int)(resources.GetObject("selectDbButton.TabIndex")));
            this.selectDbButton.Text = resources.GetString("selectDbButton.Text");
            this.selectDbButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectDbButton.TextAlign")));
            this.selectDbButton.Visible = ((bool)(resources.GetObject("selectDbButton.Visible")));
            this.selectDbButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // MainForm
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.databasesGroupBox);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.gbxUserRoles);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "MainForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbxUserRoles.ResumeLayout(false);
            this.databasesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.Run(new MainForm());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SecuritySettings securitySettings = (SecuritySettings)configurationContext.GetConfiguration(SecuritySettings.SectionName);
            foreach(AuthenticationProviderData instance in securitySettings.AuthenticationProviders) 
            {
                if (instance is DbAuthenticationProviderData) 
                    cbxDbInstances.Items.Add(instance);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUsersInRole(cbxRoles.Text);
            UpdateEditRoleButtonEnabled();
            UpdateUserToRoleButtonsEnabled();
        }

		private void ConnectToDatabase()
		{
			this.gbxUserRoles.Enabled = false;
			string databaseInstance = this.DbAuthenticationProvider.Database;

            try 
            {
                this.Cursor = Cursors.WaitCursor;

                this.userRoleMgr = new UserRoleManager(databaseInstance, ConfigurationManager.GetCurrentContext());

                GetAllUsers();
                GetAllRoles();

                this.gbxUserRoles.Enabled = true;
            } 
            catch(Exception e)
            {
                string msg = SR.NoDatabaseExceptionMessage(paragraphSeparator, paragraphSeparator, paragraphSeparator, e.Message);
                MessageBox.Show(msg, SR.DatabaseConnectionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }
		}


    	private void AddUserRole()
    	{
    		string tmpUser = ((DataRowView)lbxUsers.SelectedItem)["UserName"].ToString();

			this.userRoleMgr.CreateUserRole(tmpUser, cbxRoles.Text);
			GetUsersInRole(cbxRoles.Text);
            UpdateUserToRoleButtonsEnabled();
    	}

	   	private void RemoveUserRole()
    	{
			string tmpUser = ((DataRowView)lbxRoleUsers.SelectedItem)["UserName"].ToString();

			this.userRoleMgr.DeleteUserRole(tmpUser, cbxRoles.Text);
			GetUsersInRole(cbxRoles.Text);
            UpdateUserToRoleButtonsEnabled();
		}

    	private void GetUsersInRole(string role)
        {
            if (role.Trim().Length > 0) 
            {
                DataSet ds = this.userRoleMgr.GetRoleUsers(role.Trim());

                lbxRoleUsers.BeginUpdate();
                lbxRoleUsers.DisplayMember = "UserName";
                lbxRoleUsers.ValueMember = "UserName";
                lbxRoleUsers.DataSource = ds.Tables[0];
                lbxRoleUsers.EndUpdate();

            } 
            else 
            {
                lbxRoleUsers.DataSource = null;
            }

            lblRoleUsers.Text = SR.UsersInRole(role);
        }

        private void GetAllRoles()
        {
            DataSet ds = this.userRoleMgr.GetAllRoles();

            cbxRoles.BeginUpdate();
			cbxRoles.DisplayMember = "RoleName";
			cbxRoles.ValueMember = "RoleName";
            cbxRoles.DataSource = ds.Tables[0];
			cbxRoles.EndUpdate();
        }

        private void GetAllUsers()
        {
            DataSet ds = this.userRoleMgr.GetAllUsers();

			lbxUsers.BeginUpdate();
			lbxUsers.DisplayMember = "UserName";
			lbxUsers.ValueMember = "UserName";
			lbxUsers.DataSource = ds.Tables[0];
			lbxUsers.EndUpdate();
        }

		private void connectButton_Click(object sender, EventArgs e)
		{
			ConnectToDatabase();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddUserRole();
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			RemoveUserRole();
		}

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            ShowUserFormDialog();
        }

        private void btnAddNewRole_Click(object sender, EventArgs e)
        {
            ShowRoleFormDialog();
            UpdateUserToRoleButtonsEnabled();
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            if (lbxUsers.SelectedIndex < 0)
            {
                MessageBox.Show(SR.MustSelectUserToDelete);
                return;
            }
            else
            {
                string message = SR.DeleteUserConfirmation;
                string caption = SR.DeleteUserConfirmationCaption;
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                if (MessageBox.Show(this, message, caption, buttons) == DialogResult.No)
                {
                    return;
                }
            }

			string user = ((DataRowView)lbxUsers.SelectedItem)["UserName"].ToString();

			this.userRoleMgr.DeleteUser(user);

            GetAllUsers();
            GetUsersInRole(cbxRoles.Text);
            UpdateEditUserButtonEnabled();
            UpdateUserToRoleButtonsEnabled();
        }

        private void btnDelRole_Click(object sender, EventArgs e)
        {
            if (cbxRoles.SelectedIndex < 0)
            {
                MessageBox.Show(SR.MustSelectRoleToDelete);
                return;
            }

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            if (MessageBox.Show(this, SR.DeleteRoleConfirmation, SR.DeleteRoleConfirmation, buttons) == DialogResult.Yes)
            {
                string role = ((DataRowView)cbxRoles.SelectedItem)["RoleName"].ToString();

                this.userRoleMgr.DeleteRole(role);

                GetAllRoles();
                GetAllUsers();
            }

            GetUsersInRole(cbxRoles.Text);
            UpdateEditRoleButtonEnabled();
            UpdateUserToRoleButtonsEnabled();
        }

        private void lbxUsers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateEditUserButtonEnabled();
            UpdateUserToRoleButtonsEnabled();
        }

        private void editUserButton_Click(object sender, System.EventArgs e)
        {
            ShowUserFormDialog(lbxUsers.SelectedValue.ToString()); 
        }

        private void UpdateEditUserButtonEnabled() 
        {
            editUserButton.Enabled = lbxUsers.SelectedIndex >= 0;
        }

        private void ShowUserFormDialog(string username) 
        {
            UserForm userForm;
            
            if (username.Length > 0) 
            {
                userForm = new UserForm(DbAuthenticationProvider, configurationContext, username);
            }
            else 
            {
                userForm = new UserForm(DbAuthenticationProvider, configurationContext);
            }

            if (userForm.ShowDialog(this) == DialogResult.OK)
            {
                GetAllUsers();
            }
        }

        private void ShowUserFormDialog() 
        {
            ShowUserFormDialog(String.Empty);
        }

        private void UpdateConnectButtonEnabled() 
        {
            selectDbButton.Enabled = cbxDbInstances.SelectedItem != null;
        }

        private void cbxDbInstances_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateConnectButtonEnabled();
        }

        private void btnEditRole_Click(object sender, System.EventArgs e)
        {
            ShowRoleFormDialog(cbxRoles.SelectedValue.ToString());
        }

        private void ShowRoleFormDialog() 
        {
            ShowRoleFormDialog(String.Empty);
        }

        private void ShowRoleFormDialog(string role) 
        {
            RoleForm roleForm;
            
            if (role.Length > 0) 
            {
                roleForm = new RoleForm(DbAuthenticationProvider, configurationContext, role);
            } 
            else 
            {
                roleForm = new RoleForm(DbAuthenticationProvider, configurationContext);
            }
            
            if (roleForm.ShowDialog(this) == DialogResult.OK)
            {
                GetAllRoles();

                cbxRoles.SelectedIndex = cbxRoles.FindStringExact(roleForm.RoleName);
                GetUsersInRole(roleForm.RoleName);
            } 
        }

        private void UpdateEditRoleButtonEnabled() 
        {
            btnEditRole.Enabled = cbxRoles.SelectedItem != null;
        }

        private void UpdateUserToRoleButtonsEnabled() 
        {
            btnAdd.Enabled = lbxUsers.SelectedItem != null && cbxRoles.SelectedItem != null && lbxRoleUsers.FindStringExact(lbxUsers.SelectedValue.ToString()) == -1;
            btnRemove.Enabled = lbxRoleUsers.SelectedItem != null;
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(
                e.Exception.Message,
                SR.UnhandledException,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}