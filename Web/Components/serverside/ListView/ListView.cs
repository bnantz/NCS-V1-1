// ListView.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    [
    DefaultEvent("SelectedIndexChanged"),
    DefaultProperty("DataSource"),
    Designer(typeof(MSPress.WebControls.Design.ListViewDesigner), typeof(IDesigner))
    ]
    public class ListView : WebControl, INamingContainer, IPostBackEventHandler {

        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventItemCreated = new object();
        private static readonly object EventItemDataBound = new object();
        private static readonly object EventItemCommand = new object();

        public const string SelectCommandName = "Select";
        public const string EditCommandName = "Edit";
        public const string UpdateCommandName = "Update";
        public const string CancelEditCommandName = "Cancel";
        public const string DeleteCommandName = "Delete";

        private ITemplate _itemTemplate;
        private ITemplate _editItemTemplate;
        private ITemplate _headerTemplate;
        private ITemplate _footerTemplate;
        private TableItemStyle _itemStyle;
        private TableItemStyle _editItemStyle;
        private TableItemStyle _selectedItemStyle;
        private TableItemStyle _headerStyle;
        private TableItemStyle _footerStyle;
        private ListViewPanelStyle _viewStyle;
        private object _dataSource;

        private ListViewItemCollection _items;
        private DataKeyCollection _dataKeys;

        private bool _renderClickSelectScript;

        [
        Category("Appearance"),
        DefaultValue(-1),
        Description("The padding inside cells")
        ]
        public virtual int CellPadding {
            get {
                if (ControlStyleCreated == false) {
                    return -1;
                }
                return ((TableStyle)ControlStyle).CellPadding;
            }
            set {
                ((TableStyle)ControlStyle).CellPadding = value;
            }
        }

        [
        Category("Appearance"),
        DefaultValue(0),
        Description("The spacing around cells")
        ]
        public virtual int CellSpacing {
            get {
                if (ControlStyleCreated == false) {
                    return 0;
                }
                return ((TableStyle)ControlStyle).CellSpacing;
            }
            set {
                ((TableStyle)ControlStyle).CellSpacing = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(1),
        Description("The number of columns in the rendering")
        ]
        public virtual int Columns {
            get {
                object i = ViewState["Columns"];
                return (i == null) ? 1 : (int)i;
            }
            set {
                if (value < 1) {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["Columns"] = value;
            }
        }

        [
        Category("Data"),
        DefaultValue(""),
        Description("The name of the field that contains a key for each item")
        ]
        public virtual string DataKeyField {
            get {
                string s = (string)ViewState["DataKeyField"];
                return (s == null) ? String.Empty : s;
            }
            set {
                ViewState["DataKeyField"] = value;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public DataKeyCollection DataKeys {
            get {
                if (_dataKeys == null) {
                    _dataKeys = new DataKeyCollection(this.DataKeysArray);
                }
                return _dataKeys;
            }
        }

        private ArrayList DataKeysArray {
            get {
                object o = ViewState["DataKeys"];
                if (o == null) {
                    o = new ArrayList();
                    ViewState["DataKeys"] = o;
                }
                return (ArrayList)o;
            }
        }

        [
        Category("Data"),
        DefaultValue(""),
        Description("The name of the table within the data source")
        ]
        public virtual string DataMember {
            get {
                string s = (string)ViewState["DataMember"];
                return (s == null) ? String.Empty : s;
            }
            set {
                ViewState["DataMember"] = value;
            }
        }

        [
        Bindable(true),
        Category("Data"),
        DefaultValue(null),
        Description("The data source containing data to be rendered"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual object DataSource {
            get {
                return _dataSource;
            }
            set {
                if ((value == null) || (value is IListSource) || (value is IEnumerable)) {
                    _dataSource = value;
                }
                else {
                    throw new ArgumentException();
                }
            }
        }

        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(-1),
        Description("The index of the item in edit mode")
        ]
        public virtual int EditIndex {
            get {
                object i = ViewState["EditIndex"];
                return (i == null) ? -1 : (int)i;
            }
            set {
                if (value < -1) {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["EditIndex"] = value;
            }
        }

        [
        Category("Style"),
        Description("Thr style applied to items in edit mode"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TableItemStyle EditItemStyle {
            get {
                if (_editItemStyle == null) {
                    _editItemStyle = new TableItemStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_editItemStyle).TrackViewState();
                    }
                }
                return _editItemStyle;
            }
        }

        [
        Browsable(false),
        DefaultValue(null),
        Description("The template associated with items in edit mode"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(ListViewItem))
        ]
        public ITemplate EditItemTemplate {
            get {
                return _editItemTemplate;
            }
            set {
                _editItemTemplate = value;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether to enable the click-select behavior")
        ]
        public bool EnableClickSelect {
            get {
                object b = ViewState["EnableClickSelect"];
                return (b == null) ? false : true;
            }
            set {
                ViewState["EnableClickSelect"] = value;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the footer"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TableItemStyle FooterStyle {
            get {
                if (_footerStyle == null) {
                    _footerStyle = new TableItemStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_footerStyle).TrackViewState();
                    }
                }
                return _footerStyle;
            }
        }

        [
        Browsable(false),
        DefaultValue(null),
        Description("The template associated with the footer"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(ListViewItem))
        ]
        public ITemplate FooterTemplate {
            get {
                return _footerTemplate;
            }
            set {
                _footerTemplate = value;
            }
        }

        [
        Category("Appearance"),
        DefaultValue(GridLines.None),
        Description("The type of grid lines between cells")
        ]
        public virtual GridLines GridLines {
            get {
                if (ControlStyleCreated == false) {
                    return GridLines.None;
                }
                return ((TableStyle)ControlStyle).GridLines;
            }
            set {
                ((TableStyle)ControlStyle).GridLines = value;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the header"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TableItemStyle HeaderStyle {
            get {
                if (_headerStyle == null) {
                    _headerStyle = new TableItemStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_headerStyle).TrackViewState();
                    }
                }
                return _headerStyle;
            }
        }

        [
        Browsable(false),
        DefaultValue(null),
        Description("The template associated with the header"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(ListViewItem))
        ]
        public ITemplate HeaderTemplate {
            get {
                return _headerTemplate;
            }
            set {
                _headerTemplate = value;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public ListViewItemCollection Items {
            get {
                EnsureChildControls();
                return _items;
            }
        }

        [
        Category("Style"),
        Description("The style applied to all items"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TableItemStyle ItemStyle {
            get {
                if (_itemStyle == null) {
                    _itemStyle = new TableItemStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_itemStyle).TrackViewState();
                    }
                }
                return _itemStyle;
            }
        }

        [
        Browsable(false),
        DefaultValue(null),
        Description("The template associated with all items"),
        PersistenceMode(PersistenceMode.InnerProperty),
        TemplateContainer(typeof(ListViewItem))
        ]
        public ITemplate ItemTemplate {
            get {
                return _itemTemplate;
            }
            set {
                _itemTemplate = value;
            }
        }

        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(-1),
        Description("The index of the current selected item")
        ]
        public virtual int SelectedIndex {
            get {
                object i = ViewState["SelectedIndex"];
                return (i == null) ? -1 : (int)i;
            }
            set {
                if (value < -1) {
                    throw new ArgumentOutOfRangeException("value");
                }

                int oldSelectedIndex = SelectedIndex;
                if (oldSelectedIndex != value) {
                    if ((oldSelectedIndex != -1) && (_items != null) && (oldSelectedIndex < _items.Count)) {
                        ListViewItemType itemType = _items[oldSelectedIndex].ItemType;

                        itemType = (itemType & ~ListViewItemType.SelectedItem);
                        _items[oldSelectedIndex].ResetItemType(itemType);
                    }

                    ViewState["SelectedIndex"] = value;
                    if ((value != -1) && (_items != null) && (value < _items.Count)) {
                        ListViewItemType itemType = _items[value].ItemType;

                        itemType = (itemType | ListViewItemType.SelectedItem);
                        _items[value].ResetItemType(itemType);
                    }
                }
            }
        }

        [
        Category("Style"),
        Description("The style applied to selected items"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TableItemStyle SelectedItemStyle {
            get {
                if (_selectedItemStyle == null) {
                    _selectedItemStyle = new TableItemStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_selectedItemStyle).TrackViewState();
                    }
                }
                return _selectedItemStyle;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(false),
        Description("Whether to show the footer")
        ]
        public virtual bool ShowFooter {
            get {
                object b = ViewState["ShowFooter"];
                return (b == null) ? false : (bool)b;
            }
            set {
                ViewState["ShowFooter"] = value;
            }
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Whether to show the header")
        ]
        public virtual bool ShowHeader {
            get {
                object b = ViewState["ShowHeader"];
                return (b == null) ? true : (bool)b;
            }
            set {
                ViewState["ShowHeader"] = value;
            }
        }

        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(true),
        Description("Whether to show scrollbars")
        ]
        public virtual bool ShowScrollBars {
            get {
                object b = ViewState["ShowScrollBars"];
                return (b == null) ? true : (bool)b;
            }
            set {
                ViewState["ShowScrollBars"] = value;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the view"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public ListViewPanelStyle ViewStyle {
            get {
                if (_viewStyle == null) {
                    _viewStyle = new ListViewPanelStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_viewStyle).TrackViewState();
                    }
                }
                return _viewStyle;
            }
        }

        [
        Category("Action"),
        Description("Raised when a control within an item raises a Command event")
        ]
        public event ListViewCommandEventHandler ItemCommand {
            add {
                Events.AddHandler(EventItemCommand, value);
            }
            remove {
                Events.RemoveHandler(EventItemCommand, value);
            }
        }

        [
        Category("Behavior"),
        Description("Raised when an item is created")
        ]
        public event ListViewItemEventHandler ItemCreated {
            add {
                Events.AddHandler(EventItemCreated, value);
            }
            remove {
                Events.RemoveHandler(EventItemCreated, value);
            }
        }

        [
        Category("Behavior"),
        Description("Raised when an item has been data-bound")
        ]
        public event ListViewItemEventHandler ItemDataBound {
            add {
                Events.AddHandler(EventItemDataBound, value);
            }
            remove {
                Events.RemoveHandler(EventItemDataBound, value);
            }
        }

        [
        Category("Action"),
        Description("Raised when the selection changes")
        ]
        public event EventHandler SelectedIndexChanged {
            add {
                Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove {
                Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        protected override void CreateChildControls() {
            // If this gets called, we are re-creating children (the items)
            // from view state.
            Controls.Clear();

            // We can create the items if we have view state, so we check for
            // the number of Items we created during a previous request via a
            // call to the DataBind method.
            if (ViewState["Items"] != null) {
                CreateControlHierarchy(false);
            }
        }

        protected virtual void CreateControlHierarchy(bool useDataSource) {
            IEnumerable dataSource = null;
            int itemCount = 0;

            _items = null;

            ArrayList dataKeysArray = DataKeysArray;
            string dataKeyField = null;

            if (useDataSource) {
                dataSource = GetDataSource();

                dataKeysArray.Clear();
                dataKeyField = DataKeyField;
            }
            else {
                dataSource = new object[(int)ViewState["Items"]];
            }

            if (dataSource != null) {
                Table outerTable = new Table();
                Controls.Add(outerTable);

                ListViewItem headerItem = null;
                ListViewItem footerItem = null;
                
                if (_headerTemplate != null) {
                    TableRow headerRow = new TableRow();
                    outerTable.Rows.Add(headerRow);
                    headerItem = CreateListViewItem(headerRow, -1, ListViewItemType.Header, null, useDataSource);
                }

                TableRow bodyRow = new TableRow();
                outerTable.Rows.Add(bodyRow);

                TableCell bodyCell = new TableCell();
                bodyRow.Cells.Add(bodyCell);

                ListViewPanel viewPanel = new ListViewPanel();
                bodyCell.Controls.Add(viewPanel);

                ListViewTable innerTable = CreateListViewTable();
                viewPanel.Controls.Add(innerTable);

                TableRow itemsRow = new TableRow();
                innerTable.Rows.Add(itemsRow);

                int editIndex = EditIndex;
                int selectedIndex = SelectedIndex;

                int itemIndex = 0;
                foreach (object dataItem in dataSource) {
                    ListViewItemType itemType = ListViewItemType.Item;

                    if (itemIndex == editIndex) {
                        itemType |= ListViewItemType.EditItem;
                    }
                    if (itemIndex == selectedIndex) {
                        itemType |= ListViewItemType.SelectedItem;
                    }

                    CreateListViewItem(itemsRow, itemIndex, itemType, dataItem, useDataSource);
                    itemIndex++;
                    itemCount++;

                    if (useDataSource && (dataKeyField.Length != 0)) {
                        dataKeysArray.Add(DataBinder.GetPropertyValue(dataItem, dataKeyField));
                    }
                }

                if (_footerTemplate != null) {
                    TableRow footerRow = new TableRow();
                    outerTable.Rows.Add(footerRow);
                    CreateListViewItem(footerRow, -1, ListViewItemType.Footer, null, useDataSource);
                }

                _items = CreateListViewItemCollection(itemsRow.Cells, headerItem, footerItem);
            }

            if (useDataSource) {
                 ViewState["Items"] = itemCount;
            }
        }

        protected override Style CreateControlStyle() {
            // Because ListView renders an HTML table, an instance of
            // a TableStyle is used as the control style.
            TableStyle style = new TableStyle(ViewState);

            // This is also the right spot to initialize the style
            style.CellSpacing = 0;

            return style;
        }

        protected virtual ListViewItem CreateListViewItem(int itemIndex, ListViewItemType itemType) {
            return new ListViewItem(itemIndex, itemType);
        }
        
        private ListViewItem CreateListViewItem(TableRow rowContainer, int itemIndex, ListViewItemType itemType, object dataItem, bool dataBind) {
            ListViewItem item = CreateListViewItem(itemIndex, itemType);
            ListViewItemEventArgs e = new ListViewItemEventArgs(item);

            ITemplate template = GetTemplateForItem(item);
            if (template != null) {
                template.InstantiateIn(item);
            }

            OnItemCreated(e);
            rowContainer.Cells.Add(item);

            if (dataBind) {
                item.DataItem = dataItem;
                item.DataBind();

                OnItemDataBound(e);
            }

            return item;
        }

        protected virtual ListViewItemCollection CreateListViewItemCollection(TableCellCollection cells, ListViewItem headerItem, ListViewItem footerItem) {
            return new ListViewItemCollection(cells, headerItem, footerItem);
        }

        protected virtual ListViewTable CreateListViewTable() {
            return new ListViewTable();
        }

        public override void DataBind() {
            // Data-bound controls implement custom data-binding logic by overriding
            // this method.

            // NOTE: We still want the DataBinding event to fire, so any <%# %> expressions
            //       on this control get evaluated first
            base.OnDataBinding(EventArgs.Empty);

            // Now re-create the new control hierarchy using the assigned data source.
            Controls.Clear();

            // We also want to throw out any view state for children if it exists because
            // we're creating a new hierarchy. And then start tracking changes made
            // during the data-binding process.
            ClearChildViewState();
            TrackViewState();

            CreateControlHierarchy(true);

            // Mark the flag indicating child controls have been created, so that
            // CreateChildControls does not get called.
            ChildControlsCreated = true;
        }

        private void DetermineRenderClickSelectScript() {
            // Determine whether to render client script-based functionality.
            _renderClickSelectScript = false;

            if ((Page != null) && (Page.Request != null)) {
                // The page developer can decide to turn off the feature completely.
                if (EnableClickSelect) {
                    // The next set of checks involve looking at the capabilities of the
                    // browser making the request.
                    HttpBrowserCapabilities browserCaps = Page.Request.Browser;
                    bool hasEcmaScript = (browserCaps.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0);
                    bool hasDOM = (browserCaps.MSDomVersion.Major >= 4);

                    _renderClickSelectScript = hasEcmaScript && hasDOM;
                }
            }
        
        }

        protected virtual IEnumerable GetDataSource() {
            if (_dataSource == null) {
                return null;
            }
            
            IEnumerable resolvedDataSource = _dataSource as IEnumerable;
            if (resolvedDataSource != null) {
                return resolvedDataSource;
            }

            IListSource listSource = _dataSource as IListSource;
            if (listSource != null) {
                IList memberList = listSource.GetList();

                if (listSource.ContainsListCollection == false) {
                    return (IEnumerable)memberList;
                }

                ITypedList typedMemberList = memberList as ITypedList;
                if (typedMemberList != null) {
                    PropertyDescriptorCollection propDescs = typedMemberList.GetItemProperties(new PropertyDescriptor[0]);
                    PropertyDescriptor memberProperty = null;
                    
                    if ((propDescs != null) && (propDescs.Count != 0)) {
                        string dataMember = DataMember;

                        if (dataMember.Length == 0) {
                            memberProperty = propDescs[0];
                        }
                        else {
                            memberProperty = propDescs.Find(dataMember, true);
                        }

                        if (memberProperty != null) {
                            object listRow = memberList[0];
                            object list = memberProperty.GetValue(listRow);

                            if (list is IEnumerable) {
                                return (IEnumerable)list;
                            }
                        }
                        throw new Exception("A list corresponding to the selected DataMember was not found.");
                    }

                    throw new Exception("The selected data source did not contain any data members to bind to.");
                }
            }

            return null;
        }

        protected virtual ITemplate GetTemplateForItem(ListViewItem item) {
            ITemplate template = null;
            switch (item.ItemType) {
                case ListViewItemType.Header:
                    template = _headerTemplate;
                    break;
                case ListViewItemType.Footer:
                    template = _footerTemplate;
                    break;
                default:
                    template = _itemTemplate;
                    if ((item.ItemType & ListViewItemType.EditItem) != 0) {
                        if (_editItemTemplate != null) {
                            template = _editItemTemplate;
                        }
                    }
                    break;
            }
            return template;
        }

        protected override void LoadViewState(object savedState) {
            object baseState = null;
            object[] myState = null;

            if (savedState != null) {
                myState = (object[])savedState;
                Debug.Assert(myState.Length == 6);

                baseState = myState[0];
            }

            // Always call the base class, even if the state is null, so
            // the base class gets a chance to fully implement its LoadViewState
            // functionality.
            base.LoadViewState(baseState);
            
            if (myState == null) {
                return;
            }

            // NOTE: For performance reasons, the styles are created only if state
            //       exists for them.

            if (myState[1] != null)
                ((IStateManager)ItemStyle).LoadViewState(myState[1]);
            if (myState[2] != null)
                ((IStateManager)EditItemStyle).LoadViewState(myState[2]);
            if (myState[3] != null)
                ((IStateManager)HeaderStyle).LoadViewState(myState[3]);
            if (myState[4] != null)
                ((IStateManager)FooterStyle).LoadViewState(myState[4]);
            if (myState[5] != null)
                ((IStateManager)ViewStyle).LoadViewState(myState[5]);
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e) {
            ListViewCommandEventArgs lce = e as ListViewCommandEventArgs;

            if (lce != null) {
                OnItemCommand(lce);

                if (lce.CommandType == ListViewCommandType.Select) {
                    int oldSelectedIndex = SelectedIndex;
                    if (oldSelectedIndex != lce.Item.ItemIndex) {
                        SelectedIndex = lce.Item.ItemIndex;
                        OnSelectedIndexChanged(EventArgs.Empty);
                    }
                }
                return true;
            }
            return false;
        }

        protected virtual void OnItemCommand(ListViewCommandEventArgs e) {
            ListViewCommandEventHandler handler = (ListViewCommandEventHandler)Events[EventItemCommand];
            if (handler != null) {
                handler(this, null);
            }
        }

        protected virtual void OnItemCreated(ListViewItemEventArgs e) {
            ListViewItemEventHandler handler = (ListViewItemEventHandler)Events[EventItemCreated];
            if (handler != null) {
                handler(this, null);
            }
        }

        protected virtual void OnItemDataBound(ListViewItemEventArgs e) {
            ListViewItemEventHandler handler = (ListViewItemEventHandler)Events[EventItemDataBound];
            if (handler != null) {
                handler(this, null);
            }
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            DetermineRenderClickSelectScript();
            if (_renderClickSelectScript) {
                // NOTE: Page.GetPostBackEventReference automatically registers the page's postback script.
                //       However, if this is done during Render, the script will be rendered at the end
                //       of the page. We want the script up at the start, so the browser has already seen the script
                //       while it is still loading the rest of the page. Thus, clicking on items works while the
                //       page is loading as well.
                string dummyValue = Page.GetPostBackEventReference(this, String.Empty);
            }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e) {
            EventHandler handler = (EventHandler)Events[EventSelectedIndexChanged];
            if (handler != null) {
                handler(this, null);
            }
        }

        protected virtual void PrepareControlHierarchyForRendering() {
            ControlCollection controls = Controls;
            if (controls.Count != 1) {
                return;
            }

            Table outerTable = (Table)controls[0];
            outerTable.CopyBaseAttributes(this);
            if (ControlStyleCreated) {
                outerTable.ApplyStyle(ControlStyle);
            }
            else {
                // Because we didn't create a ControlStyle yet, the settings
                // for the default style of the control need to be applied
                // to the child table control directly.
                outerTable.CellSpacing = 0;
            }

            TableRowCollection rows = outerTable.Rows;
            TableCell bodyCell = null;

            if (_headerTemplate != null) {
                TableRow headerRow = rows[0];
                if (ShowHeader) {
                    if (_headerStyle != null) {
                        headerRow.Cells[0].MergeStyle(_headerStyle);
                    }
                }
                else {
                    headerRow.Visible = false;
                }

                bodyCell = rows[1].Cells[0];
            }
            if (_footerTemplate != null) {
                TableRow footerRow = rows[rows.Count - 1];
                if (ShowFooter) {
                    if (_footerStyle != null) {
                        footerRow.Cells[0].MergeStyle(_footerStyle);
                    }
                }
                else {
                    footerRow.Visible = false;
                }
            }

            if (bodyCell == null) {
                bodyCell = rows[0].Cells[0];
            }

            ListViewPanel viewPanel = (ListViewPanel)bodyCell.Controls[0];
            if (_viewStyle != null) {
                viewPanel.ApplyStyle(_viewStyle);

                if (ShowScrollBars) {
                    viewPanel.Style["overflow"] = "scroll";
                    viewPanel.Style["overflow-x"] = "auto";
                    viewPanel.Style["overflow-y"] = "auto";
                }
            }

            ListViewTable bodyTable = (ListViewTable)viewPanel.Controls[0];
            bodyTable.Columns = Columns;

            foreach (ListViewItem item in _items) {
                TableItemStyle style = _itemStyle;
                TableItemStyle compositeStyle = null;
                ListViewItemType itemType = item.ItemType;

                if (((itemType & ListViewItemType.EditItem) != 0) && (_editItemStyle != null)) {
                    if (style != null) {
                        compositeStyle = new TableItemStyle();
                        compositeStyle.CopyFrom(style);
                        compositeStyle.CopyFrom(_editItemStyle);
                    }
                    else {
                        style = _editItemStyle;
                    }
                }
                if (((itemType & ListViewItemType.SelectedItem) != 0) && (_selectedItemStyle != null)) {
                    if (compositeStyle != null) {
                        compositeStyle.CopyFrom(_selectedItemStyle);
                    }
                    else if (style != null) {
                        compositeStyle = new TableItemStyle();
                        compositeStyle.CopyFrom(style);
                        compositeStyle.CopyFrom(_selectedItemStyle);
                    }
                    else {
                        style = _selectedItemStyle;
                    }
                }

                if (compositeStyle != null) {
                    item.MergeStyle(compositeStyle);
                }
                else if (style != null) {
                    item.MergeStyle(style);
                }

                if (_renderClickSelectScript) {
                    if ((itemType & ListViewItemType.SelectedItem) == 0) {
                        item.Attributes["onclick"] = Page.GetPostBackEventReference(this, "S" + item.ItemIndex);
                        item.Style["cursor"] = "hand";
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            // Applying styles to the control hierarchy and then render it out.

            // NOTE: Styles are applied as late as the render time.
            //       a) User can change styles after calling DataBind.
            //       b) Changes made to items during style application do not
            //          contribute to the view state. This control manages the
            //          state for styles, so having items manage it as well would
            //          be redundant.
            PrepareControlHierarchyForRendering();

            // NOTE: We don't render out tags corresponding to ListView itself.
            //       We need to render its contents only. Therefore instead of calling
            //       base.Render, we call RenderContents.
            RenderContents(writer);
        }

        protected override object SaveViewState() {
            object[] myState = new object[6];

            // NOTE: Again, the styles are saved only if they have been created.

            myState[0] = base.SaveViewState();
            myState[1] = (_itemStyle != null) ? ((IStateManager)_itemStyle).SaveViewState() : null;
            myState[2] = (_editItemStyle != null) ? ((IStateManager)_editItemStyle).SaveViewState() : null;
            myState[3] = (_headerStyle != null) ? ((IStateManager)_headerStyle).SaveViewState() : null;
            myState[4] = (_footerStyle != null) ? ((IStateManager)_footerStyle).SaveViewState() : null;
            myState[5] = (_viewStyle != null) ? ((IStateManager)_viewStyle).SaveViewState() : null;

            // NOTE: We don't check for all nulls because the control is almost certain to
            //       have some view state, because like most data-bound controls, it saves
            //       information to re-create itself without a live data source on round-trips.
            return myState;
        }

        protected override void TrackViewState() {
            base.TrackViewState();

            // NOTE: Again, the tracking is propagated only to those styles
            //       that have been created. New styles created thereafter will be marked
            //       as tracking view state when they are demand-created.

            if (_itemStyle != null)
                ((IStateManager)_itemStyle).TrackViewState();
            if (_editItemStyle != null)
                ((IStateManager)_editItemStyle).TrackViewState();
            if (_headerStyle != null)
                ((IStateManager)_headerStyle).TrackViewState();
            if (_footerStyle != null)
                ((IStateManager)_footerStyle).TrackViewState();
            if (_viewStyle != null)
                ((IStateManager)_viewStyle).TrackViewState();
        }

        #region Implementation of IPostBackEventHandler
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument) {
            if ((eventArgument.Length > 1) && (eventArgument[0] == 'S')) {
                SelectedIndex = Int32.Parse(eventArgument.Substring(1));
                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }
        #endregion
    }
}
