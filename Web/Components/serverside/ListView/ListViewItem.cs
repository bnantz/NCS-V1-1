// ListViewItem.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    [
    ToolboxItem(false)
    ]
    public class ListViewItem : TableCell, INamingContainer {

        private int _itemIndex;
        private object _dataItem;
        private ListViewItemType _itemType;

        public ListViewItem(int itemIndex, ListViewItemType itemType) {
            _itemIndex = itemIndex;
            _itemType = itemType;
        }

        public virtual object DataItem {
            get {
                return _dataItem;
            }
            set {
                _dataItem = value;
            }
        }

        public int ItemIndex {
            get {
                return _itemIndex;
            }
        }

        public virtual ListViewItemType ItemType {
            get {
                return _itemType;
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e) {
            CommandEventArgs ce = e as CommandEventArgs;
            
            if (ce != null) {
                ListViewCommandEventArgs lce = new ListViewCommandEventArgs(this, sender, ce);
                RaiseBubbleEvent(this, lce);
                
                return true;
            }
            return false;
        }

        public virtual void ResetItemType(ListViewItemType newItemType) {
            _itemType = newItemType;
        }
    }
}
