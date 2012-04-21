// ListViewPanelStyle.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    public class ListViewPanelStyle : Style {

        internal const int PROP_BACKIMAGEURL = 1;
        internal const int PROP_HORIZONTALALIGN = 2;
        internal const int PROP_WRAP = 3;

        public ListViewPanelStyle() {
        }

        public ListViewPanelStyle(StateBag bag) : base(bag) {
        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("")
        ]
        public virtual string BackImageUrl {
            get {
                string s = (string)ViewState["BackImageUrl"];
                return (s == null) ? String.Empty : s;
            }
            set {
                ViewState["BackImageUrl"] = value;
            }
        }

        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(HorizontalAlign.NotSet),
        Description("")
        ]
        public virtual HorizontalAlign HorizontalAlign {
            get {
                object o = ViewState["HorizontalAlign"];
                return (o == null) ? HorizontalAlign.NotSet : (HorizontalAlign)o;
            }
            set {
                if (value < HorizontalAlign.NotSet || value > HorizontalAlign.Justify) {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["HorizontalAlign"] = value;
            }
        }

        protected new internal bool IsEmpty {
            get {
                return base.IsEmpty &&
                       !IsSet(PROP_BACKIMAGEURL) &&
                       !IsSet(PROP_HORIZONTALALIGN) &&
                       !IsSet(PROP_WRAP);
            }
        }

        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(true),
        Description("")
        ]
        public virtual bool Wrap {
            get {
                object b = ViewState["Wrap"];
                return (b == null) ? true : (bool)b;
            }
            set {
                ViewState["Wrap"] = value;
            }
        }

        public override void AddAttributesToRender(HtmlTextWriter writer, WebControl owner) {
            base.AddAttributesToRender(writer, owner);

            // NOTE: We don't have to add any attributes here, because Panel (which
            //       ListViewPanel derives from already does the work in its AddAttributesToRender.
            //       Ideally, the work would be done here, but, the ASP.NET Panel was not
            //       designed to have an associated typed-style, so it does the work itself.
        }

        public override void CopyFrom(Style s) {
            if (s != null) {
                base.CopyFrom(s);

                if (s is ListViewPanelStyle) {
                    ListViewPanelStyle lvs = (ListViewPanelStyle)s;

                    if (!lvs.IsEmpty) {
                        if (lvs.IsSet(PROP_BACKIMAGEURL))
                            this.BackImageUrl = lvs.BackImageUrl;
                        if (lvs.IsSet(PROP_HORIZONTALALIGN))
                            this.HorizontalAlign = lvs.HorizontalAlign;
                        if (lvs.IsSet(PROP_WRAP))
                            this.Wrap = lvs.Wrap;
                    }
                }
            }
        }

        internal bool IsSet(int propNumber) {
            string key = null;
            switch (propNumber) {
                case PROP_BACKIMAGEURL:
                    key = "BackImageUrl";
                    break;
                case PROP_HORIZONTALALIGN:
                    key = "HorizontalAlign";
                    break;
                case PROP_WRAP:
                    key = "Wrap";
                    break;
            }

            if (key != null) {
                return ViewState[key] != null;
            }
            return false;
        }

        public override void MergeWith(Style s) {
            if (s != null) {
                if (IsEmpty) {
                    // merge into an empty style is equivalent to a copy,
                    // which is more efficient
                    CopyFrom(s);
                    return;
                }

                base.MergeWith(s);

                if (s is ListViewPanelStyle) {
                    ListViewPanelStyle lvs = (ListViewPanelStyle)s;

                    if (!lvs.IsEmpty) {
                        if (lvs.IsSet(PROP_BACKIMAGEURL) && !this.IsSet(PROP_BACKIMAGEURL))
                            this.BackImageUrl = lvs.BackImageUrl;
                        if (lvs.IsSet(PROP_HORIZONTALALIGN) && !this.IsSet(PROP_HORIZONTALALIGN))
                            this.HorizontalAlign = lvs.HorizontalAlign;
                        if (lvs.IsSet(PROP_WRAP) && !this.IsSet(PROP_WRAP))
                            this.Wrap = lvs.Wrap;
                    }
                }
            }
        }

        public override void Reset() {
            if (IsSet(PROP_BACKIMAGEURL))
                ViewState.Remove("BackImageUrl");
            if (IsSet(PROP_HORIZONTALALIGN))
                ViewState.Remove("HorizontalAlign");
            if (IsSet(PROP_WRAP))
                ViewState.Remove("Wrap");

            base.Reset();
        }
    }
}
