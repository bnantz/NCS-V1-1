//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters
{
    /// <summary>
    /// UI for Category Filter Editor.
    /// </summary>
    public class CategoryFilterEditorUI : Form
    {
        private Button OKButton;

        private ArrayList availableCategoryNames;
        private ComboBox availableCategoryNamesBox;
        private ListBox selectedCategories;
        private Button removeCategoryButton;
        private Button addCategoryButton;
        private RadioButton allowCategoriesRadio;
        private RadioButton denyCategoriesRadio;
        private Label categoryFiltersDescriptionLabel;
        private GroupBox categoriesGroupBox;
        private Button cancelButton;
        private GroupBox filterModeGroupBox;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Initializes the component.
        /// </summary>
        public CategoryFilterEditorUI()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            UpdateAddFilterButtonEnabled();

            this.removeCategoryButton.Enabled = false;
        }

        /// <summary>
        /// List of available category names.
        /// </summary>
        public ArrayList AvailableCategoryNames
        {
            get { return availableCategoryNames; }
            set
            {
                availableCategoryNames = value;
                UpdateAvailableCategoryNames();
            }
        }

        /// <summary>
        /// Sets the selected category names.
        /// </summary>
        /// <param name="names">The category names.</param>
        public void SetSelectedCategoryNames(ArrayList names)
        {
            foreach (string name in names)
            {
                selectedCategories.Items.Add(name);
            }
        }

        /// <summary>
        /// Returns the currently selected category names.
        /// </summary>
        /// <returns>See summary.</returns>
        public ArrayList GetSelectedCategoryNames()
        {
            return new ArrayList(selectedCategories.Items);
        }

        /// <summary>
        /// Determines the filter mode.
        /// </summary>
        public CategoryFilterMode FilterMode
        {
            get
            {
                if (allowCategoriesRadio.Checked)
                {
                    return CategoryFilterMode.AllowAllExceptDenied;
                }
                else
                {
                    return CategoryFilterMode.DenyAllExceptAllowed;
                }
            }
            set
            {
                allowCategoriesRadio.Checked = (value == CategoryFilterMode.AllowAllExceptDenied);
                denyCategoriesRadio.Checked = (value == CategoryFilterMode.DenyAllExceptAllowed);
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CategoryFilterEditorUI));
            this.selectedCategories = new System.Windows.Forms.ListBox();
            this.addCategoryButton = new System.Windows.Forms.Button();
            this.removeCategoryButton = new System.Windows.Forms.Button();
            this.categoryFiltersDescriptionLabel = new System.Windows.Forms.Label();
            this.allowCategoriesRadio = new System.Windows.Forms.RadioButton();
            this.denyCategoriesRadio = new System.Windows.Forms.RadioButton();
            this.OKButton = new System.Windows.Forms.Button();
            this.categoriesGroupBox = new System.Windows.Forms.GroupBox();
            this.availableCategoryNamesBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filterModeGroupBox = new System.Windows.Forms.GroupBox();
            this.categoriesGroupBox.SuspendLayout();
            this.filterModeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedCategories
            // 
            this.selectedCategories.AccessibleDescription = resources.GetString("selectedCategories.AccessibleDescription");
            this.selectedCategories.AccessibleName = resources.GetString("selectedCategories.AccessibleName");
            this.selectedCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("selectedCategories.Anchor")));
            this.selectedCategories.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectedCategories.BackgroundImage")));
            this.selectedCategories.ColumnWidth = ((int)(resources.GetObject("selectedCategories.ColumnWidth")));
            this.selectedCategories.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("selectedCategories.Dock")));
            this.selectedCategories.Enabled = ((bool)(resources.GetObject("selectedCategories.Enabled")));
            this.selectedCategories.Font = ((System.Drawing.Font)(resources.GetObject("selectedCategories.Font")));
            this.selectedCategories.HorizontalExtent = ((int)(resources.GetObject("selectedCategories.HorizontalExtent")));
            this.selectedCategories.HorizontalScrollbar = ((bool)(resources.GetObject("selectedCategories.HorizontalScrollbar")));
            this.selectedCategories.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("selectedCategories.ImeMode")));
            this.selectedCategories.IntegralHeight = ((bool)(resources.GetObject("selectedCategories.IntegralHeight")));
            this.selectedCategories.ItemHeight = ((int)(resources.GetObject("selectedCategories.ItemHeight")));
            this.selectedCategories.Location = ((System.Drawing.Point)(resources.GetObject("selectedCategories.Location")));
            this.selectedCategories.Name = "selectedCategories";
            this.selectedCategories.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("selectedCategories.RightToLeft")));
            this.selectedCategories.ScrollAlwaysVisible = ((bool)(resources.GetObject("selectedCategories.ScrollAlwaysVisible")));
            this.selectedCategories.Size = ((System.Drawing.Size)(resources.GetObject("selectedCategories.Size")));
            this.selectedCategories.TabIndex = ((int)(resources.GetObject("selectedCategories.TabIndex")));
            this.selectedCategories.Visible = ((bool)(resources.GetObject("selectedCategories.Visible")));
            this.selectedCategories.SelectedIndexChanged += new System.EventHandler(this.SelectedCategories_SelectedIndexChanged);
            // 
            // addCategoryButton
            // 
            this.addCategoryButton.AccessibleDescription = resources.GetString("addCategoryButton.AccessibleDescription");
            this.addCategoryButton.AccessibleName = resources.GetString("addCategoryButton.AccessibleName");
            this.addCategoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("addCategoryButton.Anchor")));
            this.addCategoryButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("addCategoryButton.BackgroundImage")));
            this.addCategoryButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("addCategoryButton.Dock")));
            this.addCategoryButton.Enabled = ((bool)(resources.GetObject("addCategoryButton.Enabled")));
            this.addCategoryButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("addCategoryButton.FlatStyle")));
            this.addCategoryButton.Font = ((System.Drawing.Font)(resources.GetObject("addCategoryButton.Font")));
            this.addCategoryButton.Image = ((System.Drawing.Image)(resources.GetObject("addCategoryButton.Image")));
            this.addCategoryButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addCategoryButton.ImageAlign")));
            this.addCategoryButton.ImageIndex = ((int)(resources.GetObject("addCategoryButton.ImageIndex")));
            this.addCategoryButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("addCategoryButton.ImeMode")));
            this.addCategoryButton.Location = ((System.Drawing.Point)(resources.GetObject("addCategoryButton.Location")));
            this.addCategoryButton.Name = "addCategoryButton";
            this.addCategoryButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("addCategoryButton.RightToLeft")));
            this.addCategoryButton.Size = ((System.Drawing.Size)(resources.GetObject("addCategoryButton.Size")));
            this.addCategoryButton.TabIndex = ((int)(resources.GetObject("addCategoryButton.TabIndex")));
            this.addCategoryButton.Text = resources.GetString("addCategoryButton.Text");
            this.addCategoryButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("addCategoryButton.TextAlign")));
            this.addCategoryButton.Visible = ((bool)(resources.GetObject("addCategoryButton.Visible")));
            this.addCategoryButton.Click += new System.EventHandler(this.AddCategoryButton_Click);
            // 
            // removeCategoryButton
            // 
            this.removeCategoryButton.AccessibleDescription = resources.GetString("removeCategoryButton.AccessibleDescription");
            this.removeCategoryButton.AccessibleName = resources.GetString("removeCategoryButton.AccessibleName");
            this.removeCategoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("removeCategoryButton.Anchor")));
            this.removeCategoryButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("removeCategoryButton.BackgroundImage")));
            this.removeCategoryButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("removeCategoryButton.Dock")));
            this.removeCategoryButton.Enabled = ((bool)(resources.GetObject("removeCategoryButton.Enabled")));
            this.removeCategoryButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("removeCategoryButton.FlatStyle")));
            this.removeCategoryButton.Font = ((System.Drawing.Font)(resources.GetObject("removeCategoryButton.Font")));
            this.removeCategoryButton.Image = ((System.Drawing.Image)(resources.GetObject("removeCategoryButton.Image")));
            this.removeCategoryButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("removeCategoryButton.ImageAlign")));
            this.removeCategoryButton.ImageIndex = ((int)(resources.GetObject("removeCategoryButton.ImageIndex")));
            this.removeCategoryButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("removeCategoryButton.ImeMode")));
            this.removeCategoryButton.Location = ((System.Drawing.Point)(resources.GetObject("removeCategoryButton.Location")));
            this.removeCategoryButton.Name = "removeCategoryButton";
            this.removeCategoryButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("removeCategoryButton.RightToLeft")));
            this.removeCategoryButton.Size = ((System.Drawing.Size)(resources.GetObject("removeCategoryButton.Size")));
            this.removeCategoryButton.TabIndex = ((int)(resources.GetObject("removeCategoryButton.TabIndex")));
            this.removeCategoryButton.Text = resources.GetString("removeCategoryButton.Text");
            this.removeCategoryButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("removeCategoryButton.TextAlign")));
            this.removeCategoryButton.Visible = ((bool)(resources.GetObject("removeCategoryButton.Visible")));
            this.removeCategoryButton.Click += new System.EventHandler(this.RemoveCategoryButton_Click);
            // 
            // categoryFiltersDescriptionLabel
            // 
            this.categoryFiltersDescriptionLabel.AccessibleDescription = resources.GetString("categoryFiltersDescriptionLabel.AccessibleDescription");
            this.categoryFiltersDescriptionLabel.AccessibleName = resources.GetString("categoryFiltersDescriptionLabel.AccessibleName");
            this.categoryFiltersDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("categoryFiltersDescriptionLabel.Anchor")));
            this.categoryFiltersDescriptionLabel.AutoSize = ((bool)(resources.GetObject("categoryFiltersDescriptionLabel.AutoSize")));
            this.categoryFiltersDescriptionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("categoryFiltersDescriptionLabel.Dock")));
            this.categoryFiltersDescriptionLabel.Enabled = ((bool)(resources.GetObject("categoryFiltersDescriptionLabel.Enabled")));
            this.categoryFiltersDescriptionLabel.Font = ((System.Drawing.Font)(resources.GetObject("categoryFiltersDescriptionLabel.Font")));
            this.categoryFiltersDescriptionLabel.Image = ((System.Drawing.Image)(resources.GetObject("categoryFiltersDescriptionLabel.Image")));
            this.categoryFiltersDescriptionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("categoryFiltersDescriptionLabel.ImageAlign")));
            this.categoryFiltersDescriptionLabel.ImageIndex = ((int)(resources.GetObject("categoryFiltersDescriptionLabel.ImageIndex")));
            this.categoryFiltersDescriptionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("categoryFiltersDescriptionLabel.ImeMode")));
            this.categoryFiltersDescriptionLabel.Location = ((System.Drawing.Point)(resources.GetObject("categoryFiltersDescriptionLabel.Location")));
            this.categoryFiltersDescriptionLabel.Name = "categoryFiltersDescriptionLabel";
            this.categoryFiltersDescriptionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("categoryFiltersDescriptionLabel.RightToLeft")));
            this.categoryFiltersDescriptionLabel.Size = ((System.Drawing.Size)(resources.GetObject("categoryFiltersDescriptionLabel.Size")));
            this.categoryFiltersDescriptionLabel.TabIndex = ((int)(resources.GetObject("categoryFiltersDescriptionLabel.TabIndex")));
            this.categoryFiltersDescriptionLabel.Text = resources.GetString("categoryFiltersDescriptionLabel.Text");
            this.categoryFiltersDescriptionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("categoryFiltersDescriptionLabel.TextAlign")));
            this.categoryFiltersDescriptionLabel.Visible = ((bool)(resources.GetObject("categoryFiltersDescriptionLabel.Visible")));
            // 
            // allowCategoriesRadio
            // 
            this.allowCategoriesRadio.AccessibleDescription = resources.GetString("allowCategoriesRadio.AccessibleDescription");
            this.allowCategoriesRadio.AccessibleName = resources.GetString("allowCategoriesRadio.AccessibleName");
            this.allowCategoriesRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("allowCategoriesRadio.Anchor")));
            this.allowCategoriesRadio.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("allowCategoriesRadio.Appearance")));
            this.allowCategoriesRadio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("allowCategoriesRadio.BackgroundImage")));
            this.allowCategoriesRadio.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("allowCategoriesRadio.CheckAlign")));
            this.allowCategoriesRadio.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("allowCategoriesRadio.Dock")));
            this.allowCategoriesRadio.Enabled = ((bool)(resources.GetObject("allowCategoriesRadio.Enabled")));
            this.allowCategoriesRadio.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("allowCategoriesRadio.FlatStyle")));
            this.allowCategoriesRadio.Font = ((System.Drawing.Font)(resources.GetObject("allowCategoriesRadio.Font")));
            this.allowCategoriesRadio.Image = ((System.Drawing.Image)(resources.GetObject("allowCategoriesRadio.Image")));
            this.allowCategoriesRadio.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("allowCategoriesRadio.ImageAlign")));
            this.allowCategoriesRadio.ImageIndex = ((int)(resources.GetObject("allowCategoriesRadio.ImageIndex")));
            this.allowCategoriesRadio.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("allowCategoriesRadio.ImeMode")));
            this.allowCategoriesRadio.Location = ((System.Drawing.Point)(resources.GetObject("allowCategoriesRadio.Location")));
            this.allowCategoriesRadio.Name = "allowCategoriesRadio";
            this.allowCategoriesRadio.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("allowCategoriesRadio.RightToLeft")));
            this.allowCategoriesRadio.Size = ((System.Drawing.Size)(resources.GetObject("allowCategoriesRadio.Size")));
            this.allowCategoriesRadio.TabIndex = ((int)(resources.GetObject("allowCategoriesRadio.TabIndex")));
            this.allowCategoriesRadio.Text = resources.GetString("allowCategoriesRadio.Text");
            this.allowCategoriesRadio.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("allowCategoriesRadio.TextAlign")));
            this.allowCategoriesRadio.Visible = ((bool)(resources.GetObject("allowCategoriesRadio.Visible")));
            // 
            // denyCategoriesRadio
            // 
            this.denyCategoriesRadio.AccessibleDescription = resources.GetString("denyCategoriesRadio.AccessibleDescription");
            this.denyCategoriesRadio.AccessibleName = resources.GetString("denyCategoriesRadio.AccessibleName");
            this.denyCategoriesRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("denyCategoriesRadio.Anchor")));
            this.denyCategoriesRadio.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("denyCategoriesRadio.Appearance")));
            this.denyCategoriesRadio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("denyCategoriesRadio.BackgroundImage")));
            this.denyCategoriesRadio.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("denyCategoriesRadio.CheckAlign")));
            this.denyCategoriesRadio.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("denyCategoriesRadio.Dock")));
            this.denyCategoriesRadio.Enabled = ((bool)(resources.GetObject("denyCategoriesRadio.Enabled")));
            this.denyCategoriesRadio.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("denyCategoriesRadio.FlatStyle")));
            this.denyCategoriesRadio.Font = ((System.Drawing.Font)(resources.GetObject("denyCategoriesRadio.Font")));
            this.denyCategoriesRadio.Image = ((System.Drawing.Image)(resources.GetObject("denyCategoriesRadio.Image")));
            this.denyCategoriesRadio.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("denyCategoriesRadio.ImageAlign")));
            this.denyCategoriesRadio.ImageIndex = ((int)(resources.GetObject("denyCategoriesRadio.ImageIndex")));
            this.denyCategoriesRadio.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("denyCategoriesRadio.ImeMode")));
            this.denyCategoriesRadio.Location = ((System.Drawing.Point)(resources.GetObject("denyCategoriesRadio.Location")));
            this.denyCategoriesRadio.Name = "denyCategoriesRadio";
            this.denyCategoriesRadio.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("denyCategoriesRadio.RightToLeft")));
            this.denyCategoriesRadio.Size = ((System.Drawing.Size)(resources.GetObject("denyCategoriesRadio.Size")));
            this.denyCategoriesRadio.TabIndex = ((int)(resources.GetObject("denyCategoriesRadio.TabIndex")));
            this.denyCategoriesRadio.Text = resources.GetString("denyCategoriesRadio.Text");
            this.denyCategoriesRadio.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("denyCategoriesRadio.TextAlign")));
            this.denyCategoriesRadio.Visible = ((bool)(resources.GetObject("denyCategoriesRadio.Visible")));
            // 
            // OKButton
            // 
            this.OKButton.AccessibleDescription = resources.GetString("OKButton.AccessibleDescription");
            this.OKButton.AccessibleName = resources.GetString("OKButton.AccessibleName");
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("OKButton.Anchor")));
            this.OKButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OKButton.BackgroundImage")));
            this.OKButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("OKButton.Dock")));
            this.OKButton.Enabled = ((bool)(resources.GetObject("OKButton.Enabled")));
            this.OKButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("OKButton.FlatStyle")));
            this.OKButton.Font = ((System.Drawing.Font)(resources.GetObject("OKButton.Font")));
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OKButton.ImageAlign")));
            this.OKButton.ImageIndex = ((int)(resources.GetObject("OKButton.ImageIndex")));
            this.OKButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("OKButton.ImeMode")));
            this.OKButton.Location = ((System.Drawing.Point)(resources.GetObject("OKButton.Location")));
            this.OKButton.Name = "OKButton";
            this.OKButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("OKButton.RightToLeft")));
            this.OKButton.Size = ((System.Drawing.Size)(resources.GetObject("OKButton.Size")));
            this.OKButton.TabIndex = ((int)(resources.GetObject("OKButton.TabIndex")));
            this.OKButton.Text = resources.GetString("OKButton.Text");
            this.OKButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("OKButton.TextAlign")));
            this.OKButton.Visible = ((bool)(resources.GetObject("OKButton.Visible")));
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // categoriesGroupBox
            // 
            this.categoriesGroupBox.AccessibleDescription = resources.GetString("categoriesGroupBox.AccessibleDescription");
            this.categoriesGroupBox.AccessibleName = resources.GetString("categoriesGroupBox.AccessibleName");
            this.categoriesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("categoriesGroupBox.Anchor")));
            this.categoriesGroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("categoriesGroupBox.BackgroundImage")));
            this.categoriesGroupBox.Controls.Add(this.removeCategoryButton);
            this.categoriesGroupBox.Controls.Add(this.addCategoryButton);
            this.categoriesGroupBox.Controls.Add(this.selectedCategories);
            this.categoriesGroupBox.Controls.Add(this.availableCategoryNamesBox);
            this.categoriesGroupBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("categoriesGroupBox.Dock")));
            this.categoriesGroupBox.Enabled = ((bool)(resources.GetObject("categoriesGroupBox.Enabled")));
            this.categoriesGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.categoriesGroupBox.Font = ((System.Drawing.Font)(resources.GetObject("categoriesGroupBox.Font")));
            this.categoriesGroupBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("categoriesGroupBox.ImeMode")));
            this.categoriesGroupBox.Location = ((System.Drawing.Point)(resources.GetObject("categoriesGroupBox.Location")));
            this.categoriesGroupBox.Name = "categoriesGroupBox";
            this.categoriesGroupBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("categoriesGroupBox.RightToLeft")));
            this.categoriesGroupBox.Size = ((System.Drawing.Size)(resources.GetObject("categoriesGroupBox.Size")));
            this.categoriesGroupBox.TabIndex = ((int)(resources.GetObject("categoriesGroupBox.TabIndex")));
            this.categoriesGroupBox.TabStop = false;
            this.categoriesGroupBox.Text = resources.GetString("categoriesGroupBox.Text");
            this.categoriesGroupBox.Visible = ((bool)(resources.GetObject("categoriesGroupBox.Visible")));
            // 
            // availableCategoryNamesBox
            // 
            this.availableCategoryNamesBox.AccessibleDescription = resources.GetString("availableCategoryNamesBox.AccessibleDescription");
            this.availableCategoryNamesBox.AccessibleName = resources.GetString("availableCategoryNamesBox.AccessibleName");
            this.availableCategoryNamesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("availableCategoryNamesBox.Anchor")));
            this.availableCategoryNamesBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("availableCategoryNamesBox.BackgroundImage")));
            this.availableCategoryNamesBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("availableCategoryNamesBox.Dock")));
            this.availableCategoryNamesBox.DropDownWidth = 250;
            this.availableCategoryNamesBox.Enabled = ((bool)(resources.GetObject("availableCategoryNamesBox.Enabled")));
            this.availableCategoryNamesBox.Font = ((System.Drawing.Font)(resources.GetObject("availableCategoryNamesBox.Font")));
            this.availableCategoryNamesBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("availableCategoryNamesBox.ImeMode")));
            this.availableCategoryNamesBox.IntegralHeight = ((bool)(resources.GetObject("availableCategoryNamesBox.IntegralHeight")));
            this.availableCategoryNamesBox.ItemHeight = ((int)(resources.GetObject("availableCategoryNamesBox.ItemHeight")));
            this.availableCategoryNamesBox.Location = ((System.Drawing.Point)(resources.GetObject("availableCategoryNamesBox.Location")));
            this.availableCategoryNamesBox.MaxDropDownItems = ((int)(resources.GetObject("availableCategoryNamesBox.MaxDropDownItems")));
            this.availableCategoryNamesBox.MaxLength = ((int)(resources.GetObject("availableCategoryNamesBox.MaxLength")));
            this.availableCategoryNamesBox.Name = "availableCategoryNamesBox";
            this.availableCategoryNamesBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("availableCategoryNamesBox.RightToLeft")));
            this.availableCategoryNamesBox.Size = ((System.Drawing.Size)(resources.GetObject("availableCategoryNamesBox.Size")));
            this.availableCategoryNamesBox.TabIndex = ((int)(resources.GetObject("availableCategoryNamesBox.TabIndex")));
            this.availableCategoryNamesBox.Text = resources.GetString("availableCategoryNamesBox.Text");
            this.availableCategoryNamesBox.Visible = ((bool)(resources.GetObject("availableCategoryNamesBox.Visible")));
            this.availableCategoryNamesBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.availableCategoryNamesBox_KeyDown);
            this.availableCategoryNamesBox.TextChanged += new System.EventHandler(this.availableCategoryNamesBox_TextChanged);
            this.availableCategoryNamesBox.SelectedValueChanged += new System.EventHandler(this.availableCategoryNamesBox_SelectedValueChanged);
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
            // filterModeGroupBox
            // 
            this.filterModeGroupBox.AccessibleDescription = resources.GetString("filterModeGroupBox.AccessibleDescription");
            this.filterModeGroupBox.AccessibleName = resources.GetString("filterModeGroupBox.AccessibleName");
            this.filterModeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("filterModeGroupBox.Anchor")));
            this.filterModeGroupBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("filterModeGroupBox.BackgroundImage")));
            this.filterModeGroupBox.Controls.Add(this.allowCategoriesRadio);
            this.filterModeGroupBox.Controls.Add(this.denyCategoriesRadio);
            this.filterModeGroupBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("filterModeGroupBox.Dock")));
            this.filterModeGroupBox.Enabled = ((bool)(resources.GetObject("filterModeGroupBox.Enabled")));
            this.filterModeGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterModeGroupBox.Font = ((System.Drawing.Font)(resources.GetObject("filterModeGroupBox.Font")));
            this.filterModeGroupBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("filterModeGroupBox.ImeMode")));
            this.filterModeGroupBox.Location = ((System.Drawing.Point)(resources.GetObject("filterModeGroupBox.Location")));
            this.filterModeGroupBox.Name = "filterModeGroupBox";
            this.filterModeGroupBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("filterModeGroupBox.RightToLeft")));
            this.filterModeGroupBox.Size = ((System.Drawing.Size)(resources.GetObject("filterModeGroupBox.Size")));
            this.filterModeGroupBox.TabIndex = ((int)(resources.GetObject("filterModeGroupBox.TabIndex")));
            this.filterModeGroupBox.TabStop = false;
            this.filterModeGroupBox.Text = resources.GetString("filterModeGroupBox.Text");
            this.filterModeGroupBox.Visible = ((bool)(resources.GetObject("filterModeGroupBox.Visible")));
            // 
            // CategoryFilterEditorUI
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.filterModeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.categoriesGroupBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.categoryFiltersDescriptionLabel);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "CategoryFilterEditorUI";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.categoriesGroupBox.ResumeLayout(false);
            this.filterModeGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            if (availableCategoryNamesBox.SelectedIndex > 0)
            {
                selectedCategories.Items.Add(availableCategoryNamesBox.Text);
            }
            else if (availableCategoryNamesBox.Text.Length > 0
                && selectedCategories.Items.IndexOf(availableCategoryNamesBox.Text) == -1)
            {
                selectedCategories.Items.Add(availableCategoryNamesBox.Text);
            }
            UpdateAvailableCategoryNames();
        }

        private void RemoveCategoryButton_Click(object sender, EventArgs e)
        {
            if (selectedCategories.SelectedIndex > -1)
            {
                selectedCategories.Items.RemoveAt(selectedCategories.SelectedIndex);
            }
            UpdateAvailableCategoryNames();
        }

        private void SelectedCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeCategoryButton.Enabled = selectedCategories.SelectedIndex > -1;
        }

        private void UpdateAvailableCategoryNames()
        {
            availableCategoryNamesBox.Items.Clear();
            foreach (string availableCategory in this.AvailableCategoryNames)
            {
                availableCategoryNamesBox.Items.Add(availableCategory);
            }

            foreach (string selectedCategory in selectedCategories.Items)
            {
                availableCategoryNamesBox.Items.Remove(selectedCategory);
            }
            availableCategoryNamesBox.Text = String.Empty;
        }

        private void availableCategoryNamesBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addCategoryButton.PerformClick();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpdateAddFilterButtonEnabled() 
        {
            addCategoryButton.Enabled = availableCategoryNamesBox.Text != null && availableCategoryNamesBox.Text.Trim().Length > 0;
        }

        private void availableCategoryNamesBox_SelectedValueChanged(object sender, System.EventArgs e)
        {
            UpdateAddFilterButtonEnabled();
        }

        private void availableCategoryNamesBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateAddFilterButtonEnabled();
        }
    }
}