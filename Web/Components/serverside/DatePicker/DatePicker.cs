// DatePicker.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Image = System.Web.UI.WebControls.Image;

namespace MSPress.WebControls {

    // The DatePicker control allows the user to input a date in mm/dd/yyyy format
    // and provides a drop-down UI for better end-user experience.
    // 
    // On IE4, it uses a floating IFrame to implement the dropdown.
    // On IE5+, it uses a DHTML popup window, which provides real floating window implementation.
    // On downlevel browsers, it falls back on just a plain old textbox.
    [
    ControlBuilder(typeof(DatePickerControlBuilder)),
    DefaultEvent("DateChanged"),
    DefaultProperty("SelectedDate"),
    Designer(typeof(MSPress.WebControls.Design.DatePickerDesigner), typeof(IDesigner)),
    ValidationProperty("SelectedDateText")
    ]
    public class DatePicker : WebControl, INamingContainer {

        private static readonly object EventDateChanged = new object();
        private static string ClientFilesUrlPrefix;

        private TableStyle _calendarStyle;
        private Style _titleStyle;
        private Style _dayHeaderStyle;
        private Style _dayStyle;
        private Style _otherMonthDayStyle;
        private Style _todayDayStyle;
        private Style _selectedDayStyle;

        private TextBox _dateTextBox;
        private Image _pickerImage;
        private Image _errorImage;
        private RegularExpressionValidator _dateValidator;

        private bool _renderClientScript;
        private bool _renderPopupScript;

        [
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether a change in the control causes the form to be submitted.")
        ]
        public bool AutoPostBack {
            get {
                EnsureChildControls();
                return _dateTextBox.AutoPostBack;
            }
            set {
                EnsureChildControls();
                _dateTextBox.AutoPostBack = value;
            }
        }

        [
        Category("Style"),
        Description("The style used to customize the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual TableStyle CalendarStyle {
            get {
                if (_calendarStyle == null) {
                    _calendarStyle = new TableStyle();
                    if (IsTrackingViewState) {
                        ((IStateManager)_calendarStyle).TrackViewState();
                    }
                }
                return _calendarStyle;
            }
        }

        [
        Category("Appearance"),
        DefaultValue(0),
        Description("The number of columns shown in the date textbox.")
        ]
        public virtual int Columns {
            get {
                EnsureChildControls();
                return _dateTextBox.Columns;
            }
            set {
                EnsureChildControls();
                _dateTextBox.Columns = value;
            }
        }

        public override ControlCollection Controls {
            get {
                // A composite control should always override the Controls property
                // and ensure that its Controls collection is valid.
                EnsureChildControls();
                return base.Controls;
            }
        }

        [
        Category("Style"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("The style used to customize the day headers in the popup calendar."),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style DayHeaderStyle {
            get {
                if (_dayHeaderStyle == null) {
                    _dayHeaderStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_dayHeaderStyle).TrackViewState();
                    }
                }
                return _dayHeaderStyle;
            }
        }

        [
        Category("Style"),
        Description("The style used to customize all days in the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style DayStyle {
            get {
                if (_dayStyle == null) {
                    _dayStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_dayStyle).TrackViewState();
                    }
                }
                return _dayStyle;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(true),
        Description("Whether to enable the client script based functionality such as the popup calendar.")
        ]
        public bool EnableClientScript {
            get {
                object b = ViewState["EnableClientScript"];
                return (b == null) ? true : (bool)b;
            }
            set {
                ViewState["EnableClientScript"] = value;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual bool IsDateSelected {
            get {
                object d = ViewState["SelectedDate"];
                return (d != null);
            }
        }

        [
        Category("Style"),
        Description("The style used to customize days outside the current month in the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style OtherMonthDayStyle {
            get {
                if (_otherMonthDayStyle == null) {
                    _otherMonthDayStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_otherMonthDayStyle).TrackViewState();
                    }
                }
                return _otherMonthDayStyle;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(false),
        Description("Whether the date textbox is in read-only mode.")
        ]
        public virtual bool ReadOnly {
            get {
                EnsureChildControls();
                return _dateTextBox.ReadOnly;
            }
            set {
                EnsureChildControls();
                _dateTextBox.ReadOnly = value;
            }
        }

        [
        Bindable(true),
        Category("Default"),
        Description("The currently selected date.")
        ]
        public virtual DateTime SelectedDate {
            get {
                object d = ViewState["SelectedDate"];
                return (d == null) ? DateTime.Today : (DateTime)d;
            }
            set {
                ViewState["SelectedDate"] = value;

                EnsureChildControls();
                _dateTextBox.Text = SelectedDateText;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        public string SelectedDateText {
            get {
                if (IsDateSelected) {
                    return String.Format(CultureInfo.InvariantCulture, "{0:d}", new object[] { SelectedDate });
                }
                return String.Empty;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the currently selected date in the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style SelectedDayStyle {
            get {
                if (_selectedDayStyle == null) {
                    _selectedDayStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_selectedDayStyle).TrackViewState();
                    }
                }
                return _selectedDayStyle;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the title area of the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style TitleStyle {
            get {
                if (_titleStyle == null) {
                    _titleStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_titleStyle).TrackViewState();
                    }
                }
                return _titleStyle;
            }
        }

        [
        Category("Style"),
        Description("The style applied to the current day in the popup calendar."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style TodayDayStyle {
            get {
                if (_todayDayStyle == null) {
                    _todayDayStyle = new Style();
                    if (IsTrackingViewState) {
                        ((IStateManager)_todayDayStyle).TrackViewState();
                    }
                }
                return _todayDayStyle;
            }
        }

        [
        Category("Appearance"),
        DefaultValue(""),
        Description("The error message shown when the entered text does not match the mm/dd/yyyy date format.")
        ]
        public virtual string ValidationMessage {
            get {
                string s = (string)ViewState["ValidationMessage"];
                return (s == null) ? String.Empty : s;
            }
            set {
                ViewState["ValidationMessage"] = value;
            }
        }

        [
        Category("Change"),
        Description("Raised when the user selects a new date.")
        ]
        public event EventHandler DateChanged {
            add {
                Events.AddHandler(EventDateChanged, value);
            }
            remove {
                Events.RemoveHandler(EventDateChanged, value);
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer) {
            // We do not call the base class AddAttributesToRender
            // because, in the DatePicker, the attributes and styles applied to the
            // control affect the contained TextBox. The DatePicker itself renders
            // out as a <span>, which only contains only the ID attribute and any
            // attributes needed for the pop-up calendar and client script.

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);

            if (_renderClientScript) {
                if (_renderPopupScript) {
                    writer.AddAttribute("dp_htcURL", GetClientFileUrl("Calendar.htc"), false);
                }
                if (AutoPostBack) {
                    writer.AddAttribute("dp_autoPostBack", "true", false);
                }
                if (_calendarStyle != null) {
                    Unit u = _calendarStyle.Width;
                    if (!u.IsEmpty) {
                        writer.AddAttribute("dp_width", u.ToString(CultureInfo.InvariantCulture));
                    }
                    u = _calendarStyle.Height;
                    if (!u.IsEmpty) {
                        writer.AddAttribute("dp_height", u.ToString(CultureInfo.InvariantCulture));
                    }
                    string s = GetCssFromStyle(_calendarStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_calendarStyle", s, false);
                    }
                }
                if (_titleStyle != null) {
                    string s = GetCssFromStyle(_titleStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_titleStyle", s, false);
                    }
                }
                if (_dayHeaderStyle != null) {
                    string s = GetCssFromStyle(_dayHeaderStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_dayHeaderStyle", s, false);
                    }
                }
                if (_dayStyle != null) {
                    string s = GetCssFromStyle(_dayStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_dayStyle", s, false);
                    }
                }
                if (_otherMonthDayStyle != null) {
                    string s = GetCssFromStyle(_otherMonthDayStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_otherMonthDayStyle", s, false);
                    }
                }
                if (_todayDayStyle != null) {
                    string s = GetCssFromStyle(_todayDayStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_todayDayStyle", s, false);
                    }
                }
                if (_selectedDayStyle != null) {
                    string s = GetCssFromStyle(_selectedDayStyle);
                    if (s.Length != 0) {
                        writer.AddAttribute("dp_selectedDayStyle", s, false);
                    }
                }
            }
        }

        protected override void AddParsedSubObject(object o) {
            // Declaratively specified child controls get added through this method.
            // However, the DatePicker builds its own child controls, and does not
            // want to allow the page developer to add controls, which could potentially
            // mess up the control's collection.
            throw new Exception("Cannot add child objects declaratively.");
        }

        protected override void CreateChildControls() {
            _dateTextBox = new TextBox();
            _pickerImage = new Image();
            _errorImage = new Image();
            _dateValidator = new RegularExpressionValidator();

            // EnableViewState is true for the TextBox and false for all other
            // child controls.
            // The TextBox needs view state in its TextChanged event implementation.
            // However, all the other controls are set up here or customized during
            // Render. Therefore, they do not contribute to view state. Additionally,
            // setting their EnableViewState to false tells the page framework to
            // skip them completely, which makes for a small performance improvement.

            _dateTextBox.ID = "dateTextBox";
            _dateTextBox.MaxLength = 10;
            _dateTextBox.TextChanged += new EventHandler(this.dateTextBox_TextChanged);

            _pickerImage.EnableViewState = false;
            _pickerImage.ImageUrl = GetClientFileUrl("Picker.gif");
            _pickerImage.ImageAlign = ImageAlign.AbsMiddle;
            _pickerImage.Attributes["hspace"] = "4";
            _pickerImage.Width = new Unit(34);
            _pickerImage.Height = new Unit(21);

            _errorImage.EnableViewState = false;
            _errorImage.ImageUrl = GetClientFileUrl("Error.gif");
            _errorImage.ImageAlign = ImageAlign.AbsMiddle;
            _errorImage.Width = new Unit(16);
            _errorImage.Height = new Unit(16);

            _dateValidator.EnableViewState = false;
            _dateValidator.ControlToValidate = "dateTextBox";
            _dateValidator.ValidationExpression = "^\\s*(\\d{1,2})([-./])(\\d{1,2})\\2((\\d{4})|(\\d{2}))\\s*$";
            _dateValidator.Display = ValidatorDisplay.Dynamic;
            _dateValidator.Controls.Add(_errorImage);

            // All the controls are fully initialized with their property values
            // before they are added to the control tree.
            // This way, the property values become part of the controls' initial
            // state and do not contribute to view state.
            Controls.Add(_dateTextBox);
            Controls.Add(_pickerImage);
            Controls.Add(_dateValidator);
        }

        private void dateTextBox_TextChanged(object sender, EventArgs e) {
            // Handles the TextChanged event of the contained TextBox.

            try {
                string text = _dateTextBox.Text.Trim();
                DateTime date = DateTime.Parse(text, CultureInfo.InvariantCulture);

                // Determine if the selected date has changed.
                if ((IsDateSelected == false) || !(date.Equals(SelectedDate))) {
                    SelectedDate = date;
                    OnDateChanged(EventArgs.Empty);
                }
            }
            catch {
                // A bad date was entered - remove it from the ViewState which
                // effectively sets SelectedDate back to the current date
                // and IsDateSelected to false.
                ViewState.Remove("SelectedDate");
            }
        }

        private void DetermineClientScriptLevel() {
            // In a client script enabled control, always determine whether to render the
            // client script functionality.
            // The decision should be based on both browser capabilities and the page
            // developer's choice.

            _renderClientScript = false;
            _renderPopupScript = false;

            if ((Page != null) && (Page.Request != null)) {
                // The page developer can decide to turn off script completely.
                if (EnableClientScript) {
                    // The next set of checks involve looking at the capabilities of the
                    // browser making the request.
                    //
                    // The DatePicker needs to verify whether the browser has EcmaScript (JavaScript)
                    // version 1.2+, and whether the browser supports DHTML, and optionally,
                    // DHTML behaviors.

                    HttpBrowserCapabilities browserCaps = Page.Request.Browser;
                    bool hasEcmaScript = (browserCaps.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0);
                    bool hasDOM = (browserCaps.MSDomVersion.Major >= 4);
                    bool hasBehaviors = (browserCaps.MajorVersion > 5) ||
                                        ((browserCaps.MajorVersion == 5) && (browserCaps.MinorVersion >= .5));

                    _renderClientScript = hasEcmaScript && hasDOM;
                    _renderPopupScript = _renderClientScript && hasBehaviors;
                }
            }
        }

        private string GetClientFileUrl(string fileName) {
            if (ClientFilesUrlPrefix == null) {
                // Use the config setting to determine where the client files are located.
                // Client files are located in the aspnet_client v-root and then distributed
                // into subfolders by assembly name and assembly version.
                string location = null;
                if (Context != null) {
                    IDictionary configData = (IDictionary)Context.GetConfig("system.web/webControls");
                    if (configData != null) {
                        location = (string)configData["clientScriptsLocation"];
                    }
                }

                if (location == null) {
                    location = String.Empty;
                }
                else if (location.IndexOf("{0}") >= 0) {
                    AssemblyName assemblyName = GetType().Assembly.GetName();

                    string assembly = assemblyName.Name.Replace('.', '_').ToLower();
                    string version =  assemblyName.Version.ToString().Replace('.', '_');

                    location = String.Format(location, assembly, version);
                }

                ClientFilesUrlPrefix = location;
            }
            return ClientFilesUrlPrefix + fileName;
        }

        private string GetClientScriptInclude(string scriptFile) {
            return "<script language=\"JavaScript\" src=\"" + GetClientFileUrl(scriptFile) + "\"></script>";
        }

        private string GetCssFromStyle(Style style) {
            StringBuilder sb = new StringBuilder(256);
            Color c;

            c = style.ForeColor;
            if (!c.IsEmpty) {
                sb.Append("color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }
            c = style.BackColor;
            if (!c.IsEmpty) {
                sb.Append("background-color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }

            FontInfo fi = style.Font;
            string s;

            s = fi.Name;
            if (s.Length != 0) {
                sb.Append("font-family:'");
                sb.Append(s);
                sb.Append("';");
            }
            if (fi.Bold) {
                sb.Append("font-weight:bold;");
            }
            if (fi.Italic) {
                sb.Append("font-style:italic;");
            }

            s = String.Empty;
            if (fi.Underline)
                s += "underline";
            if (fi.Strikeout)
                s += " line-through";
            if (fi.Overline)
                s += " overline";
            if (s.Length != 0) {
                sb.Append("text-decoration:");
                sb.Append(s);
                sb.Append(';');
            }

            FontUnit fu = fi.Size;
            if (fu.IsEmpty == false) {
                sb.Append("font-size:");
                sb.Append(fu.ToString(CultureInfo.InvariantCulture));
                sb.Append(';');
            }

            s = String.Empty;
            Unit u = style.BorderWidth;
            BorderStyle bs = style.BorderStyle;
            if (u.IsEmpty == false) {
                s = u.ToString(CultureInfo.InvariantCulture);
                if (bs == BorderStyle.NotSet) {
                    s += " solid";
                }
            }
            c = style.BorderColor;
            if (!c.IsEmpty) {
                s += " " + ColorTranslator.ToHtml(c);
            }
            if (bs != BorderStyle.NotSet) {
                s += " " + Enum.Format(typeof(BorderStyle), bs, "G");
            }
            if (s.Length != 0) {
                sb.Append("border:");
                sb.Append(s);
                sb.Append(';');
            }

            return sb.ToString();
        }

        protected override void LoadViewState(object savedState) {
            object baseState = null;
            object[] myState = null;

            if (savedState != null) {
                myState = (object[])savedState;
                if (myState.Length != 8) {
                    throw new ArgumentException("Invalid view state");
                }

                baseState = myState[0];
            }

            // Always call the base class, even if the state is null, so
            // everyone gets a chance to fully implement their LoadViewState
            // functionality.
            base.LoadViewState(baseState);
            
            if (myState == null) {
                return;
            }

            // For perfomance reasons, the styles are created only if state
            // exists for them. It is especially important to optimize
            // their creation in complex controls because styles are
            // relatively complex objects and have an impact on the
            // rendering logic.

            if (myState[1] != null)
                ((IStateManager)CalendarStyle).LoadViewState(myState[1]);
            if (myState[2] != null)
                ((IStateManager)TitleStyle).LoadViewState(myState[1]);
            if (myState[3] != null)
                ((IStateManager)DayHeaderStyle).LoadViewState(myState[1]);
            if (myState[4] != null)
                ((IStateManager)DayStyle).LoadViewState(myState[1]);
            if (myState[5] != null)
                ((IStateManager)OtherMonthDayStyle).LoadViewState(myState[1]);
            if (myState[6] != null)
                ((IStateManager)TodayDayStyle).LoadViewState(myState[1]);
            if (myState[7] != null)
                ((IStateManager)SelectedDayStyle).LoadViewState(myState[1]);
        }

        protected virtual void OnDateChanged(EventArgs e) {
            EventHandler handler = (EventHandler)Events[EventDateChanged];
            if (handler != null) {
                handler(this, e);
            }
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            DetermineClientScriptLevel();
            if (_renderClientScript) {
                Page.RegisterClientScriptBlock(typeof(DatePicker).FullName, GetClientScriptInclude("DatePicker.js"));
            }

            // Propagate the setting into the validator as well, because it is logically
            // part of this control now.
            _dateValidator.EnableClientScript = EnableClientScript;
        }

        protected override void Render(HtmlTextWriter writer) {
            // Ensure that this control is inside a <form runat="server">
            if (Page != null) {
                Page.VerifyRenderingInServerForm(this);
            }
            base.Render(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer) {
            // Now render the actual textbox and picker UI

            Debug.Assert(_dateTextBox != null);
            Debug.Assert(_pickerImage != null);
            Debug.Assert(_dateValidator != null);

            // First prepare the text box for rendering by applying this control's
            // style and any attributes to it.
            // This is done as part of Render, so any changes made to the textbox
            // are not persisted in view state. The values are already being held
            // as part of this control's view state, so having the textbox also
            // store them would be redundant.
            if (ControlStyleCreated) {
                _dateTextBox.ApplyStyle(ControlStyle);
            }
            _dateTextBox.CopyBaseAttributes(this);
            _dateTextBox.RenderControl(writer);

            // Design-mode is determined by the availability of a non-null Site
            // that is itself in design-mode.
            bool designMode = (Site != null) && Site.DesignMode;

            bool showPicker = _renderClientScript;
            if (showPicker == false) {
                // In design-mode, we want the picker to be visible, even though
                // OnPreRender is not called, where the determination is made whether
                // or not to show the picker. Therefore, unless the user has
                // explicitely turned off client script, the picker needs to be shown.
                if (EnableClientScript && designMode) {
                    showPicker = true;
                }
            }
            if (showPicker) {
                string pickerAction;
                if (_renderPopupScript) {
                    pickerAction = "dp_showDatePickerPopup(this, document.all['" + _dateTextBox.ClientID + "'], document.all['" + ClientID + "'])";
                }
                else {
                    pickerAction = "dp_showDatePickerFrame(this, document.all['" + _dateTextBox.ClientID + "'], document.all['" + ClientID + "'], document.all['" + ClientID + "_frame'], document)";
                }
                _pickerImage.Attributes["onclick"] = pickerAction;
                _pickerImage.RenderControl(writer);

                if (_renderPopupScript == false) {
                    // Use an IFRAME instead of a DHTML popup
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_frame");
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, GetClientFileUrl("CalendarFrame.htm"));
                    writer.AddAttribute("marginheight", "0", false);
                    writer.AddAttribute("marginwidth", "0", false);
                    writer.AddAttribute("noresize", "noresize", false);
                    writer.AddAttribute("frameborder", "0", false);
                    writer.AddAttribute("scrolling", "no", false);
                    writer.AddStyleAttribute("position", "absolute");
                    writer.AddStyleAttribute("z-index", "100");
                    writer.AddStyleAttribute("display", "none");
                    writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
                    writer.RenderEndTag();
                }
            }

            // The designer sets the Visibility of the validator to false,
            // since in design-mode, the control should not show up as invalid.
            // Therefore the extra check.
            // Just the minimal bit of logic to aid the designer...
            if (_dateValidator.Visible) {
                _dateValidator.ErrorMessage = ValidationMessage;
                _dateValidator.ToolTip = ValidationMessage;
                _dateValidator.RenderControl(writer);
            }
        }

        protected override object SaveViewState() {
            object[] myState = new object[8];

            // Again, note that the styles are saved only if they have been created.

            myState[0] = base.SaveViewState();
            myState[1] = (_calendarStyle != null) ? ((IStateManager)_calendarStyle).SaveViewState() : null;
            myState[2] = (_titleStyle != null) ? ((IStateManager)_titleStyle).SaveViewState() : null;
            myState[3] = (_dayHeaderStyle != null) ? ((IStateManager)_dayHeaderStyle).SaveViewState() : null;
            myState[4] = (_dayStyle != null) ? ((IStateManager)_dayStyle).SaveViewState() : null;
            myState[5] = (_otherMonthDayStyle != null) ? ((IStateManager)_otherMonthDayStyle).SaveViewState() : null;
            myState[6] = (_todayDayStyle != null) ? ((IStateManager)_todayDayStyle).SaveViewState() : null;
            myState[7] = (_selectedDayStyle != null) ? ((IStateManager)_selectedDayStyle).SaveViewState() : null;

            for (int i = 0; i < 8; i++) {
                if (myState[i] != null) {
                    return myState;
                }
            }

            // Another perfomance optimization. If no modifications were made to any
            // properties from their persisted state, the view state for this control
            // is null. Returning null, rather than an array of null values helps
            // minimize the view state significantly.
            return null;
        }

        protected override void TrackViewState() {
            base.TrackViewState();

            // Again, note that the tracking is propagated only to those styles
            // that have been created. New styles created thereafter will be marked
            // as tracking view state when they are created on demand.

            if (_calendarStyle != null)
                ((IStateManager)_calendarStyle).TrackViewState();
            if (_titleStyle != null)
                ((IStateManager)_titleStyle).TrackViewState();
            if (_dayHeaderStyle != null)
                ((IStateManager)_dayHeaderStyle).TrackViewState();
            if (_dayStyle != null)
                ((IStateManager)_dayStyle).TrackViewState();
            if (_otherMonthDayStyle != null)
                ((IStateManager)_otherMonthDayStyle).TrackViewState();
            if (_todayDayStyle != null)
                ((IStateManager)_todayDayStyle).TrackViewState();
            if (_selectedDayStyle != null)
                ((IStateManager)_selectedDayStyle).TrackViewState();
        }
    }
}
