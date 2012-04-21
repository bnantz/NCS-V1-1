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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole
{
    internal delegate void ExceptionHandler(Exception e);

    /// <devdoc>
    /// Represents a form that contains a ConfigurationControl, PropertyGrid and ValidationErrorListView.
    /// </devdoc>
    internal class MainForm : Form, IUIService
    {
        private IContainer components;
        private ImageList toolbarImageList;
        private ImageList treeViewImageList;
        private ToolBar toolBar;
        private ToolBarButton newButton;
        private ToolBarButton openButton;
        private ToolBarButton saveAppButton;
        private ToolBarButton saveAllButton;
        private MenuItem fileMenuItem;
        private MenuItem helpMenuItem;
        private MainMenu mainMenu;
        private MenuItem exitMenuItem;
        private MenuItem aboutMenuItem;
        private MenuItem newAppMenuItem;
        private MenuItem openAppmenuItem;
        private MenuItem saveAppMenuItem;
        private MenuItem saveAllMenuItem;
        private MenuItem actionMenuItem;
        private StatusBar statusBar;
        private StatusBarPanel statusBarPanel;
        private Panel bottomPanel;
        private Panel validationErrorsPanel;
        private ListView errorsListView;
        private Panel validationTitlePanel;
        private Label validationTitleLabel;
        private ColumnHeader nameHeader;
        private ColumnHeader propertyHeader;
        private ColumnHeader descriptionHeader;
        private ColumnHeader pathHeader;
        private Splitter bottomSplitter;
        private Panel treePanel;
        private CustomTreeView treeView;
        private Splitter middleSplitter;
        private Panel objectPanel;
        private PropertyGrid propertyGrid;
        private ConfigurationDesignHost designHost;
        private MenuItem fileSplitMenuItem;
        private MenuItem newMenuItem;
        private MenuItem renameMenuItem;
        private ConfigurationTreeNode solutionTreeNode;
        private string defaultText;
        private Hashtable hierarchyDirtyMap;
        private StringCollection hierarchyNames;

        private enum MenuItemPosition
        {
            New = 0,
            Action = 1,
            Rename = 2,
            Help = 3
        }

        public MainForm()
        {
            designHost = new ConfigurationDesignHost();
            InitializeComponent();
            InitializeHierarchy();
            AddServices();
            SetupExceptionHandlers();
            defaultText = this.Text;
            hierarchyDirtyMap = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            hierarchyNames = new StringCollection();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (designHost != null)
                {
                    designHost.RemoveService(typeof(IUIService));
                    designHost.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        public void ShowError(Exception e)
        {
            MessageBox.Show(this, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        public void ShowError(Exception e, string message)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified exception and information about the exception.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="Exception"/> to display.</para>
        /// </param>
        /// <param name="message">
        /// <para>A message to display that provides information about the exception</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowError(Exception e, string message, string caption)
        {
            MessageBox.Show(this, message + Environment.NewLine + e.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        public void ShowError(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>When implemented by a class, displays the specified error message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The error message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// <para>Displays the specified message.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// <para>Displays the specified message with the specified caption.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// <para>Displays the specified message in a message box with the specified caption and buttons to place on the dialog box.</para>
        /// </summary>
        /// <param name="message">
        /// <para>The message to display.</para>
        /// </param>
        /// <param name="caption">
        /// <para>The caption for the message.</para>
        /// </param>
        /// <param name="buttons">
        /// <para>One of the <see cref="MessageBoxButtons"/> values.</para>
        /// </param>
        /// <returns>
        /// <para>One of the <see cref="DialogResult"/> values.</para>
        /// </returns>
        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Information);
        }

        public void BeginUpdate()
        {
            this.treeView.BeginUpdate();
        }

        public void EndUpdate()
        {
            this.treeView.EndUpdate();
        }

        public DialogResult ShowSaveDialog(SaveFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        public DialogResult ShowOpenDialog(OpenFileDialog dialog)
        {
            return dialog.ShowDialog(OwnerWindow);
        }

        /// <summary>
        /// <para>When implemented by a class, gets the owner window.</para>
        /// </summary>
        /// <value>
        /// <para>The owner window.</para>
        /// </value>
        public IWin32Window OwnerWindow
        {
            get { return this; }
        }

        /// <summary>
        /// <para>Activates a node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to activate.</para>
        /// </param>
        public void ActivateNode(ConfigurationNode node)
        {
            ConfigurationTreeNode treeNode = TreeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeView.SelectedNode = treeNode;
            }
        }

        public void DisplayErrorLog(IConfigurationErrorLogService errorLogService)
        {
            ResetErrorList();
            foreach (ValidationError validationError in errorLogService.ValidationErrors)
            {
                UpdateTreeNode((ConfigurationNode)validationError.InvalidObject);
                errorsListView.Items.Add(new ValidationErrorListViewItem(validationError));
            }
            foreach (ConfigurationError configurationError in errorLogService.ConfigurationErrors)
            {
                UpdateTreeNode(configurationError.ConfigurationNode);
                errorsListView.Items.Add(new ConfigurationErrorListViewItem(configurationError));
            }
        }

        public void SetUIDirty(IUIHierarchy hierarchy)
        {
            hierarchyDirtyMap[hierarchy.Id] = true;
            this.Text = String.Concat(defaultText, "*");
        }

        public bool IsDirty(IUIHierarchy hierarchy)
        {
            if (hierarchyDirtyMap.Contains(hierarchy.Id))
            {
                return (bool)hierarchyDirtyMap[hierarchy.Id];
            }
            return false;
        }

        public void SetStatus(string status)
        {
            this.statusBarPanel.Text = status;
        }

        public void ClearErrorDisplay()
        {
            ResetErrorList();
        }

        private void UpdateTreeNode(ConfigurationNode node)
        {
            if (node == null)
            {
                return;
            }
            TreeNode treeNode = TreeNodeFactory.GetTreeNode(node.Id);
            if (treeNode != null)
            {
                treeNode.ForeColor = Color.Red;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ArrayList hierarchiesToBeSaved = GetHierarchiesToBeSaved();
            if (hierarchiesToBeSaved.Count == 0)
            {
                return;
            }

            SaveChangesDialog dialog = new SaveChangesDialog(hierarchiesToBeSaved);
            DialogResult result = dialog.ShowDialog(this);
            if (DialogResult.Cancel == result)
            {
                e.Cancel = true;
                return;
            }

            if (DialogResult.No == result)
            {
                return;
            }

            IUIHierarchyService hierarchyService = designHost.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            hierarchiesToBeSaved = dialog.SelectedHieraries;
            ArrayList invalidHierarchies = new ArrayList(hierarchiesToBeSaved.Count);
            foreach (IUIHierarchy saved in hierarchiesToBeSaved)
            {
                hierarchyService.SelectedHierarchy = saved;
                if (!SaveApplication(saved))
                {
                    invalidHierarchies.Add(saved);
                }
            }
            if (invalidHierarchies.Count > 0)
            {
                MessageBox.Show(this, SR.SaveApplicationsMessage, SR.SaveApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (IUIHierarchy invalidHierarchy in invalidHierarchies)
                {
                    errorsListView.Items.Add(new ConfigurationErrorListViewItem(new ConfigurationError(invalidHierarchy.RootNode, SR.InvalidSaveHierarchyMessage(invalidHierarchy.RootNode.Name))));
                }
                e.Cancel = true;
            }
        }

        private void SaveAllApplications()
        {
            using (new WaitCursor())
            {
                IUIHierarchyService service = (IUIHierarchyService)designHost.GetService(typeof(IUIHierarchyService));
                IUIHierarchy[] hierarchies = service.GetAllHierarchies();
                IUIHierarchy currentHierachy = GetSelectedHierarchy();
                foreach (IUIHierarchy hierarchy in hierarchies)
                {
                    if (Object.ReferenceEquals(hierarchy.RootNode, solutionTreeNode.ConfigurationNode))
                    {
                        continue;
                    }
                    service.SelectedHierarchy = hierarchy;
                    SaveApplication(hierarchy);
                }
                service.SelectedHierarchy = currentHierachy;
            }
        }

        private ArrayList GetHierarchiesToBeSaved()
        {
            IUIHierarchyService service = designHost.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            IUIHierarchy[] hierarchies = service.GetAllHierarchies();
            ArrayList hierarchiesToBeSaved = new ArrayList(hierarchies.Length);
            foreach (IUIHierarchy hierarchy in hierarchies)
            {
                if (IsDirty(hierarchy))
                {
                    hierarchiesToBeSaved.Add(hierarchy);
                }
            }
            return hierarchiesToBeSaved;
        }

        private void AddServices()
        {
            designHost.AddService(typeof(IUIService), this);
        }

        private void InitializeHierarchy()
        {
            TreeNodeFactory.SetImageContainer(new ConfigurationNodeImageContainer(treeViewImageList));
            SolutionConfigurationNode solutionNode = new SolutionConfigurationNode();
            IUIHierarchy hierarchy = ConfigurationUIHierarchyFactory.Create(solutionNode, designHost);
            IUIHierarchyService service = designHost.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            service.AddHierarchy(hierarchy);
            service.HierarchyAdded += new HierarchyAddedEventHandler(OnHierarchyAdded);
            service.HierarchyRemoved += new HierarchyRemovedEventHandler(OnHierarchyRemoved);
            solutionTreeNode = TreeNodeFactory.Create(solutionNode);
            treeView.Nodes.Add(solutionTreeNode);
            SetSelectedNode(solutionTreeNode);
        }

        private void SetupExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledExceptionForm form = new UnhandledExceptionForm(e.Exception);
            form.Text = SR.UnhandledExceptionFormText;
            form.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.Abort)
            {
                this.Close();
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Invoke(new ThreadExceptionEventHandler(OnThreadException), new Object[] {sender, new ThreadExceptionEventArgs((Exception)e.ExceptionObject)});
        }

        private void ResetErrorList()
        {
            foreach (ConfigurationNodeListViewItem item in errorsListView.Items)
            {
                TreeNode treeNode = TreeNodeFactory.GetTreeNode(item.ConfigurationNode.Id);

                if (treeNode != null)
                {
                    treeNode.ForeColor = treeView.ForeColor;
                }
            }
            errorsListView.Items.Clear();
        }

        private string GenerateUniqueName(string nodeName)
        {
            int i = 0;
            string name = nodeName;
            while (true)
            {
                i++;
                if (!hierarchyNames.Contains(name + i.ToString(CultureInfo.CurrentUICulture)))
                {
                    break;
                }
            }

            return name + i.ToString(CultureInfo.CurrentUICulture);
        }

        private void OnHierarchyAdded(object sender, HierarchyAddedEventArgs args)
        {
            ConfigurationTreeNode node = TreeNodeFactory.Create(args.UIHierarchy.RootNode);
            solutionTreeNode.Nodes.Add(node);
            if (node.Text.Length > 0)
            {
                node.ConfigurationNode.Name = GenerateUniqueName(node.ConfigurationNode.Name);
            }
            hierarchyNames.Add(node.Text);
            solutionTreeNode.Expand();
            hierarchyDirtyMap[args.UIHierarchy.Id] = false;
            SetSelectedNode(node);
            EnableSaveOptions(true);
            args.UIHierarchy.Saved += new HierarchySavedEventHandler(UIHierarchySaved);
        }

        private void OnHierarchyRemoved(object sender, HierarchyRemovedEventArgs args)
        {
            ConfigurationTreeNode node = TreeNodeFactory.GetTreeNode(args.UIHierarchy.RootNode.Id);
            node.Remove();
            args.UIHierarchy.Saved -= new HierarchySavedEventHandler(UIHierarchySaved);
            hierarchyDirtyMap.Remove(args.UIHierarchy.Id);
            hierarchyNames.Remove(node.Text);
            UpdateSaveStatus(args.UIHierarchy);
            if (solutionTreeNode.Nodes.Count == 0)
            {
                EnableSaveOptions(false);
            }
            ResetErrorList();
        }

        private void UIHierarchySaved(object sender, HierarchySavedEventArgs args)
        {
            UpdateSaveStatus(args.UIHierarchy);
        }

        private void UpdateSaveStatus(IUIHierarchy hierarchy)
        {
            hierarchyDirtyMap[hierarchy.Id] = false;
            foreach (DictionaryEntry entry in hierarchyDirtyMap)
            {
                if (true == (bool)entry.Value)
                {
                    break;
                }
            }
            this.Text = defaultText;
        }

        private void OnTreeViewBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ConfigurationTreeNode node = e.Node as ConfigurationTreeNode;
            if (node == null)
            {
                return;
            }
            bool readOnly = NodeReadOnly(node.ConfigurationNode);
            if (readOnly)
            {
                e.CancelEdit = true;
            }
        }

        private static bool NodeReadOnly(ConfigurationNode node)
        {
            object[] attributes = node.GetType().GetProperty("Name").GetCustomAttributes(typeof(ReadOnlyAttribute), true);
            return (attributes.Length > 0);
        }

        private void OnTreeViewAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!e.CancelEdit)
            {
                ConfigurationTreeNode selectedNode = (ConfigurationTreeNode)e.Node;
                string label = e.Label;
                if (label != null)
                {
                    if (label.Trim().Length == 0)
                    {
                        e.CancelEdit = true;
                        ShowMessage(SR.NodeLabelEditText, SR.NodeLabelEditCaption);
                        selectedNode.BeginEdit();
                    }
                    else
                    {
                        try
                        {
                            selectedNode.ConfigurationNode.Name = label;
                            SetUIDirty(selectedNode.ConfigurationNode.Hierarchy);
                            selectedNode.EndEdit(false);
                            UpdateCurrentSelection();
                        }
                        catch (InvalidOperationException ex)
                        {
                            e.CancelEdit = true;
                            ShowMessage(SR.NodeLabelEditFailedText + ex.Message, SR.NodeLabelEditCaption);
                            selectedNode.BeginEdit();
                        }
                    }
                }
            }
        }

        private void OnToolbarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == newButton)
            {
                CreateNewApplication();
            }
            else if (e.Button == openButton)
            {
                OpenApplication();
            }
            else if (e.Button == saveAppButton)
            {
                SaveApplication(GetSelectedHierarchy());
            }
            else if (e.Button == this.saveAllButton)
            {
                SaveAllApplications();
            }
        }

        protected override void OnMenuComplete(EventArgs e)
        {
            base.OnMenuComplete(e);
            this.statusBarPanel.Text = SR.DefaultStatusBarText;
        }

        private void OnTreeViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ConfigurationTreeNode selectedNode = (ConfigurationTreeNode)treeView.GetNodeAt(e.X, e.Y);
                SetSelectedNode(selectedNode);
            }
        }

        private void SetSelectedNode(ConfigurationTreeNode selectedNode)
        {
            treeView.SelectedNode = null;
            treeView.SelectedNode = selectedNode;
        }

        private void CreateContextMenu()
        {
            MenuItem cloneActionMenuItem = actionMenuItem.CloneMenu();
            MenuItem cloneNewMenuItem = newMenuItem.CloneMenu();
            MenuItem cloneHelpMenuItem = helpMenuItem.CloneMenu();
            MenuItem cloneRenameMenuItem = renameMenuItem.CloneMenu();
            LoadMenuItemsFromNode((ConfigurationTreeNode)treeView.SelectedNode, new MenuItem[] {cloneNewMenuItem, cloneActionMenuItem, cloneRenameMenuItem, cloneHelpMenuItem});
            ContextMenu menu = new ContextMenu();
            foreach (MenuItem menuItem in cloneActionMenuItem.MenuItems)
            {
                menu.MenuItems.Add(menuItem.CloneMenu());
            }
            if (menu.MenuItems.Count > 0)
            {
                menu.MenuItems.Add("-");
            }
            //menu.Popup += new EventHandler(OnContextMenuItemPopup);
            menu.MenuItems.Add(newAppMenuItem.CloneMenu());
            menu.MenuItems.Add(saveAppMenuItem.CloneMenu());
            UpdateNewMenuItem(cloneNewMenuItem);
            treeView.ContextMenu = menu;
            FindAndUpdateRenameMenuItem(menu);
        }

        private void FindAndUpdateRenameMenuItem(ContextMenu menu)
        {
            foreach (MenuItem item in menu.MenuItems)
            {
                if (string.Compare(item.Text, renameMenuItem.Text, true, CultureInfo.CurrentUICulture) == 0)
                {
                    UpdateRenameMenuItem(item);
                }
            }
        }

        private void UpdateRenameMenuItem(MenuItem item)
        {
            if (treeView.SelectedNode != null)
            {
                ConfigurationTreeNode treeNode = (ConfigurationTreeNode)treeView.SelectedNode;
                item.Enabled = !NodeReadOnly(treeNode.ConfigurationNode);
            }
            else
            {
                item.Enabled = false;
            }
        }

        private void UpdateCurrentSelection()
        {
            ConfigurationTreeNode selectedNode = treeView.SelectedNode as ConfigurationTreeNode;
            UpdateSelection(selectedNode);
        }

        private void UpdateSelection(ConfigurationTreeNode treeNode)
        {
            using (new WaitCursor())
            {
                object[] objects = new object[] {treeNode.ConfigurationNode};
                propertyGrid.SelectedObjects = objects;
            }
        }

        private void OnTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            ConfigurationTreeNode node = e.Node as ConfigurationTreeNode;
            Debug.WriteLine("Current selected node is " + treeView.SelectedNode.Text);
            Debug.WriteLine("Node to be selected from event " + e.Node.Text);
            if (node == null)
            {
                return;
            }
            UpdateSelection(node);
            if (!(node.ConfigurationNode.Id == solutionTreeNode.ConfigurationNode.Id))
            {
                SetSelectedHierarchy(node);
            }
            LoadMenuItemsFromNode(node, new MenuItem[] {newMenuItem, actionMenuItem, renameMenuItem, helpMenuItem});
            CreateContextMenu();
        }

        private void LoadMenuItemsFromNode(ConfigurationTreeNode node, MenuItem[] menuItems)
        {
            IMenuContainerService service = (IMenuContainerService)designHost.GetService(typeof(IMenuContainerService));
            node.ConfigurationNode.AddMenuItems(service);
            AddMenus(menuItems);
        }

        private void SetSelectedHierarchy(ConfigurationTreeNode node)
        {
            IUIHierarchyService hierarchyService = (IUIHierarchyService)designHost.GetService(typeof(IUIHierarchyService));
            hierarchyService.SelectedHierarchy = node.ConfigurationNode.Hierarchy;
        }

        private void OnExitMenItemClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (errorsListView.SelectedItems.Count == 1)
            {
                ConfigurationNodeListViewItem item = (ConfigurationNodeListViewItem)errorsListView.SelectedItems[0];

                if (item.ConfigurationNode != null)
                {
                    treeView.SelectedNode = TreeNodeFactory.GetTreeNode(item.ConfigurationNode.Id);
                }
            }
        }

        private void AddMenus(MenuItem[] menuItems)
        {
            ClearMenuItems(menuItems);
            IMenuContainerService service = (IMenuContainerService)designHost.GetService(typeof(IMenuContainerService));

            ArrayList arrayList = ArrayList.Adapter(service.MenuItems);
            arrayList.Sort(new MenuItemComparer());

            foreach (ConfigurationMenuItem menuItem in service.MenuItems)
            {
                menuItem.Select += new EventHandler(OnMenuItemSelect);
                switch (menuItem.InsertionPoint)
                {
                    case InsertionPoint.Action:
                        menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItem);
                        break;
                    case InsertionPoint.Help:
                        menuItems[(int)MenuItemPosition.Help].MenuItems.Add(menuItem);
                        break;
                    case InsertionPoint.New:
                        menuItems[(int)MenuItemPosition.New].MenuItems.Add(menuItem);
                        break;
                }
            }
            AddDefaultMenuItems(menuItems);
            service.MenuItems.Clear();
        }

        private void OnMenuItemSelect(object sender, EventArgs e)
        {
            ConfigurationMenuItem item = sender as ConfigurationMenuItem;
            if (item != null)
            {
                this.statusBarPanel.Text = item.StatusBarText;
            }
            else
            {
                this.statusBarPanel.Text = SR.DefaultStatusBarText;
            }
        }

        private void OnRenameMenuItemClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }

        private void OnNewMenuItemPopup(object sender, EventArgs e)
        {
            UpdateNewMenuItem((MenuItem)sender);
        }

        private void OnActionMenuItemPopup(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            foreach (MenuItem menuItem in item.MenuItems)
            {
                if (menuItem.Text.Equals(newMenuItem.Text))
                {
                    UpdateNewMenuItem(menuItem);
                }
                if (menuItem.Text.Equals(renameMenuItem.Text))
                {
                    UpdateRenameMenuItem(menuItem);
                }
            }
        }

        private void UpdateNewMenuItem(MenuItem newMenuItem)
        {
            if (newMenuItem.MenuItems.Count == 0)
            {
                newMenuItem.Enabled = false;
            }
            else
            {
                newMenuItem.Enabled = true;
            }
        }

        private void OnSaveAllMenuItemClick(object sender, EventArgs e)
        {
            SaveAllApplications();
        }

        private void EnableSaveOptions(bool enable)
        {
            saveAllButton.Enabled = enable;
            saveAllMenuItem.Enabled = enable;
            saveAppMenuItem.Enabled = enable;
            saveAppButton.Enabled = enable;
        }

        private void OnNewAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = SR.NewApplicationStatusBarText;
        }

        private void OnNewAppMenuItemClick(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                CreateNewApplication();
            }
        }

        private void CreateNewApplication()
        {
            using (AddApplicationConfigurationNodeCommand cmd = new AddApplicationConfigurationNodeCommand(designHost))
            {
                cmd.Execute(null);
            }
        }

        private void OnOpenAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = SR.OpenApplicationStatusBarText;
        }

        private void OnSaveAppMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = SR.SaveApplicationStatusBarText;
        }

        private void OnSaveAllMenuItemSelect(object sender, EventArgs e)
        {
            this.statusBarPanel.Text = SR.SaveAllStatusBarText;
        }

        private void AddDefaultMenuItems(MenuItem[] menuItems)
        {
            if (menuItems[(int)MenuItemPosition.Action].MenuItems.Count == 0)
            {
                menuItems[(int)MenuItemPosition.Action].Enabled = false;
            }
            else
            {
                menuItems[(int)MenuItemPosition.Action].Enabled = true;
            }
            if (menuItems[(int)MenuItemPosition.Help].MenuItems.Count > 0)
            {
                menuItems[(int)MenuItemPosition.Help].MenuItems.Add(new MenuItem("-"));
            }
            helpMenuItem.MenuItems.Add(aboutMenuItem);
            UpdateNewMenuItem(menuItems[(int)MenuItemPosition.New]);
        }

        private void ClearMenuItems(MenuItem[] menuItems)
        {
            menuItems[(int)MenuItemPosition.Action].MenuItems.Clear();
            menuItems[(int)MenuItemPosition.New].MenuItems.Clear();
            menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItems[(int)MenuItemPosition.New]);
            menuItems[(int)MenuItemPosition.Action].MenuItems.Add(menuItems[(int)MenuItemPosition.Rename]);
            helpMenuItem.MenuItems.Clear();
        }

        private void OnSaveApplicationClick(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                SaveApplication(GetSelectedHierarchy());
            }
        }

        private void OnOpenAppMenuItemClick(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                OpenApplication();
            }
        }

        private void OnPropertyChanged(object s, PropertyValueChangedEventArgs e)
        {
            IUIHierarchyService service = designHost.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            if (service.SelectedHierarchy == null)
            {
                return;
            }
            SetUIDirty(service.SelectedHierarchy);
        }

        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            AboutForm frm = new AboutForm();
            frm.ShowDialog(this);
        }

        private void OpenApplication()
        {
            using (new WaitCursor())
            {
                using(OpenApplicationConfigurationCommand cmd = new OpenApplicationConfigurationCommand(designHost))
                {
                    IUIHierarchy hierarchy = GetSelectedHierarchy();
                    cmd.Execute(hierarchy.RootNode);
                }
            }
        }

        private bool SaveApplication(IUIHierarchy hierarchy)
        {
            using (new WaitCursor())
            {
                SaveApplicationConfigurationNodeCommand cmd = new SaveApplicationConfigurationNodeCommand(designHost);
                cmd.Execute(hierarchy.RootNode);
                return cmd.SaveSucceeded;
            }
        }

        private IUIHierarchy GetSelectedHierarchy()
        {
            IUIHierarchyService uiHierarchyService = designHost.GetService(typeof(IUIHierarchyService)) as IUIHierarchyService;
            Debug.Assert(uiHierarchyService != null, "Could not find IUIHierarchyService");
            IUIHierarchy hierarchy = uiHierarchyService.SelectedHierarchy;
            Debug.Assert(hierarchy != null, "Could not find IUIHierarchy");
            return hierarchy;
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.toolbarImageList = new System.Windows.Forms.ImageList(this.components);
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.newButton = new System.Windows.Forms.ToolBarButton();
            this.openButton = new System.Windows.Forms.ToolBarButton();
            this.saveAppButton = new System.Windows.Forms.ToolBarButton();
            this.saveAllButton = new System.Windows.Forms.ToolBarButton();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.newAppMenuItem = new System.Windows.Forms.MenuItem();
            this.openAppmenuItem = new System.Windows.Forms.MenuItem();
            this.saveAppMenuItem = new System.Windows.Forms.MenuItem();
            this.saveAllMenuItem = new System.Windows.Forms.MenuItem();
            this.fileSplitMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.actionMenuItem = new System.Windows.Forms.MenuItem();
            this.newMenuItem = new System.Windows.Forms.MenuItem();
            this.renameMenuItem = new System.Windows.Forms.MenuItem();
            this.helpMenuItem = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.validationErrorsPanel = new System.Windows.Forms.Panel();
            this.errorsListView = new System.Windows.Forms.ListView();
            this.nameHeader = new System.Windows.Forms.ColumnHeader();
            this.propertyHeader = new System.Windows.Forms.ColumnHeader();
            this.descriptionHeader = new System.Windows.Forms.ColumnHeader();
            this.pathHeader = new System.Windows.Forms.ColumnHeader();
            this.validationTitlePanel = new System.Windows.Forms.Panel();
            this.validationTitleLabel = new System.Windows.Forms.Label();
            this.bottomSplitter = new System.Windows.Forms.Splitter();
            this.treePanel = new System.Windows.Forms.Panel();
            this.treeView = new CustomTreeView();
            this.middleSplitter = new System.Windows.Forms.Splitter();
            this.objectPanel = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.validationErrorsPanel.SuspendLayout();
            this.validationTitlePanel.SuspendLayout();
            this.treePanel.SuspendLayout();
            this.objectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarImageList
            // 
            this.toolbarImageList.ImageSize = ((System.Drawing.Size)(resources.GetObject("toolbarImageList.ImageSize")));
            this.toolbarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImageList.ImageStream")));
            this.toolbarImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageSize = ((System.Drawing.Size)(resources.GetObject("treeViewImageList.ImageSize")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolBar
            // 
            this.toolBar.AccessibleDescription = resources.GetString("toolBar.AccessibleDescription");
            this.toolBar.AccessibleName = resources.GetString("toolBar.AccessibleName");
            this.toolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("toolBar.Anchor")));
            this.toolBar.Appearance = ((System.Windows.Forms.ToolBarAppearance)(resources.GetObject("toolBar.Appearance")));
            this.toolBar.AutoSize = ((bool)(resources.GetObject("toolBar.AutoSize")));
            this.toolBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolBar.BackgroundImage")));
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[]
                {
                    this.newButton,
                    this.openButton,
                    this.saveAppButton,
                    this.saveAllButton
                });
            this.toolBar.ButtonSize = ((System.Drawing.Size)(resources.GetObject("toolBar.ButtonSize")));
            this.toolBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("toolBar.Dock")));
            this.toolBar.DropDownArrows = ((bool)(resources.GetObject("toolBar.DropDownArrows")));
            this.toolBar.Enabled = ((bool)(resources.GetObject("toolBar.Enabled")));
            this.toolBar.Font = ((System.Drawing.Font)(resources.GetObject("toolBar.Font")));
            this.toolBar.ImageList = this.toolbarImageList;
            this.toolBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("toolBar.ImeMode")));
            this.toolBar.Location = ((System.Drawing.Point)(resources.GetObject("toolBar.Location")));
            this.toolBar.Name = "toolBar";
            this.toolBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("toolBar.RightToLeft")));
            this.toolBar.ShowToolTips = ((bool)(resources.GetObject("toolBar.ShowToolTips")));
            this.toolBar.Size = ((System.Drawing.Size)(resources.GetObject("toolBar.Size")));
            this.toolBar.TabIndex = ((int)(resources.GetObject("toolBar.TabIndex")));
            this.toolBar.TextAlign = ((System.Windows.Forms.ToolBarTextAlign)(resources.GetObject("toolBar.TextAlign")));
            this.toolBar.Visible = ((bool)(resources.GetObject("toolBar.Visible")));
            this.toolBar.Wrappable = ((bool)(resources.GetObject("toolBar.Wrappable")));
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.OnToolbarButtonClick);
            // 
            // newButton
            // 
            this.newButton.Enabled = ((bool)(resources.GetObject("newButton.Enabled")));
            this.newButton.ImageIndex = ((int)(resources.GetObject("newButton.ImageIndex")));
            this.newButton.Text = resources.GetString("newButton.Text");
            this.newButton.ToolTipText = resources.GetString("newButton.ToolTipText");
            this.newButton.Visible = ((bool)(resources.GetObject("newButton.Visible")));
            // 
            // openButton
            // 
            this.openButton.Enabled = ((bool)(resources.GetObject("openButton.Enabled")));
            this.openButton.ImageIndex = ((int)(resources.GetObject("openButton.ImageIndex")));
            this.openButton.Text = resources.GetString("openButton.Text");
            this.openButton.ToolTipText = resources.GetString("openButton.ToolTipText");
            this.openButton.Visible = ((bool)(resources.GetObject("openButton.Visible")));
            // 
            // saveAppButton
            // 
            this.saveAppButton.Enabled = ((bool)(resources.GetObject("saveAppButton.Enabled")));
            this.saveAppButton.ImageIndex = ((int)(resources.GetObject("saveAppButton.ImageIndex")));
            this.saveAppButton.Text = resources.GetString("saveAppButton.Text");
            this.saveAppButton.ToolTipText = resources.GetString("saveAppButton.ToolTipText");
            this.saveAppButton.Visible = ((bool)(resources.GetObject("saveAppButton.Visible")));
            // 
            // saveAllButton
            // 
            this.saveAllButton.Enabled = ((bool)(resources.GetObject("saveAllButton.Enabled")));
            this.saveAllButton.ImageIndex = ((int)(resources.GetObject("saveAllButton.ImageIndex")));
            this.saveAllButton.Text = resources.GetString("saveAllButton.Text");
            this.saveAllButton.ToolTipText = resources.GetString("saveAllButton.ToolTipText");
            this.saveAllButton.Visible = ((bool)(resources.GetObject("saveAllButton.Visible")));
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
                {
                    this.fileMenuItem,
                    this.actionMenuItem,
                    this.helpMenuItem
                });
            this.mainMenu.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mainMenu.RightToLeft")));
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Enabled = ((bool)(resources.GetObject("fileMenuItem.Enabled")));
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
                {
                    this.newAppMenuItem,
                    this.openAppmenuItem,
                    this.saveAppMenuItem,
                    this.saveAllMenuItem,
                    this.fileSplitMenuItem,
                    this.exitMenuItem
                });
            this.fileMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileMenuItem.Shortcut")));
            this.fileMenuItem.ShowShortcut = ((bool)(resources.GetObject("fileMenuItem.ShowShortcut")));
            this.fileMenuItem.Text = resources.GetString("fileMenuItem.Text");
            this.fileMenuItem.Visible = ((bool)(resources.GetObject("fileMenuItem.Visible")));
            this.fileMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // newAppMenuItem
            // 
            this.newAppMenuItem.Enabled = ((bool)(resources.GetObject("newAppMenuItem.Enabled")));
            this.newAppMenuItem.Index = 0;
            this.newAppMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("newAppMenuItem.Shortcut")));
            this.newAppMenuItem.ShowShortcut = ((bool)(resources.GetObject("newAppMenuItem.ShowShortcut")));
            this.newAppMenuItem.Text = resources.GetString("newAppMenuItem.Text");
            this.newAppMenuItem.Visible = ((bool)(resources.GetObject("newAppMenuItem.Visible")));
            this.newAppMenuItem.Click += new System.EventHandler(this.OnNewAppMenuItemClick);
            this.newAppMenuItem.Select += new System.EventHandler(this.OnNewAppMenuItemSelect);
            // 
            // openAppmenuItem
            // 
            this.openAppmenuItem.Enabled = ((bool)(resources.GetObject("openAppmenuItem.Enabled")));
            this.openAppmenuItem.Index = 1;
            this.openAppmenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("openAppmenuItem.Shortcut")));
            this.openAppmenuItem.ShowShortcut = ((bool)(resources.GetObject("openAppmenuItem.ShowShortcut")));
            this.openAppmenuItem.Text = resources.GetString("openAppmenuItem.Text");
            this.openAppmenuItem.Visible = ((bool)(resources.GetObject("openAppmenuItem.Visible")));
            this.openAppmenuItem.Click += new System.EventHandler(this.OnOpenAppMenuItemClick);
            this.openAppmenuItem.Select += new System.EventHandler(this.OnOpenAppMenuItemSelect);
            // 
            // saveAppMenuItem
            // 
            this.saveAppMenuItem.Enabled = ((bool)(resources.GetObject("saveAppMenuItem.Enabled")));
            this.saveAppMenuItem.Index = 2;
            this.saveAppMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("saveAppMenuItem.Shortcut")));
            this.saveAppMenuItem.ShowShortcut = ((bool)(resources.GetObject("saveAppMenuItem.ShowShortcut")));
            this.saveAppMenuItem.Text = resources.GetString("saveAppMenuItem.Text");
            this.saveAppMenuItem.Visible = ((bool)(resources.GetObject("saveAppMenuItem.Visible")));
            this.saveAppMenuItem.Click += new System.EventHandler(this.OnSaveApplicationClick);
            this.saveAppMenuItem.Select += new System.EventHandler(this.OnSaveAppMenuItemSelect);
            // 
            // saveAllMenuItem
            // 
            this.saveAllMenuItem.Enabled = ((bool)(resources.GetObject("saveAllMenuItem.Enabled")));
            this.saveAllMenuItem.Index = 3;
            this.saveAllMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("saveAllMenuItem.Shortcut")));
            this.saveAllMenuItem.ShowShortcut = ((bool)(resources.GetObject("saveAllMenuItem.ShowShortcut")));
            this.saveAllMenuItem.Text = resources.GetString("saveAllMenuItem.Text");
            this.saveAllMenuItem.Visible = ((bool)(resources.GetObject("saveAllMenuItem.Visible")));
            this.saveAllMenuItem.Click += new System.EventHandler(this.OnSaveAllMenuItemClick);
            this.saveAllMenuItem.Select += new System.EventHandler(this.OnSaveAllMenuItemSelect);
            // 
            // fileSplitMenuItem
            // 
            this.fileSplitMenuItem.Enabled = ((bool)(resources.GetObject("fileSplitMenuItem.Enabled")));
            this.fileSplitMenuItem.Index = 4;
            this.fileSplitMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("fileSplitMenuItem.Shortcut")));
            this.fileSplitMenuItem.ShowShortcut = ((bool)(resources.GetObject("fileSplitMenuItem.ShowShortcut")));
            this.fileSplitMenuItem.Text = resources.GetString("fileSplitMenuItem.Text");
            this.fileSplitMenuItem.Visible = ((bool)(resources.GetObject("fileSplitMenuItem.Visible")));
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Enabled = ((bool)(resources.GetObject("exitMenuItem.Enabled")));
            this.exitMenuItem.Index = 5;
            this.exitMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("exitMenuItem.Shortcut")));
            this.exitMenuItem.ShowShortcut = ((bool)(resources.GetObject("exitMenuItem.ShowShortcut")));
            this.exitMenuItem.Text = resources.GetString("exitMenuItem.Text");
            this.exitMenuItem.Visible = ((bool)(resources.GetObject("exitMenuItem.Visible")));
            this.exitMenuItem.Click += new System.EventHandler(this.OnExitMenItemClick);
            this.exitMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // actionMenuItem
            // 
            this.actionMenuItem.Enabled = ((bool)(resources.GetObject("actionMenuItem.Enabled")));
            this.actionMenuItem.Index = 1;
            this.actionMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
                {
                    this.newMenuItem,
                    this.renameMenuItem
                });
            this.actionMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("actionMenuItem.Shortcut")));
            this.actionMenuItem.ShowShortcut = ((bool)(resources.GetObject("actionMenuItem.ShowShortcut")));
            this.actionMenuItem.Text = resources.GetString("actionMenuItem.Text");
            this.actionMenuItem.Visible = ((bool)(resources.GetObject("actionMenuItem.Visible")));
            this.actionMenuItem.Popup += new System.EventHandler(this.OnActionMenuItemPopup);
            // 
            // newMenuItem
            // 
            this.newMenuItem.Enabled = ((bool)(resources.GetObject("newMenuItem.Enabled")));
            this.newMenuItem.Index = 0;
            this.newMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("newMenuItem.Shortcut")));
            this.newMenuItem.ShowShortcut = ((bool)(resources.GetObject("newMenuItem.ShowShortcut")));
            this.newMenuItem.Text = resources.GetString("newMenuItem.Text");
            this.newMenuItem.Visible = ((bool)(resources.GetObject("newMenuItem.Visible")));
            this.newMenuItem.Popup += new System.EventHandler(this.OnNewMenuItemPopup);
            // 
            // renameMenuItem
            // 
            this.renameMenuItem.Enabled = ((bool)(resources.GetObject("renameMenuItem.Enabled")));
            this.renameMenuItem.Index = 1;
            this.renameMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("renameMenuItem.Shortcut")));
            this.renameMenuItem.ShowShortcut = ((bool)(resources.GetObject("renameMenuItem.ShowShortcut")));
            this.renameMenuItem.Text = resources.GetString("renameMenuItem.Text");
            this.renameMenuItem.Visible = ((bool)(resources.GetObject("renameMenuItem.Visible")));
            this.renameMenuItem.Click += new System.EventHandler(this.OnRenameMenuItemClick);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Enabled = ((bool)(resources.GetObject("helpMenuItem.Enabled")));
            this.helpMenuItem.Index = 2;
            this.helpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
                {
                    this.aboutMenuItem
                });
            this.helpMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("helpMenuItem.Shortcut")));
            this.helpMenuItem.ShowShortcut = ((bool)(resources.GetObject("helpMenuItem.ShowShortcut")));
            this.helpMenuItem.Text = resources.GetString("helpMenuItem.Text");
            this.helpMenuItem.Visible = ((bool)(resources.GetObject("helpMenuItem.Visible")));
            this.helpMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Enabled = ((bool)(resources.GetObject("aboutMenuItem.Enabled")));
            this.aboutMenuItem.Index = 0;
            this.aboutMenuItem.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("aboutMenuItem.Shortcut")));
            this.aboutMenuItem.ShowShortcut = ((bool)(resources.GetObject("aboutMenuItem.ShowShortcut")));
            this.aboutMenuItem.Text = resources.GetString("aboutMenuItem.Text");
            this.aboutMenuItem.Visible = ((bool)(resources.GetObject("aboutMenuItem.Visible")));
            this.aboutMenuItem.Click += new System.EventHandler(this.OnAboutMenuItemClick);
            this.aboutMenuItem.Select += new System.EventHandler(this.OnMenuItemSelect);
            // 
            // statusBar
            // 
            this.statusBar.AccessibleDescription = resources.GetString("statusBar.AccessibleDescription");
            this.statusBar.AccessibleName = resources.GetString("statusBar.AccessibleName");
            this.statusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("statusBar.Anchor")));
            this.statusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar.BackgroundImage")));
            this.statusBar.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("statusBar.Dock")));
            this.statusBar.Enabled = ((bool)(resources.GetObject("statusBar.Enabled")));
            this.statusBar.Font = ((System.Drawing.Font)(resources.GetObject("statusBar.Font")));
            this.statusBar.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("statusBar.ImeMode")));
            this.statusBar.Location = ((System.Drawing.Point)(resources.GetObject("statusBar.Location")));
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[]
                {
                    this.statusBarPanel
                });
            this.statusBar.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("statusBar.RightToLeft")));
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = ((System.Drawing.Size)(resources.GetObject("statusBar.Size")));
            this.statusBar.TabIndex = ((int)(resources.GetObject("statusBar.TabIndex")));
            this.statusBar.Text = resources.GetString("statusBar.Text");
            this.statusBar.Visible = ((bool)(resources.GetObject("statusBar.Visible")));
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.Alignment = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("statusBarPanel.Alignment")));
            this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel.Icon")));
            this.statusBarPanel.MinWidth = ((int)(resources.GetObject("statusBarPanel.MinWidth")));
            this.statusBarPanel.Text = resources.GetString("statusBarPanel.Text");
            this.statusBarPanel.ToolTipText = resources.GetString("statusBarPanel.ToolTipText");
            this.statusBarPanel.Width = ((int)(resources.GetObject("statusBarPanel.Width")));
            // 
            // bottomPanel
            // 
            this.bottomPanel.AccessibleDescription = resources.GetString("bottomPanel.AccessibleDescription");
            this.bottomPanel.AccessibleName = resources.GetString("bottomPanel.AccessibleName");
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("bottomPanel.Anchor")));
            this.bottomPanel.AutoScroll = ((bool)(resources.GetObject("bottomPanel.AutoScroll")));
            this.bottomPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("bottomPanel.AutoScrollMargin")));
            this.bottomPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("bottomPanel.AutoScrollMinSize")));
            this.bottomPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomPanel.BackgroundImage")));
            this.bottomPanel.Controls.Add(this.validationErrorsPanel);
            this.bottomPanel.Controls.Add(this.validationTitlePanel);
            this.bottomPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("bottomPanel.Dock")));
            this.bottomPanel.Enabled = ((bool)(resources.GetObject("bottomPanel.Enabled")));
            this.bottomPanel.Font = ((System.Drawing.Font)(resources.GetObject("bottomPanel.Font")));
            this.bottomPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("bottomPanel.ImeMode")));
            this.bottomPanel.Location = ((System.Drawing.Point)(resources.GetObject("bottomPanel.Location")));
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("bottomPanel.RightToLeft")));
            this.bottomPanel.Size = ((System.Drawing.Size)(resources.GetObject("bottomPanel.Size")));
            this.bottomPanel.TabIndex = ((int)(resources.GetObject("bottomPanel.TabIndex")));
            this.bottomPanel.Text = resources.GetString("bottomPanel.Text");
            this.bottomPanel.Visible = ((bool)(resources.GetObject("bottomPanel.Visible")));
            // 
            // validationErrorsPanel
            // 
            this.validationErrorsPanel.AccessibleDescription = resources.GetString("validationErrorsPanel.AccessibleDescription");
            this.validationErrorsPanel.AccessibleName = resources.GetString("validationErrorsPanel.AccessibleName");
            this.validationErrorsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("validationErrorsPanel.Anchor")));
            this.validationErrorsPanel.AutoScroll = ((bool)(resources.GetObject("validationErrorsPanel.AutoScroll")));
            this.validationErrorsPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("validationErrorsPanel.AutoScrollMargin")));
            this.validationErrorsPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("validationErrorsPanel.AutoScrollMinSize")));
            this.validationErrorsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.validationErrorsPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("validationErrorsPanel.BackgroundImage")));
            this.validationErrorsPanel.Controls.Add(this.errorsListView);
            this.validationErrorsPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("validationErrorsPanel.Dock")));
            this.validationErrorsPanel.DockPadding.All = 1;
            this.validationErrorsPanel.Enabled = ((bool)(resources.GetObject("validationErrorsPanel.Enabled")));
            this.validationErrorsPanel.Font = ((System.Drawing.Font)(resources.GetObject("validationErrorsPanel.Font")));
            this.validationErrorsPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("validationErrorsPanel.ImeMode")));
            this.validationErrorsPanel.Location = ((System.Drawing.Point)(resources.GetObject("validationErrorsPanel.Location")));
            this.validationErrorsPanel.Name = "validationErrorsPanel";
            this.validationErrorsPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("validationErrorsPanel.RightToLeft")));
            this.validationErrorsPanel.Size = ((System.Drawing.Size)(resources.GetObject("validationErrorsPanel.Size")));
            this.validationErrorsPanel.TabIndex = ((int)(resources.GetObject("validationErrorsPanel.TabIndex")));
            this.validationErrorsPanel.Text = resources.GetString("validationErrorsPanel.Text");
            this.validationErrorsPanel.Visible = ((bool)(resources.GetObject("validationErrorsPanel.Visible")));
            // 
            // errorsListView
            // 
            this.errorsListView.AccessibleDescription = resources.GetString("errorsListView.AccessibleDescription");
            this.errorsListView.AccessibleName = resources.GetString("errorsListView.AccessibleName");
            this.errorsListView.Alignment = ((System.Windows.Forms.ListViewAlignment)(resources.GetObject("errorsListView.Alignment")));
            this.errorsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("errorsListView.Anchor")));
            this.errorsListView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("errorsListView.BackgroundImage")));
            this.errorsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
                {
                    this.nameHeader,
                    this.propertyHeader,
                    this.descriptionHeader,
                    this.pathHeader
                });
            this.errorsListView.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("errorsListView.Dock")));
            this.errorsListView.Enabled = ((bool)(resources.GetObject("errorsListView.Enabled")));
            this.errorsListView.Font = ((System.Drawing.Font)(resources.GetObject("errorsListView.Font")));
            this.errorsListView.FullRowSelect = true;
            this.errorsListView.GridLines = true;
            this.errorsListView.HideSelection = false;
            this.errorsListView.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("errorsListView.ImeMode")));
            this.errorsListView.LabelWrap = ((bool)(resources.GetObject("errorsListView.LabelWrap")));
            this.errorsListView.Location = ((System.Drawing.Point)(resources.GetObject("errorsListView.Location")));
            this.errorsListView.Name = "errorsListView";
            this.errorsListView.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("errorsListView.RightToLeft")));
            this.errorsListView.Size = ((System.Drawing.Size)(resources.GetObject("errorsListView.Size")));
            this.errorsListView.TabIndex = ((int)(resources.GetObject("errorsListView.TabIndex")));
            this.errorsListView.Text = resources.GetString("errorsListView.Text");
            this.errorsListView.View = System.Windows.Forms.View.Details;
            this.errorsListView.Visible = ((bool)(resources.GetObject("errorsListView.Visible")));
            this.errorsListView.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // nameHeader
            // 
            this.nameHeader.Text = resources.GetString("nameHeader.Text");
            this.nameHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("nameHeader.TextAlign")));
            this.nameHeader.Width = ((int)(resources.GetObject("nameHeader.Width")));
            // 
            // propertyHeader
            // 
            this.propertyHeader.Text = resources.GetString("propertyHeader.Text");
            this.propertyHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("propertyHeader.TextAlign")));
            this.propertyHeader.Width = ((int)(resources.GetObject("propertyHeader.Width")));
            // 
            // descriptionHeader
            // 
            this.descriptionHeader.Text = resources.GetString("descriptionHeader.Text");
            this.descriptionHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("descriptionHeader.TextAlign")));
            this.descriptionHeader.Width = ((int)(resources.GetObject("descriptionHeader.Width")));
            // 
            // pathHeader
            // 
            this.pathHeader.Text = resources.GetString("pathHeader.Text");
            this.pathHeader.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("pathHeader.TextAlign")));
            this.pathHeader.Width = ((int)(resources.GetObject("pathHeader.Width")));
            // 
            // validationTitlePanel
            // 
            this.validationTitlePanel.AccessibleDescription = resources.GetString("validationTitlePanel.AccessibleDescription");
            this.validationTitlePanel.AccessibleName = resources.GetString("validationTitlePanel.AccessibleName");
            this.validationTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("validationTitlePanel.Anchor")));
            this.validationTitlePanel.AutoScroll = ((bool)(resources.GetObject("validationTitlePanel.AutoScroll")));
            this.validationTitlePanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("validationTitlePanel.AutoScrollMargin")));
            this.validationTitlePanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("validationTitlePanel.AutoScrollMinSize")));
            this.validationTitlePanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("validationTitlePanel.BackgroundImage")));
            this.validationTitlePanel.Controls.Add(this.validationTitleLabel);
            this.validationTitlePanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("validationTitlePanel.Dock")));
            this.validationTitlePanel.Enabled = ((bool)(resources.GetObject("validationTitlePanel.Enabled")));
            this.validationTitlePanel.Font = ((System.Drawing.Font)(resources.GetObject("validationTitlePanel.Font")));
            this.validationTitlePanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("validationTitlePanel.ImeMode")));
            this.validationTitlePanel.Location = ((System.Drawing.Point)(resources.GetObject("validationTitlePanel.Location")));
            this.validationTitlePanel.Name = "validationTitlePanel";
            this.validationTitlePanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("validationTitlePanel.RightToLeft")));
            this.validationTitlePanel.Size = ((System.Drawing.Size)(resources.GetObject("validationTitlePanel.Size")));
            this.validationTitlePanel.TabIndex = ((int)(resources.GetObject("validationTitlePanel.TabIndex")));
            this.validationTitlePanel.Text = resources.GetString("validationTitlePanel.Text");
            this.validationTitlePanel.Visible = ((bool)(resources.GetObject("validationTitlePanel.Visible")));
            // 
            // validationTitleLabel
            // 
            this.validationTitleLabel.AccessibleDescription = resources.GetString("validationTitleLabel.AccessibleDescription");
            this.validationTitleLabel.AccessibleName = resources.GetString("validationTitleLabel.AccessibleName");
            this.validationTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("validationTitleLabel.Anchor")));
            this.validationTitleLabel.AutoSize = ((bool)(resources.GetObject("validationTitleLabel.AutoSize")));
            this.validationTitleLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("validationTitleLabel.Dock")));
            this.validationTitleLabel.Enabled = ((bool)(resources.GetObject("validationTitleLabel.Enabled")));
            this.validationTitleLabel.Font = ((System.Drawing.Font)(resources.GetObject("validationTitleLabel.Font")));
            this.validationTitleLabel.Image = ((System.Drawing.Image)(resources.GetObject("validationTitleLabel.Image")));
            this.validationTitleLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("validationTitleLabel.ImageAlign")));
            this.validationTitleLabel.ImageIndex = ((int)(resources.GetObject("validationTitleLabel.ImageIndex")));
            this.validationTitleLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("validationTitleLabel.ImeMode")));
            this.validationTitleLabel.Location = ((System.Drawing.Point)(resources.GetObject("validationTitleLabel.Location")));
            this.validationTitleLabel.Name = "validationTitleLabel";
            this.validationTitleLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("validationTitleLabel.RightToLeft")));
            this.validationTitleLabel.Size = ((System.Drawing.Size)(resources.GetObject("validationTitleLabel.Size")));
            this.validationTitleLabel.TabIndex = ((int)(resources.GetObject("validationTitleLabel.TabIndex")));
            this.validationTitleLabel.Text = resources.GetString("validationTitleLabel.Text");
            this.validationTitleLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("validationTitleLabel.TextAlign")));
            this.validationTitleLabel.Visible = ((bool)(resources.GetObject("validationTitleLabel.Visible")));
            // 
            // bottomSplitter
            // 
            this.bottomSplitter.AccessibleDescription = resources.GetString("bottomSplitter.AccessibleDescription");
            this.bottomSplitter.AccessibleName = resources.GetString("bottomSplitter.AccessibleName");
            this.bottomSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("bottomSplitter.Anchor")));
            this.bottomSplitter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bottomSplitter.BackgroundImage")));
            this.bottomSplitter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("bottomSplitter.Dock")));
            this.bottomSplitter.Enabled = ((bool)(resources.GetObject("bottomSplitter.Enabled")));
            this.bottomSplitter.Font = ((System.Drawing.Font)(resources.GetObject("bottomSplitter.Font")));
            this.bottomSplitter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("bottomSplitter.ImeMode")));
            this.bottomSplitter.Location = ((System.Drawing.Point)(resources.GetObject("bottomSplitter.Location")));
            this.bottomSplitter.MinExtra = ((int)(resources.GetObject("bottomSplitter.MinExtra")));
            this.bottomSplitter.MinSize = ((int)(resources.GetObject("bottomSplitter.MinSize")));
            this.bottomSplitter.Name = "bottomSplitter";
            this.bottomSplitter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("bottomSplitter.RightToLeft")));
            this.bottomSplitter.Size = ((System.Drawing.Size)(resources.GetObject("bottomSplitter.Size")));
            this.bottomSplitter.TabIndex = ((int)(resources.GetObject("bottomSplitter.TabIndex")));
            this.bottomSplitter.TabStop = false;
            this.bottomSplitter.Visible = ((bool)(resources.GetObject("bottomSplitter.Visible")));
            // 
            // treePanel
            // 
            this.treePanel.AccessibleDescription = resources.GetString("treePanel.AccessibleDescription");
            this.treePanel.AccessibleName = resources.GetString("treePanel.AccessibleName");
            this.treePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("treePanel.Anchor")));
            this.treePanel.AutoScroll = ((bool)(resources.GetObject("treePanel.AutoScroll")));
            this.treePanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("treePanel.AutoScrollMargin")));
            this.treePanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("treePanel.AutoScrollMinSize")));
            this.treePanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.treePanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("treePanel.BackgroundImage")));
            this.treePanel.Controls.Add(this.treeView);
            this.treePanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("treePanel.Dock")));
            this.treePanel.DockPadding.All = 1;
            this.treePanel.Enabled = ((bool)(resources.GetObject("treePanel.Enabled")));
            this.treePanel.Font = ((System.Drawing.Font)(resources.GetObject("treePanel.Font")));
            this.treePanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("treePanel.ImeMode")));
            this.treePanel.Location = ((System.Drawing.Point)(resources.GetObject("treePanel.Location")));
            this.treePanel.Name = "treePanel";
            this.treePanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("treePanel.RightToLeft")));
            this.treePanel.Size = ((System.Drawing.Size)(resources.GetObject("treePanel.Size")));
            this.treePanel.TabIndex = ((int)(resources.GetObject("treePanel.TabIndex")));
            this.treePanel.Text = resources.GetString("treePanel.Text");
            this.treePanel.Visible = ((bool)(resources.GetObject("treePanel.Visible")));
            // 
            // treeView
            // 
            this.treeView.AccessibleDescription = resources.GetString("treeView.AccessibleDescription");
            this.treeView.AccessibleName = resources.GetString("treeView.AccessibleName");
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("treeView.Anchor")));
            this.treeView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("treeView.BackgroundImage")));
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("treeView.Dock")));
            this.treeView.Enabled = ((bool)(resources.GetObject("treeView.Enabled")));
            this.treeView.Font = ((System.Drawing.Font)(resources.GetObject("treeView.Font")));
            this.treeView.HideSelection = false;
            this.treeView.HotTracking = true;
            this.treeView.ImageIndex = ((int)(resources.GetObject("treeView.ImageIndex")));
            this.treeView.ImageList = this.treeViewImageList;
            this.treeView.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("treeView.ImeMode")));
            this.treeView.Indent = ((int)(resources.GetObject("treeView.Indent")));
            this.treeView.ItemHeight = ((int)(resources.GetObject("treeView.ItemHeight")));
            this.treeView.LabelEdit = true;
            this.treeView.Location = ((System.Drawing.Point)(resources.GetObject("treeView.Location")));
            this.treeView.Name = "treeView";
            this.treeView.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("treeView.RightToLeft")));
            this.treeView.SelectedImageIndex = ((int)(resources.GetObject("treeView.SelectedImageIndex")));
            this.treeView.Size = ((System.Drawing.Size)(resources.GetObject("treeView.Size")));
            this.treeView.TabIndex = ((int)(resources.GetObject("treeView.TabIndex")));
            this.treeView.Text = resources.GetString("treeView.Text");
            this.treeView.Visible = ((bool)(resources.GetObject("treeView.Visible")));
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnTreeViewMouseDown);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeViewAfterSelect);
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnTreeViewAfterLabelEdit);
            this.treeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnTreeViewBeforeLabelEdit);
            // 
            // middleSplitter
            // 
            this.middleSplitter.AccessibleDescription = resources.GetString("middleSplitter.AccessibleDescription");
            this.middleSplitter.AccessibleName = resources.GetString("middleSplitter.AccessibleName");
            this.middleSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("middleSplitter.Anchor")));
            this.middleSplitter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("middleSplitter.BackgroundImage")));
            this.middleSplitter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("middleSplitter.Dock")));
            this.middleSplitter.Enabled = ((bool)(resources.GetObject("middleSplitter.Enabled")));
            this.middleSplitter.Font = ((System.Drawing.Font)(resources.GetObject("middleSplitter.Font")));
            this.middleSplitter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("middleSplitter.ImeMode")));
            this.middleSplitter.Location = ((System.Drawing.Point)(resources.GetObject("middleSplitter.Location")));
            this.middleSplitter.MinExtra = ((int)(resources.GetObject("middleSplitter.MinExtra")));
            this.middleSplitter.MinSize = ((int)(resources.GetObject("middleSplitter.MinSize")));
            this.middleSplitter.Name = "middleSplitter";
            this.middleSplitter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("middleSplitter.RightToLeft")));
            this.middleSplitter.Size = ((System.Drawing.Size)(resources.GetObject("middleSplitter.Size")));
            this.middleSplitter.TabIndex = ((int)(resources.GetObject("middleSplitter.TabIndex")));
            this.middleSplitter.TabStop = false;
            this.middleSplitter.Visible = ((bool)(resources.GetObject("middleSplitter.Visible")));
            // 
            // objectPanel
            // 
            this.objectPanel.AccessibleDescription = resources.GetString("objectPanel.AccessibleDescription");
            this.objectPanel.AccessibleName = resources.GetString("objectPanel.AccessibleName");
            this.objectPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("objectPanel.Anchor")));
            this.objectPanel.AutoScroll = ((bool)(resources.GetObject("objectPanel.AutoScroll")));
            this.objectPanel.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("objectPanel.AutoScrollMargin")));
            this.objectPanel.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("objectPanel.AutoScrollMinSize")));
            this.objectPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("objectPanel.BackgroundImage")));
            this.objectPanel.Controls.Add(this.propertyGrid);
            this.objectPanel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("objectPanel.Dock")));
            this.objectPanel.Enabled = ((bool)(resources.GetObject("objectPanel.Enabled")));
            this.objectPanel.Font = ((System.Drawing.Font)(resources.GetObject("objectPanel.Font")));
            this.objectPanel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("objectPanel.ImeMode")));
            this.objectPanel.Location = ((System.Drawing.Point)(resources.GetObject("objectPanel.Location")));
            this.objectPanel.Name = "objectPanel";
            this.objectPanel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("objectPanel.RightToLeft")));
            this.objectPanel.Size = ((System.Drawing.Size)(resources.GetObject("objectPanel.Size")));
            this.objectPanel.TabIndex = ((int)(resources.GetObject("objectPanel.TabIndex")));
            this.objectPanel.Text = resources.GetString("objectPanel.Text");
            this.objectPanel.Visible = ((bool)(resources.GetObject("objectPanel.Visible")));
            // 
            // propertyGrid
            // 
            this.propertyGrid.AccessibleDescription = resources.GetString("propertyGrid.AccessibleDescription");
            this.propertyGrid.AccessibleName = resources.GetString("propertyGrid.AccessibleName");
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("propertyGrid.Anchor")));
            this.propertyGrid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("propertyGrid.BackgroundImage")));
            this.propertyGrid.CommandsVisibleIfAvailable = true;
            this.propertyGrid.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("propertyGrid.Dock")));
            this.propertyGrid.Enabled = ((bool)(resources.GetObject("propertyGrid.Enabled")));
            this.propertyGrid.Font = ((System.Drawing.Font)(resources.GetObject("propertyGrid.Font")));
            this.propertyGrid.HelpVisible = ((bool)(resources.GetObject("propertyGrid.HelpVisible")));
            this.propertyGrid.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("propertyGrid.ImeMode")));
            this.propertyGrid.LargeButtons = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = ((System.Drawing.Point)(resources.GetObject("propertyGrid.Location")));
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("propertyGrid.RightToLeft")));
            this.propertyGrid.Size = ((System.Drawing.Size)(resources.GetObject("propertyGrid.Size")));
            this.propertyGrid.TabIndex = ((int)(resources.GetObject("propertyGrid.TabIndex")));
            this.propertyGrid.Text = resources.GetString("propertyGrid.Text");
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
            this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
            this.propertyGrid.Visible = ((bool)(resources.GetObject("propertyGrid.Visible")));
            this.propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyChanged);
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
            this.Controls.Add(this.objectPanel);
            this.Controls.Add(this.middleSplitter);
            this.Controls.Add(this.treePanel);
            this.Controls.Add(this.bottomSplitter);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.Menu = this.mainMenu;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "MainForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.validationErrorsPanel.ResumeLayout(false);
            this.validationTitlePanel.ResumeLayout(false);
            this.treePanel.ResumeLayout(false);
            this.objectPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}