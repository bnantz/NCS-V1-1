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
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Import key dialog.
    /// </summary>
    public class ImportKey : Form
    {
        private byte[] importedKey;

        #region Form Designer
        private Button btnCancel;
        private Button btnImport;
        private GroupBox grpDestination;
        private Button btnBrowse;
        private TextBox txtDestination;
        private OpenFileDialog openDialog;
        private Container components = null;

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        public ImportKey()
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImportKey));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.grpDestination.SuspendLayout();
            this.SuspendLayout();
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
            // btnImport
            // 
            this.btnImport.AccessibleDescription = resources.GetString("btnImport.AccessibleDescription");
            this.btnImport.AccessibleName = resources.GetString("btnImport.AccessibleName");
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnImport.Anchor")));
            this.btnImport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnImport.BackgroundImage")));
            this.btnImport.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnImport.Dock")));
            this.btnImport.Enabled = ((bool)(resources.GetObject("btnImport.Enabled")));
            this.btnImport.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnImport.FlatStyle")));
            this.btnImport.Font = ((System.Drawing.Font)(resources.GetObject("btnImport.Font")));
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnImport.ImageAlign")));
            this.btnImport.ImageIndex = ((int)(resources.GetObject("btnImport.ImageIndex")));
            this.btnImport.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnImport.ImeMode")));
            this.btnImport.Location = ((System.Drawing.Point)(resources.GetObject("btnImport.Location")));
            this.btnImport.Name = "btnImport";
            this.btnImport.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnImport.RightToLeft")));
            this.btnImport.Size = ((System.Drawing.Size)(resources.GetObject("btnImport.Size")));
            this.btnImport.TabIndex = ((int)(resources.GetObject("btnImport.TabIndex")));
            this.btnImport.Text = resources.GetString("btnImport.Text");
            this.btnImport.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnImport.TextAlign")));
            this.btnImport.Visible = ((bool)(resources.GetObject("btnImport.Visible")));
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
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
            // openDialog
            // 
            this.openDialog.Filter = resources.GetString("openDialog.Filter");
            this.openDialog.Title = resources.GetString("openDialog.Title");
            // 
            // ImportKey
            // 
            this.AcceptButton = this.btnImport;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnCancel;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.grpDestination);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "ImportKey";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.ImportKey_Load);
            this.grpDestination.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        private void ImportKey_Load(object sender, EventArgs e)
        {
            txtDestination.Text = String.Empty;
        }

        /// <summary>
        /// Gets the imported key.
        /// </summary>
        public byte[] ImportedKey
        {
            get { return this.importedKey; }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDestination.Text = openDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Import()
        {
            if (!ValidateArguments())
            {
                return;
            }

            byte[] key = null;

            ImportExportUtility utility = new ImportExportUtility();
            try
            {
                bool keyFileIsPasswordProtected = utility.IsPasswordProtected(txtDestination.Text);
                if (keyFileIsPasswordProtected)
                {
                    key = ImportPasswordProtectedKey();
                }
                else
                {
                    key = ImportKeyFromFile(txtDestination.Text,  string.Empty);
                }

                if (key != null)
                {
                    this.importedKey = key;

                    MessageBox.Show(SR.ImportSuccessful, SR.ImportSuccessfulTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }        
            }
            catch (FormatException)
            {
                MessageBox.Show(SR.ImportDecryptFormatError, SR.ImportDialogErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] ImportPasswordProtectedKey()
        {
            byte[] key = null;

            ImportPassword form = new ImportPassword();
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                string destinationFileName = txtDestination.Text;
                string password = form.Password;

                key = ImportKeyFromFile(destinationFileName, password);
            }

            return key;
        }

        private byte[] ImportKeyFromFile(string destinationFileName, string password)
        {
            byte[] key = null;
            try
            {
                ImportExportUtility utility = new ImportExportUtility();
                key = utility.Import(destinationFileName, password);
            }
            catch (CryptographicException)
            {
                MessageBox.Show(SR.ImportDecryptError, SR.ImportDialogErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(FormatException)
            {
                MessageBox.Show(SR.ImportDecryptFormatError, SR.ImportDialogErrorTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return key;
        }

        private bool ValidateArguments()
        {
            bool result = true;

            if (!File.Exists(txtDestination.Text))
            {
                result = false;
                MessageBox.Show(SR.ImportSourceFileInvalid, SR.ImportDialogErrorTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}