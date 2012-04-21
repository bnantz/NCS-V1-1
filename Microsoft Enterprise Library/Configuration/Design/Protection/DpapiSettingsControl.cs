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
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <devdoc>
    /// A control for Dpapi settings.
    /// </devdoc>
    internal class DpapiSettingsControl : UserControl
    {
        private RadioButton radNone;
        private Panel pnlEntropy;
        private Label lblEntropy;
        private TextBox txtEntropy;
        private RadioButton radMachine;
        private RadioButton radUser;
        private bool modeRequired;
        private Container components = null;

        private const int EntropySize = 16;
        private const int EntropySizeBase64 = 24;
        
        public DpapiSettingsControl() : this(true)
        {
        }

        public DpapiSettingsControl(bool modeRequired)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            ModeRequired = modeRequired;
            pnlEntropy.Enabled = false;
        }
        
        public bool ModeRequired
        {
            get { return modeRequired; }
            set
            {
                modeRequired = value;
                radNone.Enabled = !modeRequired;
            }
        }
        
        public DpapiStorageMode DpapiStorageMode
        {
            get
            {
                if (radUser.Checked)
                {
                    return DpapiStorageMode.User;
                }
                if (radMachine.Checked)
                {
                    return DpapiStorageMode.Machine;
                }
                return DpapiStorageMode.None;
            }
            set
            {
                switch (value)
                {
                    case DpapiStorageMode.User:
                        radUser.Checked = true;
                        break;
                    case DpapiStorageMode.Machine:
                        radMachine.Checked = true;
                        break;
                    case DpapiStorageMode.None:
                        radNone.Checked = true;
                        break;
                }
            }
        }

        /// <devdoc>
        /// Gets or sets the configured entropy for use with machine mode DPAPI.
        /// </devdoc>
        public byte[] Entropy
        {
            get
            {
                if (txtEntropy.Enabled && ValidateEntropy())
                {
                    return CryptographyUtility.GetBytesFromHexString(txtEntropy.Text);
                }
                else
                {
                    return new byte[0];
                }
            }
            set
            {
                if (value.Length == 0)
                {
                    txtEntropy.Text = String.Empty;
                }
                else
                {
                    txtEntropy.Text = CryptographyUtility.GetHexStringFromBytes(value);
                }
            }
        }
        
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (Enabled)
            {
                UpdateEntropyPanel();
                radMachine.Enabled = true;
                radUser.Enabled = true;
                radNone.Enabled = !ModeRequired;
            }
            else
            {
                radMachine.Enabled = false;
                radUser.Enabled = false;
                radNone.Enabled = false;
                pnlEntropy.Enabled = false;
            }
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

        private bool ValidateEntropy()
        {
            bool result = true;
            if (txtEntropy.TextLength < EntropySizeBase64)
            {
                result = false;
            }

            try
            {
                CryptographyUtility.GetBytesFromHexString(txtEntropy.Text);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private void GenerateEntropy()
        {
            if (txtEntropy.TextLength == 0)
            {
                byte[] entropy = new byte[EntropySize];
                RandomNumberGenerator.Create().GetBytes(entropy);

                string hex = CryptographyUtility.GetHexStringFromBytes(entropy);
                txtEntropy.Text = hex;
            }
        }

        private void UpdateEntropyPanel()
        {
            pnlEntropy.Enabled = this.radMachine.Checked;

            if (pnlEntropy.Enabled && txtEntropy.Text.Length == 0)
            {
                GenerateEntropy();
            }
        }

        private void radUser_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntropyPanel();
        }

        private void radMachine_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntropyPanel();
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DpapiSettingsControl));
            this.radNone = new System.Windows.Forms.RadioButton();
            this.pnlEntropy = new System.Windows.Forms.Panel();
            this.lblEntropy = new System.Windows.Forms.Label();
            this.txtEntropy = new System.Windows.Forms.TextBox();
            this.radMachine = new System.Windows.Forms.RadioButton();
            this.radUser = new System.Windows.Forms.RadioButton();
            this.pnlEntropy.SuspendLayout();
            this.SuspendLayout();
            // 
            // radNone
            // 
            this.radNone.AccessibleDescription = resources.GetString("radNone.AccessibleDescription");
            this.radNone.AccessibleName = resources.GetString("radNone.AccessibleName");
            this.radNone.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radNone.Anchor")));
            this.radNone.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radNone.Appearance")));
            this.radNone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radNone.BackgroundImage")));
            this.radNone.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNone.CheckAlign")));
            this.radNone.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radNone.Dock")));
            this.radNone.Enabled = ((bool)(resources.GetObject("radNone.Enabled")));
            this.radNone.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radNone.FlatStyle")));
            this.radNone.Font = ((System.Drawing.Font)(resources.GetObject("radNone.Font")));
            this.radNone.Image = ((System.Drawing.Image)(resources.GetObject("radNone.Image")));
            this.radNone.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNone.ImageAlign")));
            this.radNone.ImageIndex = ((int)(resources.GetObject("radNone.ImageIndex")));
            this.radNone.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radNone.ImeMode")));
            this.radNone.Location = ((System.Drawing.Point)(resources.GetObject("radNone.Location")));
            this.radNone.Name = "radNone";
            this.radNone.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radNone.RightToLeft")));
            this.radNone.Size = ((System.Drawing.Size)(resources.GetObject("radNone.Size")));
            this.radNone.TabIndex = ((int)(resources.GetObject("radNone.TabIndex")));
            this.radNone.Text = resources.GetString("radNone.Text");
            this.radNone.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNone.TextAlign")));
            this.radNone.Visible = ((bool)(resources.GetObject("radNone.Visible")));
            // 
            // pnlEntropy
            // 
            this.pnlEntropy.AccessibleDescription = resources.GetString("pnlEntropy.AccessibleDescription");
            this.pnlEntropy.AccessibleName = resources.GetString("pnlEntropy.AccessibleName");
            this.pnlEntropy.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlEntropy.Anchor")));
            this.pnlEntropy.AutoScroll = ((bool)(resources.GetObject("pnlEntropy.AutoScroll")));
            this.pnlEntropy.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlEntropy.AutoScrollMargin")));
            this.pnlEntropy.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlEntropy.AutoScrollMinSize")));
            this.pnlEntropy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlEntropy.BackgroundImage")));
            this.pnlEntropy.Controls.Add(this.lblEntropy);
            this.pnlEntropy.Controls.Add(this.txtEntropy);
            this.pnlEntropy.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlEntropy.Dock")));
            this.pnlEntropy.Enabled = ((bool)(resources.GetObject("pnlEntropy.Enabled")));
            this.pnlEntropy.Font = ((System.Drawing.Font)(resources.GetObject("pnlEntropy.Font")));
            this.pnlEntropy.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlEntropy.ImeMode")));
            this.pnlEntropy.Location = ((System.Drawing.Point)(resources.GetObject("pnlEntropy.Location")));
            this.pnlEntropy.Name = "pnlEntropy";
            this.pnlEntropy.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlEntropy.RightToLeft")));
            this.pnlEntropy.Size = ((System.Drawing.Size)(resources.GetObject("pnlEntropy.Size")));
            this.pnlEntropy.TabIndex = ((int)(resources.GetObject("pnlEntropy.TabIndex")));
            this.pnlEntropy.Text = resources.GetString("pnlEntropy.Text");
            this.pnlEntropy.Visible = ((bool)(resources.GetObject("pnlEntropy.Visible")));
            // 
            // lblEntropy
            // 
            this.lblEntropy.AccessibleDescription = resources.GetString("lblEntropy.AccessibleDescription");
            this.lblEntropy.AccessibleName = resources.GetString("lblEntropy.AccessibleName");
            this.lblEntropy.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblEntropy.Anchor")));
            this.lblEntropy.AutoSize = ((bool)(resources.GetObject("lblEntropy.AutoSize")));
            this.lblEntropy.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblEntropy.Dock")));
            this.lblEntropy.Enabled = ((bool)(resources.GetObject("lblEntropy.Enabled")));
            this.lblEntropy.Font = ((System.Drawing.Font)(resources.GetObject("lblEntropy.Font")));
            this.lblEntropy.Image = ((System.Drawing.Image)(resources.GetObject("lblEntropy.Image")));
            this.lblEntropy.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEntropy.ImageAlign")));
            this.lblEntropy.ImageIndex = ((int)(resources.GetObject("lblEntropy.ImageIndex")));
            this.lblEntropy.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblEntropy.ImeMode")));
            this.lblEntropy.Location = ((System.Drawing.Point)(resources.GetObject("lblEntropy.Location")));
            this.lblEntropy.Name = "lblEntropy";
            this.lblEntropy.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblEntropy.RightToLeft")));
            this.lblEntropy.Size = ((System.Drawing.Size)(resources.GetObject("lblEntropy.Size")));
            this.lblEntropy.TabIndex = ((int)(resources.GetObject("lblEntropy.TabIndex")));
            this.lblEntropy.Text = resources.GetString("lblEntropy.Text");
            this.lblEntropy.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEntropy.TextAlign")));
            this.lblEntropy.Visible = ((bool)(resources.GetObject("lblEntropy.Visible")));
            // 
            // txtEntropy
            // 
            this.txtEntropy.AccessibleDescription = resources.GetString("txtEntropy.AccessibleDescription");
            this.txtEntropy.AccessibleName = resources.GetString("txtEntropy.AccessibleName");
            this.txtEntropy.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtEntropy.Anchor")));
            this.txtEntropy.AutoSize = ((bool)(resources.GetObject("txtEntropy.AutoSize")));
            this.txtEntropy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtEntropy.BackgroundImage")));
            this.txtEntropy.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtEntropy.Dock")));
            this.txtEntropy.Enabled = ((bool)(resources.GetObject("txtEntropy.Enabled")));
            this.txtEntropy.Font = ((System.Drawing.Font)(resources.GetObject("txtEntropy.Font")));
            this.txtEntropy.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtEntropy.ImeMode")));
            this.txtEntropy.Location = ((System.Drawing.Point)(resources.GetObject("txtEntropy.Location")));
            this.txtEntropy.MaxLength = ((int)(resources.GetObject("txtEntropy.MaxLength")));
            this.txtEntropy.Multiline = ((bool)(resources.GetObject("txtEntropy.Multiline")));
            this.txtEntropy.Name = "txtEntropy";
            this.txtEntropy.PasswordChar = ((char)(resources.GetObject("txtEntropy.PasswordChar")));
            this.txtEntropy.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtEntropy.RightToLeft")));
            this.txtEntropy.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtEntropy.ScrollBars")));
            this.txtEntropy.Size = ((System.Drawing.Size)(resources.GetObject("txtEntropy.Size")));
            this.txtEntropy.TabIndex = ((int)(resources.GetObject("txtEntropy.TabIndex")));
            this.txtEntropy.Text = resources.GetString("txtEntropy.Text");
            this.txtEntropy.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtEntropy.TextAlign")));
            this.txtEntropy.Visible = ((bool)(resources.GetObject("txtEntropy.Visible")));
            this.txtEntropy.WordWrap = ((bool)(resources.GetObject("txtEntropy.WordWrap")));
            // 
            // radMachine
            // 
            this.radMachine.AccessibleDescription = resources.GetString("radMachine.AccessibleDescription");
            this.radMachine.AccessibleName = resources.GetString("radMachine.AccessibleName");
            this.radMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radMachine.Anchor")));
            this.radMachine.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radMachine.Appearance")));
            this.radMachine.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radMachine.BackgroundImage")));
            this.radMachine.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radMachine.CheckAlign")));
            this.radMachine.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radMachine.Dock")));
            this.radMachine.Enabled = ((bool)(resources.GetObject("radMachine.Enabled")));
            this.radMachine.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radMachine.FlatStyle")));
            this.radMachine.Font = ((System.Drawing.Font)(resources.GetObject("radMachine.Font")));
            this.radMachine.Image = ((System.Drawing.Image)(resources.GetObject("radMachine.Image")));
            this.radMachine.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radMachine.ImageAlign")));
            this.radMachine.ImageIndex = ((int)(resources.GetObject("radMachine.ImageIndex")));
            this.radMachine.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radMachine.ImeMode")));
            this.radMachine.Location = ((System.Drawing.Point)(resources.GetObject("radMachine.Location")));
            this.radMachine.Name = "radMachine";
            this.radMachine.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radMachine.RightToLeft")));
            this.radMachine.Size = ((System.Drawing.Size)(resources.GetObject("radMachine.Size")));
            this.radMachine.TabIndex = ((int)(resources.GetObject("radMachine.TabIndex")));
            this.radMachine.Text = resources.GetString("radMachine.Text");
            this.radMachine.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radMachine.TextAlign")));
            this.radMachine.Visible = ((bool)(resources.GetObject("radMachine.Visible")));
            this.radMachine.CheckedChanged += new System.EventHandler(this.radMachine_CheckedChanged);
            // 
            // radUser
            // 
            this.radUser.AccessibleDescription = resources.GetString("radUser.AccessibleDescription");
            this.radUser.AccessibleName = resources.GetString("radUser.AccessibleName");
            this.radUser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radUser.Anchor")));
            this.radUser.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radUser.Appearance")));
            this.radUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radUser.BackgroundImage")));
            this.radUser.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radUser.CheckAlign")));
            this.radUser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radUser.Dock")));
            this.radUser.Enabled = ((bool)(resources.GetObject("radUser.Enabled")));
            this.radUser.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radUser.FlatStyle")));
            this.radUser.Font = ((System.Drawing.Font)(resources.GetObject("radUser.Font")));
            this.radUser.Image = ((System.Drawing.Image)(resources.GetObject("radUser.Image")));
            this.radUser.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radUser.ImageAlign")));
            this.radUser.ImageIndex = ((int)(resources.GetObject("radUser.ImageIndex")));
            this.radUser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radUser.ImeMode")));
            this.radUser.Location = ((System.Drawing.Point)(resources.GetObject("radUser.Location")));
            this.radUser.Name = "radUser";
            this.radUser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radUser.RightToLeft")));
            this.radUser.Size = ((System.Drawing.Size)(resources.GetObject("radUser.Size")));
            this.radUser.TabIndex = ((int)(resources.GetObject("radUser.TabIndex")));
            this.radUser.Text = resources.GetString("radUser.Text");
            this.radUser.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radUser.TextAlign")));
            this.radUser.Visible = ((bool)(resources.GetObject("radUser.Visible")));
            this.radUser.CheckedChanged += new System.EventHandler(this.radUser_CheckedChanged);
            // 
            // DpapiSettingsControl
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.radNone);
            this.Controls.Add(this.pnlEntropy);
            this.Controls.Add(this.radMachine);
            this.Controls.Add(this.radUser);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.Name = "DpapiSettingsControl";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.Size = ((System.Drawing.Size)(resources.GetObject("$this.Size")));
            this.pnlEntropy.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}