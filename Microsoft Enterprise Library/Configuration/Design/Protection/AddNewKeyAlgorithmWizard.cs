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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <devdoc>
    /// A wizard for adding key key algorithms.
    /// </devdoc>
    internal class AddNewKeyAlgorithmWizard : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private Button nextButton;
        private Button backButton;
        private Button cancelButton;
        private Panel introPanel;
        private Panel algorithmPanel;
        private Label label4;
        private GroupBox groupBox1;
        private Label symmetricAlgorithmLabel;
        private Button selectAlgorithmButton;
        private RadioButton createNewKeyRadio;
        private RadioButton loadExistingKeyRadio;

        private Type symmetricAlgorithmType;
        private int step;
        private Panel keyPanel;
        private byte[] key;
        private Label label5;
        private KeyEditor keyEditor;
        private Label keyMessageLabel;

        private Container components = null;

        public AddNewKeyAlgorithmWizard()
        {
            InitializeComponent();
            nextButton.Enabled = false;
            backButton.Enabled = false;
            introPanel.Visible = true;
            algorithmPanel.Visible = false;
            keyPanel.Visible = false;
            keyEditor.TextChanged += new EventHandler(keyEditor_TextChanged);
        }

        public KeyAlgorithmPair KeyAlgorithmPair
        {
            get
            {
                if (key == null || symmetricAlgorithmType == null)
                {
                    return null;
                }
                else
                {
                    return new KeyAlgorithmPair(key, symmetricAlgorithmType.AssemblyQualifiedName);
                }
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

        private void nextButton_Click(object sender, EventArgs e)
        {
            switch (step)
            {
                case 0:
                    if (createNewKeyRadio.Checked)
                    {
                        introPanel.Visible = false;
                        algorithmPanel.Visible = true;
                        nextButton.Enabled = (symmetricAlgorithmType != null);
                        backButton.Enabled = true;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    break;
                case 1:
                    algorithmPanel.Visible = false;
                    keyPanel.Visible = true;
                    string message = String.Empty;
                    nextButton.Enabled = keyEditor.KeyIsValid(out message);
                    backButton.Enabled = true;
                    keyEditor.KeySettings = new KeySettings(new SymmetricAlgorithmKeyCreator(symmetricAlgorithmType.AssemblyQualifiedName));
                    break;
                case 2:
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
            ++step;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            switch (step)
            {
                case 1:
                    introPanel.Visible = true;
                    algorithmPanel.Visible = false;
                    nextButton.Enabled = true;
                    backButton.Enabled = false;
                    break;
                case 2:
                    algorithmPanel.Visible = true;
                    keyPanel.Visible = false;
                    backButton.Enabled = true;
                    nextButton.Enabled = true;
                    break;
            }
            --step;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void selectAlgorithmButton_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                TypeSelectorUI dialog = new TypeSelectorUI(
                    typeof(RijndaelManaged),
                    typeof(SymmetricAlgorithm),
                    TypeSelectorIncludeFlags.Default);

                dialog.ShowDialog();

                if (dialog.DialogResult == DialogResult.OK)
                {
                    symmetricAlgorithmType = dialog.SelectedType;
                    symmetricAlgorithmLabel.Text = symmetricAlgorithmType.Name;
                    nextButton.Enabled = true;
                }
            }
        }

        private void createNewKeyRadio_CheckedChanged(object sender, EventArgs e)
        {
            nextButton.Enabled = true;
        }

        private void loadExistingKeyRadio_CheckedChanged(object sender, EventArgs e)
        {
            nextButton.Enabled = true;
        }

        private void keyEditor_TextChanged(object sender, EventArgs e)
        {
            string message = String.Empty;
            if (keyEditor.KeyIsValid(out message))
            {
                key = keyEditor.Key;
                nextButton.Enabled = true;
            }
            else
            {
                nextButton.Enabled = false;
            }

            keyMessageLabel.Text = message;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddNewKeyAlgorithmWizard));
            this.label1 = new System.Windows.Forms.Label();
            this.createNewKeyRadio = new System.Windows.Forms.RadioButton();
            this.loadExistingKeyRadio = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.introPanel = new System.Windows.Forms.Panel();
            this.keyPanel = new System.Windows.Forms.Panel();
            this.keyMessageLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.keyEditor = new Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.KeyEditor();
            this.algorithmPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectAlgorithmButton = new System.Windows.Forms.Button();
            this.symmetricAlgorithmLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.introPanel.SuspendLayout();
            this.keyPanel.SuspendLayout();
            this.algorithmPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            // createNewKeyRadio
            // 
            this.createNewKeyRadio.AccessibleDescription = resources.GetString("createNewKeyRadio.AccessibleDescription");
            this.createNewKeyRadio.AccessibleName = resources.GetString("createNewKeyRadio.AccessibleName");
            this.createNewKeyRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("createNewKeyRadio.Anchor")));
            this.createNewKeyRadio.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("createNewKeyRadio.Appearance")));
            this.createNewKeyRadio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("createNewKeyRadio.BackgroundImage")));
            this.createNewKeyRadio.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("createNewKeyRadio.CheckAlign")));
            this.createNewKeyRadio.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("createNewKeyRadio.Dock")));
            this.createNewKeyRadio.Enabled = ((bool)(resources.GetObject("createNewKeyRadio.Enabled")));
            this.createNewKeyRadio.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("createNewKeyRadio.FlatStyle")));
            this.createNewKeyRadio.Font = ((System.Drawing.Font)(resources.GetObject("createNewKeyRadio.Font")));
            this.createNewKeyRadio.Image = ((System.Drawing.Image)(resources.GetObject("createNewKeyRadio.Image")));
            this.createNewKeyRadio.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("createNewKeyRadio.ImageAlign")));
            this.createNewKeyRadio.ImageIndex = ((int)(resources.GetObject("createNewKeyRadio.ImageIndex")));
            this.createNewKeyRadio.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("createNewKeyRadio.ImeMode")));
            this.createNewKeyRadio.Location = ((System.Drawing.Point)(resources.GetObject("createNewKeyRadio.Location")));
            this.createNewKeyRadio.Name = "createNewKeyRadio";
            this.createNewKeyRadio.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("createNewKeyRadio.RightToLeft")));
            this.createNewKeyRadio.Size = ((System.Drawing.Size)(resources.GetObject("createNewKeyRadio.Size")));
            this.createNewKeyRadio.TabIndex = ((int)(resources.GetObject("createNewKeyRadio.TabIndex")));
            this.createNewKeyRadio.Text = resources.GetString("createNewKeyRadio.Text");
            this.createNewKeyRadio.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("createNewKeyRadio.TextAlign")));
            this.createNewKeyRadio.Visible = ((bool)(resources.GetObject("createNewKeyRadio.Visible")));
            this.createNewKeyRadio.CheckedChanged += new System.EventHandler(this.createNewKeyRadio_CheckedChanged);
            // 
            // loadExistingKeyRadio
            // 
            this.loadExistingKeyRadio.AccessibleDescription = resources.GetString("loadExistingKeyRadio.AccessibleDescription");
            this.loadExistingKeyRadio.AccessibleName = resources.GetString("loadExistingKeyRadio.AccessibleName");
            this.loadExistingKeyRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("loadExistingKeyRadio.Anchor")));
            this.loadExistingKeyRadio.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("loadExistingKeyRadio.Appearance")));
            this.loadExistingKeyRadio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("loadExistingKeyRadio.BackgroundImage")));
            this.loadExistingKeyRadio.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("loadExistingKeyRadio.CheckAlign")));
            this.loadExistingKeyRadio.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("loadExistingKeyRadio.Dock")));
            this.loadExistingKeyRadio.Enabled = ((bool)(resources.GetObject("loadExistingKeyRadio.Enabled")));
            this.loadExistingKeyRadio.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("loadExistingKeyRadio.FlatStyle")));
            this.loadExistingKeyRadio.Font = ((System.Drawing.Font)(resources.GetObject("loadExistingKeyRadio.Font")));
            this.loadExistingKeyRadio.Image = ((System.Drawing.Image)(resources.GetObject("loadExistingKeyRadio.Image")));
            this.loadExistingKeyRadio.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("loadExistingKeyRadio.ImageAlign")));
            this.loadExistingKeyRadio.ImageIndex = ((int)(resources.GetObject("loadExistingKeyRadio.ImageIndex")));
            this.loadExistingKeyRadio.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("loadExistingKeyRadio.ImeMode")));
            this.loadExistingKeyRadio.Location = ((System.Drawing.Point)(resources.GetObject("loadExistingKeyRadio.Location")));
            this.loadExistingKeyRadio.Name = "loadExistingKeyRadio";
            this.loadExistingKeyRadio.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("loadExistingKeyRadio.RightToLeft")));
            this.loadExistingKeyRadio.Size = ((System.Drawing.Size)(resources.GetObject("loadExistingKeyRadio.Size")));
            this.loadExistingKeyRadio.TabIndex = ((int)(resources.GetObject("loadExistingKeyRadio.TabIndex")));
            this.loadExistingKeyRadio.Text = resources.GetString("loadExistingKeyRadio.Text");
            this.loadExistingKeyRadio.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("loadExistingKeyRadio.TextAlign")));
            this.loadExistingKeyRadio.Visible = ((bool)(resources.GetObject("loadExistingKeyRadio.Visible")));
            this.loadExistingKeyRadio.CheckedChanged += new System.EventHandler(this.loadExistingKeyRadio_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
            this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
            this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
            this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
            this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
            // nextButton
            // 
            this.nextButton.AccessibleDescription = resources.GetString("nextButton.AccessibleDescription");
            this.nextButton.AccessibleName = resources.GetString("nextButton.AccessibleName");
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("nextButton.Anchor")));
            this.nextButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("nextButton.BackgroundImage")));
            this.nextButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("nextButton.Dock")));
            this.nextButton.Enabled = ((bool)(resources.GetObject("nextButton.Enabled")));
            this.nextButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("nextButton.FlatStyle")));
            this.nextButton.Font = ((System.Drawing.Font)(resources.GetObject("nextButton.Font")));
            this.nextButton.Image = ((System.Drawing.Image)(resources.GetObject("nextButton.Image")));
            this.nextButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("nextButton.ImageAlign")));
            this.nextButton.ImageIndex = ((int)(resources.GetObject("nextButton.ImageIndex")));
            this.nextButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("nextButton.ImeMode")));
            this.nextButton.Location = ((System.Drawing.Point)(resources.GetObject("nextButton.Location")));
            this.nextButton.Name = "nextButton";
            this.nextButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("nextButton.RightToLeft")));
            this.nextButton.Size = ((System.Drawing.Size)(resources.GetObject("nextButton.Size")));
            this.nextButton.TabIndex = ((int)(resources.GetObject("nextButton.TabIndex")));
            this.nextButton.Text = resources.GetString("nextButton.Text");
            this.nextButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("nextButton.TextAlign")));
            this.nextButton.Visible = ((bool)(resources.GetObject("nextButton.Visible")));
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // backButton
            // 
            this.backButton.AccessibleDescription = resources.GetString("backButton.AccessibleDescription");
            this.backButton.AccessibleName = resources.GetString("backButton.AccessibleName");
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("backButton.Anchor")));
            this.backButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("backButton.BackgroundImage")));
            this.backButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("backButton.Dock")));
            this.backButton.Enabled = ((bool)(resources.GetObject("backButton.Enabled")));
            this.backButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("backButton.FlatStyle")));
            this.backButton.Font = ((System.Drawing.Font)(resources.GetObject("backButton.Font")));
            this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
            this.backButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("backButton.ImageAlign")));
            this.backButton.ImageIndex = ((int)(resources.GetObject("backButton.ImageIndex")));
            this.backButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("backButton.ImeMode")));
            this.backButton.Location = ((System.Drawing.Point)(resources.GetObject("backButton.Location")));
            this.backButton.Name = "backButton";
            this.backButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("backButton.RightToLeft")));
            this.backButton.Size = ((System.Drawing.Size)(resources.GetObject("backButton.Size")));
            this.backButton.TabIndex = ((int)(resources.GetObject("backButton.TabIndex")));
            this.backButton.Text = resources.GetString("backButton.Text");
            this.backButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("backButton.TextAlign")));
            this.backButton.Visible = ((bool)(resources.GetObject("backButton.Visible")));
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
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
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // introPanel
            // 
            this.introPanel.AccessibleDescription = resources.GetString("introPanel.AccessibleDescription");
            this.introPanel.AccessibleName = resources.GetString("introPanel.AccessibleName");
            this.introPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("introPanel.Anchor")));
            this.introPanel.AutoScroll = ((bool)(resources.GetObject("introPanel.AutoScroll")));
            this.introPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("introPanel.AutoScrollMargin")));
            this.introPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("introPanel.AutoScrollMinSize")));
            this.introPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("introPanel.BackgroundImage")));
            this.introPanel.Controls.Add(this.label2);
            this.introPanel.Controls.Add(this.label3);
            this.introPanel.Controls.Add(this.loadExistingKeyRadio);
            this.introPanel.Controls.Add(this.createNewKeyRadio);
            this.introPanel.Controls.Add(this.label1);
            this.introPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("introPanel.Dock")));
            this.introPanel.Enabled = ((bool)(resources.GetObject("introPanel.Enabled")));
            this.introPanel.Font = ((System.Drawing.Font)(resources.GetObject("introPanel.Font")));
            this.introPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("introPanel.ImeMode")));
            this.introPanel.Location = ((System.Drawing.Point)(resources.GetObject("introPanel.Location")));
            this.introPanel.Name = "introPanel";
            this.introPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("introPanel.RightToLeft")));
            this.introPanel.Size = ((System.Drawing.Size)(resources.GetObject("introPanel.Size")));
            this.introPanel.TabIndex = ((int)(resources.GetObject("introPanel.TabIndex")));
            this.introPanel.Text = resources.GetString("introPanel.Text");
            this.introPanel.Visible = ((bool)(resources.GetObject("introPanel.Visible")));
            // 
            // keyPanel
            // 
            this.keyPanel.AccessibleDescription = resources.GetString("keyPanel.AccessibleDescription");
            this.keyPanel.AccessibleName = resources.GetString("keyPanel.AccessibleName");
            this.keyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyPanel.Anchor")));
            this.keyPanel.AutoScroll = ((bool)(resources.GetObject("keyPanel.AutoScroll")));
            this.keyPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("keyPanel.AutoScrollMargin")));
            this.keyPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("keyPanel.AutoScrollMinSize")));
            this.keyPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyPanel.BackgroundImage")));
            this.keyPanel.Controls.Add(this.keyMessageLabel);
            this.keyPanel.Controls.Add(this.label5);
            this.keyPanel.Controls.Add(this.keyEditor);
            this.keyPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyPanel.Dock")));
            this.keyPanel.Enabled = ((bool)(resources.GetObject("keyPanel.Enabled")));
            this.keyPanel.Font = ((System.Drawing.Font)(resources.GetObject("keyPanel.Font")));
            this.keyPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyPanel.ImeMode")));
            this.keyPanel.Location = ((System.Drawing.Point)(resources.GetObject("keyPanel.Location")));
            this.keyPanel.Name = "keyPanel";
            this.keyPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyPanel.RightToLeft")));
            this.keyPanel.Size = ((System.Drawing.Size)(resources.GetObject("keyPanel.Size")));
            this.keyPanel.TabIndex = ((int)(resources.GetObject("keyPanel.TabIndex")));
            this.keyPanel.Text = resources.GetString("keyPanel.Text");
            this.keyPanel.Visible = ((bool)(resources.GetObject("keyPanel.Visible")));
            // 
            // keyMessageLabel
            // 
            this.keyMessageLabel.AccessibleDescription = resources.GetString("keyMessageLabel.AccessibleDescription");
            this.keyMessageLabel.AccessibleName = resources.GetString("keyMessageLabel.AccessibleName");
            this.keyMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyMessageLabel.Anchor")));
            this.keyMessageLabel.AutoSize = ((bool)(resources.GetObject("keyMessageLabel.AutoSize")));
            this.keyMessageLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyMessageLabel.Dock")));
            this.keyMessageLabel.Enabled = ((bool)(resources.GetObject("keyMessageLabel.Enabled")));
            this.keyMessageLabel.Font = ((System.Drawing.Font)(resources.GetObject("keyMessageLabel.Font")));
            this.keyMessageLabel.Image = ((System.Drawing.Image)(resources.GetObject("keyMessageLabel.Image")));
            this.keyMessageLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyMessageLabel.ImageAlign")));
            this.keyMessageLabel.ImageIndex = ((int)(resources.GetObject("keyMessageLabel.ImageIndex")));
            this.keyMessageLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyMessageLabel.ImeMode")));
            this.keyMessageLabel.Location = ((System.Drawing.Point)(resources.GetObject("keyMessageLabel.Location")));
            this.keyMessageLabel.Name = "keyMessageLabel";
            this.keyMessageLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyMessageLabel.RightToLeft")));
            this.keyMessageLabel.Size = ((System.Drawing.Size)(resources.GetObject("keyMessageLabel.Size")));
            this.keyMessageLabel.TabIndex = ((int)(resources.GetObject("keyMessageLabel.TabIndex")));
            this.keyMessageLabel.Text = resources.GetString("keyMessageLabel.Text");
            this.keyMessageLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("keyMessageLabel.TextAlign")));
            this.keyMessageLabel.Visible = ((bool)(resources.GetObject("keyMessageLabel.Visible")));
            // 
            // label5
            // 
            this.label5.AccessibleDescription = resources.GetString("label5.AccessibleDescription");
            this.label5.AccessibleName = resources.GetString("label5.AccessibleName");
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label5.Anchor")));
            this.label5.AutoSize = ((bool)(resources.GetObject("label5.AutoSize")));
            this.label5.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label5.Dock")));
            this.label5.Enabled = ((bool)(resources.GetObject("label5.Enabled")));
            this.label5.Font = ((System.Drawing.Font)(resources.GetObject("label5.Font")));
            this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
            this.label5.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.ImageAlign")));
            this.label5.ImageIndex = ((int)(resources.GetObject("label5.ImageIndex")));
            this.label5.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label5.ImeMode")));
            this.label5.Location = ((System.Drawing.Point)(resources.GetObject("label5.Location")));
            this.label5.Name = "label5";
            this.label5.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label5.RightToLeft")));
            this.label5.Size = ((System.Drawing.Size)(resources.GetObject("label5.Size")));
            this.label5.TabIndex = ((int)(resources.GetObject("label5.TabIndex")));
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label5.TextAlign")));
            this.label5.Visible = ((bool)(resources.GetObject("label5.Visible")));
            // 
            // keyEditor
            // 
            this.keyEditor.AccessibleDescription = resources.GetString("keyEditor.AccessibleDescription");
            this.keyEditor.AccessibleName = resources.GetString("keyEditor.AccessibleName");
            this.keyEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("keyEditor.Anchor")));
            this.keyEditor.AutoScroll = ((bool)(resources.GetObject("keyEditor.AutoScroll")));
            this.keyEditor.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("keyEditor.AutoScrollMargin")));
            this.keyEditor.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("keyEditor.AutoScrollMinSize")));
            this.keyEditor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("keyEditor.BackgroundImage")));
            this.keyEditor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("keyEditor.Dock")));
            this.keyEditor.Enabled = ((bool)(resources.GetObject("keyEditor.Enabled")));
            this.keyEditor.Font = ((System.Drawing.Font)(resources.GetObject("keyEditor.Font")));
            this.keyEditor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("keyEditor.ImeMode")));
            this.keyEditor.Key = new System.Byte[0];
            this.keyEditor.Location = ((System.Drawing.Point)(resources.GetObject("keyEditor.Location")));
            this.keyEditor.Name = "keyEditor";
            this.keyEditor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("keyEditor.RightToLeft")));
            this.keyEditor.Size = ((System.Drawing.Size)(resources.GetObject("keyEditor.Size")));
            this.keyEditor.TabIndex = ((int)(resources.GetObject("keyEditor.TabIndex")));
            this.keyEditor.Visible = ((bool)(resources.GetObject("keyEditor.Visible")));
            // 
            // algorithmPanel
            // 
            this.algorithmPanel.AccessibleDescription = resources.GetString("algorithmPanel.AccessibleDescription");
            this.algorithmPanel.AccessibleName = resources.GetString("algorithmPanel.AccessibleName");
            this.algorithmPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("algorithmPanel.Anchor")));
            this.algorithmPanel.AutoScroll = ((bool)(resources.GetObject("algorithmPanel.AutoScroll")));
            this.algorithmPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("algorithmPanel.AutoScrollMargin")));
            this.algorithmPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("algorithmPanel.AutoScrollMinSize")));
            this.algorithmPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("algorithmPanel.BackgroundImage")));
            this.algorithmPanel.Controls.Add(this.groupBox1);
            this.algorithmPanel.Controls.Add(this.label4);
            this.algorithmPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("algorithmPanel.Dock")));
            this.algorithmPanel.Enabled = ((bool)(resources.GetObject("algorithmPanel.Enabled")));
            this.algorithmPanel.Font = ((System.Drawing.Font)(resources.GetObject("algorithmPanel.Font")));
            this.algorithmPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("algorithmPanel.ImeMode")));
            this.algorithmPanel.Location = ((System.Drawing.Point)(resources.GetObject("algorithmPanel.Location")));
            this.algorithmPanel.Name = "algorithmPanel";
            this.algorithmPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("algorithmPanel.RightToLeft")));
            this.algorithmPanel.Size = ((System.Drawing.Size)(resources.GetObject("algorithmPanel.Size")));
            this.algorithmPanel.TabIndex = ((int)(resources.GetObject("algorithmPanel.TabIndex")));
            this.algorithmPanel.Text = resources.GetString("algorithmPanel.Text");
            this.algorithmPanel.Visible = ((bool)(resources.GetObject("algorithmPanel.Visible")));
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
            this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.selectAlgorithmButton);
            this.groupBox1.Controls.Add(this.symmetricAlgorithmLabel);
            this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
            this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
            this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
            this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
            this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
            this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = resources.GetString("groupBox1.Text");
            this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
            // 
            // selectAlgorithmButton
            // 
            this.selectAlgorithmButton.AccessibleDescription = resources.GetString("selectAlgorithmButton.AccessibleDescription");
            this.selectAlgorithmButton.AccessibleName = resources.GetString("selectAlgorithmButton.AccessibleName");
            this.selectAlgorithmButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("selectAlgorithmButton.Anchor")));
            this.selectAlgorithmButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectAlgorithmButton.BackgroundImage")));
            this.selectAlgorithmButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("selectAlgorithmButton.Dock")));
            this.selectAlgorithmButton.Enabled = ((bool)(resources.GetObject("selectAlgorithmButton.Enabled")));
            this.selectAlgorithmButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("selectAlgorithmButton.FlatStyle")));
            this.selectAlgorithmButton.Font = ((System.Drawing.Font)(resources.GetObject("selectAlgorithmButton.Font")));
            this.selectAlgorithmButton.Image = ((System.Drawing.Image)(resources.GetObject("selectAlgorithmButton.Image")));
            this.selectAlgorithmButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectAlgorithmButton.ImageAlign")));
            this.selectAlgorithmButton.ImageIndex = ((int)(resources.GetObject("selectAlgorithmButton.ImageIndex")));
            this.selectAlgorithmButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("selectAlgorithmButton.ImeMode")));
            this.selectAlgorithmButton.Location = ((System.Drawing.Point)(resources.GetObject("selectAlgorithmButton.Location")));
            this.selectAlgorithmButton.Name = "selectAlgorithmButton";
            this.selectAlgorithmButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("selectAlgorithmButton.RightToLeft")));
            this.selectAlgorithmButton.Size = ((System.Drawing.Size)(resources.GetObject("selectAlgorithmButton.Size")));
            this.selectAlgorithmButton.TabIndex = ((int)(resources.GetObject("selectAlgorithmButton.TabIndex")));
            this.selectAlgorithmButton.Text = resources.GetString("selectAlgorithmButton.Text");
            this.selectAlgorithmButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("selectAlgorithmButton.TextAlign")));
            this.selectAlgorithmButton.Visible = ((bool)(resources.GetObject("selectAlgorithmButton.Visible")));
            this.selectAlgorithmButton.Click += new System.EventHandler(this.selectAlgorithmButton_Click);
            // 
            // symmetricAlgorithmLabel
            // 
            this.symmetricAlgorithmLabel.AccessibleDescription = resources.GetString("symmetricAlgorithmLabel.AccessibleDescription");
            this.symmetricAlgorithmLabel.AccessibleName = resources.GetString("symmetricAlgorithmLabel.AccessibleName");
            this.symmetricAlgorithmLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("symmetricAlgorithmLabel.Anchor")));
            this.symmetricAlgorithmLabel.AutoSize = ((bool)(resources.GetObject("symmetricAlgorithmLabel.AutoSize")));
            this.symmetricAlgorithmLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("symmetricAlgorithmLabel.Dock")));
            this.symmetricAlgorithmLabel.Enabled = ((bool)(resources.GetObject("symmetricAlgorithmLabel.Enabled")));
            this.symmetricAlgorithmLabel.Font = ((System.Drawing.Font)(resources.GetObject("symmetricAlgorithmLabel.Font")));
            this.symmetricAlgorithmLabel.Image = ((System.Drawing.Image)(resources.GetObject("symmetricAlgorithmLabel.Image")));
            this.symmetricAlgorithmLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("symmetricAlgorithmLabel.ImageAlign")));
            this.symmetricAlgorithmLabel.ImageIndex = ((int)(resources.GetObject("symmetricAlgorithmLabel.ImageIndex")));
            this.symmetricAlgorithmLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("symmetricAlgorithmLabel.ImeMode")));
            this.symmetricAlgorithmLabel.Location = ((System.Drawing.Point)(resources.GetObject("symmetricAlgorithmLabel.Location")));
            this.symmetricAlgorithmLabel.Name = "symmetricAlgorithmLabel";
            this.symmetricAlgorithmLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("symmetricAlgorithmLabel.RightToLeft")));
            this.symmetricAlgorithmLabel.Size = ((System.Drawing.Size)(resources.GetObject("symmetricAlgorithmLabel.Size")));
            this.symmetricAlgorithmLabel.TabIndex = ((int)(resources.GetObject("symmetricAlgorithmLabel.TabIndex")));
            this.symmetricAlgorithmLabel.Text = resources.GetString("symmetricAlgorithmLabel.Text");
            this.symmetricAlgorithmLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("symmetricAlgorithmLabel.TextAlign")));
            this.symmetricAlgorithmLabel.Visible = ((bool)(resources.GetObject("symmetricAlgorithmLabel.Visible")));
            // 
            // label4
            // 
            this.label4.AccessibleDescription = resources.GetString("label4.AccessibleDescription");
            this.label4.AccessibleName = resources.GetString("label4.AccessibleName");
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label4.Anchor")));
            this.label4.AutoSize = ((bool)(resources.GetObject("label4.AutoSize")));
            this.label4.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label4.Dock")));
            this.label4.Enabled = ((bool)(resources.GetObject("label4.Enabled")));
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Font = ((System.Drawing.Font)(resources.GetObject("label4.Font")));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.ImageAlign")));
            this.label4.ImageIndex = ((int)(resources.GetObject("label4.ImageIndex")));
            this.label4.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label4.ImeMode")));
            this.label4.Location = ((System.Drawing.Point)(resources.GetObject("label4.Location")));
            this.label4.Name = "label4";
            this.label4.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label4.RightToLeft")));
            this.label4.Size = ((System.Drawing.Size)(resources.GetObject("label4.Size")));
            this.label4.TabIndex = ((int)(resources.GetObject("label4.TabIndex")));
            this.label4.Text = resources.GetString("label4.Text");
            this.label4.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label4.TextAlign")));
            this.label4.Visible = ((bool)(resources.GetObject("label4.Visible")));
            // 
            // AddNewKeyAlgorithmWizard
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.algorithmPanel);
            this.Controls.Add(this.keyPanel);
            this.Controls.Add(this.introPanel);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "AddNewKeyAlgorithmWizard";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.introPanel.ResumeLayout(false);
            this.keyPanel.ResumeLayout(false);
            this.algorithmPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}