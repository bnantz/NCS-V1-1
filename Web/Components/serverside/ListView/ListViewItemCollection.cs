// ListViewItemCollection.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    public class ListViewItemCollection : ICollection {

        private TableCellCollection _itemCellCollection;
        private ListViewItem _headerItem;
        private ListViewItem _footerItem;

        internal ListViewItemCollection(TableCellCollection itemCellCollection, ListViewItem headerItem, ListViewItem footerItem) {
            _itemCellCollection = itemCellCollection;
            _headerItem = headerItem;
            _footerItem = footerItem;
        }

        public virtual int Count {
            get {
                return _itemCellCollection.Count;
            }
        }

        public virtual ListViewItem FooterItem {
            get {
                return _footerItem;
            }
        }
        
        public virtual ListViewItem HeaderItem {
            get {
                return _headerItem;
            }
        }

        public virtual bool IsReadOnly {
            get {
                return true;
            }
        }

        public virtual bool IsSynchronized {
            get {
                return false;
            }
        }

        public virtual object SyncRoot {
            get {
                return this;
            }
        }

        public virtual ListViewItem this[int index] {
            get {
                return (ListViewItem)_itemCellCollection[index];
            }
        }

        public virtual IEnumerator GetEnumerator() {
            return _itemCellCollection.GetEnumerator();
        }

        public virtual void CopyTo(Array array, int index) {
            _itemCellCollection.CopyTo(array, index);
        }
    }
}
