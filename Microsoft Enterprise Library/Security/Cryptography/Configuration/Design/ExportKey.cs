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
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Export key dialog.
    /// </summary>
    public class ExportKey : Form
    {
        private const int MinPasswordLength = 5;
        private byte[] key;

        #region Form Designer
        private GroupBox grpDestination;
        private Button btnBrowse;
        private TextBox txtDestination;
        private Button btnExport;
        private Button btnCancel;
        private GroupBox grpPassword;
        private TextBox txtPassword2;
        private Label lblPassword2;
        private TextBox txtPassword1;
        private Label lblPassword1;
        private CheckBox chkEnabled;
        private Panel pnlPassword;
        private SaveFileDialog browseDialog;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExportKey));
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpPassword = new System.Windows.Forms.GroupBox();
            this.pnlPassword = new System.Windows.Forms.Panel();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.lblPassword1 = new System.Windows.Forms.Label();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.lblPassword2 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.browseDialog = new System.Windows.Forms.SaveFileDialog();
            this.grpDestination.SuspendLayout();
            this.grpPassword.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDestination
            // 
            this.grpDestination.AccessibleDescription = resources.GetString("grpDestination.AccessibleDescription");
            this.grpDestination.AccessibleName = resources.GetString("grpDestination.AccessibleName");
            this.grpDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDestination.Anchor")));
            this.grpDestination.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDestination.BackgroundImage")));
            this.grpDestination.Controls.Add(this.btnBrowse);
            this.grpDestination.Controls.Add(this.txtDestination);
            this.grpDestination.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDestination.Dock")));
            this.grpDestination.Enabled = ((bool)(resources.GetObject("grpDestination.Enabled")));
            this.grpDestination.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpDestination.Font = ((System.Drawing.Font)(resources.GetObject("grpDestination.Font")));
            this.grpDestination.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDestination.ImeMode")));
            this.grpDestination.Location = ((System.Drawing.Point)(resources.GetObject("grpDestination.Location")));
            this.grpDestination.Name = "grpDestination";
            this.grpDestination.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDestination.RightToLeft")));
            this.grpDestination.Size = ((System.Drawing.Size)(resources.GetObject("grpDestination.Size")));
            this.grpDestination.TabIndex = ((int)(resources.GetObject("grpDestination.TabIndex")));
            this.grpDestination.TabStop = false;
            this.grpDestination.Text = resources.GetString("grpDestination.Text");
            this.grpDestination.Visible = ((bool)(resources.GetObject("grpDestination.Visible")));
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleDescription = resources.GetString("btnBrowse.AccessibleDescription");
            this.btnBrowse.AccessibleName = resources.GetString("btnBrowse.AccessibleName");
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBrowse.Anchor")));
            this.btnBrowse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowse.BackgroundImage")));
            this.btnBrowse.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBrowse.Dock")));
            this.btnBrowse.Enabled = ((bool)(resources.GetObject("btnBrowse.Enabled")));
            this.btnBrowse.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBrowse.FlatStyle")));
            this.btnBrowse.Font = ((System.Drawing.Font)(resources.GetObject("btnBrowse.Font")));
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowse.ImageAlign")));
            this.btnBrowse.ImageIndex = ((int)(resources.GetObject("btnBrowse.ImageIndex")));
            this.btnBrowse.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBrowse.ImeMode")));
            this.btnBrowse.Location = ((System.Drawing.Point)(resources.GetObject("btnBrowse.Location")));
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBrowse.RightToLeft")));
            this.btnBrowse.Size = ((System.Drawing.Size)(resources.GetObject("btnBrowse.Size")));
            this.btnBrowse.TabIndex = ((int)(resources.GetObject("btnBrowse.TabIndex")));
            this.btnBrowse.Text = resources.GetString("btnBrowse.Text");
            this.btnBrowse.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowse.TextAlign")));
            this.btnBrowse.Visible = ((bool)(resources.GetObject("btnBrowse.Visible")));
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtDestination
            // 
            this.txtDestination.AccessibleDescription = resources.GetString("txtDestination.AccessibleDescription");
            this.txtDestination.AccessibleName = resources.GetString("txtDestination.AccessibleName");
            this.txtDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDestination.Anchor")));
            this.txtDestination.AutoSize = ((bool)(resources.GetObject("txtDestination.AutoSize")));
            this.txtDestination.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDestination.BackgroundImage")));
            this.txtDestination.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDestination.Dock")));
            this.txtDestination.Enabled = ((bool)(resources.GetObject("txtDestination.Enabled")));
            this.txtDestination.Font = ((System.Drawing.Font)(resources.GetObject("txtDestination.Font")));
            this.txtDestination.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDestination.ImeMode")));
            this.txtDestination.Location = ((System.Drawing.Point)(resources.GetObject("txtDestination.Location")));
            this.txtDestination.MaxLength = ((int)(resources.GetObject("txtDestination.MaxLength")));
            this.txtDestination.Multiline = ((bool)(resources.GetObject("txtDestination.Multiline")));
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.PasswordChar = ((char)(resources.GetObject("txtDestination.PasswordChar")));
            this.txtDestination.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDestination.RightToLeft")));
            this.txtDestination.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDestination.ScrollBars")));
            this.txtDestination.Size = ((System.Drawing.Size)(resources.GetObject("txtDestination.Size")));
            this.txtDestination.TabIndex = ((int)(resources.GetObject("txtDestination.TabIndex")));
            this.txtDestination.Text = resources.GetString("txtDestination.Text");
            this.txtDestination.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDestination.TextAlign")));
            this.txtDestination.Visible = ((bool)(resources.GetObject("txtDestination.Visible")));
            this.txtDestination.WordWrap = ((bool)(resources.GetObject("txtDestination.WordWrap")));
            // 
            // btnExport
            // 
            this.btnExport.AccessibleDescription = resources.GetString("btnExport.AccessibleDescription");
            this.btnExport.AccessibleName = resources.GetString("btnExport.AccessibleName");
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnExport.Anchor")));
            this.btnExport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExport.BackgroundImage")));
            this.btnExport.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnExport.Dock")));
            this.btnExport.Enabled = ((bool)(resources.GetObject("btnExport.Enabled")));
            this.btnExport.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnExport.FlatStyle")));
            this.btnExport.Font = ((System.Drawing.Font)(resources.GetObject("btnExport.Font")));
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnExport.ImageAlign")));
            this.btnExport.ImageIndex = ((int)(resources.GetObject("btnExport.ImageIndex")));
            this.btnExport.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnExport.ImeMode")));
            this.btnExport.Location = ((System.Drawing.Point)(resources.GetObject("btnExport.Location")));
            this.btnExport.Name = "btnExport";
            this.btnExport.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnExport.RightToLeft")));
            this.btnExport.Size = ((System.Drawing.Size)(resources.GetObject("btnExport.Size")));
            this.btnExport.TabIndex = ((int)(resources.GetObject("btnExport.TabIndex")));
            this.btnExport.Text = resources.GetString("btnExport.Text");
            this.btnExport.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnExport.TextAlign")));
            this.btnExport.Visible = ((bool)(resources.GetObject("btnExport.Visible")));
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            // grpPassword
            // 
            this.grpPassword.AccessibleDescription = resources.GetString("grpPassword.AccessibleDescription");
            this.grpPassword.AccessibleName = resources.GetString("grpPassword.AccessibleName");
            this.grpPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpPassword.Anchor")));
            this.grpPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpPassword.BackgroundImage")));
            this.grpPassword.Controls.Add(this.pnlPassword);
            this.grpPassword.Controls.Add(this.chkEnabled);
            this.grpPassword.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpPassword.Dock")));
            this.grpPassword.Enabled = ((bool)(resources.GetObject("grpPassword.Enabled")));
            this.grpPassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpPassword.Font = ((System.Drawing.Font)(resources.GetObject("grpPassword.Font")));
            this.grpPassword.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpPassword.ImeMode")));
            this.grpPassword.Location = ((System.Drawing.Point)(resources.GetObject("grpPassword.Location")));
            this.grpPassword.Name = "grpPassword";
            this.grpPassword.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpPassword.RightToLeft")));
            this.grpPassword.Size = ((System.Drawing.Size)(resources.GetObject("grpPassword.Size")));
            this.grpPassword.TabIndex = ((int)(resources.GetObject("grpPassword.TabIndex")));
            this.grpPassword.TabStop = false;
            this.grpPassword.Text = resources.GetString("grpPassword.Text");
            this.grpPassword.Visible = ((bool)(resources.GetObject("grpPassword.Visible")));
            // 
            // pnlPassword
            // 
            this.pnlPassword.AccessibleDescription = resources.GetString("pnlPassword.AccessibleDescription");
            this.pnlPassword.AccessibleName = resources.GetString("pnlPassword.AccessibleName");
            this.pnlPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlPassword.Anchor")));
            this.pnlPassword.AutoScroll = ((bool)(resources.GetObject("pnlPassword.AutoScroll")));
            this.pnlPassword.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlPassword.AutoScrollMargin")));
            this.pnlPassword.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlPassword.AutoScrollMinSize")));
            this.pnlPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlPassword.BackgroundImage")));
            this.pnlPassword.Controls.Add(this.txtPassword1);
            this.pnlPassword.Controls.Add(this.lblPassword1);
            this.pnlPassword.Controls.Add(this.txtPassword2);
            this.pnlPassword.Controls.Add(this.lblPassword2);
            this.pnlPassword.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlPassword.Dock")));
            this.pnlPassword.Enabled = ((bool)(resources.GetObject("pnlPassword.Enabled")));
            this.pnlPassword.Font = ((System.Drawing.Font)(resources.GetObject("pnlPassword.Font")));
            this.pnlPassword.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlPassword.ImeMode")));
            this.pnlPassword.Location = ((System.Drawing.Point)(resources.GetObject("pnlPassword.Location")));
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlPassword.RightToLeft")));
            this.pnlPassword.Size = ((System.Drawing.Size)(resources.GetObject("pnlPassword.Size")));
            this.pnlPassword.TabIndex = ((int)(resources.GetObject("pnlPassword.TabIndex")));
            this.pnlPassword.Text = resources.GetString("pnlPassword.Text");
            this.pnlPassword.Visible = ((bool)(resources.GetObject("pnlPassword.Visible")));
            // 
            // txtPassword1
            // 
            this.txtPassword1.AccessibleDescription = resources.GetString("txtPassword1.AccessibleDescription");
            this.txtPassword1.AccessibleName = resources.GetString("txtPassword1.AccessibleName");
            this.txtPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtPassword1.Anchor")));
            this.txtPassword1.AutoSize = ((bool)(resources.GetObject("txtPassword1.AutoSize")));
            this.txtPassword1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtPassword1.BackgroundImage")));
            this.txtPassword1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtPassword1.Dock")));
            this.txtPassword1.Enabled = ((bool)(resources.GetObject("txtPassword1.Enabled")));
            this.txtPassword1.Font = ((System.Drawing.Font)(resources.GetObject("txtPassword1.Font")));
            this.txtPassword1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtPassword1.ImeMode")));
            this.txtPassword1.Location = ((System.Drawing.Point)(resources.GetObject("txtPassword1.Location")));
            this.txtPassword1.MaxLength = ((int)(resources.GetObject("txtPassword1.MaxLength")));
            this.txtPassword1.Multiline = ((bool)(resources.GetObject("txtPassword1.Multiline")));
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.PasswordChar = ((char)(resources.GetObject("txtPassword1.PasswordChar")));
            this.txtPassword1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtPassword1.RightToLeft")));
            this.txtPassword1.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtPassword1.ScrollBars")));
            this.txtPassword1.Size = ((System.Drawing.Size)(resources.GetObject("txtPassword1.Size")));
            this.txtPassword1.TabIndex = ((int)(resources.GetObject("txtPassword1.TabIndex")));
            this.txtPassword1.Text = resources.GetString("txtPassword1.Text");
            this.txtPassword1.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtPassword1.TextAlign")));
            this.txtPassword1.Visible = ((bool)(resources.GetObject("txtPassword1.Visible")));
            this.txtPassword1.WordWrap = ((bool)(resources.GetObject("txtPassword1.WordWrap")));
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
            // txtPassword2
            // 
            this.txtPassword2.AccessibleDescription = resources.GetString("txtPassword2.AccessibleDescription");
            this.txtPassword2.AccessibleName = resources.GetString("txtPassword2.AccessibleName");
            this.txtPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtPassword2.Anchor")));
            this.txtPassword2.AutoSize = ((bool)(resources.GetObject("txtPassword2.AutoSize")));
            this.txtPassword2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtPassword2.BackgroundImage")));
            this.txtPassword2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtPassword2.Dock")));
            this.txtPassword2.Enabled = ((bool)(resources.GetObject("txtPassword2.Enabled")));
            this.txtPassword2.Font = ((System.Drawing.Font)(resources.GetObject("txtPassword2.Font")));
            this.txtPassword2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtPassword2.ImeMode")));
            this.txtPassword2.Location = ((System.Drawing.Point)(resources.GetObject("txtPassword2.Location")));
            this.txtPassword2.MaxLength = ((int)(resources.GetObject("txtPassword2.MaxLength")));
            this.txtPassword2.Multiline = ((bool)(resources.GetObject("txtPassword2.Multiline")));
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.PasswordChar = ((char)(resources.GetObject("txtPassword2.PasswordChar")));
            this.txtPassword2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtPassword2.RightToLeft")));
            this.txtPassword2.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtPassword2.ScrollBars")));
            this.txtPassword2.Size = ((System.Drawing.Size)(resources.GetObject("txtPassword2.Size")));
            this.txtPassword2.TabIndex = ((int)(resources.GetObject("txtPassword2.TabIndex")));
            this.txtPassword2.Text = resources.GetString("txtPassword2.Text");
            this.txtPassword2.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtPassword2.TextAlign")));
            this.txtPassword2.Visible = ((bool)(resources.GetObject("txtPassword2.Visible")));
            this.txtPassword2.WordWrap = ((bool)(resources.GetObject("txtPassword2.WordWrap")));
            // 
            // lblPassword2
            // 
            this.lblPassword2.AccessibleDescription = resources.GetString("lblPassword2.AccessibleDescription");
            this.lblPassword2.AccessibleName = resources.GetString("lblPassword2.AccessibleName");
            this.lblPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPassword2.Anchor")));
            this.lblPassword2.AutoSize = ((bool)(resources.GetObject("lblPassword2.AutoSize")));
            this.lblPassword2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPassword2.Dock")));
            this.lblPassword2.Enabled = ((bool)(resources.GetObject("lblPassword2.Enabled")));
            this.lblPassword2.Font = ((System.Drawing.Font)(resources.GetObject("lblPassword2.Font")));
            this.lblPassword2.Image = ((System.Drawing.Image)(resources.GetObject("lblPassword2.Image")));
            this.lblPassword2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPassword2.ImageAlign")));
            this.lblPassword2.ImageIndex = ((int)(resources.GetObject("lblPassword2.ImageIndex")));
            this.lblPassword2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPassword2.ImeMode")));
            this.lblPassword2.Location = ((System.Drawing.Point)(resources.GetObject("lblPassword2.Location")));
            this.lblPassword2.Name = "lblPassword2";
            this.lblPassword2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPassword2.RightToLeft")));
            this.lblPassword2.Size = ((System.Drawing.Size)(resources.GetObject("lblPassword2.Size")));
            this.lblPassword2.TabIndex = ((int)(resources.GetObject("lblPassword2.TabIndex")));
            this.lblPassword2.Text = resources.GetString("lblPassword2.Text");
            this.lblPassword2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPassword2.TextAlign")));
            this.lblPassword2.Visible = ((bool)(resources.GetObject("lblPassword2.Visible")));
            // 
            // chkEnabled
            // 
            this.chkEnabled.AccessibleDescription = resources.GetString("chkEnabled.AccessibleDescription");
            this.chkEnabled.AccessibleName = resources.GetString("chkEnabled.AccessibleName");
            this.chkEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkEnabled.Anchor")));
            this.chkEnabled.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkEnabled.Appearance")));
            this.chkEnabled.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkEnabled.BackgroundImage")));
            this.chkEnabled.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEnabled.CheckAlign")));
            this.chkEnabled.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkEnabled.Dock")));
            this.chkEnabled.Enabled = ((bool)(resources.GetObject("chkEnabled.Enabled")));
            this.chkEnabled.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkEnabled.FlatStyle")));
            this.chkEnabled.Font = ((System.Drawing.Font)(resources.GetObject("chkEnabled.Font")));
            this.chkEnabled.Image = ((System.Drawing.Image)(resources.GetObject("chkEnabled.Image")));
            this.chkEnabled.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEnabled.ImageAlign")));
            this.chkEnabled.ImageIndex = ((int)(resources.GetObject("chkEnabled.ImageIndex")));
            this.chkEnabled.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkEnabled.ImeMode")));
            this.chkEnabled.Location = ((System.Drawing.Point)(resources.GetObject("chkEnabled.Location")));
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkEnabled.RightToLeft")));
            this.chkEnabled.Size = ((System.Drawing.Size)(resources.GetObject("chkEnabled.Size")));
            this.chkEnabled.TabIndex = ((int)(resources.GetObject("chkEnabled.TabIndex")));
            this.chkEnabled.Text = resources.GetString("chkEnabled.Text");
            this.chkEnabled.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkEnabled.TextAlign")));
            this.chkEnabled.Visible = ((bool)(resources.GetObject("chkEnabled.Visible")));
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // browseDialog
            // 
            this.browseDialog.Filter = resources.GetString("browseDialog.Filter");
            this.browseDialog.Title = resources.GetString("browseDialog.Title");
            // 
            // ExportKey
            // 
            this.AcceptButton = this.btnExport;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnCancel;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.grpPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.grpDestination);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "ExportKey";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.grpDestination.ResumeLayout(false);
            this.grpPassword.ResumeLayout(false);
            this.pnlPassword.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        /// <summary>
        /// Initializes with the key to export.
        /// </summary>
        /// <param name="key">The key to export.</param>
        public ExportKey(byte[] key)
        {
            this.key = key;

            InitializeComponent();

            txtPassword1.Text = String.Empty;
            txtPassword2.Text = String.Empty;
            txtDestination.Text = String.Empty;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            browseDialog.Filter = SR.ExportFileFilter;
            browseDialog.Title = SR.ExportDialogTitle;
            DialogResult result = browseDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDestination.Text = browseDialog.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Export())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool Export()
        {
            if (!ConfirmPassword())
            {
                return false;
            }
            if (!ValidatePath())
            {
                return false;
            }
            
            if (!OverwriteFile())
            {
                return false;
            }

            ImportExportUtility utility = new ImportExportUtility();
            
            utility.Export(this.key, txtDestination.Text, chkEnabled.Checked ? txtPassword1.Text : String.Empty);

            MessageBox.Show(SR.ExportSuccessful, SR.ExportSuccessfulTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;
        }

        private bool OverwriteFile()
        {
            if (!FileHelper.IsFileReadOnly(txtDestination.Text)) return true;

            DialogResult result = MessageBox.Show(SR.OverwriteExportFileMessage(txtDestination.Text), SR.OverwriteExportFileCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult.No == result) return false;
            FileHelper.ChangeFileAttributesToWritable(txtDestination.Text);
            return true;
        }

        private bool ValidatePath()
        {
            bool result = true;

            if (txtDestination.Text.Length == 0)
            {
                result = false;
            }
            else
            {
                string directory = Path.GetDirectoryName(txtDestination.Text);
                if (!Directory.Exists(directory))
                {
                    result = false;
                }
            }

            if (!result)
            {
                MessageBox.Show(SR.ExportDirectoryInvalid, SR.ExportDialogErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return result;
        }

        private bool ConfirmPassword()
        {
            bool result = true;

            if (chkEnabled.Checked)
            {
                result = (txtPassword1.Text == txtPassword2.Text);
                if (!result)
                {
                    MessageBox.Show(SR.ExportPasswordsDoNotMatch, SR.ExportDialogErrorTitle,
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (txtPassword1.Text.Length < MinPasswordLength)
                    {
                        MessageBox.Show(SR.ExportPasswordMinLength(MinPasswordLength.ToString(CultureInfo.CurrentCulture)),
                                        SR.ExportDialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        result = false;
                    }
                }
            }
            return result;
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            pnlPassword.Enabled = chkEnabled.Checked;
        }
    }
}