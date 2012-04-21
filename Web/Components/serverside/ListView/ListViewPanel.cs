// ListViewPanel.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    [
    ToolboxItem(false)
    ]
    public class ListViewPanel : Panel {

        public override string BackImageUrl {
            get {
                if (ControlStyleCreated) {
                    return ((ListViewPanelStyle)ControlStyle).BackImageUrl;
                }
                return String.Empty;
            }
            set {
                ((ListViewPanelStyle)ControlStyle).BackImageUrl = value;
            }
        }

        public override HorizontalAlign HorizontalAlign {
            get {
                if (ControlStyleCreated) {
                    return ((ListViewPanelStyle)ControlStyle).HorizontalAlign;
                }
                return HorizontalAlign.NotSet;
            }
            set {
                ((ListViewPanelStyle)ControlStyle).HorizontalAlign = value;
            }
        }

        public override bool Wrap {
            get {
                if (ControlStyleCreated) {
                    return ((ListViewPanelStyle)ControlStyle).Wrap;
                }
                return true;
            }
            set {
                ((ListViewPanelStyle)ControlStyle).Wrap = value;
            }
        }

        protected override Style CreateControlStyle() {
            return new ListViewPanelStyle(ViewState);
        }
    }
}
