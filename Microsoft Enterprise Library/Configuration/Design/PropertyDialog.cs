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
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a base <see cref="Form"/> that has
    /// an OK and Cancel button anchored to the bottom
    /// right hand corner.
    /// </summary>
    public class PropertyDialog : Form
    {
        private Button defaultButton;
        private Button cancelButton;
        private Container components = null;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="PropertyDialog"/> class.
        /// </summary>
        public PropertyDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the OK button.
        /// </summary>
        protected Button OK
        {
            get { return defaultButton; }
        }

        /// <summary>
        /// Gets the Cancel button.
        /// </summary>
        protected Button Cancel
        {
            get { return cancelButton; }
        }

        /// <summary>
        /// System.Windows.Forms.Form.Disposing
        /// </summary>
        /// <param name="disposing"><strong>true</strong> to release 
        /// both managed and unmanaged resources; <strong>false</strong> 
        /// to release only unmanaged resources.</param>
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
        /// An event handler for the System.Windows.Forms.Button.Click event
        /// of the OK button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/>
        /// containing the event data.</param>
        protected virtual void OnOKButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// An event handler for the System.Windows.Forms.Button.Click event
        /// of the Cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/>
        /// containing the event data.</param>
        protected virtual void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager resources = new ResourceManager(typeof(PropertyDialog));
            this.defaultButton = new Button();
            this.cancelButton = new Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.defaultButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
            this.defaultButton.AccessibleName = resources.GetString("okButton.AccessibleName");
            this.defaultButton.Anchor = ((AnchorStyles)(resources.GetObject("okButton.Anchor")));
            this.defaultButton.BackgroundImage = ((Image)(resources.GetObject("okButton.BackgroundImage")));
            this.defaultButton.Dock = ((DockStyle)(resources.GetObject("okButton.Dock")));
            this.defaultButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
            this.defaultButton.FlatStyle = ((FlatStyle)(resources.GetObject("okButton.FlatStyle")));
            this.defaultButton.Font = ((Font)(resources.GetObject("okButton.Font")));
            this.defaultButton.Image = ((Image)(resources.GetObject("okButton.Image")));
            this.defaultButton.ImageAlign = ((ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
            this.defaultButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
            this.defaultButton.ImeMode = ((ImeMode)(resources.GetObject("okButton.ImeMode")));
            this.defaultButton.Location = ((Point)(resources.GetObject("okButton.Location")));
            this.defaultButton.Name = "okButton";
            this.defaultButton.RightToLeft = ((RightToLeft)(resources.GetObject("okButton.RightToLeft")));
            this.defaultButton.Size = ((Size)(resources.GetObject("okButton.Size")));
            this.defaultButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.defaultButton.Text = resources.GetString("okButton.Text");
            this.defaultButton.TextAlign = ((ContentAlignment)(resources.GetObject("okButton.TextAlign")));
            this.defaultButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
            this.defaultButton.Click += new EventHandler(this.OnOKButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Dock = ((DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            // 
            // PropertyDialog
            // 
            this.AcceptButton = this.defaultButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.defaultButton);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((Font)(resources.GetObject("$this.Font")));
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimumSize = ((Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "PropertyDialog";
            this.RightToLeft = ((RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
        #endregion
    }
}