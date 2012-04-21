// ListViewItemEventArgs.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;

namespace MSPress.WebControls {

    public class ListViewItemEventArgs : EventArgs {

        private ListViewItem _item;

        public ListViewItemEventArgs(ListViewItem item) {
            _item = item;
        }

        public ListViewItem Item {
            get {
                return _item;
            }
        }
    }

    public delegate void ListViewItemEventHandler(object sender, ListViewItemEventArgs e);
}
