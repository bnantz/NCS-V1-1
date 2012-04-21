// ListViewDesigner.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using MSPress.WebControls;

using AttributeCollection = System.ComponentModel.AttributeCollection;

namespace MSPress.WebControls.Design {

    public class ListViewDesigner : TemplatedControlDesigner, IDataSourceProvider {

        private const int HeaderFooterTemplates = 0;
        private const int ItemTemplates = 1;

        private static string[] HeaderFooterTemplateNames = new string[] { "HeaderTemplate", "FooterTemplate" };
        private const int HeaderTemplate = 0;
        private const int FooterTemplate = 1;

        private static string[] ItemTemplateNames = new string[] { "ItemTemplate", "EditItemTemplate" };
        private const int ItemTemplate = 0;
        private const int EditItemTemplate = 1;

        private TemplateEditingVerb[] _templateEditingVerbs;
        private DataTable _dummyDataTable;
        private DataTable _designTimeDataTable;

        public override bool AllowResize {
            get {
                return InTemplateMode || (((ListView)Component).ItemTemplate != null);
            }
        }

        public string DataKeyField {
            get {
                return ((ListView)Component).DataKeyField;
            }
            set {
                ((ListView)Component).DataKeyField = value;
            }
        }

        public string DataMember {
            get {
                return ((ListView)Component).DataMember;
            }
            set {
                ((ListView)Component).DataMember = value;
                OnDataSourceChanged();
            }
        }

        public string DataSource {
            get {
                DataBinding binding = DataBindings["DataSource"];

                if (binding != null) {
                    return binding.Expression;
                }
                return String.Empty;
            }
            set {
                if ((value == null) || (value.Length == 0)) {
                    DataBindings.Remove("DataSource");
                }
                else {
                    DataBinding binding = DataBindings["DataSource"];

                    if (binding == null) {
                        binding = new DataBinding("DataSource", typeof(object), value);
                    }
                    else {
                        binding.Expression = value;
                    }
                    DataBindings.Add(binding);
                }

                OnDataSourceChanged();
                OnBindingsCollectionChanged("DataSource");
            }
        }

        public override bool DesignTimeHtmlRequiresLoadComplete {
            get {
                // If we have a data source, we're going to look it up in the container
                // and require the document to be loaded completely.
                return (DataSource.Length != 0);
            }
        }

        protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb) {
            ITemplateEditingFrame frame = null;

            if ((_templateEditingVerbs != null) && ((IList)_templateEditingVerbs).Contains(verb)) {
                ITemplateEditingService teService = (ITemplateEditingService)GetService(typeof(ITemplateEditingService));

                if (teService != null) {
                    ListView lv = (ListView)Component;
                    string[] templateNames = null;
                    Style[] templateStyles = null;

                    switch (verb.Index) {
                        case HeaderFooterTemplates:
                            templateNames = HeaderFooterTemplateNames;
                            templateStyles = new Style[] { lv.HeaderStyle, lv.FooterStyle };
                            break;
                        case ItemTemplates:
                            templateNames = ItemTemplateNames;
                            templateStyles = new Style[] { lv.ItemStyle, lv.EditItemStyle };
                            break;
                    }
                    frame = teService.CreateFrame(this, verb.Text, templateNames, lv.ControlStyle, templateStyles);
                }
            }
            return frame;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                DisposeTemplateEditingVerbs();
            }
            base.Dispose(disposing);
        }

        private void DisposeTemplateEditingVerbs() {
            if (_templateEditingVerbs != null) {
                for (int i = 0; i < _templateEditingVerbs.Length; i++) {
                    _templateEditingVerbs[i].Dispose();
                }
                _templateEditingVerbs = null;
            }
        }

        protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs() {
            if (_templateEditingVerbs == null) {
                _templateEditingVerbs = new TemplateEditingVerb[2];

                _templateEditingVerbs[0] = new TemplateEditingVerb("Header and Footer", HeaderFooterTemplates, this);
                _templateEditingVerbs[1] = new TemplateEditingVerb("Items", ItemTemplates, this);
            }
            return _templateEditingVerbs;
        }

    private IEnumerable GetDesignTimeDataSource(int minimumRows) {
        IEnumerable selectedDataSource = ((IDataSourceProvider)this).GetResolvedSelectedDataSource();
        DataTable dataTable = _designTimeDataTable;

        // Use the data table corresponding to the selected data source if possible.
        if (dataTable == null) {
            if (selectedDataSource != null) {
                _designTimeDataTable = DesignTimeData.CreateSampleDataTable(selectedDataSource);
                dataTable = _designTimeDataTable;
            }

            if (dataTable == null) {
                // Fall back on a dummy data source if we can't create a sample data table.
                if (_dummyDataTable == null) {
                    _dummyDataTable = DesignTimeData.CreateDummyDataTable();
                }

                dataTable = _dummyDataTable;
            }
        }

        IEnumerable liveDataSource = DesignTimeData.GetDesignTimeDataSource(dataTable, minimumRows);
        return liveDataSource;
    }

    public override string GetDesignTimeHtml() {
        ListView lv = (ListView)Component;

        if (lv.ItemTemplate == null) {
            return GetEmptyDesignTimeHtml();
        }
        
        string designTimeHTML = null;

        IEnumerable designTimeDataSource = GetDesignTimeDataSource(5);

        bool dataKeyFieldChanged = false;
        string oldDataKeyField = null;

        try {
            lv.DataSource = designTimeDataSource;

            oldDataKeyField = lv.DataKeyField;
            if (oldDataKeyField.Length != 0) {
                dataKeyFieldChanged = true;
                lv.DataKeyField = String.Empty;
            }
            lv.DataBind();

            designTimeHTML = base.GetDesignTimeHtml();
        }
        catch (Exception e) {
            designTimeHTML = GetErrorDesignTimeHtml(e);
        }
        finally {
            lv.DataSource = null;
            if (dataKeyFieldChanged) {
                lv.DataKeyField = oldDataKeyField;
            }
        }
        return designTimeHTML;
    }

        protected override string GetEmptyDesignTimeHtml() {
            return CreatePlaceHolderDesignTimeHtml("You must create a non-empty ItemTemplate to use a ListView control.");
        }

        protected override string GetErrorDesignTimeHtml(Exception e) {
            return CreatePlaceHolderDesignTimeHtml("There was an error rendering the ListView.");
        }

        public override string GetTemplateContainerDataItemProperty(string templateName) {
            return "DataItem";
        }

        public override IEnumerable GetTemplateContainerDataSource(string templateName) {
            return ((IDataSourceProvider)this).GetResolvedSelectedDataSource();
        }

        public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing) {
            allowEditing = true;

            if ((_templateEditingVerbs != null) && ((IList)_templateEditingVerbs).Contains(editingFrame.Verb)) {
                ListView lv = (ListView)Component;
                ITemplate template = null;

                switch (editingFrame.Verb.Index) {
                    case HeaderFooterTemplates:
                        if (templateName.Equals(HeaderFooterTemplateNames[HeaderTemplate])) {
                            template = lv.HeaderTemplate;
                        }
                        else if (templateName.Equals(HeaderFooterTemplateNames[FooterTemplate])) {
                            template = lv.FooterTemplate;
                        }
                        break;
                    case ItemTemplates:
                        if (templateName.Equals(ItemTemplateNames[ItemTemplate])) {
                            template = lv.ItemTemplate;
                        }
                        else if (templateName.Equals(ItemTemplateNames[EditItemTemplate])) {
                            template = lv.EditItemTemplate;
                        }
                        break;
                }

                string templateContent = String.Empty;
                if (template != null) {
                    templateContent = GetTextFromTemplate(template);
                }

                return templateContent;
            }
            return String.Empty;
        }

        public override void Initialize(IComponent component) {
            if (!(component is ListView)) {
                throw new ArgumentException("Component must be a ListView", "component");
            }
            base.Initialize(component);
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs e) {
            if (e.Member != null) {
                string memberName = e.Member.Name;
                if (memberName.Equals("Font") ||
                    memberName.Equals("ForeColor") ||
                    memberName.Equals("BackColor") ||
                    memberName.Equals("ItemStyle") ||
                    memberName.Equals("HeaderStyle") ||
                    memberName.Equals("FooterStyle") ||
                    memberName.Equals("EditItemStyle")) {
                    DisposeTemplateEditingVerbs();
                }
            }

            base.OnComponentChanged(sender, e);
        }

        protected internal virtual void OnDataSourceChanged() {
            _designTimeDataTable = null;
        }

        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);

            PropertyDescriptor prop;

            prop = (PropertyDescriptor)properties["DataSource"];
            Debug.Assert(prop != null);

            // We can't create the designer DataSource property based on the run-time property because these
            // types do not match. Therefore, we have to copy over all the attributes from the runtime
            // and use them that way.
            AttributeCollection runtimeAttributes = prop.Attributes;
            Attribute[] attrs = new Attribute[runtimeAttributes.Count + 1];

            runtimeAttributes.CopyTo(attrs, 0);
            attrs[runtimeAttributes.Count] = new TypeConverterAttribute(typeof(DataSourceConverter));
            prop = TypeDescriptor.CreateProperty(this.GetType(), "DataSource", typeof(string),
                                                 attrs);
            properties["DataSource"] = prop;
            
            prop = (PropertyDescriptor)properties["DataMember"];
            Debug.Assert(prop != null);
            prop = TypeDescriptor.CreateProperty(this.GetType(), prop,
                                                 new Attribute[] {
                                                     new TypeConverterAttribute(typeof(DataMemberConverter))
                                                 });
            properties["DataMember"] = prop;

            prop = (PropertyDescriptor)properties["DataKeyField"];
            Debug.Assert(prop != null);
            prop = TypeDescriptor.CreateProperty(this.GetType(), prop,
                                                 new Attribute[] {
                                                     new TypeConverterAttribute(typeof(DataFieldConverter))
                                                 });
            properties["DataKeyField"] = prop;
        }

        public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent) {
            if ((_templateEditingVerbs != null) && ((IList)_templateEditingVerbs).Contains(editingFrame.Verb)) {
                ListView lv = (ListView)Component;
                ITemplate newTemplate = null;

                try {
                    newTemplate = GetTemplateFromText(templateContent);
                }
                catch {
                    return;
                }

                switch (editingFrame.Verb.Index) {
                    case HeaderFooterTemplates:
                        if (templateName.Equals(HeaderFooterTemplateNames[HeaderTemplate])) {
                            lv.HeaderTemplate = newTemplate;
                        }
                        else if (templateName.Equals(HeaderFooterTemplateNames[FooterTemplate])) {
                            lv.FooterTemplate = newTemplate;
                        }
                        break;
                    case ItemTemplates:
                        if (templateName.Equals(ItemTemplateNames[ItemTemplate])) {
                            lv.ItemTemplate = newTemplate;
                        }
                        else if (templateName.Equals(ItemTemplateNames[EditItemTemplate])) {
                            lv.EditItemTemplate = newTemplate;
                        }
                        break;
                }
            }
        }

        #region Implementation of IDataSourceProvider
        object IDataSourceProvider.GetSelectedDataSource() {
            object selectedDataSource = null;

            DataBinding binding = DataBindings["DataSource"];
            if (binding != null) {
                selectedDataSource = DesignTimeData.GetSelectedDataSource(Component, binding.Expression);
            }

            return selectedDataSource;
        }

        IEnumerable IDataSourceProvider.GetResolvedSelectedDataSource() {
            IEnumerable selectedDataSource = null;

            DataBinding binding = DataBindings["DataSource"];
            if (binding != null) {
                selectedDataSource = DesignTimeData.GetSelectedDataSource(Component, binding.Expression, DataMember);
            }

            return selectedDataSource;
        }
        #endregion
    }
}
