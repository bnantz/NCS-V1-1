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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Provides a user interface that can edit keys for keyed algorithms at design time.</para>
    /// </summary>
    public class KeyEditor : UserControl
    {
        private GroupBox keyGroupBox;
        private TextBox keyBox;
        private Button generateKeyButton;
        private Regex hexRegex = new Regex("^[0-9a-fA-F]+$", RegexOptions.Compiled | RegexOptions.Singleline);
        private string lastValidationMessage = String.Empty;
        private KeySettings keySettings;
        private Container components = null;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="KeyEditor"/> class.</para>
        /// </summary>
        public KeyEditor() : this(new KeySettings(new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName)))
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="KeyEditor"/> class with a <see cref="KeySettings"/> object.</para>
        /// </summary>
        /// <param name="keySettings">
        /// <para>The key information to edit.</para>
        /// </param>
        public KeyEditor(KeySettings keySettings)
        {
            InitializeComponent();
            this.keySettings = keySettings;
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="KeyEditor"/> and optionally releases the managed resources.</para>
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

        /// <summary>
        /// <para>Gets or sets the <see cref="KeySettings"/> that are being edited.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="KeySettings"/> that are being edited.</para>
        /// </value>
        public KeySettings KeySettings
        {
            get { return keySettings; }
            set { keySettings = value; }
        }

        /// <summary>
        /// <para>Gets or sets the key for the algorithm.</para>
        /// </summary>
        /// <value>
        /// <para>The key for the algorithm</para>
        /// </value>
        public byte[] Key
        {
            get { return GetKeyBox(); }
            set { SetKeyBox(value); }
        }

        /// <summary>
        /// <para>Gets the length of the current key.</para>
        /// </summary>
        /// <value>
        /// <para>The length of the current key.</para>
        /// </value>
        public int KeyLength
        {
            get
            {
                int length = 0;
                if (keyBox.Text.Length != 0)
                {
                    length = GetKeyBox().Length * 8;
                }
                return length;
            }
        }

        /// <summary>
        /// <para>Determines if the current key is valid.</para>
        /// </summary>
        /// <param name="message"><para>The error message to display in the user interface if invalid.</para></param>
        /// <returns>
        /// <para><see langword="true"/> if the key is valid; otherwise <see langword="false"/></para>.
        /// </returns>
        public bool KeyIsValid(out string message)
        {
            bool ret = ValidateKeyBox();
            message = this.lastValidationMessage;
            return ret;
        }

        /// <summary>
        /// <para>Raises the <see cref="Control.EnabledChanged"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>An <see cref="EventArgs"/> that contains the event data</para>
        /// </param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            keyBox.Enabled = Enabled;
            generateKeyButton.Enabled = Enabled;

        }

        private byte[] GetKeyBox()
        {
            if (keyBox.Text.Length > 0)
            {
                byte [] hexBytes = CryptographyUtility.GetBytesFromHexString(keyBox.Text);
                return hexBytes;
            }
            else
            {
                return new byte[0];
            }
        }

        private void SetKeyBox(byte[] key)
        {
            if (key.Length > 0)
            {
                keyBox.Text = CryptographyUtility.GetHexStringFromBytes(key);
            }
        }

        private void generateKeyButton_Click(object sender, EventArgs e)
        {
            SetKeyBox(this.keySettings.KeyCreator.GenerateKey());
        }

        private void keyBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        private bool ValidateKeyBox()
        {
            bool valid = false;

            lastValidationMessage = String.Empty;

            if (keyBox.Text.Length % 2 != 0 || !this.hexRegex.IsMatch(keyBox.Text))
            {
                lastValidationMessage = SR.KeyManagerUIInvalidKeyCharactersErrorMessage;
            }
            else if (this.keySettings.KeyCreator.KeyIsValid(GetKeyBox()))
            {
                valid = true;
            }
            else
            {
                lastValidationMessage = SR.KeyManagerUIInvalidKeyLengthErrorMessage;
            }

            return valid;
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(KeyEditor));
            this.keyGroupBox = new System.Windows.Forms.GroupBox();
            this.keyBox = new System.Windows.Forms.TextBox();
            this.generateKeyButton = new System.Windows.Forms.Button();
            this.keyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // keyGroupBox
            // 
            this.keyGroupBox.AccessibleDescription = resources.GetString("keyGroupBox.AccessibleDescription");
            this.keyGroupBox.AccessibleName = resources.GetString("keyGroupBox.AccessibleName");
            this.keyGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyGroupBox.Anchor")));
            this.keyGroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyGroupBox.BackgroundImage")));
            this.keyGroupBox.Controls.Add(this.keyBox);
            this.keyGroupBox.Controls.Add(this.generateKeyButton);
            this.keyGroupBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyGroupBox.Dock")));
            this.keyGroupBox.Enabled = ((bool)(resources.GetObject("keyGroupBox.Enabled")));
            this.keyGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.keyGroupBox.Font = ((System.Drawing.Font)(resources.GetObject("keyGroupBox.Font")));
            this.keyGroupBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyGroupBox.ImeMode")));
            this.keyGroupBox.Location = ((System.Drawing.Point)(resources.GetObject("keyGroupBox.Location")));
            this.keyGroupBox.Name = "keyGroupBox";
            this.keyGroupBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyGroupBox.RightToLeft")));
            this.keyGroupBox.Size = ((System.Drawing.Size)(resources.GetObject("keyGroupBox.Size")));
            this.keyGroupBox.TabIndex = ((int)(resources.GetObject("keyGroupBox.TabIndex")));
            this.keyGroupBox.TabStop = false;
            this.keyGroupBox.Text = resources.GetString("keyGroupBox.Text");
            this.keyGroupBox.Visible = ((bool)(resources.GetObject("keyGroupBox.Visible")));
            // 
            // keyBox
            // 
            this.keyBox.AccessibleDescription = resources.GetString("keyBox.AccessibleDescription");
            this.keyBox.AccessibleName = resources.GetString("keyBox.AccessibleName");
            this.keyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyBox.Anchor")));
            this.keyBox.AutoSize = ((bool)(resources.GetObject("keyBox.AutoSize")));
            this.keyBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyBox.BackgroundImage")));
            this.keyBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.keyBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyBox.Dock")));
            this.keyBox.Enabled = ((bool)(resources.GetObject("keyBox.Enabled")));
            this.keyBox.Font = ((System.Drawing.Font)(resources.GetObject("keyBox.Font")));
            this.keyBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyBox.ImeMode")));
            this.keyBox.Location = ((System.Drawing.Point)(resources.GetObject("keyBox.Location")));
            this.keyBox.MaxLength = ((int)(resources.GetObject("keyBox.MaxLength")));
            this.keyBox.Multiline = ((bool)(resources.GetObject("keyBox.Multiline")));
            this.keyBox.Name = "keyBox";
            this.keyBox.PasswordChar = ((char)(resources.GetObject("keyBox.PasswordChar")));
            this.keyBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyBox.RightToLeft")));
            this.keyBox.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("keyBox.ScrollBars")));
            this.keyBox.Size = ((System.Drawing.Size)(resources.GetObject("keyBox.Size")));
            this.keyBox.TabIndex = ((int)(resources.GetObject("keyBox.TabIndex")));
            this.keyBox.Text = resources.GetString("keyBox.Text");
            this.keyBox.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("keyBox.TextAlign")));
            this.keyBox.Visible = ((bool)(resources.GetObject("keyBox.Visible")));
            this.keyBox.WordWrap = ((bool)(resources.GetObject("keyBox.WordWrap")));
            this.keyBox.TextChanged += new System.EventHandler(this.keyBox_TextChanged);
            // 
            // generateKeyButton
            // 
            this.generateKeyButton.AccessibleDescription = resources.GetString("generateKeyButton.AccessibleDescription");
            this.generateKeyButton.AccessibleName = resources.GetString("generateKeyButton.AccessibleName");
            this.generateKeyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("generateKeyButton.Anchor")));
            this.generateKeyButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("generateKeyButton.BackgroundImage")));
            this.generateKeyButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("generateKeyButton.Dock")));
            this.generateKeyButton.Enabled = ((bool)(resources.GetObject("generateKeyButton.Enabled")));
            this.generateKeyButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("generateKeyButton.FlatStyle")));
            this.generateKeyButton.Font = ((System.Drawing.Font)(resources.GetObject("generateKeyButton.Font")));
            this.generateKeyButton.Image = ((System.Drawing.Image)(resources.GetObject("generateKeyButton.Image")));
            this.generateKeyButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("generateKeyButton.ImageAlign")));
            this.generateKeyButton.ImageIndex = ((int)(resources.GetObject("generateKeyButton.ImageIndex")));
            this.generateKeyButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("generateKeyButton.ImeMode")));
            this.generateKeyButton.Location = ((System.Drawing.Point)(resources.GetObject("generateKeyButton.Location")));
            this.generateKeyButton.Name = "generateKeyButton";
            this.generateKeyButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("generateKeyButton.RightToLeft")));
            this.generateKeyButton.Size = ((System.Drawing.Size)(resources.GetObject("generateKeyButton.Size")));
            this.generateKeyButton.TabIndex = ((int)(resources.GetObject("generateKeyButton.TabIndex")));
            this.generateKeyButton.Text = resources.GetString("generateKeyButton.Text");
            this.generateKeyButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("generateKeyButton.TextAlign")));
            this.generateKeyButton.Visible = ((bool)(resources.GetObject("generateKeyButton.Visible")));
            this.generateKeyButton.Click += new System.EventHandler(this.generateKeyButton_Click);
            // 
            // KeyEditor
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.keyGroupBox);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.Name = "KeyEditor";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.Size = ((System.Drawing.Size)(resources.GetObject("$this.Size")));
            this.keyGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}